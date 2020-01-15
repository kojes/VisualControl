using System.Collections.Generic;
using System.Linq;
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
        public async Task<AfterDbManipulationObject<MeasuredDevice>> Create(MeasuredDeviceViewModel measuredDeviceViewModel)
        {
            var obj = new AfterDbManipulationObject<MeasuredDevice>();
            if(HasDuplicate(measuredDeviceViewModel.Name, measuredDeviceViewModel.WaferId))
            {
                obj.AddError(new Error(@"Такое устройство уже добавлено"));
            }             
             
            if(obj.HasErrors)
            {
                return obj;
            }

            var measuredDevice = _mapper.Map<MeasuredDevice>(measuredDeviceViewModel);
            _applicationContext.Add(measuredDevice);
            await _applicationContext.SaveChangesAsync();
            obj.SetObject(measuredDevice);    
            return obj;
        }

        public async Task Delete(string waferId, string code)
        {
            var deleted = await _applicationContext.MeasuredDevice.FirstOrDefaultAsync(x => x.Name == code && x.WaferId == waferId);
            _applicationContext.MeasuredDevice.Remove(deleted);
            await _applicationContext.SaveChangesAsync();

        }

        public async Task<AfterDbManipulationObject<List<MeasuredDeviceViewModel>>> GetAll()
        {
            var obj = new AfterDbManipulationObject<List<MeasuredDeviceViewModel>>();
            var devicesList = await _applicationContext.MeasuredDevice.Select(x => _mapper.Map<MeasuredDeviceViewModel>(x)).ToListAsync();
            if(devicesList.Count() == 0)
            {
                 obj.AddError(new Error(@"Приборы не найдены"));
                 return obj;
            }
            obj.SetObject(devicesList);
            return obj;
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

        private bool HasDuplicate(string name, string waferId)
        {
            return _applicationContext.MeasuredDevice.Count(x => x.WaferId == waferId && x.Name == name) > 0;
        }
    }
}