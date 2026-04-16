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
            return (Simples?.SequenceEqual(other.Simples) ?? other.Simples is null)
                && (Polylines?.SequenceEqual(other.Polylines) ?? other.Polylines is null)
                && (Icons?.SequenceEqual(other.Icons) ?? other.Icons is null);
        }

        public override bool Equals(object? obj) => Equals(obj as UpdateMapMarkersMessage);

        public override int GetHashCode() => HashCode.Combine(Simples?.Count, Polylines?.Count, Icons?.Count);
#nullable disable
    }
}
