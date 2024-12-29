using System;
using System.Collections.Generic;

namespace cTabWebApp
{
    public class UpdateMessageTemplatesMessage
    {
        public UpdateMessageTemplatesMessage()
        {
        }

        public DateTime Timestamp { get; set; }
        public List<MessageTemplate> Templates { get; set; }
    }
}