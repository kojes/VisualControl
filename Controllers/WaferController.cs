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
        private readonly WaferProvider _waferProvider = new WaferProvider();
        private readonly DefectProvider _defectProvider = new DefectProvider();
        
        [HttpGet]
        public IActionResult GetAll()
        {
           return Ok(_waferProvider.GetWafers());
        }

        [HttpGet]
        public IActionResult GetAllWithDefects()
        {
            return Ok(_defectProvider.GetAll().Select(x=>x.WaferId).Distinct());
        }
    }
}
