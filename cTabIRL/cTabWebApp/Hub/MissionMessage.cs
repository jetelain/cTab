using System;

namespace cTabWebApp
{
    internal class MissionMessage : RecordableMessageBase
    {
        public string WorldName { get; set; }
        public double Size { get; set; }
        public DateTime Date { get; set; }
        public int CtabFeatureLevel { get; set; }
        public int IrlFeatureLevel { get; set; }
    }
}