using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VueExample.Models;
using VueExample.Providers;
using VueExample.ViewModels;

namespace VueExample.Controllers
{
    [Route("api/[controller]/[action]")]
    public class DefectController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IDefectProvider _defectProvider;
        public DefectController(IMapper mapper, IDefectProvider defectProvider)
        {
            _mapper = mapper;
            _defectProvider = defectProvider;
        }

        [HttpPost]
        public IActionResult SaveNewDefect([FromBody]DefectViewModel defectViewModel)
        {

            defectViewModel.Date = DateTime.Now;
            var defect = _mapper.Map<Defect>(defectViewModel);
            return BadRequest(defect);
        }
    }
}
