using VueExample.Contexts;
using VueExample.Models;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using VueExample.ViewModels;

namespace VueExample.Providers
{
    public class MaterialProvider : IMaterialProvider
    {
        private IMapper _mapper;
        public MaterialProvider(IMapper mapper)
        {
            _mapper = mapper;
        }
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


        public List<MaterialViewModel> GetAll()
        {
              using(ApplicationContext applicationContext = new ApplicationContext())
              {
                  return (from material in applicationContext.Material.ToList()
                                               select _mapper.Map<MaterialViewModel>(material)).ToList();
            }
        }
    }
}