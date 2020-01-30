using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VueExample.Contexts;
using VueExample.Exceptions;
using VueExample.Models;
using VueExample.Providers.Srv6.Interfaces;

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

        public async Task<Stage> Create(Stage stage)
        {
            using (Srv6Context srv6Context = new Srv6Context())
            {
                if(await srv6Context.Stages.AnyAsync(x => x.ProcessId == stage.ProcessId && x.StageName == stage.StageName) || String.IsNullOrEmpty(stage.StageName))
                    throw new ValidationErrorException();
                srv6Context.Stages.Add(stage);
                await srv6Context.SaveChangesAsync();
                return stage;
            }
        }

        public async Task Delete(int stageId)
        {
           using (Srv6Context srv6Context = new Srv6Context())
           {
                if(await srv6Context.MeasurementRecordings.AnyAsync(x => x.StageId == stageId))
                    throw new ValidationErrorException();
                var stage = await srv6Context.Stages.FirstOrDefaultAsync(x => x.StageId == stageId) ?? throw new RecordNotFoundException();
                srv6Context.Remove(stage);
                await srv6Context.SaveChangesAsync();
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
               return await srv6Context.Stages.FirstOrDefaultAsync(x => x.StageId == stageId) ?? throw new RecordNotFoundException();
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
                var stageList = await srv6Context.Stages.Where(x => x.ProcessId == processId && x.CodeProductId == null).ToListAsync();
                return stageList.Count == 0 ? throw new RecordNotFoundException() : stageList;
            }
        }
        
        public async Task<List<Stage>> GetStagesByWaferId(string waferId)
        {
            using (var srv6Context = new Srv6Context())
            {
                var stageList = await (from stage in srv6Context.Stages 
                        join process in srv6Context.Processes on stage.ProcessId equals process.ProcessId
                        join codeProduct in srv6Context.CodeProducts on process.ProcessId equals codeProduct.ProcessId
                        join wafer in srv6Context.Wafers on codeProduct.IdCp equals wafer.CodeProductId
                        where wafer.WaferId == waferId
                        select stage).ToListAsync();
                return stageList.Count == 0 ? throw new RecordNotFoundException() : stageList; 
            }
            
        }

        public async Task<Stage> Update(Stage stage)
        {
            using (var srv6Context = new Srv6Context())
            {
                var stageUpdate = await srv6Context.Stages.FirstOrDefaultAsync(x => x.StageId == stage.StageId) ?? throw new RecordNotFoundException();
                srv6Context.Entry(stageUpdate).CurrentValues.SetValues(stage);
                await srv6Context.SaveChangesAsync();
                return stage;
            }
        }
    }
}
