using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VueExample.Models.SRV6;

namespace VueExample.Providers.Srv6.Interfaces
{
    public interface IElementService
    {
         Task<Element> GetById(int elementId);
         Task<Element> GetByNameAndWafer(string name, string waferId);
         Task<List<Element>> GetByIdmr(int idmr);
    }
}