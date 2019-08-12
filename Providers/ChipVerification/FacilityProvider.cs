using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VueExample.Contexts;
using VueExample.Models;
using VueExample.Providers.ChipVerification.Abstract;
using VueExample.ResponseObjects;
using VueExample.ViewModels;

namespace VueExample.Providers.ChipVerification
{
    public class FacilityProvider : IFacilityProvider
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IMapper _mapper;
        public FacilityProvider(ApplicationContext applicationContext, IMapper mapper)
        {
            _applicationContext = applicationContext;
            _mapper = mapper;
        }

        public async Task<AfterDbManipulationObject<List<FacilityViewModel>>> GetAllAsync()
        {
            var facilityList = await _applicationContext.Facility.Join(_applicationContext.Measurement, 
                                                           f => f.FacilityId, m => m.FacilityId, 
                                                           (f,m) => new Facility{FacilityId = m.FacilityId, Name = f.Name})
                                                           .Distinct().ToListAsync();

            var obj = new AfterDbManipulationObject<List<FacilityViewModel>>();
            if(facilityList.Count == 0)
            {
                obj.AddError(new Error("@Установки не найдены"));
                return obj;
            }
            obj.SetObject(facilityList.Select(x => _mapper.Map<FacilityViewModel>(x)).ToList());
            return obj;
        }
    }
}