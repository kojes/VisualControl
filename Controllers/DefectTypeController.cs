using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Pages.Internal.Account.Manage;
using Microsoft.AspNetCore.Mvc;
using VueExample.Models;
using VueExample.Providers;

namespace VueExample.Controllers
{
    [Route("api/[controller]/[action]")]
    public class DefectTypeController : Controller
    {
        DefectTypeProvider defectTypeProvider = new DefectTypeProvider();
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(defectTypeProvider.GetDefectTypes());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
            
        }

        [HttpGet]
        public IActionResult GetById(int defecttypeId)
        {
            return Ok(defectTypeProvider.GetById(defecttypeId));
        }
    }
}
