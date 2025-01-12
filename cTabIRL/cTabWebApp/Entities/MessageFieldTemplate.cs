using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace cTabWebApp.Entities
{
    public class MessageFieldTemplate
    {
        [Display(Name = "Prefix")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Title { get; set; }

        [Display(Name = "Description")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Description { get; set; }

        [Display(Name = "Input type")]
        [JsonConverter(typeof(JsonStringEnumConverter<MessageFieldType>))]
        public MessageFieldType Type { get; set; }
    }
}