using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VueExample.Contexts;
using VueExample.Models;
using VueExample.Repository;

namespace VueExample.Providers
{
    public class CodeProductProvider : RepositorySRV6<CodeProduct>
    {
        public async Task<CodeProduct> GetByWaferId(string waferId)
        {
            using (var applicationContext = new Srv6Context())
            {
                var wafer = applicationContext.Wafers.FirstOrDefault(x => x.WaferId == waferId);
                return wafer != null ? await applicationContext.CodeProducts.FindAsync(wafer.CodeProductId) : null;
            }
        }

        public async Task<IList<CodeProduct>> GetByProcessId(int processId) 
        {
            using (var applicationContext = new Srv6Context())
            {
                return await applicationContext.CodeProducts.Where(x => x.ProcessId == processId).ToListAsync();
            }
        }

        public async Task<List<CodeProduct>> GetCodeProductsByDieType(int dieTypeId) 
        {
            using (var applicationContext = new Srv6Context())
            {
                return await applicationContext.DieTypes.Join(applicationContext.DieTypeCodeProducts, c => c.DieTypeId, p => p.DieTypeId, (c,p) => p.CodeProduct).ToListAsync();
            }
        }
    }
}
