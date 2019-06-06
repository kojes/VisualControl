using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VueExample.ChartModels.AmChart
{
    public abstract class AmChart<T> where T : class 
    {
        public abstract List<T> Data { get; set; }
        public string Title { get; set; }


    }
}
