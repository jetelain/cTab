using System;

namespace cTabExtension
{
    internal class ArmaMessage
    {
        public ArmaMessage()
        {
        }

        public required DateTime Timestamp { get; set; }
        public required string?[] Args { get; set; }
    }
}