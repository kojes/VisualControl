using System;
using System.Collections.Generic;
using VueExample.Models;
using VueExample.Contexts;
using System.Linq;
using VueExample.ResponseObjects;
using Microsoft.EntityFrameworkCore;
using VueExample.Providers.ChipVerification.Abstract;
using System.Threading.Tasks;
using VueExample.ViewModels;
using AutoMapper;

namespace VueExample.Providers.ChipVerification
{
    public class DeviceProvider : IDeviceProvider
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IMapper _mapper;
        public DeviceProvider(ApplicationContext applicationContext, IMapper mapper)
        {
            _applicationContext = applicationContext;
            _mapper = mapper;
        }
        public List<Device> GetAll()
        {
            return _applicationContext.Device.ToList();            
        }

        public async Task<AfterDbManipulationObject<Device>> Create(DeviceViewModel deviceViewModel) 
        {
               
            var obj = new AfterDbManipulationObject<Device>();
            if(HasDuplicate("Name", deviceViewModel.Name))
            {
                obj.AddError(new Error(@"Такое имя уже существует"));
            }             
             
            if(obj.HasErrors)
            {
                return obj;
            }

            var device = _mapper.Map<Device>(deviceViewModel);
            _applicationContext.Add(device);
            await _applicationContext.SaveChangesAsync();
            obj.SetObject(device);    
            return obj;
        }

        public async Task<AfterDbManipulationObject<Device>> Delete(int deviceId) 
        {
            try
            {
                var entry = _applicationContext.Entry( new Device { DeviceId = deviceId });
                entry.State = EntityState.Deleted;
                await _applicationContext.SaveChangesAsync();
                return new AfterDbManipulationObject<Device>("DELETE");
            }
            catch (Exception ex)
            {
                 return new AfterDbManipulationObject<Device>(new Error(ex.Message, ex.HResult.ToString()));            
            }
        }

        public async Task<List<Device>> GetAvailableByMeasurementId(int measurementId)
        {
            return await _applicationContext.Point.Where(x => x.MeasurementId == measurementId).Join(_applicationContext.Device, 
                                                        p => p.DeviceId, 
                                                        d => d.DeviceId, 
                                                        (p, d) => new Device { DeviceId = p.DeviceId, 
                                                                               Address = d.Address, Model = d.Model, 
                                                                               Name = d.Name})
                                                        .Distinct().ToListAsync();
        }

        public async Task<AfterDbManipulationObject<Device>> Edit(DeviceViewModel deviceViewModel)
        {
            var obj = new AfterDbManipulationObject<Device>();
            var editedDevice = await _applicationContext.Device.FirstOrDefaultAsync(x => x.DeviceId == deviceViewModel.Id);
            if(editedDevice is null)
            {
                obj.AddError(new Error("@Прибор не найден"));
                return obj;
            }
            else
            {
                if(editedDevice.Model != deviceViewModel.Model)
                {
                    obj.AddError(new Error("@Не разрешается изменять модель"));
                }

                if(editedDevice.Name != deviceViewModel.Name)
                {
                    obj.AddError(new Error("@Не разрешается изменять имя прибора"));
                }

                if(obj.HasErrors)
                {
                    return obj;
                }
                else
                {
                    editedDevice = _mapper.Map<Device>(deviceViewModel);
                    await _applicationContext.SaveChangesAsync();
                    obj.SetObject(editedDevice);
                    return obj;
                }
            }  
        }

        public async Task<AfterDbManipulationObject<Device>> GetByName(string name)
        {          
            var device = await _applicationContext.Device.FirstOrDefaultAsync(x => x.Name == name);
            var obj = new AfterDbManipulationObject<Device>(device);
            if(device is null)
            {
                obj.AddError(new Error("@Прибор не найден"));
            }
            return obj;
        }

        private bool HasDuplicate(string columnName, string newValue)
        {
            return _applicationContext.Device.Count(x => x.GetType().GetProperty(columnName).GetValue(x).ToString() == newValue) > 0;
        }

       
    }
}