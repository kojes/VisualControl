using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VueExample.Models;
using VueExample.Providers.ChipVerification.Abstract;
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
        [Route("getall")]
        public IActionResult GetAll() 
        {
            return Ok(_deviceProvider.GetAll());
        }

        [HttpGet]
        [Route("av/measurementid/{measurementId:int}")]
        public async Task<IActionResult> GetAvailableByMeasurementId([FromRoute] int measurementId)
        {
            var deviceList = await _deviceProvider.GetAvailableByMeasurementId(measurementId);
            return Ok(deviceList);
        }

        [HttpPut]
        [ProducesResponseType(typeof(DeviceViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseObjects.Error), StatusCodes.Status409Conflict)]
        [Route("addnew")]
        public IActionResult Add([FromBody] DeviceViewModel deviceViewModel)
        {
            var response = _deviceProvider.Add(_mapper.Map<Device>(deviceViewModel));
            if(response.Item2.IsEmpty())
            {
                return CreatedAtRoute("addnew", _mapper.Map<DeviceViewModel>(response.Item1));
            }
            else
            {
                return Conflict(response.Item2);
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseObjects.Error), StatusCodes.Status409Conflict)]
        [Route("delete/{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var error = _deviceProvider.Delete(id);
            if(error.IsEmpty())
            {
                return NoContent();
            }
            else
            {
                return Conflict(error);
            }
        }
    }
}