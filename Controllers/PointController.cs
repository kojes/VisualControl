using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VueExample.Extensions;
using VueExample.Providers.ChipVerification.Abstract;
using VueExample.ViewModels;

namespace VueExample.Controllers
{
    [Route("api/[controller]")]

    public class PointController : Controller
    {
        private readonly IPointProvider _pointProvider;
        private readonly IMeasurementProvider _measurementProvider;
        public PointController(IPointProvider pointProvider, IMeasurementProvider measurementProvider)
        {
            _pointProvider = pointProvider;
            _measurementProvider = measurementProvider;            
        }

        [HttpGet]
        [ProducesResponseType(typeof(Dictionary<string, PointsInMeasurementViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ResponseObjects.Error>), StatusCodes.Status404NotFound)]
        [Route("get/withspaces")]
        public async Task<IActionResult> GetPoints([FromQuery(Name = "measurementid")] int measurementId, [FromQuery(Name = "deviceid")] int deviceId, [FromQuery(Name = "graphicid")] int graphicId, [FromQuery(Name = "port")] int port)
        {
            var pointsDictionary = new Dictionary<string, PointsInMeasurementViewModel>();
            var pointViewModel = new PointsInMeasurementViewModel();
            var result = await _pointProvider.GetPoints(measurementId, deviceId, graphicId, port);
            if(result.HasErrors)
            {
                return NotFound(result.GetErrors());
            }
            var pointsList = result.TObject;
            var k = Math.Ceiling((double)pointsList.Count / 500);
            var filteredPointsList = pointsList.GetNth<PointViewModel>(Convert.ToInt32(k)).ToList();
            filteredPointsList.Add(pointsList.LastOrDefault());
            pointsDictionary.Add($"M{measurementId}D{deviceId}PN{port}", new PointsInMeasurementViewModel{
                                                                         PointsList = filteredPointsList,
                                                                         MeasurementName = _measurementProvider.GetById(measurementId).Name});
            return Ok(pointsDictionary);
        }

        [HttpGet]
        [ProducesResponseType(typeof(Dictionary<string, PointsInMeasurementViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ResponseObjects.Error>), StatusCodes.Status404NotFound)]
        [Route("get/withoutspaces")]        
        public async Task<IActionResult> GetPointsWithoutSpaces([FromQuery(Name = "measurementid")] int measurementId, [FromQuery(Name = "deviceid")] int deviceId, [FromQuery(Name = "graphicid")] int graphicId, [FromQuery(Name = "port")] int port)
        {
            var pointsDictionary = new Dictionary<string, PointsInMeasurementViewModel>();
            var pointViewModel = new PointsInMeasurementViewModel();
            var result = await _pointProvider.GetPoints(measurementId, deviceId, graphicId, port);
            if(result.HasErrors)
            {
                return NotFound(result.GetErrors());
            }
            var pointsList = result.TObject;
            var measurement = _measurementProvider.GetById(measurementId);
            
            for (var i = 0; i < pointsList.Count; i++)
            {

                if (i > 0 && (pointsList[i].Time - pointsList[i - 1].Time).TotalSeconds > 2*measurement.IntervalInSeconds)
                {
                    var spaceduration = pointsList[i].Time  - pointsList[i - 1].Time;
                    for (var j = i; j < pointsList.Count; j++) 
                    {
                        pointsList[j].Time = pointsList[j].Time - spaceduration;
                    }
                }                                   
            }


            var k = Math.Ceiling((double)pointsList.Count / 500);
            var filteredPointsList = pointsList.GetNth<PointViewModel>(Convert.ToInt32(k)).ToList();
            filteredPointsList.Add(pointsList.LastOrDefault());
            pointsDictionary.Add($"M{measurementId}D{deviceId}PN{port}", new PointsInMeasurementViewModel{
                                                                         PointsList = filteredPointsList,
                                                                         MeasurementName = measurement.Name});
            return Ok(pointsDictionary);
        }

        [EnableCors]
        [HttpPost]
        [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
        [Route("createsinglepoint")]
        public async Task<IActionResult> CreateSinglePoint([FromBody] PointViewModel pointViewModel)
        {
            var result = await _pointProvider.CreateSinglePoint(pointViewModel);
            return CreatedAtAction("CreateSinglePoint", result.TObject.PointId);
        }

        [EnableCors]
        [HttpPost]
        [ProducesResponseType(typeof(List<long>), StatusCodes.Status201Created)]
        [Route("createpointset")]
        public async Task<IActionResult> CreatePointSet ([FromBody] PointSetViewModel pointSetViewModel)
        {
            var result = await _pointProvider.CreatePointSet(pointSetViewModel);
            return CreatedAtAction("CreatePointSet", result.TObject);
        }
    }
}