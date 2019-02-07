using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using VueExample.Models;
using VueExample.ServiceModels;

namespace VueExample.Providers
{
    public class DefectiveDieProvider
    {
        public string GetByDangerLevel(List<Defect> defectList, DangerLevel dangerLevel)
        {
            var dieIdList = defectList.Where(x => x.DangerLevelId == dangerLevel.DangerLevelId).Select(x => x.DieId).Distinct()
                .ToList();
            var defectiveDiesList = new List<DefectiveDie>();
            foreach (var dieId in dieIdList)
                defectiveDiesList.Add(new DefectiveDie {DieId = dieId, HexColor = dangerLevel.Color});
            return JsonConvert.SerializeObject(defectiveDiesList);
        }

        public string GetBadByDefectType(List<Defect> defectList, DefectType defectType)
        {
            var dieIdList = defectList.Where(x => x.DangerLevelId == 1 && x.DefectTypeId == defectType.DefectTypeId)
                .Select(x => x.DieId).Distinct().ToList();
            var defectiveDiesList = new List<DefectiveDie>();
            foreach (var dieId in dieIdList)
                defectiveDiesList.Add(new DefectiveDie {DieId = dieId, HexColor = defectType.Color});
            return JsonConvert.SerializeObject(defectiveDiesList);
        }
    }
}
