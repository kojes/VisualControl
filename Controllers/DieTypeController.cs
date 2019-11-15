using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using VueExample.Models.SRV6;
using VueExample.Providers.Srv6.Interfaces;
using VueExample.ViewModels;

namespace VueExample.Controllers
{
    [Route("api/[controller]")]
    public class DieTypeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IDieTypeProvider _dieTypeProvider;
        public DieTypeController(IDieTypeProvider dieTypeProvider, IMapper mapper)
        {
            _mapper = mapper;
            _dieTypeProvider = dieTypeProvider;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<DieTypeViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("all")]
        public async Task<IActionResult> GetAll()
        {
            var dieTypeList = await _dieTypeProvider.GetAll();
            return dieTypeList.Count > 0 ? Ok(_mapper.Map<List<DieType>, List<DieTypeViewModel>>(dieTypeList)) : (IActionResult)NotFound();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] JObject dieTypeUpdatingJObject)
        {           
            var dieTypeUpdatingViewModel = dieTypeUpdatingJObject.ToObject<DieTypeUpdatingViewModel>();
            var dieType = await _dieTypeProvider.Create(dieTypeUpdatingViewModel);
            return Created("", dieType.Name);
        }
    }
}