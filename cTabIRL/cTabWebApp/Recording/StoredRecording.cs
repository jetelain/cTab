using System;

#nullable enable

namespace cTabWebApp.Recording
{
    public class StoredRecording
    {
        public required string Token { get; set; }
        public required string SteamId { get; set; }
        public string? WorldName { get; set; }
        public DateTime RecordingStart { get; set; }
        public DateTime RecordingEnd { get; set; }
        public DateTime ExpiresUtc { get; set; }
    }
}
