using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VueExample.Contexts;
using VueExample.Models;

namespace VueExample.Providers
{
    public class ProcessProvider
    {
        public int GetProcessIdByCodeProductId(int codeProductId)
        {
            using (Srv6Context srv6Context = new Srv6Context())
            {
                return srv6Context.CodeProducts.FirstOrDefault(x => x.IdCp == codeProductId).ProcessId;
            }
        }

        public async Task<List<Process>> GetAll() 
        {
            using (Srv6Context srv6Context = new Srv6Context())
            {
                return await srv6Context.Processes.ToListAsync();
            }   
        }
    }
}
