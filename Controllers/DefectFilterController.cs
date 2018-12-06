using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VueExample.Models;
using VueExample.Providers;
using VueExample.ViewModels;
using VueExample.ResponseObjects;

namespace VueExample.Controllers
{
    [Route("api/[controller]/[action]")]
    public class DefectFilterController : Controller
    {
        private StageProvider _stageProvider = new StageProvider();
        private DieProvider _dieProvider = new DieProvider();
        private DefectTypeProvider _defectTypeProvider = new DefectTypeProvider();
        private DangerLevelProvider _dangerLevelProvider = new DangerLevelProvider();
        

        [HttpGet]
        public IActionResult GetDefectFilter([FromQuery] string defects)
        {
            var defectList = JsonConvert.DeserializeObject<List<Defect>>(defects);
            if (defectList.Count == 0)
            {
                return Ok(new StandardResponseObject { ResponseType = "warning", ErrorCode = "DFC001" });
            }
            try
            {
                var defectFilter = new DefectFilterViewModel
                {
                    AvbDangerLevelList = _dangerLevelProvider.GetAll().FindAll(d =>
                        defectList.Select(x => x.DangerLevelId).Distinct().Contains(d.DangerLevelId)),
                    AvbDefectTypesList = _defectTypeProvider.GetAll().FindAll(d =>
                        defectList.Select(x => x.DefectTypeId).Distinct().Contains(d.DefectTypeId)),
                    AvbStagesList = _stageProvider.GetAll().FindAll(d =>
                        defectList.Select(x => x.StageId).Distinct().Contains(d.StageId)),
                    AvbDiesList = _dieProvider.GetAll().FindAll(d =>
                        defectList.Select(x => x.DieId).Distinct().Contains(d.DieId))
                };

                return Ok(new StandardResponseObject<DefectFilterViewModel> { ResponseType = "success", Body = defectFilter, ErrorCode = "OK"});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Ok(new StandardResponseObject {ResponseType = "error", ErrorCode = "UE001"});

            }
          

        }
    }
}
