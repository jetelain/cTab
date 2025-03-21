using System;
using System.Text.Json.Serialization;

#nullable enable

namespace cTabWebApp.Services.Images
{
    public class PlayerTakenImage
    {
        public DateTime TimestampUtc { get; set; } = DateTime.MinValue;

        public DateTime ExpiresUtc { get; set; } = DateTime.MinValue;

        public required string Token { get; set; }

        public string? OwnerSteamId { get; set; }

        public string? Data { get; set; }

        public string? WorldName { get; set; }

        public string? Remote { get; set; }

        public int Size { get; set; }

        [JsonIgnore]
        public PlayerState? Player { get; set; }
    }
}