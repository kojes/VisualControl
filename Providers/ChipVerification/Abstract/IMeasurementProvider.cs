using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using VueExample.Models;
using VueExample.ViewModels;

namespace VueExample.Providers.ChipVerification.Abstract
{
    public interface IMeasurementProvider
    {
        (List<Process>, List<CodeProduct>, List<MeasuredDevice>, List<Measurement>) GetAllMeasurementInfo(int facilityId);
        Object GetPointsByMeasurementId(int measurementId);
        Task<List<int>> GetAvailablePorts(int measurementId);
        Measurement GetById(int measurementId);
        ViewModels.MaterialViewModel GetMaterial(int measurementId);
        MeasurementOnlineStatus GetMeasurementOnlineStatus(int measurementId);
        bool IsMeasurementOnline(int measurementId);
        List<PointViewModel> GetPoints(int measurementId, int deviceId, int graphicId, int port);
        List<LivePointViewModel> GetLivePoints(List<AtomicMeasurementExtendedViewModel> atomicMeasurementViewModelList);
        List<MeasurementStatisticsViewModel> GetMeasurementStatistics(List<AtomicMeasurementExtendedViewModel> atomicMeasurementViewModelList);

    }
}
