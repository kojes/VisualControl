using System.Linq;
using VueExample.Contexts;
using VueExample.Models;
using VueExample.Repository;

namespace VueExample.Providers
{
    public class CodeProductProvider : RepositorySRV6<CodeProduct>
    {
        public CodeProduct GetByWaferId(string waferId)
        {
            using (var applicationContext = new Srv6Context())
            {
                var wafer = applicationContext.Wafers.FirstOrDefault(x => x.WaferId == waferId);
                return wafer != null ? applicationContext.CodeProducts.Find(wafer.CodeProductId) : null;
            }
        }
    }
}
