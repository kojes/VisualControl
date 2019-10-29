using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VueExample.Contexts;
using VueExample.Models.SRV6;
using VueExample.Providers.Srv6.Interfaces;
using System.Collections.Generic;

namespace VueExample.Providers.Srv6
{
    public class ElementService : IElementService
    {
        public async Task<Element> GetById(int elementId)
        {
             using (Srv6Context db = new Srv6Context())
             {
                return await db.Elements.FindAsync(elementId);
             }
            
        }

        public async Task<List<Element>> GetByIdmr(int idmr) 
        {
             using (Srv6Context db = new Srv6Context())
             {
                var elementList = await db.Elements .Include(m => m.MeasurementRecordingElements.Where(x => x.MeasurementRecordingId == idmr))
                                                    .ThenInclude(e => e.MeasurementRecording)                                                             
                                                    .ToListAsync();
                return elementList;
             }            
        }

        public async Task<Element> GetByNameAndWafer(string name, string waferId)
        {
             using (Srv6Context db = new Srv6Context())
             {
                 var waferIdSqlParameter = new SqlParameter("waferId", waferId);
                 var elementNameSqlParameter = new SqlParameter("name", name);
                 return await db.Elements.FromSql("EXECUTE dbo.select_element_by_waferId_elementname @waferId, @name", waferIdSqlParameter, elementNameSqlParameter).FirstOrDefaultAsync();
             }
        }
    }
}