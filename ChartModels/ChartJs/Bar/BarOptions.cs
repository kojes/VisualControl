using VueExample.ChartModels.ChartJs.Options;

namespace VueExample.ChartModels.ChartJs.Bar
{
    public class BarOptions : VueExample.ChartModels.ChartJs.Abstract.Options
    {
        public BarOptions()
        {
            Legend = new ChartJs.Options.Legend();
    }

        public ChartJs.Options.Legend Legend { get; set; }
        
    }
}