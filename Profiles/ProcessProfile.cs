using AutoMapper;
using VueExample.Models;
using VueExample.ViewModels;

namespace VueExample.Profiles
{
    public class ProcessProfile : Profile
    {
        public ProcessProfile()
        {
            CreateMap<ProcessViewModel, Process>().ReverseMap();            
        }
       
    }
}