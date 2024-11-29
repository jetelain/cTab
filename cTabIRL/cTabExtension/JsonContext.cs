using System.Text.Json.Serialization;

namespace cTabExtension
{
    [JsonSerializable(typeof(ArmaHelloMessage))]
    [JsonSerializable(typeof(ArmaMessage))]
    internal partial class JsonContext : JsonSerializerContext
    {
    }
}
