namespace cTabWebApp.Models
{
    public class MenuItem
    {
        public string Label { get; set; }
        public string Tooltip { get; set; }
        public int? NextMenu { get; set; }
        public int? Select1 { get; set; }
        public int? Select2 { get; set; }
        public int? Select3 { get; set; }
    }
}