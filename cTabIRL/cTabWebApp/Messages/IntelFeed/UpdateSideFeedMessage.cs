using System;
using cTabWebApp.Messaging;
using System.Collections.Generic;

namespace cTabWebApp.Messages.IntelFeed
{
    public class UpdateSideFeedMessage
    {
        public DateTime Timestamp { get; set; }

        public List<IntelEntry> Entries { get; set; } = new List<IntelEntry>();
    }
}