using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VueExample.Controllers
{
    [Route("api/[controller]/[action]")]
    public class DefectController : Controller
    {
        [HttpPost]
        public IActionResult SaveNewDefect()
        {
            var r = Request.Body;
            return BadRequest();
        }
    }
}
