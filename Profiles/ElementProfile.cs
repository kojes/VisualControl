using AutoMapper;
using VueExample.Models.SRV6;
using VueExample.ViewModels;

namespace VueExample.Profiles
{
    public class ElementProfile : Profile
    {
        public ElementProfile()
        {
            CreateMap<ElementViewModel, Element>().ReverseMap();
        }
    }
}