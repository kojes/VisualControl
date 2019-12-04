using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VueExample.Contexts;
using VueExample.Entities;
using VueExample.Models;
using VueExample.Repository;

namespace VueExample.Providers
{
    public class DieProvider : RepositorySRV6<Die>, IDieProvider
    {
        public async Task CreateDieParameter(int dieId, int measurementRecordingId, int parameterId = 247, string value = "0")
        {
            using (var db = new Srv6Context()) 
            {
                var dieParameter = new DieParameterOld{DieId = dieId, MeasurementRecordingId = measurementRecordingId, Id247 = parameterId, Value = value};
                db.DiesParameterOld.Add(dieParameter);
                await db.SaveChangesAsync();
            }
        }

        public async Task<Die> GetByWaferIdAndCode(string waferId, string code)
        {
            using (var db = new Srv6Context()) 
            {
                return await db.Dies.FirstOrDefaultAsync(x => x.WaferId == waferId && x.Code == code);
            }
        }

        public List<Die> GetDiesByWaferId(string waferId)
        {
            using (Srv6Context srv6Context = new Srv6Context())
            {
                var diesList = srv6Context.Dies.Where(x => x.WaferId == waferId).ToList();
                return diesList;
            }
        }

    }
}
