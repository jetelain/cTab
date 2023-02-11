using System;
using System.Text.Json.Serialization;

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

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double[] VhlDir { get; internal set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double[] VhlVel { get; internal set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double[] VhlPos { get; internal set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double[] Wind { get; internal set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double[] VhlUp { get; internal set; }
    }
}