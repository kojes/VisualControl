using System.Data;
using System.Collections.Generic;
using VueExample.ChartModels.ChartJs.Abstract;

namespace VueExample.ChartModels.ChartJs.Linear
{
    public class LinearChart : AbstractChart
    {
        public LinearChart(List<string> labelsList, IEnumerable<Dataset> dataSetList)
        {
            this.Options = new LinearOptions();
            this.ChartData = new ChartData(labelsList, dataSetList);
        }
    }
}