using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VueExample.Contexts;
using VueExample.Providers.Srv6.Interfaces;

namespace VueExample.Providers.Srv6
{
    public class StandartWaferService : IStandartWaferService
    {
        public async Task<string> GetCodeFromStandartWafer(string code, string map)
        {
             using(var db = new Srv6Context())
             {
                return (await db.CodeProductStandartWafers.
                              FirstOrDefaultAsync(x => x.Id == db.MapStandartWafers.FirstOrDefault(m => m.NewCode == code && m.MapName == map).Idfk)).Code;
             }
             
        }
    }
}