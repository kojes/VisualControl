using System.Collections.Generic;
using System.Threading.Tasks;
using VueExample.ResponseObjects;
using VueExample.ViewModels;

namespace VueExample.Providers.ChipVerification.Abstract
{
    public interface IMaterialProvider
    {    
        Task<AfterDbManipulationObject<List<MaterialViewModel>>> GetAll();
        Task<AfterDbManipulationObject<MaterialViewModel>> ChangeMaterialOnMeasurement(int measurementId, int materialId);
        Task<AfterDbManipulationObject<MaterialViewModel>> GetMaterialByMeasurementId(int measurementId);
    } 
    
}