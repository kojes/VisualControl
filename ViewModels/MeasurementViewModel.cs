namespace VueExample.ViewModels
{
    public class MeasurementViewModel
    {
        public int MeasurementId { get; set; }
        
        public string Name { get; set; }
        public int MeasuredDeviceId { get; set; }
        public int FacilityId { get; set; }
        public int IntervalInSeconds {get; set;}
    }
}