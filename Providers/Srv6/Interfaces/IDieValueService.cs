using System.Collections.Generic;
using System.Threading.Tasks;
using VueExample.Entities;
using VueExample.Models.SRV6;

namespace VueExample.Providers.Srv6.Interfaces
{
    public interface IDieValueService
    {
        Dictionary<string, List<DieValue>> GetDieValuesByMeasurementRecording(int measurementRecordingId);
        List<long?> GetSelectedDiesByMeasurementRecordingId(int measurementRecordingId);
        Task CreateDieGraphics(List<DieGraphics> dieGraphics); 
    }
}