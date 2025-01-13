using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace cTabWebApp.Messaging
{
    public class MessageLineTemplate
    {
        [Display(Name = "Prefix")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Title { get; set; }

        [Display(Name = "Description")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Description { get; set; }

        public List<MessageFieldTemplate>? Fields { get; set; }
    }
}