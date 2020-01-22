using System.Collections.Generic;
using VueExample.Contexts;
using System.Linq;
using VueExample.Models.SRV6;
using VueExample.Repository;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System;
using System.Threading.Tasks;
using VueExample.Entities;
using VueExample.Providers.Srv6.Interfaces;

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

        public async Task<FkMrP> CreateFkMrP(int measurementRecordingId, short parameterId, string waferId)
        {
            using (Srv6Context srv6Context = new Srv6Context())
            {
                var fkmrp = new FkMrP{MeasurementRecordingId = measurementRecordingId, WaferId = waferId, Id247 = parameterId};
                srv6Context.Add(fkmrp);
                await srv6Context.SaveChangesAsync();
                return fkmrp;
            }
        }

        public async Task<bool> IsExistFkMrGraphics(int measurementRecordingId, int graphicId) 
        {
            using(var db = new Srv6Context())
            {
                 return await db.FkMrGraphics.AnyAsync(x => x.MeasurementRecordingId == measurementRecordingId
                                                                                 && x.GraphicId == graphicId);
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
    }
}