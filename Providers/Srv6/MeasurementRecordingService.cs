using System.Runtime.CompilerServices;
using System.Collections.Generic;
using VueExample.Contexts;
using System.Linq;
using VueExample.Models.SRV6;
using VueExample.Repository;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace VueExample.Providers.Srv6
{
    public class MeasurementRecordingService : RepositorySRV6<MeasurementRecording>
    {
        public List<MeasurementRecording> GetByWaferId (string waferId)
        {
            var measurementRecordingsList = new List<MeasurementRecording>();
            using (Srv6Context srv6Context = new Srv6Context())
            {
                var idmrList = srv6Context.FkMrPs.Where(x => x.WaferId == waferId).Select(x => x.MeasurementRecordingId).ToList();
                foreach (var idmr in idmrList)
                {
                    measurementRecordingsList.Add(srv6Context.MeasurementRecordings.Find(idmr));
                }
            }
            return measurementRecordingsList;
        }

        public int? GetStageId(int measurementRecordingId)
        {
            using (Srv6Context srv6Context = new Srv6Context())
            {
                return srv6Context.MeasurementRecordings.FirstOrDefault(x => x.Id == measurementRecordingId).StageId;
            }
        }

        public List<MeasurementRecording> GetByWaferIdAndStageNameAndElementId(string waferId, string stageName, int elementId)
        {
            using (Srv6Context srv6Context = new Srv6Context())
            {
                 var waferIdSqlParameter = new SqlParameter("waferId", waferId);
                 var stageNameSqlParameter = new SqlParameter("stageName", stageName);
                 var elementIdSqlParameter = new SqlParameter("elementId", elementId);
                 return srv6Context.MeasurementRecordings.FromSql("EXECUTE select_mr_by_stagename_waferid_elementid @waferId, @elementId, @stageName", waferIdSqlParameter, elementIdSqlParameter, stageNameSqlParameter).ToList();
            }
        }         
    }
}