using System.Collections.Generic;
namespace VueExample.ViewModels
{
    public class StatisticSingleGraphicViewModel
    {
        public int MeasurementId  { get; set; }
        public string KeyGraphicState { get; set; }
        public List<long?> dieIdList {get; set; }
        public string Divider { get; set; }

        public StatisticSingleGraphicViewModel()
        {
            this.dieIdList = new List<long?>();
        }
    }
}