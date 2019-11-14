using System.Collections.Generic;
using System.Threading.Tasks;
using VueExample.Models.SRV6;
using VueExample.ResponseObjects;
using VueExample.ViewModels;

namespace VueExample.Providers.Srv6.Interfaces
{
    public interface IDieTypeProvider
    {
        Task<List<DieType>> GetAll();
        Task<DieType> Create(DieTypeUpdatingViewModel dieTypeViewModel);
        Task<AfterDbManipulationObject<DieType>> Update(DieTypeViewModel dieTypeViewModel);
        Task<StandardResponseObject> UpdateCodeProductsMap(int dieTypeId, IList<int> codeProductsIdList);

    }
}