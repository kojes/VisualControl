using System.Collections.Generic;
using System.Threading.Tasks;
using VueExample.Models;
using VueExample.ResponseObjects;
using VueExample.ViewModels;

namespace VueExample.Providers.ChipVerification.Abstract
{
    public interface IMeasuredDeviceProvider
    {
        Task<AfterDbManipulationObject<List<MeasuredDeviceViewModel>>> GetAll();
        Task<AfterDbManipulationObject<MeasuredDevice>> Create(MeasuredDeviceViewModel measuredDeviceViewModel);
        Task<AfterDbManipulationObject<MeasuredDevice>> GetByWaferIdAndCode(string waferId, string code);
        Task<AfterDbManipulationObject<MeasuredDevice>> GetById(int measuredDeviceId);
                
    }
}