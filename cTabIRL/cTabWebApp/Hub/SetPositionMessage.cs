using System;

namespace cTabWebApp
{
    internal class SetPositionMessage
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Altitude { get; set; }
        public double Heading { get; set; }
        public DateTime Date { get; set; }
        public DateTime Timestamp { get; set; }
        public string Group { get; internal set; }
        public string Vehicle { get; internal set; }
    }
}