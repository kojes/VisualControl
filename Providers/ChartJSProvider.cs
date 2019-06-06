using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using VueExample.ChartModels.ChartJs;
using VueExample.ChartModels.ChartJs.Bar;
using VueExample.ChartModels.ChartJs.Linear;
using VueExample.Color;
using VueExample.Models.SRV6;

namespace VueExample.Providers {
    public class ChartJSProvider : IChartJSProvider {

        private IDieProvider _dieProvider;
        private IColorService _colorService;

        public ChartJSProvider (IDieProvider dieProvider, IColorService colorService) {
            this._dieProvider = dieProvider;
            this._colorService = colorService;
        }

        public AbstractChart GetLinearFromDieValues (List<DieValue> dieValuesList, List<long?> dieIdList, double divider) {

            var datasetList = new ConcurrentBag<Dataset> ();
            var currentDieValues = dieValuesList.Where (x => dieIdList.Contains (x.DieId)).ToList ();

            Parallel.ForEach (currentDieValues, (dieValue) => {

                var dataset = new Dataset ();
                dataset.BorderColor = _colorService.GetHexColorByDieId (dieValue.DieId);
                for (int i = 0; i < dieValue.XList.Count; i++) {
                    if (i < dieValue.YList.Count) {
                        dataset.Data.Add (double.Parse (dieValue.YList[i], CultureInfo.InvariantCulture) / divider);
                    }

                }
                datasetList.Add (dataset);

            });
            var labelsList = new List<string> ();
            var chart = new LinearChart (labelsList, datasetList);
            labelsList.AddRange (dieValuesList.FirstOrDefault ().XList);
            return chart;
        }

        public AbstractChart GetHistogramFromDieValues (List<DieValue> dieValuesList, List<long?> dieIdList, double divider) {

            var labelsList = new List<string> ();
            var datasetList = new List<Dataset> ();
            var chart = new BarChart (labelsList, datasetList);
            var dataset = new Dataset ();
            foreach (var dieValue in dieValuesList.Where (x => dieIdList.Contains (x.DieId)).ToList ()) {

                labelsList.Add (Convert.ToString (_dieProvider.GetById ((long) dieValue.DieId).Code));
                dataset.BackgroundColor = _colorService.GetHexColorByDieId (dieValue.DieId);
                dataset.Data.Add (double.Parse (dieValue.YList[0], CultureInfo.InvariantCulture) / divider);

            }
            datasetList.Add (dataset);

            return chart;
        }
    }
}