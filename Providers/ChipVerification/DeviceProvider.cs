using System;
using System.Collections.Generic;
using VueExample.Models;
using VueExample.Contexts;
using System.Linq;
using VueExample.ResponseObjects;
using Microsoft.EntityFrameworkCore;
using VueExample.Providers.ChipVerification.Abstract;
using System.Threading.Tasks;

namespace VueExample.Providers.ChipVerification
{
    public class DeviceProvider : IDeviceProvider
    {
        private readonly ApplicationContext _applicationContext;
        public DeviceProvider(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        public List<Device> GetAll()
        {
            return _applicationContext.Device.ToList();            
        }

        public (Device, Error) Add(Device device) 
        {
            if(HasDuplicate("Address", device.Address))
                return (device, new Error("Такой адрес уже существует", "DPLCT"));

            if(HasDuplicate("Name", device.Name))
                return (device, new Error("Такое имя уже существует", "DPLCT"));

       
            _applicationContext.Add(device);
            _applicationContext.SaveChanges();

            return (device, new Error());
        }

        public Error Delete(int deviceId) 
        {
            try
            {
                var entry = _applicationContext.Entry( new Device { DeviceId = deviceId });
                entry.State = EntityState.Deleted;
                _applicationContext.SaveChanges();
                return new Error();
            }
            catch (Exception ex)
            {
                 return new Error(ex.Message, ex.HResult.ToString());            
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

        private bool HasDuplicate(string columnName, string newValue)
        {
            return _applicationContext.Device.Count(x => x.GetType().GetProperty(columnName).GetValue(x).ToString() == newValue) > 0;
        }

     
    }
}