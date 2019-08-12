using System;
using System.Collections.Generic;

namespace VueExample.ViewModels
{
    public class PointSetViewModel
    {
        public int PortNumber { get; set; }       
        public int DeviceId { get; set; }
        public int MeasurementId { get; set; }
        public List<AtomicPoint> AtomicPointList { get; set; } = new List<AtomicPoint>();
       
    }
    public class AtomicPoint
    {
        public int GraphicId { get; set; }
        public DateTime Time { get; set; }
        public string Value { get; set; }
    }
}