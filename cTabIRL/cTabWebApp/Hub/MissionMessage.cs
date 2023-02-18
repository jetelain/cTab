using System;

namespace cTabWebApp
{
    internal class MissionMessage
    {
        public string WorldName { get; set; }
        public double Size { get; set; }
        public DateTime Date { get; set; }
        public DateTime Timestamp { get; set; }
        public string ServerName { get; set; }
        public string MissionName { get; set; }
        public string CtabSessionId { get; set; }
    }
}