using System.Collections.Generic;
using System.Threading.Tasks;
using VueExample.Models.SRV6;

namespace VueExample.Providers.Srv6.Interfaces
{
    public interface IElementTypeService
    {
        Task<IList<ElementType>> GetAll();
    }
}