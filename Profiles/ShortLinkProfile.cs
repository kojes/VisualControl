using AutoMapper;
using VueExample.Entities;
using VueExample.Models.SRV6;
using VueExample.ViewModels;

namespace VueExample.Profiles
{
    public class ShortLinkProfile : Profile
    {
        public ShortLinkProfile()
        {
             CreateMap<ShortLinkEntity, ShortLink>().ForMember(dest => dest.GeneratedId, source => source.MapFrom(_ => _.GeneratedId));
             CreateMap<ShortLink, ShortLinkInfoViewModel>();
        }
        
    }
}