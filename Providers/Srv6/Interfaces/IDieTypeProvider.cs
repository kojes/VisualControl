using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VueExample.Models.SRV6;
using VueExample.ViewModels;

namespace VueExample.Providers.Srv6.Interfaces
{
    public interface IDieTypeProvider
    {
        Task<List<DieType>> GetAll();
        Task<DieType> GetByName(string name);
        Task<DieTypeUpdatingViewModel> GetCodeProductsAndElements(int id);
        Task<DieType> Create(DieTypeUpdatingViewModel dieTypeViewModel);
        Task<DieType> Update(DieTypeViewModel dieTypeViewModel);
        Task<List<DieTypeViewModel>> GetByCodeProductId(int codeProductId);
        Task<Tuple<CodeProductViewModel, string>> UpdateCodeProductsMap(int dieTypeId, int codeProductId);

    }
}