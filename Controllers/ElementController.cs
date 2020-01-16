using System.Data;
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
    public class ElementController : Controller
    {
        private readonly IElementService _elementService;
        private readonly IMapper _mapper;
        public ElementController(IElementService elementService, IMapper mapper)
        {
            _elementService = elementService;
            _mapper = mapper;
        }

        [HttpPut]
        [ProducesResponseType(typeof(ElementViewModel),StatusCodes.Status201Created)]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] JObject elementViewModelJObject)
        {
            var createdElement = await _elementService.Create(elementViewModelJObject.ToObject<ElementViewModel>());           
            return Created("", _mapper.Map<Element, ElementViewModel>(createdElement));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ElementViewModel),StatusCodes.Status200OK)]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] JObject elementJObject)
        {
            var elementViewModel = elementJObject.ToObject<ElementViewModel>();            
            return Ok(_mapper.Map<Element, ElementViewModel>(await _elementService.Update(elementViewModel)));
        }

        [HttpPut]
        [ProducesResponseType(typeof(System.Int32),StatusCodes.Status201Created)]
        [Route("{elementId:int}/dietype/{dieTypeId:int}")]
        public async Task<IActionResult> Put([FromRoute] int elementId, int dieTypeId)
        {
            await _elementService.AddToDieType(elementId, dieTypeId);
            return Created("", elementId);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ElementViewModel), StatusCodes.Status201Created)]
        [Route(("dietypeid/{id:int}/name/{name}"))]
        public async Task<IActionResult> GetByDieTypeAndName([FromRoute] int id, [FromRoute] string name)
        {
            var element = await _elementService.GetByDieTypeIdAndName(id, name);
            return element.ElementId == 0 ? (IActionResult)NotFound() : Ok(_mapper.Map<Element, ElementViewModel>(element));      
        }

        [HttpGet]
        [Route(("dietype/{id:int}"))]
        public async Task<IActionResult> GetByDieType([FromRoute] int id)
        {
            var elementList = await _elementService.GetByDieType(id);
            return elementList.Count > 0 ? Ok(elementList) : (IActionResult)NotFound();      
        }

        [HttpGet]
        [Route(("getbynameandwaferid"))]
        public async Task<IActionResult> GetByNameAndWaferId([FromQuery] string waferId, [FromQuery] string name)
        {
            var element = await _elementService.GetByNameAndWafer(name, waferId);
            return Ok(element);
        }

        [HttpGet]
        [Route("getbyidmr")]
        public async Task<IActionResult> GetByIdmr([FromQuery] int idmr)
        {
            var elementList = await _elementService.GetByIdmr(idmr);
            return elementList.Count > 0 ? Ok(elementList) : (IActionResult)NotFound();           
        }

        [HttpPost]
        [Route("updateElementOnIdmr")]
        public async Task<IActionResult> UpdateElementOnIdmr([FromBody] JObject ElementMeasurementRecordingChunkViewModelJObject)
        {
            var elementMeasurementRecordingChunkViewModel = ElementMeasurementRecordingChunkViewModelJObject.ToObject<ElementMeasurementRecordingChunkViewModel>();
            var element = await _elementService.UpdateElementOnIdmr(elementMeasurementRecordingChunkViewModel.MeasurementRecordingId, elementMeasurementRecordingChunkViewModel.ElementId);
            return Ok(_mapper.Map<ElementViewModel>(element));
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
           await _elementService.Delete(id);
           return Ok();
        }
    }
}