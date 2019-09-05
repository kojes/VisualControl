
using System.Collections.Generic;
using System.Linq;
using VueExample.Models.SRV6;
using VueExample.Providers.Srv6;
using VueExample.StatisticsCore.DataModels;

namespace VueExample.StatisticsCore.SingleStatisticServices
{
    public class SingleParameterServiceLNR : VueExample.StatisticsCore.SingleStatisticServices.Abstract.SingleStatisticsServiceAbstract
    {
        public override List<SingleParameterStatistic> CreateSingleParameterStatisticsList(List<DieValue> dieValues, Graphic graphic, int? stageId, double divider)
        {
            var statisticsItem = new Statistics();
            var dieIdList = new List<long?>();             
            var singleParameterStatisticsList = new List<SingleParameterStatistic>();
            var commonYList = new List<List<string>>();

            foreach (var gdv in dieValues) 
            {
                commonYList.Add(gdv.YList);
                dieIdList.Add(gdv.DieId);
            }

            foreach (var stat in statisticsItem.GetStatistics(dieValues.FirstOrDefault().XList, commonYList, graphic, divider)) 
            {
                singleParameterStatisticsList.Add(new SingleParameterStatistic(stat.StatisticsName, dieIdList, stat.FullList).CalculateDirtyCellsFixed(StatParameterService.GetByStatParameterIdAndStageId(stat.ParameterID, stageId)));
            }

            return singleParameterStatisticsList;
        }

        public override List<SingleStatisticData> CreateSingleStatisticData(List<long?> dieList, Graphic graphic, List<DieValue> dieValuesList, double divider, List<VueExample.StatisticsCore.SingleParameterStatistic> singleParameterStatisticsList)
        {
            var statisticsItem = new Statistics();
            var selectedDieList = new List<long?> ();
            var xList = dieValuesList.FirstOrDefault ().XList;
            var commonYList = new List<List<string>> ();
            foreach (var dieValue in dieValuesList.Where (d => dieList.Contains (d.DieId))) {
                commonYList.Add (dieValue.YList);
                selectedDieList.Add(dieValue.DieId);
            }
            var statistics = statisticsItem.GetStatistics (xList, commonYList, graphic, divider);
            var singleStatisticDataList = StatisticDataMapping (statistics, selectedDieList, singleParameterStatisticsList);
            return singleStatisticDataList;
        }
    }
}