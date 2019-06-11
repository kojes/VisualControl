using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VueExample.Providers;
using VueExample.Extensions;
using VueExample.ViewModels;

namespace VueExample.Controllers
{
    [Route("api/[controller]")]
    public class MeasurementController : Controller
    {
        private readonly IMeasurementProvider measurementProvider;

        public MeasurementController(IMeasurementProvider measurementProvider)
        {
            this.measurementProvider = measurementProvider;
        }

        [HttpGet("[action]")]
        public IActionResult FullInfo()
        {
            var measurementInfo = measurementProvider.GetAllMeasurementInfo();
            return Ok(measurementInfo);
        }

        [HttpGet("[action]")]
        public IActionResult GetExtraInfo([FromQuery(Name = "measurementid")] int measurementId)
        {
            return Ok(measurementProvider.GetPointsByMeasurementId(measurementId));
        }

        [HttpGet("[action]")]
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
