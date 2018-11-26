using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VueExample.Providers;

namespace VueExample.Controllers
{
    [Route("api/[controller]/[action]")]
    public class DieController : Controller
    {
        readonly DieProvider dieProvider = new DieProvider();

        [HttpGet]
        public IActionResult GetByWaferId([FromQuery(Name = "waferid")] string waferId)
        {
            var graphic = dieProvider.GetDiesByWaferId(waferId);
            return Ok(graphic);
        }
    }
}
