using System.Collections.Generic;
using System.Threading.Tasks;
using VueExample.Models.SRV6;
using VueExample.ViewModels;

namespace VueExample.Providers.Srv6.Interfaces
{
    public interface IElementService
    {
        Task<Element> Create(ElementViewModel elementViewModel);
        Task<Element> Update(ElementViewModel elementViewModel);
        Task Delete(int id);
        Task AddToDieType(int elementId, int dieTypeId);
        Task<Element> GetById(int elementId);
        Task<Element> GetByDieTypeIdAndName(int dieTypeId, string name);
        Task<Element> GetByNameAndWafer(string name, string waferId);
        Task<List<Element>> GetByDieType(int dieTypeId);
        Task<List<Element>> GetByIdmr(int idmr);
        Task<Element> UpdateElementOnIdmr(int measurementRecordingId, int newElementId);
    }
}