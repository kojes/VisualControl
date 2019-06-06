using System.Linq;
using System.Collections.Generic;
using VueExample.Models.SRV6;
using VueExample.StatisticsCore.DataModels;

namespace VueExample.StatisticsCore.SingleStatisticServices
{
    public class SingleParameterServiceHSTG : VueExample.StatisticsCore.SingleStatisticServices.Abstract.SingleStatisticsServiceAbstract
    {
        public override List<SingleParameterStatistic> CreateSingleParameterStatisticsList(List<DieValue> dieValues, Graphic graphic, int? stageId, double divider)
        {
                var statisticsItem = new Statistics();
                var dieIdList = new List<long?> ();
                var valueList = new List<string> ();
                var singleParameterStatisticsList = new List<SingleParameterStatistic> ();
                foreach (var gdv in dieValues) {
                    
                    dieIdList.Add (gdv.DieId);
                    valueList.Add (gdv.YList.FirstOrDefault());
                }

                foreach (var stat in statisticsItem.GetStatistics(valueList, graphic)) {
                    singleParameterStatisticsList.Add (new SingleParameterStatistic (stat.StatisticsName, dieIdList, stat.FullList).CalculateDirtyCellsFixed(StatParameterService.GetByStatParameterIdAndStageId(stat.ParameterID, stageId)));
                }

                return singleParameterStatisticsList;
        }

        public override List<SingleStatisticData> CreateSingleStatisticData(List<long?> dieList, Graphic graphic, List<DieValue> dieValuesList, double divider, List<VueExample.StatisticsCore.SingleParameterStatistic> singleParameterStatisticsList)
        {
            var statisticsItem = new Statistics();
            var selectedDieList = new List<long?> ();
            var valueList = new List<string> ();
         
            foreach (var dieValue in dieValuesList.Where (d => dieList.Contains (d.DieId))) {
                valueList.Add (dieValue.YList.FirstOrDefault());
                selectedDieList.Add(dieValue.DieId);
            }
            var statistics = statisticsItem.GetStatistics(valueList, graphic);
            var singleStatisticDataList = StatisticDataMapping (statistics, selectedDieList, singleParameterStatisticsList);
            return singleStatisticDataList;
        }
    }
}