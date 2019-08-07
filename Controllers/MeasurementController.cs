using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VueExample.Extensions;
using VueExample.ViewModels;
using Newtonsoft.Json;
using VueExample.Providers.ChipVerification.Abstract;
using System.Threading.Tasks;

namespace VueExample.Controllers
{
    [Route("api/[controller]")]
    public class MeasurementController : Controller
    {
        private readonly IMeasurementProvider _measurementProvider;
        private readonly IMaterialProvider _materialProvider;
     

        public MeasurementController(IMeasurementProvider measurementProvider, IMaterialProvider materialProvider)
        {
            _measurementProvider = measurementProvider;
            _materialProvider = materialProvider;
           
        }

       
        [HttpGet]
        [Route("fullinfo/{facilityId:int}")]
        public IActionResult FullInfo([FromRoute] int facilityId)
        {
            var measurementInfo = _measurementProvider.GetAllMeasurementInfo(facilityId);
            return Ok(measurementInfo);
        }

        [HttpGet]
        [Route("getonlinestatus")]
        public IActionResult GetOnlineStatus([FromQuery(Name = "measurementid")] int measurementId)
        {
            var onlineStatus = _measurementProvider.GetMeasurementOnlineStatus(measurementId);
            return Ok(onlineStatus);
        }

        [HttpGet]
        [Route("getmaterial")]
        public IActionResult GetMaterial([FromQuery(Name = "measurementid")] int measurementId)
        {
            var material = _measurementProvider.GetMaterial(measurementId);           
            return Ok(material);
        }

        [HttpPost]
        [Route("changematerial")]
        public IActionResult ChangeMaterial([FromBody] ChangeMaterialViewModel changeMaterialViewModel)
        {
            var newMaterial = _materialProvider.ChangeMaterialOnMeasurement(changeMaterialViewModel.MeasurementId, changeMaterialViewModel.MaterialId);
            return Ok(newMaterial);
        }

        [HttpGet]
        [Route("getextrainfo")]
        public IActionResult GetExtraInfo([FromQuery(Name = "measurementid")] int measurementId)
        {
            return Ok(_measurementProvider.GetPointsByMeasurementId(measurementId));
        }

        [HttpGet]
        [Route("getmeasurementstatistics")]
        public IActionResult GetMeasurementStatistics([FromQuery(Name = "atomiclist")] string atomicListJSON)
        {
            return Ok(_measurementProvider.GetMeasurementStatistics(JsonConvert.DeserializeObject<List<AtomicMeasurementExtendedViewModel>>(atomicListJSON)));
        }

        [HttpGet]
        [Route("getports/av/{measurementId:int}")]
        public async Task<IActionResult> GetAvailablePorts([FromRoute] int measurementId)
        {
           var ports = await _measurementProvider.GetAvailablePorts(measurementId);
           return Ok(ports);
        }

        [HttpGet]
        [Route("getpoints/withspaces")]
        public IActionResult GetPoints([FromQuery(Name = "measurementid")] int measurementId, [FromQuery(Name = "deviceid")] int deviceId, [FromQuery(Name = "graphicid")] int graphicId, [FromQuery(Name = "port")] int port)
        {
            var pointsDictionary = new Dictionary<string, PointsInMeasurementViewModel>();
            var pointViewModel = new PointsInMeasurementViewModel();
            var pointsList = new List<PointViewModel>(_measurementProvider.GetPoints(measurementId, deviceId, graphicId, port));

            var k = Math.Ceiling((double)pointsList.Count / 500);
            var filteredPointsList = pointsList.GetNth<PointViewModel>(Convert.ToInt32(k)).ToList();
            filteredPointsList.Add(pointsList.LastOrDefault());
            pointsDictionary.Add($"M{measurementId}D{deviceId}PN{port}", new PointsInMeasurementViewModel{
                                                                         PointsList = filteredPointsList,
                                                                         MeasurementName = _measurementProvider.GetById(measurementId).Name});
            if (pointsList.Count == 0)
            {
                return NoContent();
            }
            return Ok(pointsDictionary);
        }

        [HttpGet]
        [Route("getpoints/withoutspaces")]
        public IActionResult GetPointsWithoutSpaces([FromQuery(Name = "measurementid")] int measurementId, [FromQuery(Name = "deviceid")] int deviceId, [FromQuery(Name = "graphicid")] int graphicId, [FromQuery(Name = "port")] int port)
        {
            var pointsDictionary = new Dictionary<string, PointsInMeasurementViewModel>();
            var pointViewModel = new PointsInMeasurementViewModel();
            var pointsList = new List<PointViewModel>(_measurementProvider.GetPoints(measurementId, deviceId, graphicId, port));
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
            if (pointsList.Count == 0)
            {
                return NoContent();
            }
            return Ok(pointsDictionary);
        }
    }
}
