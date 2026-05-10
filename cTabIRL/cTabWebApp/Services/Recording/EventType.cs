using System.Text.Json.Serialization;

namespace cTabWebApp.Services.Recording
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EventType
    {
        Mission = 0,
        SetPosition = 1,
        UpdateMarkers = 2,
        UpdateMarkersPosition = 3,
        UpdateMapMarkers = 4
    }
}
