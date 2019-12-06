using AutoMapper;
using VueExample.Models;
using VueExample.ViewModels;

namespace VueExample.Profiles
{
    public class CodeProductProfile : Profile
    {
        public CodeProductProfile()
        {
            CreateMap<CodeProductViewModel, CodeProduct>()
                .ForMember(dest => dest.IdCp, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CodeProductName, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();
        }
    }
}