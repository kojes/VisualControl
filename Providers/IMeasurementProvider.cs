using System;
using System.Collections.Generic;
using VueExample.Models;
using VueExample.ViewModels;

namespace VueExample.Providers
{
    public interface IMeasurementProvider
    {
        (List<Process>, List<CodeProduct>, List<MeasuredDevice>, List<Measurement>) GetAllMeasurementInfo();
        Object GetPointsByMeasurementId(int measurementId);
        Measurement GetById(int measurementId);
        ViewModels.MaterialViewModel GetMaterial(int measurementId);
        MeasurementOnlineStatus GetMeasurementOnlineStatus(int measurementId);
        bool IsMeasurementOnline(int measurementId);
        List<PointViewModel> GetPoints(int measurementId, int deviceId, int graphicId, int port);
    }
}
