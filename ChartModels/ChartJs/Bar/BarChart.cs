using System.Collections.Generic;

namespace VueExample.ChartModels.ChartJs.Bar
{
    public class BarChart : AbstractChart
    {
        public BarChart(List<string> labelsList, List<Dataset> dataSetList)
        {
            this.Options = new BarOptions();
            this.ChartData = new ChartData(labelsList, dataSetList);
        }
    }
}