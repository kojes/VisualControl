using VueExample.ChartModels.ChartJs.Options.ElementsObject;

namespace VueExample.ChartModels.ChartJs.Options
{
    public class Elements
    {
        public VueExample.ChartModels.ChartJs.Options.ElementsObject.Line Line { get; set; }

        public Elements()
        {
            this.Line = new VueExample.ChartModels.ChartJs.Options.ElementsObject.Line();
        }
    }
}