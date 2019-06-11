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
        public List<Device> GetAll()
        {
            using(ApplicationContext applicationContext = new ApplicationContext())
            {
                return applicationContext.Device.ToList();
            }
        }
    }
}