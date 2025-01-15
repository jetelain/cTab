using System;
using System.Collections.Generic;
using cTabWebApp.Messaging;

namespace cTabWebApp
{
    public class UpdateMessageTemplatesMessage
    {
        public DateTime Timestamp { get; set; }

        public List<MessageTemplate> Templates { get; set; } = new List<MessageTemplate>();
    }
}