using System;

namespace cTabExtension
{
    internal class ArmaMessage
    {
        public ArmaMessage()
        {
        }

        public DateTime Timestamp { get; set; }
        public string[] Args { get; set; }
    }
}