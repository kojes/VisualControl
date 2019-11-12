using AutoMapper;
using VueExample.Models.SRV6;
using VueExample.ViewModels;

namespace VueExample.Profiles
{
    public class TypeElementProfile : Profile
    {
        public TypeElementProfile() 
        {
            CreateMap<ElementType, TypeElementViewModel>().ReverseMap();
        }
    }
}