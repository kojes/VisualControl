using AutoMapper;
using VueExample.Models;
using VueExample.ViewModels;

namespace VueExample.Profiles
{
    public class MeasuredDeviceProfile : Profile
    {
        public MeasuredDeviceProfile()
        {
            CreateMap<MeasuredDeviceViewModel, MeasuredDevice>().ReverseMap();
        }
       
    }
}