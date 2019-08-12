using VueExample.Contexts;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using VueExample.ViewModels;
using VueExample.Providers.ChipVerification.Abstract;
using System.Threading.Tasks;
using VueExample.ResponseObjects;
using Microsoft.EntityFrameworkCore;

namespace VueExample.Providers.ChipVerification
{
    public class MaterialProvider : IMaterialProvider
    {
        private readonly IMapper _mapper;
        private readonly ApplicationContext _applicationContext;
        public MaterialProvider(ApplicationContext applicationContext, IMapper mapper)
        {
            _mapper = mapper;
            _applicationContext = applicationContext;
        }
         
        public async Task<AfterDbManipulationObject<MaterialViewModel>> ChangeMaterialOnMeasurement(int measurementId, int materialId)
        {           
            var measurement = await _applicationContext.Measurement.FindAsync(measurementId);
            var material = await _applicationContext.Material.FindAsync(materialId);
            var obj = new AfterDbManipulationObject<MaterialViewModel>();

            if(measurement is null)
            {
                obj.AddError(new Error("@Измерение не найдено"));
            }

            if(material is null)
            {
                obj.AddError(new Error("@Материал не найден"));
            }

            if(obj.HasErrors)
            {
                return obj;
            }
        
            material.MaterialId = materialId;
            await _applicationContext.SaveChangesAsync();
            obj.SetObject(_mapper.Map<MaterialViewModel>(material));
            return obj;                                        

        }

        public async Task<AfterDbManipulationObject<List<MaterialViewModel>>> GetAll()
        {              
            var obj = new AfterDbManipulationObject<List<MaterialViewModel>>();
            var materialList = await _applicationContext.Material.ToListAsync();

            if(materialList.Count == 0)
            {
                obj.AddError(new Error("@Нет материалов"));
                return obj;
            }

            obj.SetObject(materialList.Select(x => _mapper.Map<MaterialViewModel>(x)).ToList());           
            return obj;
        }

        public async Task<AfterDbManipulationObject<MaterialViewModel>> GetMaterialByMeasurementId(int measurementId)
        {
            var obj = new AfterDbManipulationObject<MaterialViewModel>();
            var material = await _applicationContext.Measurement.Where(m => m.MeasurementId == measurementId).Join(_applicationContext.Material, 
                                                            m1 => m1.MaterialId, 
                                                            m2 => m2.MaterialId,
                                                            (m1, m2) => new MaterialViewModel{MaterialId = m2.MaterialId, Name = m2.Name}).FirstOrDefaultAsync();
                                                            
            if(material is null)
            {
                obj.AddError(new Error(@"Материал не найден"));
                return obj;
            }

            obj.SetObject(material);
            return obj;
        }
    }
}