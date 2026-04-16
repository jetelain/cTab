using System;
using System.Linq;

namespace cTabWebApp
{
    public class IconMapMarker : MapMarkerBase, IEquatable<IconMapMarker>
    {
        public double[] Pos { get; internal set; }
        public string Icon { get; internal set; }
        public double[] Size { get; internal set; }
        public double Dir { get; internal set; }
        public string Label { get; internal set; }

#nullable enable
        public bool Equals(IconMapMarker? other)
        {
            if (other is null) 
            { 
                return false; 
            }

            return Name == other.Name
                && Alpha == other.Alpha
                && Icon == other.Icon
                && Dir == other.Dir
                && Label == other.Label
                && (Pos?.SequenceEqual(other.Pos) ?? other.Pos is null)
                && (Size?.SequenceEqual(other.Size) ?? other.Size is null);
        }

        public override bool Equals(object? obj) => Equals(obj as IconMapMarker);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Name);
            hash.Add(Alpha);
            hash.Add(Icon);
            hash.Add(Dir);
            hash.Add(Label);

            if (Pos != null)
            {
                foreach (var pos in Pos)
                {
                    hash.Add(pos);
                }
            }

            if (Size != null)
            {
                foreach (var size in Size)
                {
                    hash.Add(size);
                }
            }

            return hash.ToHashCode();
        }
#nullable disable
    }
}