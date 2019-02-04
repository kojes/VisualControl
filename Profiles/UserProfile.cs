using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using VueExample.Models;
using VueExample.ViewModels;

namespace VueExample.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserViewModel, User>();
            CreateMap<RegistryViewModel, User>().ForMember(dest => dest.Username, opts => opts.MapFrom(src => src.Login));
        }
    }
}
