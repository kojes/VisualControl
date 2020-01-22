using System.Globalization;
using System;
using System.Collections.Generic;
using LazyCache;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VueExample.Models.SRV6;
using VueExample.Services;
using VueExample.Providers.Srv6;
using System.Threading.Tasks;
using VueExample.Providers.Srv6.Interfaces;

namespace VueExample.Controllers
{
    [Route ("api/[controller]/[action]")]
    public class StatisticController : Controller 
    {
        private readonly IAppCache cache;
        private readonly DieValueService dieValueService = new DieValueService();
        private readonly IStageProvider _stageProvider;
        private readonly StatisticsCore.Services.StatisticService statisticService = new StatisticsCore.Services.StatisticService();
        public StatisticController (IAppCache cache, IStageProvider stageProvider) 
        {
            _stageProvider = stageProvider;
            this.cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> GetDirtyCellsByMeasurementRecording (int measurementRecordingId) 
        {
            string measurementRecordingIdAsKey = Convert.ToString (measurementRecordingId);
            var stageId = (await _stageProvider.GetByMeasurementRecordingId(measurementRecordingId)).StageId;
            Func<Dictionary<string, List<DieValue>>> cachedDieValueService = () => dieValueService.GetDieValuesByMeasurementRecording(measurementRecordingId);
            var dieValuesDictionary = cache.GetOrAdd($"V_{measurementRecordingIdAsKey}", cachedDieValueService);
            Func<Dictionary<string, List<VueExample.StatisticsCore.SingleParameterStatistic>>> cachedStatisticService = () => statisticService.GetSingleParameterStatisticByDieValues(dieValuesDictionary, stageId, 1.0);
            var statDictionary = cache.GetOrAdd($"S_{measurementRecordingIdAsKey}", cachedStatisticService);
            return Ok(statisticService.GetDirtyCellsBySPSDictionary(statDictionary));
        }

        [HttpGet]
        public IActionResult GetStatisticSingleGraphic(string statisticSingleGraphicViewModelJSON)
        {
            var statisticSingleGraphicViewModel = JsonConvert.DeserializeObject<VueExample.ViewModels.StatisticSingleGraphicViewModel>(statisticSingleGraphicViewModelJSON);
            string measurementRecordingIdAsKey = Convert.ToString(statisticSingleGraphicViewModel.MeasurementId);
            string keyGraphic = statisticSingleGraphicViewModel.KeyGraphicState;
            var statistics = new StatisticsCore.Statistics();
            var dieValueList = cache.Get<Dictionary<string, List<DieValue>>>($"V_{measurementRecordingIdAsKey}")[keyGraphic];
            var singleParameterStatisticList = cache.Get<Dictionary<string, List<VueExample.StatisticsCore.SingleParameterStatistic>>>($"S_{measurementRecordingIdAsKey}")[keyGraphic];
            var statisticDataList = statisticService.GetStatisticsDataByGraphicState(statisticSingleGraphicViewModel.dieIdList, statisticSingleGraphicViewModel.KeyGraphicState, dieValueList, double.Parse(statisticSingleGraphicViewModel.Divider, CultureInfo.InvariantCulture), singleParameterStatisticList);
            return Ok(statisticDataList);
        }
        
        [HttpGet]
        public IActionResult GetDirtyCellsSingleGraphic(string statisticSingleGraphicViewModelJSON)
        {
            var statisticSingleGraphicViewModel = JsonConvert.DeserializeObject<VueExample.ViewModels.StatisticSingleGraphicViewModel>(statisticSingleGraphicViewModelJSON);
            string measurementRecordingIdAsKey = Convert.ToString(statisticSingleGraphicViewModel.MeasurementId);
            string keyGraphic = statisticSingleGraphicViewModel.KeyGraphicState;
            var statistics = new StatisticsCore.Statistics();
            var statDictionary = cache.Get<Dictionary<string, List<VueExample.StatisticsCore.SingleParameterStatistic>>>($"S_{measurementRecordingIdAsKey}")[keyGraphic];
            return Ok(statDictionary);
        }
    }
}