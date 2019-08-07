using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VueExample.Contexts;
using VueExample.Models;
using VueExample.Providers.ChipVerification.Abstract;

namespace VueExample.Providers.ChipVerification
{
    public class FacilityProvider : IFacilityProvider
    {
        private readonly ApplicationContext _applicationContext;
        public FacilityProvider(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        public async Task<List<Facility>> GetAllAsync()
        {
           return await _applicationContext.Facility.ToListAsync();
        }
    }
}