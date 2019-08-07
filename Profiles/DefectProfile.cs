using AutoMapper;
using VueExample.Models;
using VueExample.ViewModels;

namespace VueExample.Profiles
{
    public class DefectProfile : Profile
    {
        public DefectProfile()
        {
            CreateMap<DefectViewModel, Defect>();
        }
    }
}
