using System.Collections.Generic;
using VueExample.Models.SRV6;

namespace VueExample.Providers.Srv6.Interfaces
{
    public interface IDieValueService
    {
         Dictionary<string, List<DieValue>> GetDieValuesByMeasurementRecording(int measurementRecordingId);
         List<long?> GetSelectedDiesByMeasurementRecordingId(int measurementRecordingId);
         

    }
}