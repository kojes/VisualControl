using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VueExample.Contexts;
using VueExample.Models;

namespace VueExample.Providers
{
    public class DangerLevelProvider
    {
        public List<DangerLevel> GetDangerLevels()
        {
            using (VisualControlContext appContext = new VisualControlContext())
            {
                return appContext.DangerLevels.ToList();
            }
        }
    }
}
