using AutoMapper;
using VueExample.Models;
using VueExample.ViewModels;

namespace VueExample.Profiles
{
    public class DeviceProfile : Profile
    {
        public DeviceProfile()
        {
            CreateMap<DeviceViewModel, Device>().ForMember(dest => dest.DeviceId, opt => opt.MapFrom(src => src.Id)).ReverseMap();
        }
    }
}