using Microsoft.AspNetCore.Mvc;
using VueExample.Providers.Srv6;

namespace VueExample.Controllers
{
    [Route("api/[controller]/[action]")]
    
    public class MeasurementRecordingController : Controller
    {
        MeasurementRecordingService measurementRecordingService = new MeasurementRecordingService();
        [HttpGet]
        public IActionResult GetMeasurementRecordingsByWaferId(string waferId)
        {
            return Ok(measurementRecordingService.GetByWaferId(waferId));
        }
        
    }
}