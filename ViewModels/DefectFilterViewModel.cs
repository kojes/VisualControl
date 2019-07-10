using System.Collections.Generic;
using VueExample.Models;

namespace VueExample.ViewModels
{
    public class DefectFilterViewModel
    {
        public List<Die> AvbDiesList { get; set; }
        public List<Stage> AvbStagesList { get; set; }
        public List<DefectType> AvbDefectTypesList { get; set; }
        public List<DangerLevel> AvbDangerLevelList { get; set; }
    }
}
