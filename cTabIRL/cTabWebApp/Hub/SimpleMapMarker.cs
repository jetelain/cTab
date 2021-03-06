namespace cTabWebApp
{
    public class SimpleMapMarker : MapMarkerBase
    {
        public double Dir { get; set; }
        public double[] Size { get; set; }
        public string Shape { get; set; }
        public double[] Pos { get; set; }
        public string Brush { get; set; }
        public string Color { get; set; }
    }
}