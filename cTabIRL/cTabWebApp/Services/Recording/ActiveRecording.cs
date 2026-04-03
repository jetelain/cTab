using System;
using System.Collections.Generic;

namespace cTabWebApp.Services.Recording
{
    public class ActiveRecording
    {
        public DateTime StartedAt { get; } = DateTime.UtcNow;

        public DateTime LastEventAt { get; private set; } = DateTime.UtcNow;

        private readonly List<SessionEvent> _events = new List<SessionEvent>();

        public IReadOnlyList<SessionEvent> Events
        {
            get
            {
                lock (_events)
                {
                    return _events.ToArray();
                }
            }
        }

        public List<SessionEvent> TakeSnapshot()
        {
            lock (_events)
            {
                return new List<SessionEvent>(_events);
            }
        }

        public void Append(string type, object data)
        {
            lock (_events)
            {
                _events.Add(new SessionEvent { Type = type, Data = data });
                LastEventAt = DateTime.UtcNow;
            }
        }
    }
}
