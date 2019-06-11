using System;
using System.Collections.Generic;

namespace VueExample.ViewModels
{
    public class PointsViewModel
    {
        public string MeasurementName { get; set; }
        public List<object> PointsList {get; set;} 

        public PointsViewModel()
        {
            PointsList = new List<object>();
        }
    }
}