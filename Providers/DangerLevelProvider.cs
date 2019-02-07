using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VueExample.Contexts;
using VueExample.Models;
using VueExample.Repository;

namespace VueExample.Providers
{
    public class DangerLevelProvider : Repository<DangerLevel>
    {
        public List<DangerLevel> GetDangerLevelFromDefectList(List<Defect> defectList)
        {
            var dangerLevelList = new List<DangerLevel>();
            foreach (var defect in defectList)
            {
                dangerLevelList.Add(defect.DangerLevel);
            }

            return dangerLevelList;
        }
    }
}
