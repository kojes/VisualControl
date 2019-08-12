using AutoMapper;
using VueExample.Models;
using VueExample.ViewModels;

namespace VueExample.Profiles
{
    public class FacilityProfile : Profile
    {
        public FacilityProfile()
        {
            CreateMap<Facility, FacilityViewModel>().ReverseMap();
        }
    }
}