using System.Collections.Generic;
using System.Threading.Tasks;
using VueExample.ResponseObjects;
using VueExample.ViewModels;

namespace VueExample.Providers.ChipVerification.Abstract
{
    public interface IGraphicProvider
    {
        Task<AfterDbManipulationObject<GraphicViewModel>> GetGraphicById(int id);
        Task<AfterDbManipulationObject<GraphicViewModel>> GetGraphicByNameAndType(string name, string type);
        Task<List<GraphicViewModel>> GetAvailiableByMeasurementId(int measurementId);
    }
}
