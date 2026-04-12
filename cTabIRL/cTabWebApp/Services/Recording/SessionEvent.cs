using System;
using System.Text.Json.Serialization;

namespace cTabWebApp.Services.Recording
{
    public class SessionEvent
    {
        public EventType Type { get; set; }

        public RecordableMessageBase Data { get; set; }

        [JsonPropertyName("time")]
        public long TimestampMs { get; set; }
    }
}
