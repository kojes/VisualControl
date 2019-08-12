using AutoMapper;
using VueExample.Models;
using VueExample.ViewModels;

namespace VueExample.Profiles
{
    public class MeasurementProfile : Profile
    {
        public MeasurementProfile()
        {
            CreateMap<MeasurementViewModel, Measurement>().ReverseMap();
        }
    }
}