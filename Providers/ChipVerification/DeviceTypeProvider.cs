using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VueExample.Contexts;
using VueExample.Providers.ChipVerification.Abstract;
using VueExample.ResponseObjects;
using VueExample.ViewModels;

namespace VueExample.Providers.ChipVerification
{
    public class DeviceTypeProvider : IDeviceTypeProvider
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IMapper _mapper;
        public DeviceTypeProvider(ApplicationContext applicationContext, IMapper mapper)
        {
            _applicationContext = applicationContext;
            _mapper = mapper;
        }
        public async Task<AfterDbManipulationObject<List<DeviceTypeViewModel>>> GetAll()
        {
            var obj = new AfterDbManipulationObject<List<DeviceTypeViewModel>>();
            var deviceTypesList = await _applicationContext.DeviceType.Select(x => _mapper.Map<DeviceTypeViewModel>(x)).ToListAsync();
            if(deviceTypesList.Count() == 0)
            {
                 obj.AddError(new Error(@"Типы приборов не найдены"));
                 return obj;
            }
            obj.SetObject(deviceTypesList);
            return obj;
        }
    }
}