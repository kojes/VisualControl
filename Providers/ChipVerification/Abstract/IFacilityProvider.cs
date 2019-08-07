using System.Threading.Tasks;
using System.Collections.Generic;
using VueExample.Models;

namespace VueExample.Providers.ChipVerification.Abstract
{
   public interface IFacilityProvider
   {
       Task<List<Facility>> GetAllAsync();
   }
}