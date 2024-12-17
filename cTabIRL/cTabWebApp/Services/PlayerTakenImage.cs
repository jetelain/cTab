using System;
using System.Text.Json.Serialization;

#nullable enable

namespace cTabWebApp.Services
{
    public class PlayerTakenImage
    {
        public DateTime TimestampUtc { get; set; } = DateTime.UtcNow;

        public required string Token { get; set; }

        [JsonIgnore]
        public bool IsExpired => TimestampUtc.AddHours(12) < DateTime.UtcNow;

        public string? OwnerSteamId { get; set; }

        public string? Data { get; set; }

        public string? WorldName { get; set; }
    }
}