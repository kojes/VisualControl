using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VueExample.Extensions;
using VueExample.ViewModels;
using Newtonsoft.Json;
using VueExample.Providers.ChipVerification.Abstract;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using VueExample.Models;
using VueExample.ResponseObjects;
using Newtonsoft.Json.Linq;

namespace VueExample.Controllers
{
    [Route("api/[controller]")]
    public class MeasurementController : Controller
    {
        private readonly IMeasurementProvider _measurementProvider;          
        public MeasurementController(IMeasurementProvider measurementProvider)
        {
            _measurementProvider = measurementProvider;                   
        }

        [HttpPut]
        [ProducesResponseType(typeof(MeasurementViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(List<ResponseObjects.Error>), StatusCodes.Status409Conflict)]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] JObject measurementViewModel)
        {
            var result = await _measurementProvider.Create(measurementViewModel.ToObject<MeasurementViewModel>());
            return result.HasErrors ? (IActionResult)Conflict(result.GetErrors()) : (IActionResult)CreatedAtAction("Create", result.TObject);         
        }

        [HttpDelete]     
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(List<ResponseObjects.Error>), StatusCodes.Status409Conflict)]
        [Route("delete/{measurementId:int}")]
        public async Task<IActionResult> Delete([FromRoute] int measurementId)
        {
            var result = await _measurementProvider.Delete(measurementId);
            return result.HasErrors ? (IActionResult)Conflict(result.GetErrors()) : (IActionResult)NoContent();
        }

        [HttpGet]
        [ProducesResponseType(typeof(MeasurementViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<Error>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByMeasuredDeviceIdAndName([FromQuery(Name = "measureddeviceid")] int measuredDeviceId, [FromQuery(Name = "name")] string name)
        {
             var result = await _measurementProvider.GetByMeasuredDeviceIdAndName(measuredDeviceId, name);
             return result.HasErrors ? (IActionResult)NotFound(result.GetErrors()) : (IActionResult)Ok(result.TObject);
        }        
       
        [HttpGet]
        [Route("fullinfo/{facilityId:int}")]
        public IActionResult FullInfo([FromRoute] int facilityId)
        {
            var measurementInfo = _measurementProvider.GetAllMeasurementInfo(facilityId);
            return Ok(measurementInfo);
        }

        [HttpGet]
        [ProducesResponseType(typeof(MeasurementOnlineStatus), StatusCodes.Status200OK)]
        [Route("getonlinestatus")]
        public IActionResult GetOnlineStatus([FromQuery(Name = "measurementid")] int measurementId)
        {
            var onlineStatus = _measurementProvider.GetMeasurementOnlineStatus(measurementId);
            return Ok(onlineStatus);
        }     
     
        [HttpGet]
        [ProducesResponseType(typeof(List<MeasurementStatisticsViewModel>), StatusCodes.Status200OK)]
        [Route("getmeasurementstatistics")]
        public IActionResult GetMeasurementStatistics([FromQuery(Name = "atomiclist")] string atomicListJSON)
        {
            return Ok(_measurementProvider.GetMeasurementStatistics(JsonConvert.DeserializeObject<List<AtomicMeasurementExtendedViewModel>>(atomicListJSON)));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<int>), StatusCodes.Status200OK)]
        [Route("getports/av/{measurementId:int}")]
        public async Task<IActionResult> GetAvailablePorts([FromRoute] int measurementId)
        {
           var ports = await _measurementProvider.GetAvailablePorts(measurementId);
           return Ok(ports);
        }

        
    }
}
