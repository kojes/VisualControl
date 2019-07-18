using System;
using System.Collections.Generic;
using System.Net.Mime;
using VueExample.Models;
using VueExample.Contexts;
using System.Linq;

namespace VueExample.Providers
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
    }
}