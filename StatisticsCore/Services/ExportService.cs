using System.Globalization;
using System.Linq;
using System;
using System.Collections.Generic;
using LazyCache;
using VueExample.Models.SRV6;
using VueExample.Services;
using VueExample.StatisticsCore.Abstract;
using VueExample.Providers;
using VueExample.Models.SRV6.Export;
using System.Threading.Tasks;
using VueExample.Providers.Srv6.Interfaces;
using VueExample.Contexts;

namespace VueExample.StatisticsCore.Services
{
    public class ExportService : IExportProvider
    {
        private readonly IAppCache _appCache;        
        private readonly Srv6Context _srv6Context;
        private readonly IDieValueService _dieValueService;
        private readonly IStageProvider _stageProvider;
        private readonly StatisticService _statisticService;
        private readonly IDieProvider _dieProvider;
        public ExportService(IAppCache appCache, Srv6Context srv6Context, IStageProvider stageProvider, IDieValueService dieValueService, IDieProvider dieProvider, ISRV6GraphicService graphicService)
        {
            _dieValueService = dieValueService;
            _srv6Context = srv6Context;
            _dieProvider = dieProvider;
            _appCache = appCache;
            _stageProvider = stageProvider;
            _statisticService = new StatisticService(graphicService, _srv6Context);
        }
        public async Task PopulateKurbatovXLSByValues(KurbatovXLS kurbatovXLS)
        {
            foreach (var kurbatovParameter in kurbatovXLS.kpList)
            {
                await GetDieValues(kurbatovParameter); 
            }
            kurbatovXLS.FindDirty();            
        }

        public async Task<List<string>> GetStatisticsNameByMeasurementId(int measurementRecordingId)
        {
            var statNamesList = new List<string>();
            string measurementRecordingIdAsKey = Convert.ToString(measurementRecordingId);
            var stageId = (await _stageProvider.GetByMeasurementRecordingId(measurementRecordingId)).StageId;
            Func<Dictionary<string, List<DieValue>>> cachedDieValueService = () => _dieValueService.GetDieValuesByMeasurementRecording(measurementRecordingId);
            var dieValuesDictionary = _appCache.GetOrAdd($"V_{measurementRecordingIdAsKey}", cachedDieValueService);
            Func<Dictionary<string, List<VueExample.StatisticsCore.SingleParameterStatistic>>> cachedStatisticService = () => _statisticService.GetSingleParameterStatisticByDieValues(dieValuesDictionary, stageId, 1.0);
            var statDictionary = _appCache.GetOrAdd($"S_{measurementRecordingIdAsKey}", cachedStatisticService);
            statNamesList.AddRange(from stat in statDictionary
                                   from singleParameterStatistic in stat.Value
                                   select singleParameterStatistic.Name);
            return statNamesList;
        }

        

        private async Task GetDieValues(KurbatovParameter kurbatovParameter)
        {
            string measurementRecordingIdAsKey = Convert.ToString(kurbatovParameter.MeasurementRecordingId);
            var stageId = (await _stageProvider.GetByMeasurementRecordingId(kurbatovParameter.MeasurementRecordingId)).StageId;
            Func<Dictionary<string, List<DieValue>>> cachedDieValueService = () => _dieValueService.GetDieValuesByMeasurementRecording(kurbatovParameter.MeasurementRecordingId);
            var dieValuesDictionary = _appCache.GetOrAdd($"V_{measurementRecordingIdAsKey}", cachedDieValueService);
            Func<Dictionary<string, List<VueExample.StatisticsCore.SingleParameterStatistic>>> cachedStatisticService = () => _statisticService.GetSingleParameterStatisticByDieValues(dieValuesDictionary, stageId, 1.0);
            var statDictionary = _appCache.GetOrAdd($"S_{measurementRecordingIdAsKey}", cachedStatisticService);
            foreach (var stat in statDictionary) 
            {
                foreach (var singleParameterStatistic in stat.Value) 
                {
                    if(kurbatovParameter.ParameterNameStat == singleParameterStatistic.Name)
                    {
                        for (int i = 0; i < singleParameterStatistic.dieList.Count; i++)
                        {
                            
                            long? die = (long?)singleParameterStatistic.dieList[i];
                            var value = singleParameterStatistic.valueList[i] / kurbatovParameter.Divider;
                            kurbatovParameter.advList.Add
                            (
                                new AtomicDieValue
                                {
                                    DieCode = Convert.ToInt32((await _dieProvider.GetById((long)die)).Code), 
                                    Value = value, 
                                    Status = GetStatus(kurbatovParameter.Lower, kurbatovParameter.Upper, value)
                                }
                            );

                           kurbatovParameter.advList = kurbatovParameter.advList.OrderBy(_ => _.DieCode).ToList();                           
                        }
                    }
                    
                }
            }
           
        }

        private string GetStatus(double lower, double upper, double value)
        {
            if((value >= lower || Double.IsNaN(lower)) && (value <= upper || Double.IsNaN(upper)))
            {
                return "Годен";
            }
            else
            {
                return "Брак";
            }
        }

        public async Task<List<Dictionary<string, string>>> Export(int measurementRecordingId, string statNames, string delimeter)
        {
            string measurementRecordingIdAsKey = Convert.ToString(measurementRecordingId);
            var statNamesList = statNames.Split(delimeter).ToList();
            var exportList = new List<Dictionary<string, string>>();
            var stageId = (await _stageProvider.GetByMeasurementRecordingId(measurementRecordingId)).StageId;
            Func<Dictionary<string, List<DieValue>>> cachedDieValueService = () => _dieValueService.GetDieValuesByMeasurementRecording(measurementRecordingId);
            var dieValuesDictionary = _appCache.GetOrAdd($"V_{measurementRecordingIdAsKey}", cachedDieValueService);
            Func<Dictionary<string, List<VueExample.StatisticsCore.SingleParameterStatistic>>> cachedStatisticService = () => _statisticService.GetSingleParameterStatisticByDieValues(dieValuesDictionary, stageId, 1.0);
            var statDictionary = _appCache.GetOrAdd($"S_{measurementRecordingIdAsKey}", cachedStatisticService);
            foreach (var stat in statDictionary) 
            {
                foreach (var singleParameterStatistic in stat.Value) 
                {
                    if(statNamesList.Contains(singleParameterStatistic.Name))
                    {
                        
                        var d = new Dictionary<string, string>();
                        var dieList = new List<int>();
                        d["name"] = singleParameterStatistic.Name;
                      
                        for (int i = 0; i < singleParameterStatistic.dieList.Count; i++)
                        {
                            long? die = (long?)singleParameterStatistic.dieList[i];
                            dieList.Add(Convert.ToInt32((await _dieProvider.GetById((long)die)).Code));
                        }
                        for (int i = 0; i < dieList.Count; i++)
                        {
                            d["#" + Convert.ToString(i+1)] = Convert.ToString(singleParameterStatistic.valueList[i], CultureInfo.InvariantCulture);
                        }
                        
                        exportList.Add(d);                    
                    }
                 
                }
            }
            return exportList;
        }

    
    }
}