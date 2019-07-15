using System;

namespace VueExample.ViewModels
{
    public class PointViewModel
    {    
        public Int64 PointId { get; set; }
        public int PortNumber { get; set; }       
        public int GraphicId { get; set; }
        public int DeviceId { get; set; }
        public int MeasurementId { get; set; }
        public DateTime Time { get; set; }
        public string Value { get; set; }
      
    }
}