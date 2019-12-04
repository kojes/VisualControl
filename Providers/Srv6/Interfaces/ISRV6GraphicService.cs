using System.Collections.Generic;
using System.Threading.Tasks;
using VueExample.Models.SRV6;

namespace VueExample.Providers.Srv6.Interfaces
{
    public interface ISRV6GraphicService
    {
        Task<Graphic> GetByCodeProductAndName(int codeProductId, string name);        
    }
}