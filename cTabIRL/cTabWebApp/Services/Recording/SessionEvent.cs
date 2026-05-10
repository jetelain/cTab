using System;
using System.Text.Json.Serialization;

namespace cTabWebApp.Services.Recording
{
    public class SessionEvent
    {
        public required EventType Type { get; set; }

        public required RecordableMessageBase Data { get; set; }

        [JsonPropertyName("time")]
        public required long TimestampMs { get; set; }
    }
}
