using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace cTabWebApp.Services.Recording
{
    public class ActiveRecording
    {
        // Thresholds stored as ticks so Append only needs an array index + long subtraction.
        // Zero means no rate limit.
        private static readonly long[] RateLimitTicks = CreateRateLimitTicks();

        // When true the existing event's Data is overwritten in place (preserving list position).
        // When false the incoming event is silently dropped.
        private static readonly bool[] RateLimitKeepLatest = CreateRateLimitKeepLatest();

        private static long[] CreateRateLimitTicks()
        {
            var ticks = new long[Enum.GetValues<EventType>().Length];
            ticks[(int)EventType.SetPosition]           = (long)(1 * Stopwatch.Frequency); // The typical rate of this message is 0.25 sec
            ticks[(int)EventType.UpdateMarkersPosition] = (long)(1 * Stopwatch.Frequency); // The typical rate of this message is 1.5 sec, rate limiting is only a safeguard
            ticks[(int)EventType.UpdateMapMarkers]      = (long)(5 * Stopwatch.Frequency); // This message is "on demand", but custom scripts can make it really frequent (up to 0.25 sec) and this message is massive 
            return ticks;
        }

        private static bool[] CreateRateLimitKeepLatest()
        {
            var modes = new bool[Enum.GetValues<EventType>().Length];
            modes[(int)EventType.UpdateMapMarkers] = true;
            return modes;
        }

        public DateTime StartedAt { get; } = DateTime.UtcNow;

        public DateTime LastEventAt { get; private set; } = DateTime.UtcNow;

        private readonly List<SessionEvent> _events = new List<SessionEvent>();
        private readonly long[] _lastRecordedTicks = new long[RateLimitTicks.Length];

        // Index into _events of the last recorded event for each keep-latest type.
        private readonly int[] _lastRecordedEventIndex = new int[RateLimitTicks.Length];

        public List<SessionEvent> TakeSnapshot()
        {
            lock (_events)
            {
                return new List<SessionEvent>(_events);
            }
        }

        public void Append(EventType type, RecordableMessageBase data)
        {
            lock (_events)
            {
                var idx = (int)type;
                var limit = RateLimitTicks[idx];
                if (limit > 0)
                {
                    var now = Stopwatch.GetTimestamp();
                    if (_lastRecordedTicks[idx] != 0 && (now - _lastRecordedTicks[idx]) < limit)
                    {
                        if (RateLimitKeepLatest[idx])
                        {
                            // We intentionally keep the timestamp of the original event to preserve the timing of the first update to avoid weird "jumps" in the UI
                            // This kind of event is not regularly sent, so to avoid data loss, we update the existing event with the latest data.
                            _events[_lastRecordedEventIndex[idx]].Data = data;
                        }
                        return;
                    }
                    _lastRecordedTicks[idx] = now;
                }

                if (RateLimitKeepLatest[idx])
                {
                    _lastRecordedEventIndex[idx] = _events.Count;
                }
                _events.Add(new SessionEvent { Type = type, Data = data, TimestampMs = (long)(data.Timestamp - DateTime.UnixEpoch).TotalMilliseconds });
                LastEventAt = DateTime.UtcNow;
            }
        }
    }
}
