using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VueExample.ChartModels.AmChart;
using VueExample.ChartModels.AmChart.Linear;
using VueExample.Models;
using VueExample.Models.SRV6;

namespace VueExample.Providers
{
    public class AmChartProvider
    {

      
        public AmChart3DPieChart GetBadGoodAmChart3DPieChart(List<Defect> defects, IEnumerable<Die> dies)
        {
            var amChart3DPieChart = new AmChart3DPieChart();
            var diesList = dies.ToList();
            amChart3DPieChart.Title = "BadGood";
            var badDiesCount = defects.Where(x => x.DangerLevelId == 1).Select(x => x.DieId).Distinct().Count();
            amChart3DPieChart.Data.Add(new _3DPieChart{V = diesList.Count - badDiesCount, Ctg = "Годные" , AmChartConfig = new AmChartConfig{Fill = "#1e5938"}});
            amChart3DPieChart.Data.Add(new _3DPieChart { V = badDiesCount, Ctg = "Брак", AmChartConfig = new AmChartConfig{Fill = "#c94c54"}});
            return amChart3DPieChart;
        }

        public AmChart<Series> GetLinearFromDieValues(List<DieValue> dieValuesList, List<long?> dieIdList)
        {
            var amchart = new LinearChart();
            foreach (var dieValue in dieValuesList.Where(x => dieIdList.Contains(x.DieId)).ToList())
            {
                var series = new Series();
                for (int i = 0; i < dieValue.XList.Count; i++)
                {
                    var point = new VueExample.ChartModels.AmChart.Linear.Point(dieValue.XList[i], dieValue.YList[i]);
                    series.pointList.Add(point);
                }
                amchart.Data.Add(series);
            }
            return amchart;
        }
    }
}
