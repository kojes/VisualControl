using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VueExample.Contexts;
using VueExample.Models;

namespace VueExample.Providers
{
    public class DefectTypeProvider
    {
        public List<DefectType> GetDefectTypes()
        {
            using (VisualControlContext applicationContext = new VisualControlContext())
            {
                return applicationContext.DefectTypes.ToList();
            }
        }
    }
}
