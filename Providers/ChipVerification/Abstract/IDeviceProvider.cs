using System.Collections.Generic;
using System.Threading.Tasks;
using VueExample.Models;
using VueExample.ResponseObjects;

namespace VueExample.Providers.ChipVerification.Abstract
{
    public interface IDeviceProvider
    {
        List<Device> GetAll();
        Task<List<Device>> GetAvailableByMeasurementId(int measurementId);
        Error Delete(int deviceId);
        (Device, Error) Add(Device device);
    }
}