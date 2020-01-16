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
using VueExample.StatisticsCore.Abstract;
using VueExample.ViewModels;

namespace VueExample.Controllers
{
    [Route("api/[controller]")]
    
    public class MeasurementRecordingController : Controller
    {
        private readonly MeasurementRecordingService measurementRecordingService = new MeasurementRecordingService();
        private readonly StageProvider _stageProvider = new StageProvider();
        private readonly IElementService _elementService;
        private readonly IExportProvider _exportProvider;
        private readonly IMapper _mapper;   
       
        public MeasurementRecordingController(IElementService elementService, IMapper mapper, IExportProvider exportProvider)
        {
            _elementService = elementService;
            _mapper = mapper;
            _exportProvider = exportProvider;
        }

        [HttpGet]
        [ProducesResponseType(typeof(MeasurementRecording), StatusCodes.Status200OK)]
        [Route("")]
        public IActionResult GetMeasurementRecordingsByWaferId([FromQuery] string waferId)
        {
            return Ok(measurementRecordingService.GetByWaferId(waferId));
        }

        [HttpPost]
        [ProducesResponseType(typeof(MeasurementRecording), StatusCodes.Status201Created)]
        [Route("getorcreate")]
        public async Task<IActionResult> GetOrCreate([FromBody] MeasurementRecordingWithBigMeasurementViewModel measurementRecordingWithBigMeasurementViewModel)
        {
            var measurementRecording = await measurementRecordingService.GetOrCreate(measurementRecordingWithBigMeasurementViewModel.Name, 
                                                                                2, 
                                                                                measurementRecordingWithBigMeasurementViewModel.BmrId, 
                                                                                measurementRecordingWithBigMeasurementViewModel.StageId);
            return Ok(measurementRecording);
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

        
        [HttpPost]
        [ProducesResponseType(typeof(BigMeasurementRecording), StatusCodes.Status200OK)]
        [Route("bmr/getorcreate")]
        public async Task<IActionResult> GetOrAddBigMeasurement([FromBody] BigMeasurementRecordingViewModel bigMeasurementRecordingViewModel)
        {
            var bigMeasurementRecording = await measurementRecordingService.GetOrAddBigMeasurement(bigMeasurementRecordingViewModel.Name, bigMeasurementRecordingViewModel.WaferId);
            return Ok(bigMeasurementRecording);
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
        [ProducesResponseType(typeof(List<MeasurementRecordingViewModel>), StatusCodes.Status200OK)]
        [Route("getbyelement")]
        public async Task<IActionResult> GetByWaferIdAndStageNameAndElementName([FromQuery] string waferId, [FromQuery] string stageName, [FromQuery] string elementName)
        {
            var element = await _elementService.GetByNameAndWafer(elementName, waferId);
            var mrList = new List<MeasurementRecordingViewModel>();
            if(element is null)
                return (IActionResult)NotFound();
            var measurementRecordingsList = measurementRecordingService.GetByWaferIdAndStageNameAndElementId(waferId, stageName, element.ElementId);
            foreach (var measurementRecording in measurementRecordingsList)
            {
                mrList.Add(new MeasurementRecordingViewModel {Id = measurementRecording.Id, 
                                                              Name = measurementRecording.Name, 
                                                              WaferId = waferId,
                                                              avStatisticParameters = await _exportProvider.GetStatisticsNameByMeasurementId(measurementRecording.Id)});
            }
            return mrList.Count == 0 ? (IActionResult)NotFound() : Ok(mrList);
        }        
    }
}