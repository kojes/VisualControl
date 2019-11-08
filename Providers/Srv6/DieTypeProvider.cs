using System.Collections.Generic;
using System.Threading.Tasks;
using VueExample.Models.SRV6;
using VueExample.Providers.Srv6.Interfaces;
using VueExample.ResponseObjects;
using VueExample.ViewModels;

namespace VueExample.Providers.Srv6
{
    public class DieTypeProvider : IDieTypeProvider
    {
        public Task<AfterDbManipulationObject<DieType>> Create(DieTypeViewModel dieTypeViewModel)
        {
            throw new System.NotImplementedException();
        }

        public Task<AfterDbManipulationObject<DieType>> Update(DieTypeViewModel dieTypeViewModel)
        {
            throw new System.NotImplementedException();
        }

        public Task<StandardResponseObject> UpdateCodeProductsMap(int dieTypeId, IList<int> codeProductsIdList)
        {
            throw new System.NotImplementedException();
        }
    }
}