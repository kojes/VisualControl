using System;
using VueExample.ChartModels.ChartJs.Linear;
using VueExample.ChartModels.ChartJs.Options;

namespace VueExample.ChartModels.ChartJs.Abstract
{
    public abstract class Options
    {
        public bool Responsive { get; set; }
        public int ResponsiveAnimationDuration { get; set; }
        public ChartJs.Options.Animation Animation { get; set; }
        public ChartJs.Options.Hover Hover { get; set; }

        public Options()
        {
            Responsive = false;
            ResponsiveAnimationDuration = 0;
            Animation = new ChartJs.Options.Animation();
            Hover = new ChartJs.Options.Hover();
            
        }

      
    }
}