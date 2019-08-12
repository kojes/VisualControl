using System.Collections.Generic;
using System.Threading.Tasks;
using VueExample.Models;
using VueExample.ResponseObjects;
using VueExample.ViewModels;

namespace VueExample.Providers.ChipVerification.Abstract
{
    public interface IDeviceProvider
    {
        List<Device> GetAll();
        Task<AfterDbManipulationObject<Device>> GetByName(string name);
        Task<List<Device>> GetAvailableByMeasurementId(int measurementId);
        Task<AfterDbManipulationObject<Device>> Delete(int deviceId);
        Task<AfterDbManipulationObject<Device>> Create(DeviceViewModel deviceViewModel);
        Task<AfterDbManipulationObject<Device>> Edit(DeviceViewModel deviceViewModel);
    }
}