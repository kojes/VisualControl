using AutoMapper;
using VueExample.Entities;
using VueExample.ViewModels;

namespace VueExample.Profiles
{
    public class FKMRGraphicsProfile : Profile
    {
        public FKMRGraphicsProfile()
        {
            CreateMap<FKMRGraphicsViewModel, FkMrGraphic>().ReverseMap();
        }
    }
}