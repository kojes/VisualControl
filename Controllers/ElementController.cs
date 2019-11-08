using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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
    }
}