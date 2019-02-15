using System.Linq;
using VueExample.Contexts;
using VueExample.Models;
using VueExample.Repository;

namespace VueExample.Providers
{
    public class WaferMapProvider : RepositorySRV6<WaferMap>, IWaferMapProvider
    {
        public WaferMap GetByWaferId(string waferId)
        {
            using (Srv6Context srv6Context = new Srv6Context())
            {
                return srv6Context.WaferMaps.FirstOrDefault(x=>x.WaferId == waferId);
            }
        }
    }
}
