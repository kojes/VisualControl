using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VueExample.Providers;

namespace VueExample.Controllers
{
    [Route("api/[controller]/[action]")]

    public class DangerLevelController : Controller
    {
        private readonly DangerLevelProvider _dangerLevelProvider = new DangerLevelProvider();
        private readonly IDefectProvider _defectProvider;

        public DangerLevelController(IDefectProvider defectProvider)
        {
            _defectProvider = defectProvider;
        }

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
        public IActionResult GetByWaferId(string waferId)
        {
            return Ok(JsonConvert.SerializeObject(_dangerLevelProvider.GetDangerLevelFromDefectList(_defectProvider.GetByWaferIdWithIncludes(waferId))));
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
