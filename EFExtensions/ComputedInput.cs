using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VueExample.EFExtensions
{
    public class ComputedInput<TSource, TItem>
    {
        public TSource Source { get; set; }
        public TItem Item { get; set; }
    }
}
