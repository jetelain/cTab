using System.Collections.Generic;
using System.Text.Json.Serialization;
using cTabWebApp.Messaging;
#nullable enable
namespace cTabWebApp
{
    public class WebSendMessageMessage
    {
        public string? To { get; set; }
        public string? Body { get; set; }
        public string? Title { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter<MessageTemplateType>))]
        public MessageTemplateType Type { get; set; }
        public List<MessageAttachment>? Attachments { get; set; }
    }
}