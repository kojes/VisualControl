using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VueExample.Contexts;
using VueExample.Models;

namespace VueExample.Providers
{
    public class DieProvider
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
