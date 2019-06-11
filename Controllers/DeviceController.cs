using Microsoft.AspNetCore.Mvc;
using VueExample.Providers;

namespace VueExample.Controllers {
    [Route ("api/[controller]/[action]")]
    public class DeviceController : Controller {
        private readonly IDeviceProvider _deviceProvider;
        public DeviceController (IDeviceProvider deviceProvider) {
            _deviceProvider = deviceProvider;
        }

        [HttpGet]
        public IActionResult GetAll () {
            return Ok (_deviceProvider.GetAll ());
        }
    }
}