using System;
using System.Linq;

namespace cTabWebApp
{
    public class SimpleMapMarker : MapMarkerBase, IEquatable<SimpleMapMarker>
    {
        public double Dir { get; set; }
        public double[] Size { get; set; }
        public string Shape { get; set; }
        public double[] Pos { get; set; }
        public string Brush { get; set; }
        public string Color { get; set; }

#nullable enable
        public bool Equals(SimpleMapMarker? other)
        {
            if (other is null) 
            {
                return false; 
            }
            return Name == other.Name
                && Alpha == other.Alpha
                && Dir == other.Dir
                && Shape == other.Shape
                && Brush == other.Brush
                && Color == other.Color
                && (Size?.SequenceEqual(other.Size) ?? other.Size is null)
                && (Pos?.SequenceEqual(other.Pos) ?? other.Pos is null);
        }

        public override bool Equals(object? obj) => Equals(obj as SimpleMapMarker);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Name);
            hash.Add(Alpha);
            hash.Add(Dir);
            hash.Add(Shape);
            hash.Add(Brush);
            hash.Add(Color);

            if (Size is not null)
            {
                foreach (var value in Size)
                {
                    hash.Add(value);
                }
            }

            if (Pos is not null)
            {
                foreach (var value in Pos)
                {
                    hash.Add(value);
                }
            }

            return hash.ToHashCode();
        }
#nullable disable
    }
}