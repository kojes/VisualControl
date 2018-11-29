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
        public int InsertNewDefect(Defect defect)
        {
            using (VisualControlContext visualControlContext = new VisualControlContext())
            {
                visualControlContext.Defects.Add(defect);
                visualControlContext.SaveChanges();
            }
            return defect.DefectId;
        }

        public void DeleteById(int defectId)
        {
            using (VisualControlContext visualControlContext = new VisualControlContext())
            {
                var deletedDefect = visualControlContext.Defects.FirstOrDefault(x => x.DefectId == defectId);
                if (deletedDefect != null) visualControlContext.Defects.Remove(deletedDefect);
                visualControlContext.SaveChanges();
            }
        }
    }
}
