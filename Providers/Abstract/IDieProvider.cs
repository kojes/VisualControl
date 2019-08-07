using System.Collections.Generic;
using VueExample.Models;
using VueExample.Repository;

namespace VueExample.Providers
{
    public interface IDieProvider : IRepository<Die>
    {
        List<Die> GetDiesByWaferId(string waferId);
    }
}
