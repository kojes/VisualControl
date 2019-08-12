using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VueExample.Models;
using VueExample.Providers.ChipVerification.Abstract;
using VueExample.ResponseObjects;
using VueExample.ViewModels;

namespace VueExample.Controllers
{
    [Route ("api/[controller]/")]

    public class DeviceController : Controller 
    {
        private readonly IDeviceProvider _deviceProvider;
        private readonly IMapper _mapper;
        public DeviceController (IDeviceProvider deviceProvider, IMapper mapper) 
        {
            _deviceProvider = deviceProvider;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Device>), StatusCodes.Status200OK)]
        [Route("getall")]

        public IActionResult GetAll() 
        {
            return Ok(_deviceProvider.GetAll());
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Device>), StatusCodes.Status200OK)]
        [Route("av/measurementid/{measurementId:int}")]
        public async Task<IActionResult> GetAvailableByMeasurementId([FromRoute] int measurementId)
        {
            var deviceList = await _deviceProvider.GetAvailableByMeasurementId(measurementId);
            return Ok(deviceList);
        }

        [HttpGet]
        [ProducesResponseType(typeof(DeviceViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<Error>), StatusCodes.Status404NotFound)]
        [Route("getbyname/{name}")]
        public async Task<IActionResult> GetByName([FromRoute] string name)
        {
            var result = await _deviceProvider.GetByName(name);
            return result.HasErrors ? (IActionResult)NotFound(result.GetErrors()) : (IActionResult)Ok(_mapper.Map<DeviceViewModel>(result.TObject));
        }

        [HttpPut]
        [ProducesResponseType(typeof(DeviceViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseObjects.Error), StatusCodes.Status409Conflict)]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] DeviceViewModel deviceViewModel)
        {
            var result = await _deviceProvider.Create(deviceViewModel);
            return result.HasErrors ? (IActionResult)Conflict(result.GetErrors()) : (IActionResult)CreatedAtRoute("Create", _mapper.Map<DeviceViewModel>(result.TObject));           
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseObjects.Error), StatusCodes.Status409Conflict)]
        [Route("delete/{deviceId:int}")]
        public async Task<IActionResult> Delete([FromRoute] int deviceId)
        {
            var result = await _deviceProvider.Delete(deviceId);
            return result.HasErrors ? (IActionResult)Conflict(result.GetErrors()) : (IActionResult)NoContent();
        }

        [HttpPost]
        [ProducesResponseType(typeof(DeviceViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseObjects.Error), StatusCodes.Status409Conflict)]
        [Route("edit")]
        public async Task<IActionResult> Edit([FromBody] DeviceViewModel deviceViewModel)
        {
            var result = await _deviceProvider.Edit(deviceViewModel);
            return result.HasErrors ? (IActionResult)Conflict(result.GetErrors()) : (IActionResult)Ok(_mapper.Map<DefectViewModel>(result.TObject));

        }
    }
}