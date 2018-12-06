using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
