using System;

namespace VueExample.ViewModels {
    public class AtomicMeasurementViewModel 
    {
        public Guid MeasurementSetId { get; set; }
        public int MeasurementId { get; set; }
        public int GraphicId { get; set; }
        public int DeviceId { get; set; }        
        public int PortNumber { get; set; }
    }
}