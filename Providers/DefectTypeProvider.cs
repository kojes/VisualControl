using System.Collections.Generic;
using System.Linq;
using VueExample.Contexts;
using VueExample.Models;
using VueExample.Repository;
using VueExample.ResponseObjects;

namespace VueExample.Providers
{
    public class DefectTypeProvider : Repository<DefectType>
    {
        public List<DefectType> GetDefectTypes()
        {
            using (VisualControlContext applicationContext = new VisualControlContext())
            {
                return applicationContext.DefectTypes.ToList();
            }
        }

        public List<DefectType> GetDefectTypesFromDefectList(List<Defect> defectList)
        {
            var defectTypeList = new List<DefectType>();
            foreach (var defect in defectList)
            {
                defectTypeList.Add(defect.DefectType);
            }

            return defectTypeList;
        }

        public AfterDbManipulationObject<DefectType> AddDefectType(string description, string color)
        {
            var newDefectType = new DefectType { Description = description, Color = color };
            var obj = new AfterDbManipulationObject<DefectType>();
            using (VisualControlContext applicationContext = new VisualControlContext())
            {
               
                if (applicationContext.DefectTypes.Any(x => x.Description == description))
                {
                    obj.AddError(new Error(@"Существует тип дефекта с идентичным именем"));
                }

                if (applicationContext.DefectTypes.Any(x => x.Color == color))
                {
                    obj.AddError(new Error(@"Существует тип дефекта с идентичным цветом"));
                }

                if (obj.HasErrors) return obj;
                applicationContext.DefectTypes.Add(newDefectType);
                applicationContext.SaveChanges();
                obj.SetObject(newDefectType);

            }

            return obj;
        }

        public AfterDbManipulationObject<DefectType> DeleteDefectType(string description)
        {
            var obj = new AfterDbManipulationObject<DefectType>("DELETE");
            using (VisualControlContext applicationContext = new VisualControlContext())
            {
                var defectType = applicationContext.DefectTypes.FirstOrDefault(x => x.Description == description);
                if (defectType == null)
                {
                    obj.AddError(new Error(@"Не найден тип с таким именем"));
                    return obj;
                }

                if (applicationContext.Defects.Any(x => x.DefectTypeId == defectType.DefectTypeId))
                {
                    obj.AddError(new Error(@"Невозможно удалить тип, так как существуют дефекты с таким типом"));
                    return obj;
                }

                obj.SetObject(defectType);
                applicationContext.DefectTypes.Remove(defectType);
                applicationContext.SaveChanges();

            }

            return obj;

        }



    }
}
