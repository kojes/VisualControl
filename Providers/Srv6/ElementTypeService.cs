using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VueExample.Contexts;
using VueExample.Models.SRV6;
using VueExample.Providers.Srv6.Interfaces;

namespace VueExample.Providers.Srv6
{
    public class ElementTypeService : IElementTypeService
    {
        public async Task<IList<ElementType>> GetAll()
        {
            using(var db = new Srv6Context())
            {
                return await db.ElementTypes.ToListAsync();
            }
        }
    }
}