using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using VueExample.Models;
using VueExample.ViewModels;
using VueExample.ResponseObjects;

namespace VueExample.Providers.ChipVerification.Abstract
{
    public interface IMeasurementProvider
    {
        (List<Process>, List<CodeProduct>, List<MeasuredDevice>, List<Measurement>) GetAllMeasurementInfo(int facilityId);
        Task<List<int>> GetAvailablePorts(int measurementId);
        Task<AfterDbManipulationObject<MeasurementViewModel>> Create(MeasurementViewModel measurementViewModel);
        Task<AfterDbManipulationObject<MeasurementViewModel>> Delete(int measurementId);        
        Measurement GetById(int measurementId);
        MeasurementOnlineStatus GetMeasurementOnlineStatus(int measurementId);
        bool IsMeasurementOnline(int measurementId);       
        List<MeasurementStatisticsViewModel> GetMeasurementStatistics(List<AtomicMeasurementExtendedViewModel> atomicMeasurementViewModelList);
        Task<AfterDbManipulationObject<MeasurementViewModel>> GetByMeasuredDeviceIdAndName(int measuredDeviceId, string name);
    }
}
