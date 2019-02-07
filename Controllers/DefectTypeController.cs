using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VueExample.Providers;
using VueExample.ViewModels;

namespace VueExample.Controllers
{
    [Route("api/[controller]/[action]")]
    public class DefectTypeController : Controller
    {
        readonly DefectTypeProvider _defectTypeProvider = new DefectTypeProvider();
        private readonly IDefectProvider _defectProvider;

        public DefectTypeController(IDefectProvider defectProvider)
        {
            _defectProvider = defectProvider;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(_defectTypeProvider.GetDefectTypes());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
            
        }

        [HttpGet]
        public IActionResult GetById(int defectTypeId)
        {
            return Ok(_defectTypeProvider.GetById(defectTypeId));
        }

        [HttpGet]
        public IActionResult GetByWaferId(string waferId)
        {
            return Ok(JsonConvert.SerializeObject(_defectTypeProvider.GetDefectTypesFromDefectList(_defectProvider.GetByWaferIdWithIncludes(waferId))));
        }

        [HttpPost]
        public IActionResult AddNewDefectType([FromBody] DefectTypeViewModel defectTypeViewModel)
        {
            var returnObject = _defectTypeProvider.AddDefectType(defectTypeViewModel.Description, defectTypeViewModel.Color);
            if (returnObject.HasErrors)
            {
                return BadRequest(returnObject.GetErrors());
            }

            return Ok(returnObject.TObject);

        }

        [HttpPost]
        public IActionResult DeleteDefectType([FromBody] DefectTypeViewModel defectTypeViewModel)
        {
            var returnObject = _defectTypeProvider.DeleteDefectType(defectTypeViewModel.Description);

            if (returnObject.HasErrors)
            {
                return BadRequest(returnObject.GetErrors());
            }

            return Ok(returnObject.TObject);
        }

    }
}
