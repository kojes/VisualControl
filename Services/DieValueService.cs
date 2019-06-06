using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using VueExample.Extensions;
using VueExample.Contexts;
using VueExample.Entities;
using VueExample.Models.SRV6;
using VueExample.Parsing.Concrete;
using VueExample.Parsing.Strategies;
using VueExample.Providers.Srv6;
using VueExample.Providers.Srv6.Interfaces;

namespace VueExample.Services
{
    public class DieValueService : IDieValueService
    {
       
        private readonly GraphicService _graphicService = new GraphicService();
        public Dictionary<string, List<DieValue>> GetDieValuesByMeasurementRecording(int measurementRecordingId)
        {
            var dieGraphicsList = new List<DieGraphics>();
            using (var srv6Context = new Srv6Context())
            {
                dieGraphicsList.AddRange(srv6Context.DieGraphics.Where(x => x.MeasurementRecordingId == measurementRecordingId).ToList());
            }

            var dgDictionary = dieGraphicsList.GroupBy(x => x.GraphicId, x => x).ToDictionary(x => x.Key, x => x.ToList());
            return DieGraphicsMappingParallel(dgDictionary).ToDictionary(entry => entry.Key, entry => entry.Value);;
        }

        public List<long?> GetSelectedDiesByMeasurementRecordingId(int measurementRecordingId)
        {
            var diesList = new List<long?>();
            using (var srv6Context = new Srv6Context())
            {
                return srv6Context.DiesParameterOld.Where( x => x.MeasurementRecordingId == measurementRecordingId).Select(x => x.DieId).ToList();
            }
        }

        private ConcurrentDictionary<string, List<DieValue>> DieGraphicsMappingParallel(Dictionary<int, List<DieGraphics>> dieGraphicsDictionary)
        {
            var dieValueDictionary = new ConcurrentDictionary<string, List<DieValue>>();
            Object lockObj = new Object();    
            Parallel.ForEach(dieGraphicsDictionary, (dieGraphicList) =>
            {
                
                Parallel.ForEach(dieGraphicList.Value, (dieGraphic) =>
                {
                    lock (lockObj)
                    {
                        var afterParseDictionary = SelectGraphicSrv6ParsingStrategy(dieGraphic.GraphicId).ParseStringGraphic(dieGraphic);
                        foreach (var item in afterParseDictionary)
                        {
                            dieValueDictionary.TryAdd(item.Key, new List<DieValue>());
                            dieValueDictionary[item.Key].Add(item.Value);
                            
                        }
                    }

                   
                });
               
               
            });

           return dieValueDictionary;
        }

        private IStringGraphicSRV6ParsingStrategy SelectGraphicSrv6ParsingStrategy(int graphicId)
        {
            var type = _graphicService.GetById(graphicId).Type;
            IStringGraphicSRV6ParsingStrategy stringGraphicSrv6ParsingStrategy = new CommonLinearStringGraphicParsingStrategy();
            if (type is 2)
                stringGraphicSrv6ParsingStrategy = new HistogramStringGraphicParsingStrategy();
            else if (type is 4) stringGraphicSrv6ParsingStrategy = new G4StringGraphicParsingStrategy();
            return stringGraphicSrv6ParsingStrategy;
        }
    }
}
