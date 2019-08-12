using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VueExample.Contexts;
using VueExample.Models;
using VueExample.Providers.ChipVerification.Abstract;
using VueExample.ResponseObjects;
using VueExample.ViewModels;

namespace VueExample.Providers.ChipVerification
{
    public class MeasuredDeviceProvider : IMeasuredDeviceProvider
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IMapper _mapper;
        public MeasuredDeviceProvider(ApplicationContext applicationContext, IMapper mapper)
        {
            _applicationContext = applicationContext;
            _mapper = mapper;
        }
        public Task<AfterDbManipulationObject<MeasuredDevice>> Create(MeasuredDeviceViewModel measuredDeviceViewModel)
        {
            throw new System.NotImplementedException();
        }

        public async Task<AfterDbManipulationObject<MeasuredDevice>> GetById(int measuredDeviceId)
        {
            var measuredDevice = await _applicationContext.MeasuredDevice.FindAsync(measuredDeviceId);
            var obj = new AfterDbManipulationObject<MeasuredDevice>(measuredDevice);
            if(measuredDevice is null)
            {
                obj.AddError(new Error(@"Кристалл не найден"));  
            }
            return obj;
        }

        public async Task<AfterDbManipulationObject<MeasuredDevice>> GetByWaferIdAndCode(string waferId, string code)
        {
            var measuredDevice = await _applicationContext.MeasuredDevice.FirstOrDefaultAsync(x => x.WaferId == waferId && x.Name == code);
            var obj = new AfterDbManipulationObject<MeasuredDevice>(measuredDevice);
            if(measuredDevice is null)
            {
                obj.AddError(new Error(@"Кристалл не найден"));  
            }
            return obj;
        }
    }
}