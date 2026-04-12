using System;
using System.Text.Json.Serialization;

namespace cTabWebApp
{
    [JsonPolymorphic]
    [JsonDerivedType(typeof(MissionMessage))]
    [JsonDerivedType(typeof(SetPositionMessage))]
    [JsonDerivedType(typeof(UpdateMapMarkersMessage))]
    [JsonDerivedType(typeof(UpdateMarkersMessage))]
    [JsonDerivedType(typeof(UpdateMarkersMessagePosition))]
    public class RecordableMessageBase
    {
        [JsonIgnore]
        public DateTime Timestamp { get; set; }
    }
}