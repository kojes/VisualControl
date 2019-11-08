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

        [HttpPost]
        [ProducesResponseType(typeof(MeasurementRecording), StatusCodes.Status200OK)]
        [Route("update-stage")]
        public async Task<IActionResult> UpdateStage([FromBody] StageMeasurementRecordingChunkViewModel stageMeasurementRecordingChunkViewModel)
        {
            var measurementRecording = await measurementRecordingService.UpdateStage(stageMeasurementRecordingChunkViewModel.MeasurementRecordingId, 
                                                                                     stageMeasurementRecordingChunkViewModel.StageId);
            return Ok(measurementRecording);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<StageFullViewModel>), StatusCodes.Status200OK)]
        [Route("wafer/{waferid}/stage")]
        public async Task<IActionResult> GetMeasurementRecordingWithStagesByWaferId([FromRoute] string waferId)
        {
            var measurementRecordingList = measurementRecordingService.GetByWaferId(waferId).Distinct();
            var stagesFullViewModelList = new List<StageFullViewModel>();
            var stagesList = measurementRecordingList.Select(x => x.StageId ?? 0).Distinct().ToList();
            stagesList.Remove(0);
            stagesList.Insert(0,0);
            foreach (var stage in stagesList)
            {
                stagesFullViewModelList.Add(new StageFullViewModel{
                    Id = stage,
                    Name = stage == 0 ? "Этап не выбран" : (await _stageProvider.GetByIdAsync(stage)).StageName,
                    MeasurementRecordingList = new List<MeasurementRecordingWithStageAndElementViewModel>()
                });
            }

            foreach (var measurementRecording in measurementRecordingList)
            {
                var elementList = await _elementService.GetByIdmr(measurementRecording.Id);
                var thisStage = stagesFullViewModelList.FirstOrDefault(x => x.Id == (measurementRecording.StageId ?? 0));
                thisStage.MeasurementRecordingList.Add(new MeasurementRecordingWithStageAndElementViewModel {
                                                        Id = measurementRecording.Id, 
                                                        Name = measurementRecording.Name, 
                                                        Element = elementList.Select(x => _mapper.Map<ElementViewModel>(x)).ToList().FirstOrDefault()});
            }

            return Ok(stagesFullViewModelList);
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