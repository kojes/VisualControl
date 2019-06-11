using System.Collections.Generic;
using VueExample.Models;

namespace VueExample.Providers
{
    public interface IDeviceProvider
    {
         List<Device> GetAll();
    }
}