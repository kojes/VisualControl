using AutoMapper;
using VueExample.Models;
using VueExample.ViewModels;

namespace VueExample.Profiles
{
    public class GraphicProfile : Profile
    {
        public GraphicProfile()
        {
            CreateMap<GraphicViewModel, Graphic>().ReverseMap();
        }
    }
}