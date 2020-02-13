using System.Collections.Generic;
using System.Threading.Tasks;
using VueExample.Models.SRV6;

namespace VueExample.Providers.Srv6.Interfaces
{
    public interface IStandartParameterService
    {
        Task<IList<StandartParameterModel>> GetAll();
        Task<StandartParameterModel> GetById(int standartParameterModelId);
        Task<StandartParameterModel> Create(StandartParameterModel standartParameterModel);
        Task<StandartParameterModel> Update(StandartParameterModel standartParameterModel);
        Task Delete(int standartParameterModelId);

    }
}