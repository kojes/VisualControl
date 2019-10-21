using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace VueExample.Models.SRV6.Export
{
    public class KurbatovParameter
    {
        [JsonProperty(Order = -3)]
        public string ParameterName { get; set; }
        [JsonProperty(Order = -3)]
        public string RussianParameterName { get; set; }
        public string ParameterNameStat { get; set; }
        [JsonProperty(Order = -2, NullValueHandling=NullValueHandling.Ignore)]
        public double Lower { get; set; } = Double.NaN;
        [JsonProperty(Order = -2, NullValueHandling=NullValueHandling.Ignore)]
        public double Upper { get; set; } = Double.NaN;
        public double Divider { get; set; } = 1.0;
        public int DividerId { get; set; }
        public double AverageGood {get; set;}
        public int MeasurementRecordingId { get; set; }
        [JsonProperty(Order = -1)]
        public List<AtomicDieValue> advList = new List<AtomicDieValue>();          

        public void FindGoodAverage(HashSet<int> dirtyCodesList)
        {
            var averageGood = 0.0;
            foreach (var adv in advList)
            {
                if(!dirtyCodesList.Contains(adv.DieCode))
                {
                    averageGood += adv.Value;
                }
            }
            this.AverageGood = averageGood / (advList.Count - dirtyCodesList.Count);
        }

    }
}