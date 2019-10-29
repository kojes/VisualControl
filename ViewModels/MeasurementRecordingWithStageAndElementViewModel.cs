using System.Collections.Generic;

namespace VueExample.ViewModels
{
    public class MeasurementRecordingWithStageAndElementViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ElementViewModel> ElementList { get; set; }
        public StageViewModel Stage { get; set; }
    }
}