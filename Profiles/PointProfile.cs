using System;
using AutoMapper;
using VueExample.Extensions;
using VueExample.Models;
using VueExample.ViewModels;

namespace VueExample.Profiles
{
    public class PointProfile : Profile
    {
        public PointProfile()
        {
             CreateMap<PointViewModel, Point>().ForMember(dest => dest.Time, source => source.MapFrom(_ => _.Time.Trim(TimeSpan.TicksPerMinute)));
        }
        
    }
}