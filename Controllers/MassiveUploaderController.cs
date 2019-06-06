using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VueExample.Services;

namespace VueExample.Controllers
{
    [Route("api/[controller]/[action]")]
    public class MassiveUploaderController : Controller
    {
        [HttpGet]
        public IActionResult GetFolderDefects([FromQuery] string folderPath)
        {
            var massiveUploaderService = new MassiveUploaderService();
            return Ok(massiveUploaderService.FindDefectsInFolder(folderPath));
        }
    }
}
