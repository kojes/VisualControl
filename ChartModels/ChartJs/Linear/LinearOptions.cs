using VueExample.ChartModels.ChartJs.Options;

namespace VueExample.ChartModels.ChartJs.Linear
{
    public class LinearOptions : VueExample.ChartModels.ChartJs.Abstract.Options
    {
        public LinearOptions()
        {
            Legend = new ChartJs.Options.Legend();
            Tooltips = new ChartJs.Options.Tooltips();
            Elements = new ChartJs.Options.Elements();
        }

        public ChartJs.Options.Legend Legend { get; set; }
        public ChartJs.Options.Tooltips Tooltips { get; set; }
        public ChartJs.Options.Elements Elements { get; set; }
    }
}