using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VueExample.Providers;

namespace VueExample.Controllers
{
    [Route("api/[controller]/[action]")]
    public class WaferController : Controller
    {
        private WaferProvider waferProvider;
        [HttpGet]
        public IActionResult GetAll()
        {
           return Ok(waferProvider.GetWafers());
        }
    }
}
