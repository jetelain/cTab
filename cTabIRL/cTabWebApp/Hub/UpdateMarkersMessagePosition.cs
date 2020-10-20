using System;
using System.Collections.Generic;

namespace cTabWebApp
{
    internal class UpdateMarkersMessagePosition
    {
        public UpdateMarkersMessagePosition()
        {
        }

        public DateTime Timestamp { get; set; }
        public List<MarkerPosition> Makers { get; set; }
    }
}