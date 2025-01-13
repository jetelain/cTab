using System.Collections.Generic;

namespace cTabWebApp.Messaging
{
    public class Message
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public int State { get; set; }
        public string Body { get; set; }
        public MessageTemplateType MessageType { get; set; }
        public List<MessageAttachment>? Attachments { get; set; }
    }
}