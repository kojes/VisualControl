using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VueExample.Models;
using VueExample.Providers;

namespace VueExample.Controllers
{
    [Route("api/[controller]/[action]")]

    public class DangerLevelController : Controller
    {
        DangerLevelProvider dangerLevelProvider = new DangerLevelProvider();

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(dangerLevelProvider.GetAll());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
            
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult GetById(int dangerlevelId)
        {
            return Ok(dangerLevelProvider.GetByIdAsync(dangerlevelId));
        }
    }
}
