using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using VueExample.Providers;
using VueExample.Extensions;
using VueExample.ViewModels;

namespace VueExample.Controllers
{
    [Route("api/[controller]/[action]")]
    public class MeasurementController : Controller
    {
        private readonly IMeasurementProvider measurementProvider;
        private readonly IMaterialProvider materialProvider;

        public MeasurementController(IMeasurementProvider measurementProvider, IMaterialProvider materialProvider)
        {
            this.measurementProvider = measurementProvider;
            this.materialProvider = materialProvider;
        }

        [HttpGet]
        public IActionResult FullInfo()
        {
            var measurementInfo = measurementProvider.GetAllMeasurementInfo();
            return Ok(measurementInfo);
        }

        [HttpGet]
        public IActionResult GetOnlineStatus([FromQuery(Name = "measurementid")] int measurementId)
        {
            var onlineStatus = measurementProvider.GetMeasurementOnlineStatus(measurementId);
            return Ok(onlineStatus);
        }

        [HttpGet]
        public IActionResult GetMaterial([FromQuery(Name = "measurementid")] int measurementId)
        {
           
            return Ok(measurementProvider.GetMaterial(measurementId));
        }

        [HttpPost]
        public IActionResult ChangeMaterial([FromBody] ChangeMaterialViewModel changeMaterialViewModel)
        {
            var newMaterial = materialProvider.ChangeMaterialOnMeasurement(changeMaterialViewModel.MeasurementId, changeMaterialViewModel.MaterialId);
            return Ok(newMaterial);
        }

        [HttpGet]
        public IActionResult GetExtraInfo([FromQuery(Name = "measurementid")] int measurementId)
        {
            return Ok(measurementProvider.GetPointsByMeasurementId(measurementId));
        }

        [HttpGet]
        public IActionResult GetPoints([FromQuery(Name = "measurementid")] int measurementId, [FromQuery(Name = "deviceid")] int deviceId, [FromQuery(Name = "graphicid")] int graphicId, [FromQuery(Name = "port")] int port)
        {
            var pointsDictionary = new Dictionary<string, PointsViewModel>();
            var pointViewModel = new PointsViewModel();
            var pointsList = new List<object>(measurementProvider.GetPoints(measurementId, deviceId, graphicId, port)
                .Select(x => new
                {
                    Value = x.Value, Time = x.Time.Trim(TimeSpan.TicksPerMinute), DeviceId = x.DeviceId,
                    PortNumber = x.PortNumber
                }));
            var k = Math.Ceiling((double)pointsList.Count / 500);
            var filteredPointsList = pointsList.GetNth(Convert.ToInt32(k));
            pointViewModel.PointsList.AddRange(filteredPointsList.ToList());
            pointViewModel.MeasurementName = measurementProvider.GetById(measurementId).Name;
            pointsDictionary.Add($"M{measurementId}D{deviceId}PN{port}", pointViewModel);
            if (pointsList.Count == 0)
            {
                return NoContent();
            }
            return Ok(pointsDictionary);
        }
    }
}
