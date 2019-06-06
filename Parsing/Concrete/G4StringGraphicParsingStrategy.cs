using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VueExample.Entities;
using VueExample.Models.SRV6;
using VueExample.Parsing.Strategies;

namespace VueExample.Parsing.Concrete
{
    public class G4StringGraphicParsingStrategy : IStringGraphicSRV6ParsingStrategy
    {
        public Dictionary<string, DieValue> ParseStringGraphic(DieGraphics dieGraphics)
        {
            throw new NotImplementedException();
        }
    }
}
