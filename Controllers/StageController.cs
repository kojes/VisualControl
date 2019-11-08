using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VueExample.Providers;

namespace VueExample.Controllers
{

    [Route("api/[controller]/[action]")]
    public class StageController: Controller
    {
        private ProcessProvider processProvider = new ProcessProvider();
        private StageProvider stageProvider = new StageProvider();

        [HttpGet]
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
        public async Task<IActionResult> GetStagesByWaferId([FromQuery(Name = "waferId")] string waferId) 
        {
            var stageList = await stageProvider.GetStagesByWaferId(waferId); 
            return stageList.Count > 0 ? Ok(stageList) : (IActionResult)NotFound();
        }

        

        [HttpGet]
        public IActionResult GetById([FromQuery(Name = "stageId")] int stageId)
        {
            return Ok(stageProvider.GetById(stageId));
        }

    }
}
