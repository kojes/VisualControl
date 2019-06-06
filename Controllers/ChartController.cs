using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VueExample.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ChartController : Controller
    {
        [HttpGet]
        public IActionResult GetBadGood(string waferId, string type)
        {
            if (type == "amcharts")
            {
               return RedirectToAction("GetBadGood", "AmChart", new {waferId = waferId});
            }

            return BadRequest();

        }

     
    }
}
