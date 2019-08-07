using VueExample.Contexts;
using VueExample.Models;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using VueExample.ViewModels;
using VueExample.Providers.ChipVerification.Abstract;

namespace VueExample.Providers.ChipVerification
{
    public class MaterialProvider : IMaterialProvider
    {
        private readonly IMapper _mapper;
        private readonly ApplicationContext _applicationContext;
        public MaterialProvider(IMapper mapper, ApplicationContext applicationContext)
        {
            _mapper = mapper;
            _applicationContext = applicationContext;
        }

        public Material ChangeName(string oldName, string newName)
        {           
            var edited = _applicationContext.Material.FirstOrDefault(_ => _.Name == oldName);

            if(edited != null)
            {
                edited.Name = newName;
            }

            _applicationContext.SaveChanges();
            return edited;           
            
        }
         
        public Material ChangeMaterialOnMeasurement(int measurementId, int materialId)
        {           
            var measurement = _applicationContext.Measurement.Find(measurementId);
            var material = _applicationContext.Material.Find(materialId);
            if(measurement != null && material != null)
            {
                 _applicationContext.Measurement.Find(measurementId).MaterialId = materialId;
                 _applicationContext.SaveChanges();
                 return material;
            }
            else
            {
                 return null;
            }                                        

        }

        public List<MaterialViewModel> GetAll()
        {              
            return (from material in _applicationContext.Material.ToList()
                    select _mapper.Map<MaterialViewModel>(material)).ToList();
            
        }
    }
}