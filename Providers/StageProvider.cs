using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VueExample.Contexts;
using VueExample.Models;
using VueExample.Providers.Srv6.Interfaces;
using VueExample.Repository;

namespace VueExample.Providers
{
    public class StageProvider : IStageProvider
    {
        public async Task<Stage> Create(string name, int processId) 
        {
            using (var srv6Context = new Srv6Context())
            {
                var newStage = new Stage{StageName = name, ProcessId = processId};
                srv6Context.Stages.Add(newStage);
                await srv6Context.SaveChangesAsync();
                return newStage;
            }
        }

        public async Task<List<Stage>> GetAll()
        {
            using (Srv6Context srv6Context = new Srv6Context())
            {
               return await srv6Context.Stages.ToListAsync();
            }
        }

        public async Task<Stage> GetById(int stageId)
        {
           using (Srv6Context srv6Context = new Srv6Context())
           {
               return await srv6Context.Stages.FirstOrDefaultAsync(x => x.StageId == stageId);
           }
        }

        public async Task<Stage> GetByMeasurementRecordingId(int measurementRecordingId)
        {
            using (Srv6Context srv6Context = new Srv6Context())
            {
                var stageId = (await srv6Context.MeasurementRecordings.FirstOrDefaultAsync(x => x.Id == measurementRecordingId))?.StageId;
                return stageId is null ? throw new Exception() : await GetById((int)stageId);
            }
        }

        public async Task<List<Stage>> GetStagesByProcessId(int processId)
        {
            using (var srv6Context = new Srv6Context())
            {
                return await srv6Context.Stages.Where(x => x.ProcessId == processId && x.CodeProductId == null).ToListAsync();
            }
        }
        public async Task<List<Stage>> GetStagesByWaferId(string waferId)
        {
            using (var srv6Context = new Srv6Context())
            {
                return await (from stage in srv6Context.Stages 
                        join process in srv6Context.Processes on stage.ProcessId equals process.ProcessId
                        join codeProduct in srv6Context.CodeProducts on process.ProcessId equals codeProduct.ProcessId
                        join wafer in srv6Context.Wafers on codeProduct.IdCp equals wafer.CodeProductId
                        where wafer.WaferId == waferId
                        select stage).ToListAsync();
            }
            
        }  
            
    }
}
