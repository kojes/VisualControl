using System.Collections.Generic;
using System.Threading.Tasks;
using VueExample.Entities;
using VueExample.Models.SRV6;

namespace VueExample.Providers.Srv6.Interfaces
{
    public interface IMeasurementRecordingService
    {
        Task<List<MeasurementRecording>> GetByWaferId(string waferId);
        Task<MeasurementRecording> GetOrCreate(string name, int v, int bmrId, int? stageId);
        Task Delete(int measurementRecordingId);
        Task DeleteSpecificMeasurement(int measurementRecordingId, int graphicId);
        Task<BigMeasurementRecording> GetOrAddBigMeasurement(string name, string waferId);
        Task<FkMrP> GetOrCreateFkMrP(int measurementRecordingId, short parameterId, string waferId);
        Task<FkMrGraphic> GetFkMrGraphics(int? measurementRecordingId, int graphicId);
        Task<FkMrGraphic> AddOrGetFkMrGraphics(FkMrGraphic fkMrGraphic);
        Task<List<MeasurementRecording>> GetByWaferIdAndStageNameAndElementId(string waferId, string stageName, int elementId);
        Task<MeasurementRecording> GetByNameAndWaferId(string name, string waferId);
        Task<MeasurementRecording> GetByBmrIdAndName(int bmrId, string name);
        Task<MeasurementRecording> UpdateStage(int measurementRecordingId, int stageId);
        Task<MeasurementRecording> GetById(int id);
    }
}