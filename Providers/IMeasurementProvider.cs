using System;
using System.Collections.Generic;
using VueExample.Models;

namespace VueExample.Providers
{
    public interface IMeasurementProvider
    {
        (List<Process>, List<CodeProduct>, List<MeasuredDevice>, List<Measurement>) GetAllMeasurementInfo();
        Object GetPointsByMeasurementId(int measurementId);
        Measurement GetById(int measurementId);
        List<Point> GetPoints(int measurementId, int deviceId, int graphicId, int port);
    }
}
