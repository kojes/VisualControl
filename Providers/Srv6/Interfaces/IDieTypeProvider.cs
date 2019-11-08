using System.Collections.Generic;
using System.Threading.Tasks;
using VueExample.Models.SRV6;
using VueExample.ResponseObjects;
using VueExample.ViewModels;

namespace VueExample.Providers.Srv6.Interfaces
{
    public interface IDieTypeProvider
    {
        Task<AfterDbManipulationObject<DieType>> Create(DieTypeViewModel dieTypeViewModel);
        Task<AfterDbManipulationObject<DieType>> Update(DieTypeViewModel dieTypeViewModel);
        Task<StandardResponseObject> UpdateCodeProductsMap(int dieTypeId, IList<int> codeProductsIdList);

    }
}