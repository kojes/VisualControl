using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VueExample.Providers.ChipVerification.Abstract;
using VueExample.ResponseObjects;
using VueExample.ViewModels;

namespace VueExample.Controllers
{
    [Route ("api/[controller]/")]
    public class DeviceTypeController : Controller
    {
        private readonly IDeviceTypeProvider _deviceTypeProvider;
        public DeviceTypeController(IDeviceTypeProvider deviceTypeProvider)
        {
            _deviceTypeProvider = deviceTypeProvider;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<DeviceTypeViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<Error>), StatusCodes.Status404NotFound)]
        [Route("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _deviceTypeProvider.GetAll();
            return result.HasErrors ? (IActionResult)NotFound(result.GetErrors()) : (IActionResult)Ok(result.TObject);
        }

        [HttpPut]
        [ProducesResponseType(typeof(DeviceTypeViewModel), StatusCodes.Status201Created)]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] DeviceTypeViewModel deviceTypeViewModel)
        {
           var device =  await _deviceTypeProvider.Create(deviceTypeViewModel);
           return Ok(device);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Route("modelName/{modelName}")]
        public async Task<IActionResult> Delete([FromRoute] string modelName)
        {
            await _deviceTypeProvider.Delete(modelName);
            return NoContent();
        }
    }
}