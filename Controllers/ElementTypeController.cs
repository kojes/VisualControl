using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VueExample.Models.SRV6;
using VueExample.Providers.Srv6.Interfaces;
using VueExample.ViewModels;

namespace VueExample.Controllers
{
    public class ElementTypeController : Controller
    {
        private readonly IElementTypeService _elementTypeService;
        private readonly IMapper _mapper;
        public ElementTypeController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IList<TypeElementViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType (StatusCodes.Status404NotFound)]
        [Route("all")]
        public async Task<IActionResult> GetAll()
        {
            var elementTypesList = _mapper.Map<IList<ElementType>, IList<TypeElementViewModel>>(await _elementTypeService.GetAll());
            return elementTypesList.Count > 0 ? Ok(elementTypesList) : (IActionResult)NotFound();
        }
    }
}