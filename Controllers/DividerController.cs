using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VueExample.Models.SRV6;
using VueExample.Providers.Srv6;

namespace VueExample.Controllers
{
    [Route("api/[controller]")]
    public class DividerController : Controller
    {
        DividerService dividerService = new DividerService();

        [HttpGet]
        [ProducesResponseType(typeof(Divider), StatusCodes.Status200OK)]
        [Route("all")]
        public IActionResult GetAll()
        {
            return Ok(dividerService.GetAll());
        }
    }
}