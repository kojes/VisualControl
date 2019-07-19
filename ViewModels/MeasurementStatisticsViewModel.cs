namespace VueExample.ViewModels
{
    public class MeasurementStatisticsViewModel
    {
        public int MeasurementId { get; set; }
        public int GraphicId { get; set; }
        public int DeviceId { get; set; }        
        public int PortNumber { get; set; }
        public string MeasurementName { get; set; }      
        public string GraphicUnit {get; set; }
        public string DeviceName {get; set; }         
        public string Minimum { get; set; }
        public string Maximum { get; set; }
        public string FirstValue { get; set; }
        public string LastValue { get; set; }
        public string DailyDifference { get; set; }
        
        public string CommonDifference { get; set; }
    }
}