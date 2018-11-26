using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using VueExample.Contexts;

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
    }
}
