using System;
using System.Linq;

namespace cTabWebApp
{
    public class PolylineMapMarker : MapMarkerBase, IEquatable<PolylineMapMarker>
    {
        public double[] Points { get; set; }
        public string Brush { get; set; }
        public string Color { get; set; }

#nullable enable
        public bool Equals(PolylineMapMarker? other)
        {
            if (other is null) 
            { 
                return false; 
            }
            return Name == other.Name
                && Alpha == other.Alpha
                && Brush == other.Brush
                && Color == other.Color
                && (Points?.SequenceEqual(other.Points) ?? other.Points is null);
        }

        public override bool Equals(object? obj) => Equals(obj as PolylineMapMarker);

        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Brush, Color);
#nullable disable
    }
}