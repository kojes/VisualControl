using System;
using System.Collections.Generic;
using VueExample.Models.SRV6;
using VueExample.Providers.Srv6;

namespace VueExample.StatisticsCore.Services
{
    public class StatisticService 
    {
        GraphicService graphicService = new GraphicService();
        
        public Dictionary<string, List<SingleParameterStatistic>> GetSingleParameterStatisticByDieValues(Dictionary<string, List<DieValue>> dieValues, int? stageId, double divider) {
           
            var statisticsDictionary = new Dictionary<string, List<SingleParameterStatistic>> ();
            foreach (var graphicDV in dieValues) 
            {
             
                var graphic = graphicService.GetById(Convert.ToInt32(graphicDV.Key.Split('_')[0]));
                var singleParameterStatisticsList = SingleStatisticsServiceCreator(graphic).CreateSingleParameterStatisticsList(graphicDV.Value, graphic, stageId, divider);
                statisticsDictionary.Add (graphicDV.Key, singleParameterStatisticsList);
            }

            return statisticsDictionary;
        }

        public List<VueExample.StatisticsCore.DataModels.SingleStatisticData> GetStatisticsDataByGraphicState (List<long?> dieList, string keyGraphicState, List<DieValue> dieValuesList, double divider, List<VueExample.StatisticsCore.SingleParameterStatistic> singleParameterStatisticsList) 
        {
            var graphicId = Convert.ToInt32(keyGraphicState.Split('_')[0]);
            var graphic = graphicService.GetById (graphicId);                          
            return SingleStatisticsServiceCreator(graphic).CreateSingleStatisticData(dieList, graphic, dieValuesList, divider, singleParameterStatisticsList);
        }

        public DirtyCells GetDirtyCellsBySPSDictionary (Dictionary<string, List<SingleParameterStatistic>> singleParameterStatistics) {
            var dirtyCells = new DirtyCells ();
            foreach (var item in singleParameterStatistics) {
                foreach (var singleParameterStatistic in item.Value) {
                    dirtyCells.StatList.AddRange (singleParameterStatistic.DirtyCells.StatList);
                    dirtyCells.FixedList.AddRange (singleParameterStatistic.DirtyCells.FixedList);
                }
            }

            return dirtyCells.Distinct();

        }

       private SingleStatisticServices.Abstract.SingleStatisticsServiceAbstract SingleStatisticsServiceCreator(Graphic graphic)
       {
            switch (graphic.Type)
            {
                case 1:
                    return new SingleStatisticServices.SingleParameterServiceLNR();
                case 2:
                    return new SingleStatisticServices.SingleParameterServiceHSTG();
            }

            return new SingleStatisticServices.SingleParameterServiceLNR();;

       }

    }
}