using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VueExample.Providers.ChipVerification;
using VueExample.ViewModels;

namespace VueExample.Controllers
{
    
    [Route("api/[controller]")]
    public class MeasuredDeviceController : Controller
    {
        private readonly MeasuredDeviceProvider _measuredDeviceProvider;
        public MeasuredDeviceController(MeasuredDeviceProvider measuredDeviceProvider)
        {
            _measuredDeviceProvider = measuredDeviceProvider;
        }

        [HttpGet]
        [ProducesResponseType(typeof(MeasuredDeviceViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ResponseObjects.Error>), StatusCodes.Status404NotFound)]
        [Route("getbyid/{measuredDeviceId:int}")]
        public async Task<IActionResult> GetById([FromRoute] int measuredDeviceId)
        {
             var result = await _measuredDeviceProvider.GetById(measuredDeviceId);
             return result.HasErrors ? (IActionResult)NotFound(result.GetErrors()) : (IActionResult)Ok(result.TObject);
        }

        [HttpGet]
        [ProducesResponseType(typeof(MeasuredDeviceViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ResponseObjects.Error>), StatusCodes.Status404NotFound)]
        [Route("getbywaferandcode")]
        public async Task<IActionResult> GetByWaferIdAndCode([FromQuery] string waferId, [FromQuery] string code)
        {
            var result = await _measuredDeviceProvider.GetByWaferIdAndCode(waferId, code);
            return result.HasErrors ? (IActionResult)NotFound(result.GetErrors()) : (IActionResult)Ok(result.TObject);
        }



    }
}