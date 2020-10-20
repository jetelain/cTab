using System;
using System.Collections.Generic;

namespace cTabWebApp
{
    internal class UpdateMessagesMessage
    {
        public UpdateMessagesMessage()
        {
        }

        public DateTime Timestamp { get; set; }
        public List<Message> Messages { get; set; }
    }
}