using System;
using System.Collections.Generic;

namespace cTabWebApp.Services.Recording
{
    public class ActiveRecording
    {
        public DateTime StartedAt { get; } = DateTime.UtcNow;

        private readonly List<SessionEvent> _events = new List<SessionEvent>();

        public List<SessionEvent> Events => _events;

        public void Append(string type, object data)
        {
            lock (_events)
            {
                _events.Add(new SessionEvent { Type = type, Data = data });
            }
        }
    }
}
