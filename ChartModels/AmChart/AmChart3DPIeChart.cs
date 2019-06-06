using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VueExample.ChartModels.AmChart
{
    public class AmChart3DPieChart : AmChart<_3DPieChart>
    {
        public override List<_3DPieChart> Data { get; set; } = new List<_3DPieChart>();
        
    }

    public class _3DPieChart
    {
        public string Ctg { get; set; }
        public int V { get; set; }
        public AmChartConfig AmChartConfig { get; set; }
    }




}
