using System.Collections.Generic;
using System.Threading.Tasks;
using VueExample.Entities;

namespace VueExample.Providers.Abstract
{
    public interface IStandartParameterProvider
    {
        Task<IList<StandartParameterEntity>> GetAll();
        Task<StandartParameterEntity> GetById(int standartParameterModelId);
        Task<StandartParameterEntity> Create(StandartParameterEntity standartParameterModel);
        Task<StandartParameterEntity> Update(StandartParameterEntity standartParameterModel);
        Task Delete(int standartParameterModelId);
    }
}