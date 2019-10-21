using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VueExample.Models.SRV6;
using VueExample.Providers.Srv6;
using VueExample.Providers.Srv6.Interfaces;

namespace VueExample.Controllers
{
    [Route("api/[controller]")]
    
    public class MeasurementRecordingController : Controller
    {
        MeasurementRecordingService measurementRecordingService = new MeasurementRecordingService();
        private readonly IElementService _elementService;
        public MeasurementRecordingController(IElementService elementService)
        {
            _elementService = elementService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(MeasurementRecording), StatusCodes.Status200OK)]
        [Route("")]
        public IActionResult GetMeasurementRecordingsByWaferId([FromQuery] string waferId)
        {
            return Ok(measurementRecordingService.GetByWaferId(waferId));
        }

        [HttpGet]
        [ProducesResponseType(typeof(MeasurementRecording), StatusCodes.Status200OK)]
        [Route("{id:int}")]
        public IActionResult GetMeasurementRecordingById([FromRoute] int id)
        {
            return Ok(measurementRecordingService.GetById(id));
        }

        [HttpGet]
        [ProducesResponseType(typeof(MeasurementRecording), StatusCodes.Status200OK)]
        [Route("getbyelement")]
        public async Task<IActionResult> GetByWaferIdAndStageNameAndElementName([FromQuery] string waferId, [FromQuery] string stageName, [FromQuery] string elementName)
        {
            var element = await _elementService.GetByNameAndWafer(elementName, waferId);
            if(element is null)
                return BadRequest();
            return Ok(measurementRecordingService.GetByWaferIdAndStageNameAndElementId(waferId, stageName, element.ElementId));
        }


        
    }
}