using System.Collections.Generic;

namespace VueExample.ChartModels.AmChart.Linear
{
    public class LinearChart : AmChart<Series>
    {
        public override List<Series> Data { get; set; }

        public LinearChart()
        {
            this.Data = new List<Series>();
        }
    }
}