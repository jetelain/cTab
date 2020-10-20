using System;

namespace cTabExtension
{
    internal class ArmaHelloMessage
    {
        public ArmaHelloMessage()
        {
        }

        public DateTime Timestamp { get; set; }
        public string SteamId { get; set; }
        public string PlayerName { get; set; }
        public string Key { get; set; }
    }
}