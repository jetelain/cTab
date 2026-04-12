using System.Globalization;
using System.Text;
using System.Text.Json;

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
//       SetPosition           4.5 s → drop
//       UpdateMarkersPosition 4.5 s → drop
//       UpdateMapMarkers      4.5 s → update-in-place (keep-latest)
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
Console.WriteLine($"Analyzing : {filePath}");
Console.WriteLine($"File size : {fileSize:N0} bytes ({fileSize / 1024.0 / 1024.0:F2} MiB)");
Console.WriteLine();

// ---- Parse ----------------------------------------------------------------
using var stream = File.OpenRead(filePath);
using var doc = JsonDocument.Parse(stream, new JsonDocumentOptions { AllowTrailingCommas = true });
var root = doc.RootElement;

var worldName      = root.TryGetProperty("worldName",      out var wnProp) ? wnProp.GetString()  : "?";
var recordingStart = root.TryGetProperty("recordingStart", out var rsProp) ? rsProp.GetString()  : null;
var recordingEnd   = root.TryGetProperty("recordingEnd",   out var reProp) ? reProp.GetString()  : null;

Console.WriteLine($"World     : {worldName}");
Console.WriteLine($"Start     : {recordingStart}");
Console.WriteLine($"End       : {recordingEnd}");
if (DateTime.TryParse(recordingStart, null, DateTimeStyles.RoundtripKind, out var startDt) &&
    DateTime.TryParse(recordingEnd,   null, DateTimeStyles.RoundtripKind, out var endDt))
{
    var dur = endDt - startDt;
    Console.WriteLine($"Duration  : {(int)dur.TotalHours:D2}:{dur.Minutes:D2}:{dur.Seconds:D2}");
}
Console.WriteLine();

// ---- Rate-limit config (mirrors ActiveRecording) --------------------------
// threshold in milliseconds; 0 = no limit
var rateLimitMs = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase)
{
    ["SetPosition"]           = 1_000,
    ["UpdateMarkersPosition"] = 1_000,
    ["UpdateMapMarkers"]      = 5_000,
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
}

int totalEvents = countByType.Values.Sum();

// ---- Report: event counts -------------------------------------------------
Console.WriteLine("=== Event Counts ===");
Console.WriteLine($"  {"Type",-28} {"Count",8}   {"% of total"}");
Console.WriteLine($"  {new string('-', 50)}");
foreach (var (type, count) in countByType.OrderByDescending(x => x.Value))
{
    Console.WriteLine($"  {type,-28} {count,8:N0}   {(double)count / totalEvents,6:P1}");
}
Console.WriteLine($"  {"TOTAL",-28} {totalEvents,8:N0}");
Console.WriteLine();

// ---- Report: timestamp format ---------------------------------------------
Console.WriteLine("=== Timestamp Format ===");
Console.WriteLine($"  Legacy  data.timestamp string : {legacyTimestampCount,8:N0}");
Console.WriteLine($"  New     event.time long       : {newTimestampCount,8:N0}");
Console.WriteLine($"  None / not applicable         : {noTimestampCount,8:N0}");

if (legacyTimestampCount > 0)
{
    long savedBytes = legacyTimestampBytes - newTimestampBytes;
    Console.WriteLine();
    Console.WriteLine($"  Bytes used by legacy \"timestamp\":\"...\" fields : {legacyTimestampBytes,10:N0}");
    Console.WriteLine($"  Bytes used by new    \"time\":<long>   fields   : {newTimestampBytes,10:N0}  (estimated)");
    Console.WriteLine($"  Gross savings                                  : {savedBytes,10:N0}  ({(double)savedBytes / fileSize,6:P2} of file)");
}
Console.WriteLine();

// ---- Report: rate-limiting simulation ------------------------------------
Console.WriteLine("=== Rate-Limiting Simulation ===");
Console.WriteLine($"  {"Type",-28} {"Threshold",10} {"Total",8} {"Kept",8} {"Dropped",8} {"Updated",8}   {"Reduction":9}");
Console.WriteLine($"  {new string('-', 87)}");

int totalRawRl = 0, totalKeptRl = 0, totalDroppedRl = 0, totalUpdatedRl = 0;

foreach (var (type, stats) in rateLimitStats.OrderBy(x => x.Key))
{
    int total    = stats.Kept + stats.Dropped + stats.Updated;
    double red   = total > 0 ? (double)(stats.Dropped + stats.Updated) / total : 0;
    Console.WriteLine($"  {type,-28} {rateLimitMs[type]/1000.0,10:N1} {total,8:N0} {stats.Kept,8:N0} {stats.Dropped,8:N0} {stats.Updated,8:N0}   {red,9:P1}");
    totalRawRl     += total;
    totalKeptRl    += stats.Kept;
    totalDroppedRl += stats.Dropped;
    totalUpdatedRl += stats.Updated;
}
if (rateLimitStats.Count > 1)
{
    double totalRed = totalRawRl > 0 ? (double)(totalDroppedRl + totalUpdatedRl) / totalRawRl : 0;
    Console.WriteLine($"  {new string('-', 87)}");
    Console.WriteLine($"  {"TOTAL (rate-limited types)",-28} {"",10} {totalRawRl,8:N0} {totalKeptRl,8:N0} {totalDroppedRl,8:N0} {totalUpdatedRl,8:N0}   {totalRed,9:P1}");
}

if (totalDroppedRl + totalUpdatedRl > 0)
{
    long totalRlSavedBytes = exactDroppedBytes + exactUpdatedBytes;
    Console.WriteLine();
    Console.WriteLine($"  Exact JSON bytes of dropped events     : {exactDroppedBytes,14:N0} ({(double)exactDroppedBytes / fileSize,7:P2} of file)");
    Console.WriteLine($"  Exact JSON bytes of updated events     : {exactUpdatedBytes,14:N0} ({(double)exactUpdatedBytes / fileSize,7:P2} of file)");
    Console.WriteLine($"  Total exact savings from rate limiting : {totalRlSavedBytes,14:N0} ({(double)totalRlSavedBytes / fileSize,7:P2} of file)");
    Console.WriteLine();
    Console.WriteLine($"  (array separators ~2 bytes/event excluded; actual savings slightly higher)");
}

Console.WriteLine();
Console.WriteLine("  Kept    = first event in the window, written to the recording list");
Console.WriteLine("  Dropped = event within window, silently discarded");
Console.WriteLine("  Updated = event within window, existing entry overwritten in-place (keep-latest)");

// Exact recording size impact


return 0;

// ---------------------------------------------------------------------------
class RateLimitStats
{
    public int Kept    { get; set; }
    public int Dropped { get; set; }
    public int Updated { get; set; }
}
