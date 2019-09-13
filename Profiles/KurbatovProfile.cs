using AutoMapper;
using VueExample.Models.SRV6.Export;
using VueExample.ViewModels;

namespace VueExample.Profiles
{
    public class KurbatovProfile : Profile
    {
        public KurbatovProfile()
        {          
            CreateMap<KurbatovXLSViewModel, KurbatovXLS>().ForMember(s => s.kpList, c => c.MapFrom(m => m.parameters));            
            
        }
    }
}