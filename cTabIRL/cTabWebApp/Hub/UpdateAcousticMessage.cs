using System;
using System.Collections.Generic;

namespace cTabWebApp
{
    public class UpdateAcousticMessage
    {
        public DateTime Timestamp { get; set; }
        public List<DetectedShot> Shots { get; set; }
        public double GameTime { get; set; }
    }
}