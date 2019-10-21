using System.Collections.Generic;
using System.Linq;
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

        

       
    }
}
