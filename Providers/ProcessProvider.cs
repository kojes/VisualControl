using System.Collections.Generic;
using System.Linq;
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

        public List<Process> GetAll() 
        {
            using (Srv6Context srv6Context = new Srv6Context())
            {
                return srv6Context.Processes.ToList();
            }   
        }
    }
}
