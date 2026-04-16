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

        public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Icon, Dir, Label);

#nullable disable
    }
}