using System.Collections.Generic;
using VueExample.Models;

namespace VueExample.Providers
{
    public interface IDieProvider
    {
        List<Die> GetDiesByWaferId(string waferId);
    }
}
