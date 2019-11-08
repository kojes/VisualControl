using System.Collections.Generic;

namespace VueExample.ViewModels
{
    public class MeasurementRecordingWithStageAndElementViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ElementViewModel Element { get; set; }
    }

    public class StageFullViewModel 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<MeasurementRecordingWithStageAndElementViewModel> MeasurementRecordingList { get; set; }
    }

    public class StageMeasurementRecordingChunkViewModel 
    {
        public int StageId { get; set; }
        public int MeasurementRecordingId { get; set; }    
    }

    public class ElementMeasurementRecordingChunkViewModel 
    {
        public int ElementId { get; set; }
        public int MeasurementRecordingId { get; set; }    
    }
}  