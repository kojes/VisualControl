using System;
using System.Collections.Generic;

namespace VueExample.ViewModels
{
    public class ShortLinkInfoViewModel
    {
        public Guid GeneratedId { get; set; }
        public int MeasurementRecordingId { get; set; }
        public string WaferId { get; set; }
        public List<string> StatisticNameList { get; set; }
    }
}