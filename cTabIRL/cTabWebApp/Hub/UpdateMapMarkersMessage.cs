using System;
using System.Collections.Generic;
using System.Linq;

namespace cTabWebApp
{
    public class UpdateMapMarkersMessage : RecordableMessageBase, IEquatable<UpdateMapMarkersMessage>
    {
        public List<SimpleMapMarker> Simples { get; internal set; }
        public List<PolylineMapMarker> Polylines { get; internal set; }
        public List<IconMapMarker> Icons { get; internal set; }

#nullable enable
        public bool Equals(UpdateMapMarkersMessage? other)
        {
            if (other is null) 
            { 
                return false; 
            }
            return ((Simples is null && other.Simples is null) || (Simples is not null && other.Simples is not null && Simples.SequenceEqual(other.Simples)))
                && ((Polylines is null && other.Polylines is null) || (Polylines is not null && other.Polylines is not null && Polylines.SequenceEqual(other.Polylines)))
                && ((Icons is null && other.Icons is null) || (Icons is not null && other.Icons is not null && Icons.SequenceEqual(other.Icons)));
        }

        public override bool Equals(object? obj) => Equals(obj as UpdateMapMarkersMessage);

        public override int GetHashCode() => HashCode.Combine(Simples?.Count, Polylines?.Count, Icons?.Count);
#nullable disable
    }
}
