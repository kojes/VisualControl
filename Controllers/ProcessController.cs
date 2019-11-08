using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VueExample.Models;
using VueExample.Providers;
using VueExample.ViewModels;

namespace VueExample.Controllers
{
    [Route("api/[controller]")]
    public class ProcessController : Controller
    {
        private readonly ProcessProvider _processProvider = new ProcessProvider();
        private readonly IMapper _mapper;
        public ProcessController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<ProcessViewModel>), StatusCodes.Status200OK)]
        [Route("all")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(Mapper.Map<List<Process>, List<ProcessViewModel>>(await _processProvider.GetAll()));
        }
    }
}