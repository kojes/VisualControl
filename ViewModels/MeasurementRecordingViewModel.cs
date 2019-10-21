using System.Collections.Generic;

namespace VueExample.ViewModels
{
    public class MeasurementRecordingViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> avStatisticParameters { get; set; } = new List<string>();
    }
}