namespace cTabWebApp
{
    public class IconMapMarker : MapMarkerBase
    {
        public double[] Pos { get; internal set; }
        public string Icon { get; internal set; }
        public double[] Size { get; internal set; }
        public double Dir { get; internal set; }
        public string Label { get; internal set; }
    }
}