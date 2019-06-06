using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.Extensions.DependencyInjection;
using VueExample.Color;
using VueExample.Services;

namespace VueExample.ChartModels.AmChart.Linear {
    public class Series {

        public List<Point> pointList { get; set; }
        public string Color { get; set; }

        public Series () {

            this.pointList = new List<Point> ();

        }

    }
}