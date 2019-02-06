using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VueExample.Providers;
using VueExample.Services;
using VueExample.ViewModels;

namespace VueExample.Controllers
{
    [Route("api/[controller]/[action]")]
    public class WaferMapController : Controller
    {
        private readonly IDieProvider _dieProvider;
       

        public WaferMapController(IDieProvider dieProvider)
        {
            _dieProvider = dieProvider;
          
        }

        [HttpPost]
        public IActionResult GetFormedWaferMap([FromBody] WaferMapFieldViewModel waferMapFieldViewModel)
        {
            var diesList = _dieProvider.GetDiesByWaferId(waferMapFieldViewModel.WaferId);
            if (diesList.Count == 0)
            {
                return BadRequest();
            }
            var waferMapFormed = new WaferMapFormationService(waferMapFieldViewModel.FieldHeight,
                waferMapFieldViewModel.FieldWidth, waferMapFieldViewModel.StreetSize, diesList).GetFormedWaferMap();
            return Ok(waferMapFormed);
        }
    }
}
