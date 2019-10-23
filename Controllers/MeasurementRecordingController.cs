using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VueExample.Models.SRV6;
using VueExample.Providers.Srv6;
using VueExample.Providers.Srv6.Interfaces;
using VueExample.StatisticsCore.Abstract;
using VueExample.ViewModels;

namespace VueExample.Controllers
{
    [Route("api/[controller]")]
    
    public class MeasurementRecordingController : Controller
    {
        MeasurementRecordingService measurementRecordingService = new MeasurementRecordingService();
        private readonly IElementService _elementService;
        private readonly IExportProvider _exportProvider;
        public MeasurementRecordingController(IElementService elementService, IExportProvider exportProvider)
        {
            _elementService = elementService;
            _exportProvider = exportProvider;
        }

        [HttpGet]
        [ProducesResponseType(typeof(MeasurementRecording), StatusCodes.Status200OK)]
        [Route("")]
        public IActionResult GetMeasurementRecordingsByWaferId([FromQuery] string waferId)
        {
            return Ok(measurementRecordingService.GetByWaferId(waferId));
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