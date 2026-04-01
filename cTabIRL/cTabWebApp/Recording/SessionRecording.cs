using System;
using System.Collections.Generic;

namespace cTabWebApp.Recording
{
    public class SessionRecording
    {
        public int Version { get; set; } = 1;
        public string WorldName { get; set; }
        public DateTime RecordingStart { get; set; }
        public DateTime RecordingEnd { get; set; }
        public List<SessionEvent> Events { get; set; } = new List<SessionEvent>();
    }
}
