using System.Collections.Generic;

namespace cTabWebApp
{
    public class UpdateMapMarkersMessage
    {
        public List<SimpleMapMarker> Simples { get; internal set; }
        public List<PolylineMapMarker> Polylines { get; internal set; }
        public List<IconMapMarker> Icons { get; internal set; }
    }
}
