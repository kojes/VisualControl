using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using VueExample.ViewModels;
using VueExample.ResponseObjects;

namespace VueExample.Providers.ChipVerification.Abstract
{
    public interface IPointProvider
    {
        Task<AfterDbManipulationObject<List<PointViewModel>>> GetPoints(int measurementId, int deviceId, int graphicId, int port);
        Task<AfterDbManipulationObject<List<LivePointViewModel>>> GetLivePoints(List<AtomicMeasurementExtendedViewModel> atomicMeasurementViewModelList);
        Task<AfterDbManipulationObject<List<long>>> CreatePointSet(PointSetViewModel pointSetViewModel);
        Task<AfterDbManipulationObject<PointViewModel>> CreateSinglePoint(PointViewModel pointViewModel);
    }
}