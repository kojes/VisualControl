using System.Collections.Generic;
using VueExample.ChartModels.ChartJs;
using VueExample.Models.SRV6;

namespace VueExample.Providers
{
    public interface IChartJSProvider
    {
         AbstractChart GetLinearFromDieValues (List<DieValue> dieValuesList, List<long?> dieIdList, double divider);
         AbstractChart GetHistogramFromDieValues (List<DieValue> dieValuesList, List<long?> dieIdList, double divider);

    }
}