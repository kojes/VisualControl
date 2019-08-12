using System.Threading.Tasks;
using System.Collections.Generic;
using VueExample.ResponseObjects;
using VueExample.ViewModels;

namespace VueExample.Providers.ChipVerification.Abstract
{
    public interface IFacilityProvider
   {
      Task<AfterDbManipulationObject<List<FacilityViewModel>>> GetAllAsync();
   }
}