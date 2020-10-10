using System;
using System.Collections.Generic;

namespace cTabWebApp
{
    public class UpdateMarkersMessage
    {
        public DateTime Timestamp { get; internal set; }
        public List<Marker> Makers { get; internal set; }
    }
}