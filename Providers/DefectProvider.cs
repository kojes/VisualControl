using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VueExample.Contexts;
using VueExample.Models;
using VueExample.ViewModels;

namespace VueExample.Providers
{
    public class DefectProvider : IDefectProvider
    {
        public int InsertNewDefect(DefectViewModel defectViewModel)
        {
            var defect = new Defect();
            using (VisualControlContext visualControlContext = new VisualControlContext())
            {
                
            }
            return 0;
        }
    }
}
