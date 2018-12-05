using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VueExample.Contexts;
using VueExample.Models;

namespace VueExample.Providers
{
    public class StageProvider
    {
        public List<Stage> GetStagesByProcessId(int processId)
        {
            using (var srv6Context = new Srv6Context())
            {
                return srv6Context.Stages.Where(x => x.ProcessId == processId && x.CodeProductId == null).ToList();
            }
        }

        public Stage GetById(int stageId)
        {
            using (var srv6Context = new Srv6Context())
            {
                return srv6Context.Stages.Find(stageId);
            }
        }
    }
}
