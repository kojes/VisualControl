using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VueExample.Contexts;
using VueExample.Models;
using VueExample.Repository;

namespace VueExample.Providers
{
    public class StageProvider : RepositorySRV6<Stage>
    {
        public List<Stage> GetStagesByProcessId(int processId)
        {
            using (var srv6Context = new Srv6Context())
            {
                return srv6Context.Stages.Where(x => x.ProcessId == processId && x.CodeProductId == null).ToList();
            }
        }
        public async Task<List<Stage>> GetStagesByWaferId(string waferId)
        {
            using (var srv6Context = new Srv6Context())
            {
                return await (from stage in srv6Context.Stages 
                        join process in srv6Context.Processes on stage.ProcessId equals process.ProcessId
                        join codeProduct in srv6Context.CodeProducts on process.ProcessId equals codeProduct.ProcessId
                        join wafer in srv6Context.Wafers on codeProduct.IdCp equals wafer.CodeProductId
                        where wafer.WaferId == waferId
                        select stage).ToListAsync();
            }
            
        }  
            
    }
}
