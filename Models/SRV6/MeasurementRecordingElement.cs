namespace VueExample.Models.SRV6
{
    public class MeasurementRecordingElement
    {
        public int MeasurementRecordingId { get; set; }
        public int ElementId { get; set; }
        public MeasurementRecording MeasurementRecording { get; set; }
        public Element Element { get; set; }
    }
}