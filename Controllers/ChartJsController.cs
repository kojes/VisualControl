using System;
using System.Collections.Generic;
using System.Globalization;
using LazyCache;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VueExample.Models.SRV6;
using VueExample.Providers;

namespace VueExample.Controllers
{
     [Route("api/[controller]/[action]")]
    public class ChartJsController : Controller
    {        
        private readonly IAppCache cache;
        private readonly IChartJSProvider _chartJSProvider;

        public ChartJsController(IChartJSProvider chartJSProvider, IAppCache cache)
        {
            this._chartJSProvider = chartJSProvider;
            this.cache = cache;
        }

        [HttpGet]
        public IActionResult GetLinearForMeasurement(string statisticSingleGraphicViewModelJSON)
        {
            var statisticSingleGraphicViewModel = JsonConvert.DeserializeObject<VueExample.ViewModels.StatisticSingleGraphicViewModel>(statisticSingleGraphicViewModelJSON);
            string measurementRecordingIdAsKey = Convert.ToString(statisticSingleGraphicViewModel.MeasurementId);
            string keyGraphic = statisticSingleGraphicViewModel.KeyGraphicState;
            var statistics = new StatisticsCore.Statistics();
            var dieValueList = cache.Get<Dictionary<string, List<DieValue>>>($"V_{measurementRecordingIdAsKey}")[keyGraphic];
            var amchart = _chartJSProvider.GetLinearFromDieValues(dieValueList, statisticSingleGraphicViewModel.dieIdList, double.Parse(statisticSingleGraphicViewModel.Divider, CultureInfo.InvariantCulture));
            return Ok(amchart);
        }

        [HttpGet]
        public IActionResult GetHistogramForMeasurement(string statisticSingleGraphicViewModelJSON)
        {
            var statisticSingleGraphicViewModel = JsonConvert.DeserializeObject<VueExample.ViewModels.StatisticSingleGraphicViewModel>(statisticSingleGraphicViewModelJSON);
            string measurementRecordingIdAsKey = Convert.ToString(statisticSingleGraphicViewModel.MeasurementId);
            string keyGraphic = statisticSingleGraphicViewModel.KeyGraphicState;
            var statistics = new StatisticsCore.Statistics();
            var dieValueList = cache.Get<Dictionary<string, List<DieValue>>>($"V_{measurementRecordingIdAsKey}")[keyGraphic];
            var amchart = _chartJSProvider.GetHistogramFromDieValues(dieValueList, statisticSingleGraphicViewModel.dieIdList, double.Parse(statisticSingleGraphicViewModel.Divider, CultureInfo.InvariantCulture));
            return Ok(amchart);
        }

    }
}