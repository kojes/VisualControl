using System.Collections.Generic;
using System.Threading.Tasks;
using VueExample.ResponseObjects;
using VueExample.ViewModels;

namespace VueExample.Providers.ChipVerification.Abstract
{
    public interface IDeviceTypeProvider
    {
        Task<AfterDbManipulationObject<List<DeviceTypeViewModel>>> GetAll();
        Task<DeviceTypeViewModel> Create(DeviceTypeViewModel deviceTypeViewModel);
        Task Delete(string modelName);
    }
}