using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace cTabWebApp.Messaging
{
    public class MessageTemplate
    {
        public string? Uid { get; set; }

        public List<MessageLineTemplate>? Lines { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter<MessageTemplateType>))]
        public MessageTemplateType Type { get; set; }

        public string? Href { get; set; }

        public string? JsonHref { get; set; }

        public string? CountryCode { get; set; }

        public string? ShortTitle { get; set; }
    }
}
