using System.Collections.Generic;
using cTabWebApp.Messaging;
#nullable enable
namespace cTabWebApp
{
    public class WebSendMessageMessage
    {
        public string? To { get; set; }
        public string? Body { get; set; }
        public string? Title { get; set; }
        public MessageTemplateType MessageType { get; set; }
        public List<MessageAttachment>? Attachments { get; set; }
    }
}