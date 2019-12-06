using AutoMapper;
using VueExample.Models;
using VueExample.ViewModels;

namespace VueExample.Profiles
{
    public class WaferProfile : Profile
    {
        public WaferProfile()
        {
            CreateMap<WaferViewModel, Wafer>().ReverseMap();
        }
    }
}