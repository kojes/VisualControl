using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VueExample.Providers;

namespace VueExample.Controllers
{
    [Route("api/[controller]/[action]")]

    public class DangerLevelController : Controller
    {
        private readonly DangerLevelProvider _dangerLevelProvider = new DangerLevelProvider();

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(_dangerLevelProvider.GetAll());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
            
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetById(int dangerLevelId)
        {
            var dangerLevel = await _dangerLevelProvider.GetByIdAsync(dangerLevelId);
            return CreatedAtAction(nameof(GetById), dangerLevel);
        }
    }
}
