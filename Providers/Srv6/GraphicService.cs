using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VueExample.Contexts;
using VueExample.Models.SRV6;
using VueExample.Providers.Srv6.Interfaces;


namespace VueExample.Providers.Srv6
{
    public class GraphicService : ISRV6GraphicService
    {
        public Graphic GetById(int graphicId) 
        {
            using(var db = new Srv6Context())
            {
                return db.Graphics.FirstOrDefault(x => x.Id == graphicId);
            }
        }
        
        public async Task<Graphic> GetByCodeProductAndName(int codeProductId, string name)
        {
            using(var db = new Srv6Context())
            {
                return await (from f in db.CodeProductGraphic
                    join g in db.Graphics on f.GraphicId equals g.Id
                    where g.Name == name && f.CodeProductId == codeProductId
                    select g).FirstOrDefaultAsync();
            }
        }

       
    }
}
