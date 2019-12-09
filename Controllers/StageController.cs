using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VueExample.Models;
using VueExample.Providers;

namespace VueExample.Controllers
{

    [Route("api/[controller]")]
    public class StageController: Controller
    {
        private readonly ProcessProvider processProvider = new ProcessProvider();
        private readonly StageProvider stageProvider = new StageProvider();

        [HttpGet]
        [Route("GetStagesByCodeProductId")]
        public IActionResult GetStagesByCodeProductId([FromQuery(Name = "codeproductid")] int codeProductId)
        {
            try
            {
                var processId = processProvider.GetProcessIdByCodeProductId(codeProductId);
                return Ok(stageProvider.GetStagesByProcessId(processId));
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
            
        }        

        [HttpGet]
        [Route("GetStagesByWaferId")]
        public async Task<IActionResult> GetStagesByWaferId([FromQuery(Name = "waferId")] string waferId) 
        {
            var stageList = await stageProvider.GetStagesByWaferId(waferId); 
            return stageList.Count > 0 ? Ok(stageList) : (IActionResult)NotFound();
        }        

        [HttpGet]
        [Route("GetById")]
        public IActionResult GetById([FromQuery(Name = "stageId")] int stageId)
        {
            return Ok(stageProvider.GetById(stageId));
        }

        [HttpPut]
        [ProducesResponseType (typeof(Stage), StatusCodes.Status201Created)]
        [Route("create/name/{name}/codeProductId/{codeProductId:int}")]
        public async Task<IActionResult> Create([FromRoute] string name, [FromRoute] int codeProductId)
        {
           var processId = processProvider.GetProcessIdByCodeProductId(codeProductId);
           var newStage = await stageProvider.Create(name, processId);          
           return CreatedAtAction("", newStage);
        }

    }
}
