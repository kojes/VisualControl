using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VueExample.Entities;
using VueExample.Models.SRV6;
using VueExample.Parsing.Strategies;

namespace VueExample.Parsing.Concrete
{
    public class HistogramStringGraphicParsingStrategy : IStringGraphicSRV6ParsingStrategy
    {
        public Dictionary<string, DieValue> ParseStringGraphic(DieGraphics dieGraphic)
        {
            var dieValueDictionary = new Dictionary<string, DieValue>();
            var dieValue = new DieValue();
            dieValue.YList.Add(dieGraphic.ValueString.Split('X')[1]);
            dieValue.State = "HSTG";
            dieValue.GraphicId = dieGraphic.GraphicId;
            dieValue.DieId = dieGraphic.DieId;
            dieValue.MeasurementRecordingId = dieGraphic.MeasurementRecordingId;
            var key = dieValue.KeyGenerate();
            dieValueDictionary.Add(key, dieValue);
            return dieValueDictionary;
        }
    }
}
