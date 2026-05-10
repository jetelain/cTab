using System;
using System.Collections.Generic;

namespace cTabWebApp
{
    internal class UpdateMarkersMessagePosition : RecordableMessageBase
    {
        public List<MarkerPosition> Makers { get; set; }
    }
}