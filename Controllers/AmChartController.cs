using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LazyCache;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VueExample.Models.SRV6;
using VueExample.Providers;
using VueExample.ViewModels;

namespace VueExample.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AmChartController : Controller
    {
        private readonly IDieProvider _dieProvider;
        private readonly IDefectProvider _defectProvider;

        private readonly AmChartProvider amChartProvider = new AmChartProvider();
        private readonly IAppCache cache;
        readonly AmChartProvider _amChartProvider = new AmChartProvider();

        public AmChartController(IDefectProvider defectProvider, IDieProvider dieProvider, IAppCache cache)
        {
            this._defectProvider = defectProvider;
            this._dieProvider = dieProvider;
            this.cache = cache;
        }

        [HttpGet]
        public IActionResult GetBadGood(string waferId)
        {
            var chart3DPieChart = _amChartProvider.GetBadGoodAmChart3DPieChart(_defectProvider.GetByWaferId(waferId),
                _dieProvider.GetDiesByWaferId(waferId));
            return Ok(chart3DPieChart);

        }

        [HttpGet]
        public IActionResult GetLinearForMeasurement(string statisticSingleGraphicViewModelJSON)
        {
            var statisticSingleGraphicViewModel = JsonConvert.DeserializeObject<VueExample.ViewModels.StatisticSingleGraphicViewModel>(statisticSingleGraphicViewModelJSON);
            string measurementRecordingIdAsKey = Convert.ToString(statisticSingleGraphicViewModel.MeasurementId);
            string keyGraphic = statisticSingleGraphicViewModel.KeyGraphicState;
            var statistics = new StatisticsCore.Statistics();
            var dieValueList = cache.Get<Dictionary<string, List<DieValue>>>($"V_{measurementRecordingIdAsKey}")[keyGraphic];
            var amchart = amChartProvider.GetLinearFromDieValues(dieValueList, statisticSingleGraphicViewModel.dieIdList);
            return Ok(amchart);
        }
    }
}
