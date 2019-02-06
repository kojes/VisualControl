using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VueExample.Providers;

namespace VueExample.Controllers
{
    [Route("api/[controller]/[action]")]
    public class CodeProductController : Controller
    {
        readonly CodeProductProvider _codeProductProvider = new CodeProductProvider();

        [HttpGet]
        public IActionResult GetByWaferId([FromQuery] string waferId)
        {
            var codeProduct = _codeProductProvider.GetByWaferId(waferId);
            if (codeProduct == null)
            {
                return BadRequest();
            }

            return Ok(codeProduct);

        }



    }
}
