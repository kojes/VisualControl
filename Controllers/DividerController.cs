using Microsoft.AspNetCore.Mvc;
using VueExample.Providers.Srv6;

namespace VueExample.Controllers
{
    [Route("api/[controller]/[action]")]
    public class DividerController : Controller
    {
        DividerService dividerService = new DividerService();

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(dividerService.GetAll());
        }
    }
}