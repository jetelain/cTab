using System.Text.Json.Serialization;

namespace cTabWebApp
{
    public class Marker
    {
        public double X { get; internal set; }
        public double Y { get; internal set; }
        public double? Heading { get; internal set; }
        public string Symbol { get; internal set; }
        public string Id { get; internal set; }
        public string Kind { get; internal set; }
        public string Name { get; internal set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Vehicle { get; internal set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Group { get; internal set; }
    }
}