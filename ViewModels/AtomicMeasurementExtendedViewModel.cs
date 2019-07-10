using System;
namespace VueExample.ViewModels
{
    public class AtomicMeasurementExtendedViewModel
    {
        public int AtomicMeasurementId { get; set; }
        public Guid MeasurementSetId { get; set; }
        public int MeasurementId { get; set; }
        public string MeasurementName {get; set; }
        public int GraphicId { get; set; }
        public string GraphicName {get; set; }
        public int DeviceId { get; set; }
        public string DeviceName {get; set; }         
        public int PortNumber { get; set; }
        public bool IsOnline { get; set; }
      
    }
}