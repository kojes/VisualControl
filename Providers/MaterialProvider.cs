using VueExample.Contexts;
using VueExample.Models;
using System.Linq;
using System.Collections.Generic;

namespace VueExample.Providers
{
    public class MaterialProvider : IMaterialProvider
    {
        public Material ChangeName(string oldName, string newName)
        {
            using(ApplicationContext applicationContext = new ApplicationContext())
            {
                var edited = applicationContext.Material.FirstOrDefault(_ => _.Name == oldName);
                if(edited != null)
                {
                    edited.Name = newName;
                }
                applicationContext.SaveChanges();

                return edited;
            }
            
        }
         
        public Material ChangeMaterialOnMeasurement(int measurementId, int materialId)
        {
            using(ApplicationContext applicationContext = new ApplicationContext())
            {
                var measurement = applicationContext.Measurement.Find(measurementId);
                var material = applicationContext.Material.Find(materialId);
                if(measurement != null && material != null)
                {
                     applicationContext.Measurement.Find(measurementId).MaterialId = materialId;
                     applicationContext.SaveChanges();
                     return material;
                }
                else
                {
                    return null;
                }
                
                
            }


        }


        public List<Material> GetAll()
        {
              using(ApplicationContext applicationContext = new ApplicationContext())
              {
                  return applicationContext.Material.ToList();
              }
        }
    }
}