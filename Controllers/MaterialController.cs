using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VueExample.Providers;
using VueExample.ViewModels;

namespace VueExample.Controllers
{
    [Route("api/[controller]/[action]")]
    public class MaterialController : Controller
    {
        private readonly IMaterialProvider _materialProvider;
        public MaterialController(IMaterialProvider materialProvider)
        {
            _materialProvider = materialProvider;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public IActionResult ChangeMaterialName([FromBody] MaterialChangeViewModel materialViewModel)
        {
            return Ok();
        }        
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
             return Ok(_materialProvider.GetAll());
        }

    }
}