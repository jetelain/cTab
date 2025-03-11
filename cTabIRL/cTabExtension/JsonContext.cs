using System.Text.Json.Serialization;

namespace cTabExtension
{
    [JsonSerializable(typeof(ArmaHelloMessage))]
    [JsonSerializable(typeof(ArmaMessage))]
    [JsonSerializable(typeof(ScreenShotOptions))]
    internal partial class JsonContext : JsonSerializerContext
    {
    }
}
