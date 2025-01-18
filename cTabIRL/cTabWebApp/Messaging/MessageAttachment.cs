#nullable enable
using System.Text.Json.Serialization;

namespace cTabWebApp.Messaging
{
    public class MessageAttachment
    {
        [JsonConverter(typeof(JsonStringEnumConverter<MessageAttachmentType>))]
        public MessageAttachmentType Type { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Label { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double[]? MarkerPosition { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double[]? Position { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double[]? PositionPrecision { get; set; }
    }
}