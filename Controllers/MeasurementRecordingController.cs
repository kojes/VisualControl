using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VueExample.Models.SRV6;
using VueExample.Providers;
using VueExample.Providers.Srv6;
using VueExample.Providers.Srv6.Interfaces;
using VueExample.ViewModels;

namespace VueExample.Controllers
{
    [Route("api/[controller]")]
    
    public class MeasurementRecordingController : Controller
    {
        private readonly MeasurementRecordingService measurementRecordingService = new MeasurementRecordingService();
        private readonly StageProvider _stageProvider = new StageProvider();
        private readonly IElementService _elementService;
        private readonly IMapper _mapper;
        public MeasurementRecordingController(IElementService elementService, IMapper mapper)
        {
            _elementService = elementService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(MeasurementRecording), StatusCodes.Status200OK)]
        [Route("")]
        public IActionResult GetMeasurementRecordingsByWaferId([FromQuery] string waferId)
        {
            return Ok(measurementRecordingService.GetByWaferId(waferId));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<MeasurementRecordingWithStageAndElementViewModel>), StatusCodes.Status200OK)]
        [Route("getbywaferidwithoutstage")]
        public async Task<IActionResult> GetMeasurementRecordingsByWaferIdWithoutStage([FromQuery] string waferId)
        {
            var measurementRecordingList = measurementRecordingService.GetByWaferId(waferId).Where(x => x.StageId == null);
            var measurementRecordingViewModelList = new List<MeasurementRecordingWithStageAndElementViewModel>();
            foreach (var measurementRecording in measurementRecordingList)
            {
                var elementList = await _elementService.GetByIdmr(measurementRecording.Id);
                measurementRecordingViewModelList.Add(
                    new MeasurementRecordingWithStageAndElementViewModel  {   
                        Id = measurementRecording.Id, 
                        Name = measurementRecording.Name, 
                        ElementList = elementList.Select(x => _mapper.Map<ElementViewModel>(x)).ToList(), 
                        Stage = null});
            }
            return Ok(measurementRecordingViewModelList);

        }

        [HttpGet]
        [ProducesResponseType(typeof(MeasurementRecording), StatusCodes.Status200OK)]
        [Route("{id:int}")]
        public IActionResult GetMeasurementRecordingById([FromRoute] int id)
        {
            return Ok(measurementRecordingService.GetById(id));
        }

        [HttpGet]
        [ProducesResponseType(typeof(MeasurementRecording), StatusCodes.Status200OK)]
        [Route("getbyelement")]
        public async Task<IActionResult> GetByWaferIdAndStageNameAndElementName([FromQuery] string waferId, [FromQuery] string stageName, [FromQuery] string elementName)
        {
            var element = await _elementService.GetByNameAndWafer(elementName, waferId);
            if(element is null)
                return BadRequest();
            return Ok(measurementRecordingService.GetByWaferIdAndStageNameAndElementId(waferId, stageName, element.ElementId));
        }


        
    }
}