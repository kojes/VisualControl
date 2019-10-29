using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VueExample.Providers.Srv6.Interfaces;

namespace VueExample.Controllers
{
    [Route("api/[controller]")]
    public class ElementController : Controller
    {
        private readonly IElementService _elementService;
        public ElementController(IElementService elementService)
        {
            _elementService = elementService;
        }
        [HttpGet]
        [Route(("getbynameandwaferid"))]
        public async Task<IActionResult> GetByNameAndWaferId([FromQuery] string waferId, [FromQuery] string name)
        {
            var element = await _elementService.GetByNameAndWafer(name, waferId);
            return Ok(element);
        }

        [HttpGet]
        [Route("getbyidmr")]
        public async Task<IActionResult> GetByIdmr([FromQuery] int idmr)
        {
            var elementList = await _elementService.GetByIdmr(idmr);
            return elementList.Count > 0 ? Ok(elementList) : (IActionResult)NotFound();           
        }
    }
}