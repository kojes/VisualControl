using System.Collections.Generic;
namespace VueExample.ChartModels.ChartJs
{
    public class ChartData
    {
        public List<string> Labels {get; set;}
        public IEnumerable<Dataset> Datasets {get; set;}
              
        public ChartData(List<string> labels, IEnumerable<Dataset> datasets)
        {
            Labels = labels;
            Datasets = datasets;
        }
    }
}