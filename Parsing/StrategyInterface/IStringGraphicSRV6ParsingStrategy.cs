using System.Collections.Generic;
using VueExample.Entities;
using VueExample.Models.SRV6;

namespace VueExample.Parsing.Strategies
{
    public interface IStringGraphicSRV6ParsingStrategy
    {
         Dictionary<string, DieValue> ParseStringGraphic(DieGraphics dieGraphic);
    }
}
