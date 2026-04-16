using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

// ---------------------------------------------------------------------------
// cTab Recording Analyzer
// Usage: dotnet run -- <recording.json>
//
// Reports:
//  1. Basic metadata (world, duration, total events)
//  2. Event counts by type
//  3. Timestamp format breakdown (legacy data.timestamp string vs new event.time long)
//     with an estimate of the JSON byte savings the new format provides
//  4. Rate-limiting simulation that mirrors ActiveRecording.Append logic:
//       SetPosition           1 s → drop
//       UpdateMarkersPosition 1 s → drop
//       UpdateMapMarkers      5 s → update-in-place (keep-latest)
// ---------------------------------------------------------------------------

CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

if (args.Length == 0)
{
    Console.Error.WriteLine("Usage: cTabRecordingAnalyzer <recording.json>");
    return 1;
}

var filePath = args[0];
if (!File.Exists(filePath))
{
    Console.Error.WriteLine($"File not found: {filePath}");
    return 1;
}

var fileSize = new FileInfo(filePath).Length;
Console.WriteLine("# cTab Recording Analyzer");
Console.WriteLine();
Console.WriteLine($"- **File**: `{filePath}`");
Console.WriteLine($"- **Size**: {fileSize:N0} bytes ({fileSize / 1024.0 / 1024.0:F2} MiB)");
Console.WriteLine();

// ---- Parse ----------------------------------------------------------------
using var stream = File.OpenRead(filePath);
using var doc = JsonDocument.Parse(stream, new JsonDocumentOptions { AllowTrailingCommas = true });
var root = doc.RootElement;

var worldName      = root.TryGetProperty("worldName",      out var wnProp) ? wnProp.GetString()  : "?";
var recordingStart = root.TryGetProperty("recordingStart", out var rsProp) ? rsProp.GetString()  : null;
var recordingEnd   = root.TryGetProperty("recordingEnd",   out var reProp) ? reProp.GetString()  : null;

Console.WriteLine("## Recording Metadata");
Console.WriteLine();
Console.WriteLine("| Property | Value |");
Console.WriteLine("|----------|-------|");
Console.WriteLine($"| World    | {worldName} |");
Console.WriteLine($"| Start    | {recordingStart} |");
Console.WriteLine($"| End      | {recordingEnd} |");
if (DateTime.TryParse(recordingStart, null, DateTimeStyles.RoundtripKind, out var startDt) &&
    DateTime.TryParse(recordingEnd,   null, DateTimeStyles.RoundtripKind, out var endDt))
{
    var dur = endDt - startDt;
    Console.WriteLine($"| Duration | {(int)dur.TotalHours:D2}:{dur.Minutes:D2}:{dur.Seconds:D2} |");
}
Console.WriteLine();

// ---- Rate-limit config (mirrors ActiveRecording) --------------------------
// threshold in milliseconds; 0 = no limit
var rateLimitMs = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase)
{
    ["SetPosition"]           = 1_000,
    ["UpdateMarkersPosition"] = 1_000,
    ["UpdateMapMarkers"]      = 1_000,
};
// Types whose last event is updated in-place rather than dropped
var keepLatest = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "UpdateMapMarkers" };

// ---- Accumulators ---------------------------------------------------------
var countByType             = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
int legacyTimestampCount    = 0;
int newTimestampCount       = 0;
int noTimestampCount        = 0;
long legacyTimestampBytes   = 0;   // bytes occupied by "timestamp":"<value>" in the source JSON
long newTimestampBytes      = 0;   // estimated bytes for "time":<long> replacement

var rateLimitStats          = new Dictionary<string, RateLimitStats>(StringComparer.OrdinalIgnoreCase);
var lastTimestampByType     = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
long exactDroppedBytes      = 0;  // raw JSON bytes of events that are silently discarded
long exactUpdatedBytes      = 0;  // raw JSON bytes of events whose data is merged in-place (not re-added to list)

// Pattern to strip the legacy data.timestamp field before content comparison
var dedupNormPattern        = new Regex(@"""timestamp""\s*:\s*""[^""]*""");
string? lastUpdateMapMarkersNorm = null;
int  dedupTotal             = 0;
int  dedupDropped           = 0;
long dedupDroppedBytes      = 0;

// Combined (dedup → rate-limit) simulation for UpdateMapMarkers
double? comboLastTs         = null;  // timestamp of the last event that opened a new rate-limit window
int     comboRlUpdated      = 0;     // passed dedup, within rate-limit window → data updated in-place
int     comboKept           = 0;     // passed dedup and rate limit → new recording entry
long    comboRlUpdatedBytes = 0;     // exact bytes of combo rate-limited events

// ---- Walk events ----------------------------------------------------------
foreach (var evt in root.GetProperty("events").EnumerateArray())
{
    var type = evt.TryGetProperty("type", out var typeProp) ? typeProp.GetString() ?? "?" : "?";
    countByType[type] = countByType.GetValueOrDefault(type) + 1;

    // Resolve timestamp in milliseconds since Unix epoch
    double? tsMs = null;

    if (evt.TryGetProperty("time", out var timeProp) && timeProp.ValueKind == JsonValueKind.Number)
    {
        tsMs = timeProp.GetDouble();
        newTimestampCount++;
    }
    else if (evt.TryGetProperty("data", out var dataProp) &&
             dataProp.TryGetProperty("timestamp", out var tsProp) &&
             tsProp.ValueKind == JsonValueKind.String)
    {
        var tsStr = tsProp.GetString()!;
        if (DateTime.TryParse(tsStr, null, DateTimeStyles.RoundtripKind, out var dt))
        {
            tsMs = (double)new DateTimeOffset(dt).ToUnixTimeMilliseconds();
        }
        // Byte cost in JSON: "timestamp":"<value>"
        // key (with quotes+colon) = "timestamp": = 13 chars; value (with quotes) = 2+len
        legacyTimestampBytes += 13 + 2 + tsStr.Length;
        // Estimated replacement: "time":<13-digit long>  →  "time": = 7 chars + 13 digits
        newTimestampBytes    += 7 + 13;
        legacyTimestampCount++;
    }
    else
    {
        noTimestampCount++;
    }

    // ---- Simulate rate limiting -------------------------------------------
    if (tsMs.HasValue && rateLimitMs.TryGetValue(type, out var limitMs))
    {
        if (!rateLimitStats.ContainsKey(type))
            rateLimitStats[type] = new RateLimitStats();

        var stats = rateLimitStats[type];

        if (lastTimestampByType.TryGetValue(type, out var lastTs) && (tsMs.Value - lastTs) < limitMs)
        {
            // GetRawText() returns the exact source JSON of this element (without surrounding
            // whitespace / commas), so UTF-8 byte count gives the precise contribution of this
            // event to the file size (plus ~2 bytes of array separator, which we ignore here).
            var rawBytes = Encoding.UTF8.GetByteCount(evt.GetRawText());
            if (keepLatest.Contains(type))
            {
                stats.Updated++;   // data overwritten in-place, event NOT re-added
                exactUpdatedBytes += rawBytes;
            }
            else
            {
                stats.Dropped++;   // silently discarded
                exactDroppedBytes += rawBytes;
            }
        }
        else
        {
            stats.Kept++;
            lastTimestampByType[type] = tsMs.Value;
        }
    }

    // ---- Simulate content-based deduplication of UpdateMapMarkers ----------
    if (string.Equals(type, "UpdateMapMarkers", StringComparison.OrdinalIgnoreCase))
    {
        dedupTotal++;
        // Normalise: get data JSON and strip the timestamp field (legacy format)
        // so that two events with identical map markers but different timestamps compare equal.
        var dataNorm    = evt.TryGetProperty("data", out var dedupData)
            ? dedupNormPattern.Replace(dedupData.GetRawText(), "")
            : "";
        var evtRawBytes = (long)Encoding.UTF8.GetByteCount(evt.GetRawText());

        if (dataNorm == lastUpdateMapMarkersNorm)
        {
            // Dedup hit: skip entirely, never reaches rate limiting
            dedupDropped++;
            dedupDroppedBytes += evtRawBytes;
        }
        else
        {
            lastUpdateMapMarkersNorm = dataNorm;
            // Combined: event passed dedup → now apply rate-limit on top
            if (tsMs.HasValue && comboLastTs.HasValue && (tsMs.Value - comboLastTs.Value) < rateLimitMs["UpdateMapMarkers"])
            {
                comboRlUpdated++;           // data updated in-place, no new recording entry
                comboRlUpdatedBytes += evtRawBytes;
            }
            else
            {
                comboKept++;                // new recording entry
                if (tsMs.HasValue) { comboLastTs = tsMs.Value; }
            }
        }
    }
}

int totalEvents = countByType.Values.Sum();

// ---- Report: event counts -------------------------------------------------
Console.WriteLine("## Event Counts");
Console.WriteLine();
Console.WriteLine("| Type | Count | % of Total |");
Console.WriteLine("|------|------:|-----------:|");
foreach (var (type, count) in countByType.OrderByDescending(x => x.Value))
{
    Console.WriteLine($"| {type} | {count:N0} | {(double)count / totalEvents:P1} |");
}
Console.WriteLine($"| **TOTAL** | **{totalEvents:N0}** | |");
Console.WriteLine();

// ---- Report: timestamp format ---------------------------------------------
Console.WriteLine("## Timestamp Format");
Console.WriteLine();
Console.WriteLine("| Format | Count |");
Console.WriteLine("|--------|------:|");
Console.WriteLine($"| Legacy `data.timestamp` string | {legacyTimestampCount:N0} |");
Console.WriteLine($"| New `event.time` long | {newTimestampCount:N0} |");
Console.WriteLine($"| None / not applicable | {noTimestampCount:N0} |");
Console.WriteLine();

if (legacyTimestampCount > 0)
{
    long savedBytes = legacyTimestampBytes - newTimestampBytes;
    Console.WriteLine("### Timestamp Byte Savings");
    Console.WriteLine();
    Console.WriteLine("| Description | Bytes | % of File |");
    Console.WriteLine("|-------------|------:|----------:|");
    Console.WriteLine($"| Legacy `data.timestamp` fields | {legacyTimestampBytes:N0} | |");
    Console.WriteLine($"| New `event.time` fields (estimated) | {newTimestampBytes:N0} | |");
    Console.WriteLine($"| **Gross savings** | **{savedBytes:N0}** | **{(double)savedBytes / fileSize:P2}** |");
    Console.WriteLine();
}
Console.WriteLine();

// ---- Report: rate-limiting simulation ------------------------------------
Console.WriteLine("## Rate-Limiting Simulation");
Console.WriteLine();
Console.WriteLine("| Type | Threshold (s) | Total | Kept | Dropped | Updated | Reduction |");
Console.WriteLine("|------|:-------------:|------:|-----:|--------:|--------:|----------:|");

int totalRawRl = 0, totalKeptRl = 0, totalDroppedRl = 0, totalUpdatedRl = 0;

foreach (var (type, stats) in rateLimitStats.OrderBy(x => x.Key))
{
    int total    = stats.Kept + stats.Dropped + stats.Updated;
    double red   = total > 0 ? (double)(stats.Dropped + stats.Updated) / total : 0;
    Console.WriteLine($"| {type} | {rateLimitMs[type]/1000.0:N1} | {total:N0} | {stats.Kept:N0} | {stats.Dropped:N0} | {stats.Updated:N0} | {red:P1} |");
    totalRawRl     += total;
    totalKeptRl    += stats.Kept;
    totalDroppedRl += stats.Dropped;
    totalUpdatedRl += stats.Updated;
}
if (rateLimitStats.Count > 1)
{
    double totalRed = totalRawRl > 0 ? (double)(totalDroppedRl + totalUpdatedRl) / totalRawRl : 0;
    Console.WriteLine($"| **TOTAL (rate-limited types)** | | {totalRawRl:N0} | {totalKeptRl:N0} | {totalDroppedRl:N0} | {totalUpdatedRl:N0} | {totalRed:P1} |");
}

if (totalDroppedRl + totalUpdatedRl > 0)
{
    long totalRlSavedBytes = exactDroppedBytes + exactUpdatedBytes;
    Console.WriteLine();
    Console.WriteLine("### Byte Savings from Rate Limiting");
    Console.WriteLine();
    Console.WriteLine("| Description | Bytes | % of File |");
    Console.WriteLine("|-------------|------:|----------:|");
    Console.WriteLine($"| Exact JSON bytes of dropped events | {exactDroppedBytes:N0} | {(double)exactDroppedBytes / fileSize:P2} |");
    Console.WriteLine($"| Exact JSON bytes of updated events | {exactUpdatedBytes:N0} | {(double)exactUpdatedBytes / fileSize:P2} |");
    Console.WriteLine($"| **Total exact savings** | **{totalRlSavedBytes:N0}** | **{(double)totalRlSavedBytes / fileSize:P2}** |");
    Console.WriteLine();
    Console.WriteLine("> Array separators (~2 bytes/event) excluded; actual savings slightly higher.");
}

Console.WriteLine();

// ---- Report: content-based deduplication of UpdateMapMarkers -------------
Console.WriteLine("## UpdateMapMarkers Deduplication Simulation");
Console.WriteLine();
Console.WriteLine("Mirrors the equality check added to `CTabHub`: an event whose `data` is identical");
Console.WriteLine("to the previous `UpdateMapMarkers` event is skipped entirely (not recorded).");
Console.WriteLine();
Console.WriteLine("| Description | Count | % of Total |" );
Console.WriteLine("|-------------|------:|-----------:|" );
Console.WriteLine($"| Total UpdateMapMarkers events    | {dedupTotal:N0}   | |");
Console.WriteLine($"| Unique (would be emitted)        | {dedupTotal - dedupDropped:N0}   | {(dedupTotal > 0 ? (double)(dedupTotal - dedupDropped) / dedupTotal : 0):P1} |");
Console.WriteLine($"| Duplicate (would be skipped)     | {dedupDropped:N0}   | {(dedupTotal > 0 ? (double)dedupDropped / dedupTotal : 0):P1} |");
Console.WriteLine();
if (dedupDropped > 0)
{
    Console.WriteLine("### Byte Savings from Deduplication");
    Console.WriteLine();
    Console.WriteLine("| Description | Bytes | % of File |");
    Console.WriteLine("|-------------|------:|----------:|");
    Console.WriteLine($"| Exact JSON bytes of duplicate events | {dedupDroppedBytes:N0} | {(double)dedupDroppedBytes / fileSize:P2} |");
    Console.WriteLine();
    Console.WriteLine("> Array separators (~2 bytes/event) excluded; actual savings slightly higher.");
    Console.WriteLine();
}

// ---- Report: combined effect (UpdateMapMarkers only) ---------------------
if (dedupTotal > 0)
{
    var rlStats = rateLimitStats.GetValueOrDefault("UpdateMapMarkers");

    // Events not tracked by the rate-limit sim (no timestamp) always become new entries
    int rlTracked      = rlStats != null ? rlStats.Kept + rlStats.Updated : 0;
    int rlNewEntries   = (rlStats?.Kept ?? 0) + (dedupTotal - rlTracked);
    long rlSavedBytes  = exactUpdatedBytes;  // UpdateMapMarkers is the only keepLatest type

    int  dedupNewEntries  = dedupTotal - dedupDropped;

    int  comboTracked     = comboKept + comboRlUpdated;
    int  comboNewEntries  = comboKept + (dedupNewEntries - comboTracked); // +untracked (no timestamp)
    long comboSavedBytes  = dedupDroppedBytes + comboRlUpdatedBytes;

    Console.WriteLine("## Combined Effect: Rate Limiting + Deduplication (UpdateMapMarkers)");
    Console.WriteLine();
    Console.WriteLine("Recording-entry count and exact byte savings for each combination of the two optimisations.");
    Console.WriteLine();
    Console.WriteLine("| Scenario | New recording entries | vs No Opt | Bytes saved | % of File |");
    Console.WriteLine("|----------|----------------------:|----------:|------------:|----------:|");
    Console.WriteLine($"| No optimisation                   | {dedupTotal:N0} | — | 0 | 0.00 % |");
    Console.WriteLine($"| Rate limiting only ({rateLimitMs["UpdateMapMarkers"]/1000.0:N1} s window) | {rlNewEntries:N0} | {(double)(dedupTotal - rlNewEntries) / dedupTotal:P1} | {rlSavedBytes:N0} | {(double)rlSavedBytes / fileSize:P2} |");
    Console.WriteLine($"| Deduplication only (content)      | {dedupNewEntries:N0} | {(double)dedupDropped / dedupTotal:P1} | {dedupDroppedBytes:N0} | {(double)dedupDroppedBytes / fileSize:P2} |");
    Console.WriteLine($"| Both (dedup first → rate limit)   | {comboNewEntries:N0} | {(double)(dedupTotal - comboNewEntries) / dedupTotal:P1} | {comboSavedBytes:N0} | {(double)comboSavedBytes / fileSize:P2} |");
    Console.WriteLine();
    Console.WriteLine("> **New recording entries** = distinct slots added to the recording array.");
    Console.WriteLine("> In rate-limit mode, within-window events update the last slot in-place (no new entry).");
    Console.WriteLine("> **Bytes saved** = exact JSON bytes of events not written as new recording entries.");
    Console.WriteLine();
}

Console.WriteLine("---");
Console.WriteLine();
Console.WriteLine("- **Kept** = first event in the window, written to the recording list");
Console.WriteLine("- **Dropped** = event within window, silently discarded");
Console.WriteLine("- **Updated** = event within window, existing entry overwritten in-place (keep-latest)");

// Exact recording size impact


return 0;

// ---------------------------------------------------------------------------
class RateLimitStats
{
    public int Kept    { get; set; }
    public int Dropped { get; set; }
    public int Updated { get; set; }
}
