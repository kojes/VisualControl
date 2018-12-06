using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VueExample.Contexts;
using VueExample.Models;
using VueExample.Repository;

namespace VueExample.Providers
{
    public class DieProvider : RepositorySRV6<Die>
    {
        public List<Die> GetDiesByWaferId(string waferId)
        {
            using (Srv6Context srv6Context = new Srv6Context())
            {
                return srv6Context.Dies.Where(x => x.WaferId == waferId).ToList();
            }
        }
    }
}
