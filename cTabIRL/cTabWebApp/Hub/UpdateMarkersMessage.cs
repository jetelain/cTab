using System;
using System.Collections.Generic;

namespace cTabWebApp
{
    public class UpdateMarkersMessage : RecordableMessageBase
    {
        public List<Marker> Makers { get; internal set; }
    }
}