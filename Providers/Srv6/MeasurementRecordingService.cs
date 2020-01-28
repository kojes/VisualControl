using System.Collections.Generic;
using VueExample.Contexts;
using System.Linq;
using VueExample.Models.SRV6;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System;
using System.Threading.Tasks;
using VueExample.Entities;
using VueExample.Providers.Srv6.Interfaces;
using VueExample.Exceptions;

namespace VueExample.Providers.Srv6
{
    public class MeasurementRecordingService : IMeasurementRecordingService
    {

        public async Task<MeasurementRecording> GetOrCreate(string name, int type, int bigMeasurementRecordingId, int? stageId = null) 
        {
            using (Srv6Context srv6Context = new Srv6Context())
            {
                var measurementRecording = await GetByBmrIdAndName(bigMeasurementRecordingId, "оп." + name);

                if(measurementRecording is null)
                {
                    var newMeasurementRecording = new MeasurementRecording{Name = "оп." + name, MeasurementDateTime = DateTime.Now, Type = type, BigMeasurementRecordingId = bigMeasurementRecordingId, StageId = stageId};
                    srv6Context.MeasurementRecordings.Add(newMeasurementRecording);
                    await srv6Context.SaveChangesAsync();
                    return newMeasurementRecording;
                }
              
                return measurementRecording;
            }  
        }

        public async Task<BigMeasurementRecording> GetOrAddBigMeasurement(string name, string waferId) 
        {
            using (Srv6Context srv6Context = new Srv6Context())
            {
                var bigMeasurementRecording = await srv6Context.BigMeasurementRecordings.FirstOrDefaultAsync(x => x.WaferId == waferId && x.Name == name);
                if(bigMeasurementRecording is null)
                {
                    bigMeasurementRecording = new BigMeasurementRecording {Name = name, WaferId = waferId};
                    srv6Context.Add(bigMeasurementRecording);
                    await srv6Context.SaveChangesAsync();
                }
                return bigMeasurementRecording;
            }
        }

        public async Task<FkMrP> GetOrCreateFkMrP(int measurementRecordingId, short parameterId, string waferId)
        {
            using (Srv6Context srv6Context = new Srv6Context())
            {
                var fkmrp = await srv6Context.FkMrPs.FirstOrDefaultAsync(x => x.MeasurementRecordingId == measurementRecordingId && x.Id247 == parameterId && x.WaferId == waferId);
                if(fkmrp is null)
                {
                    fkmrp = new FkMrP{MeasurementRecordingId = measurementRecordingId, WaferId = waferId, Id247 = parameterId};
                    srv6Context.Add(fkmrp);
                    await srv6Context.SaveChangesAsync();
                }              
                return fkmrp;
            }
        }
        public async Task<FkMrGraphic> AddOrGetFkMrGraphics(FkMrGraphic fkMrGraphic) 
        {
            using(var db = new Srv6Context())
            {
                var newFkMrGraphic = await db.FkMrGraphics.FirstOrDefaultAsync(x => x.MeasurementRecordingId == fkMrGraphic.MeasurementRecordingId
                                                                                 && x.GraphicId == fkMrGraphic.GraphicId);
                if(newFkMrGraphic is null)
                {
                    db.FkMrGraphics.Add(fkMrGraphic);
                    await db.SaveChangesAsync();
                } 
                return newFkMrGraphic;              
            }
        }

        public async Task<List<MeasurementRecording>> GetByWaferId(string waferId)
        {
            var measurementRecordingsList = new List<MeasurementRecording>();
            using (Srv6Context srv6Context = new Srv6Context())
            {
                var idmrList = await srv6Context.FkMrPs.Where(x => x.WaferId == waferId).Select(x => x.MeasurementRecordingId).ToListAsync();
                foreach (var idmr in idmrList)
                {
                    measurementRecordingsList.Add(srv6Context.MeasurementRecordings.Find(idmr));
                }
            }
            return measurementRecordingsList;
        }

        public async Task<MeasurementRecording> GetByNameAndWaferId(string name, string waferId) 
        {
            using (Srv6Context srv6Context = new Srv6Context())
            {
                var mrList = await srv6Context.FkMrPs.Where(x => x.WaferId == waferId).ToListAsync();
                return mrList.Select(measurementRecording => srv6Context.MeasurementRecordings.FirstOrDefault(x => x.Id == measurementRecording.MeasurementRecordingId)).FirstOrDefault(mr => mr != null && mr.Name == name);                
            }
        }

       

        public async Task<MeasurementRecording> GetByBmrIdAndName(int bmrId, string name)
        {
            using (Srv6Context srv6Context = new Srv6Context())
            {
                return await srv6Context.MeasurementRecordings.FirstOrDefaultAsync(x => x.Name == name && x.BigMeasurementRecordingId == bmrId);
            }
        }

        public Task<List<MeasurementRecording>> GetByWaferIdAndStageNameAndElementId(string waferId, string stageName, int elementId)
        {
            using (Srv6Context srv6Context = new Srv6Context())
            {
                var waferIdSqlParameter = new SqlParameter("waferId", waferId);
                var stageNameSqlParameter = new SqlParameter("stageName", stageName);
                var elementIdSqlParameter = new SqlParameter("elementId", elementId);
                return srv6Context.MeasurementRecordings.FromSql("EXECUTE select_mr_by_stagename_waferid_elementid @waferId, @elementId, @stageName", waferIdSqlParameter, elementIdSqlParameter, stageNameSqlParameter).ToListAsync();
            }
        }

        public async Task<List<MeasurementRecording>> GetByWaferIdAndDieType(string waferId, int dieTypeId)
        {
            using (Srv6Context srv6Context = new Srv6Context())
            {
                var waferIdSqlParameter = new SqlParameter("waferId", waferId);
                var dieTypeSqlParameter = new SqlParameter("dieTypeId", dieTypeId);              
                return await srv6Context.MeasurementRecordings.FromSql("EXECUTE select_all_mr_by_waferid_dietypeid @waferId, @dieTypeId", waferIdSqlParameter, dieTypeSqlParameter).ToListAsync();
            }
        }

        public async Task<MeasurementRecording> UpdateStage(int measurementRecordingId, int stageId)
        {
            using (Srv6Context srv6Context = new Srv6Context())
            {
                var measurementRecording = await srv6Context.MeasurementRecordings.FirstOrDefaultAsync(x => x.Id == measurementRecordingId);
                measurementRecording.StageId = stageId;
                await srv6Context.SaveChangesAsync();
                return measurementRecording;
            }

        }

        public async Task<MeasurementRecording> GetById(int id)
        {
            using (Srv6Context srv6Context = new Srv6Context())
            {
                 return await srv6Context.MeasurementRecordings.FirstOrDefaultAsync(x => x.Id == id);
            }
        }

        public async Task Delete(int measurementRecordingId)
        {
            using (Srv6Context srv6Context = new Srv6Context())
            {
                var measurementRecording = await srv6Context.MeasurementRecordings.FirstOrDefaultAsync(x => x.Id == measurementRecordingId) 
                                           ?? throw new RecordNotFoundException();                
                var measurementRecordingSqlParameter = new SqlParameter("idmr", measurementRecording.Id);
                srv6Context.Database.ExecuteSqlCommand("EXECUTE dbo.delete_full_measurement_recording @idmr", measurementRecordingSqlParameter);         
            }
        }

        public async Task DeleteSpecificMeasurement(int measurementRecordingId, int graphicId)
        {
            using (Srv6Context srv6Context = new Srv6Context())
            {
                var graphicMeasurementRecording = await srv6Context.FkMrGraphics.FirstOrDefaultAsync(x => x.GraphicId == graphicId && x.MeasurementRecordingId == measurementRecordingId) 
                                                  ?? throw new RecordNotFoundException();                                              
                var valueList = await srv6Context.DieGraphics.Where(x => x.MeasurementRecordingId == measurementRecordingId && x.GraphicId == graphicId).ToListAsync();                                                
                srv6Context.DieGraphics.RemoveRange(valueList);
                srv6Context.FkMrGraphics.Remove(graphicMeasurementRecording);
                await srv6Context.SaveChangesAsync();
                if(await srv6Context.DieGraphics.AnyAsync(x => x.MeasurementRecordingId == measurementRecordingId)) 
                {
                    await Delete(measurementRecordingId);
                }                                 
            }
        }

        public async Task<FkMrGraphic> GetFkMrGraphics(int? measurementRecordingId, int graphicId)
        {
            measurementRecordingId = measurementRecordingId ?? throw new RecordNotFoundException();
            using(var db = new Srv6Context())
            {
                 return await db.FkMrGraphics.FirstOrDefaultAsync(x => x.MeasurementRecordingId == measurementRecordingId
                                                                                 && x.GraphicId == graphicId) ?? throw new RecordNotFoundException();
            }
        }

        public async Task DeleteSet(IList<int> measurementRecordingIdList)
        {
            foreach (var measurementRecording in measurementRecordingIdList)
            {
               await Delete(measurementRecording);
            }
        }

        public async Task<MeasurementRecording> UpdateName(int measurementRecordingId, string newName)
        {
            using(var db = new Srv6Context())
            {
                if(newName.Contains("оп"))
                    throw new ValidationErrorException();
                var measurementRecording = await db.MeasurementRecordings.FirstOrDefaultAsync(x => x.Id == measurementRecordingId) ?? throw new RecordNotFoundException();
                measurementRecording.Name = $"оп.{newName}";
                await db.SaveChangesAsync();
                return measurementRecording;
            }
            
        }
    }
}