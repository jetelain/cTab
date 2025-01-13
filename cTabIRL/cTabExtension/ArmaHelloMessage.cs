using System;

namespace cTabExtension
{
    internal class ArmaHelloMessage
    {
        public ArmaHelloMessage()
        {
        }

        public required DateTime Timestamp { get; set; }
        public required string SteamId { get; set; }
        public required string PlayerName { get; set; }
        public required string Key { get; set; }
    }
}