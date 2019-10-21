using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VueExample.Providers.ChipVerification.Abstract;
using VueExample.ViewModels;

namespace VueExample.Controllers
{
    
    [Route("api/[controller]")]
    public class MeasuredDeviceController : Controller
    {
        private readonly IMeasuredDeviceProvider _measuredDeviceProvider;
        private readonly IMapper _mapper;
        public MeasuredDeviceController(IMeasuredDeviceProvider measuredDeviceProvider, IMapper mapper)
        {
            _measuredDeviceProvider = measuredDeviceProvider;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<MeasuredDeviceViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ResponseObjects.Error>), StatusCodes.Status404NotFound)]
        [Route("all")]
        public async Task<IActionResult> GetAll()
        {
             var result = await _measuredDeviceProvider.GetAll();
             return result.HasErrors ? (IActionResult)NotFound(result.GetErrors()) : (IActionResult)Ok(result.TObject);
        }

        [HttpGet]
        [ProducesResponseType(typeof(MeasuredDeviceViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ResponseObjects.Error>), StatusCodes.Status404NotFound)]
        [Route("getbyid/{measuredDeviceId:int}")]
        public async Task<IActionResult> GetById([FromRoute] int measuredDeviceId)
        {
             var result = await _measuredDeviceProvider.GetById(measuredDeviceId);
             return result.HasErrors ? (IActionResult)NotFound(result.GetErrors()) : (IActionResult)Ok(result.TObject);
        }

        [HttpGet]
        [ProducesResponseType(typeof(MeasuredDeviceViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<ResponseObjects.Error>), StatusCodes.Status404NotFound)]
        [Route("getbywaferandcode")]
        public async Task<IActionResult> GetByWaferIdAndCode([FromQuery] string waferId, [FromQuery] string code)
        {
            var result = await _measuredDeviceProvider.GetByWaferIdAndCode(waferId, code);
            return result.HasErrors ? (IActionResult)NotFound(result.GetErrors()) : (IActionResult)Ok(result.TObject);
        }

        [HttpPut]
        [ProducesResponseType(typeof(DeviceViewModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseObjects.Error), StatusCodes.Status409Conflict)]
        [Route("")]
        public async Task<IActionResult> Create([FromBody] Newtonsoft.Json.Linq.JObject deviceViewModel)
        {
            var result = await _measuredDeviceProvider.Create(deviceViewModel.ToObject<MeasuredDeviceViewModel>());
            return result.HasErrors ? (IActionResult)Conflict(result.GetErrors()) : (IActionResult)CreatedAtAction("Create", _mapper.Map<MeasuredDeviceViewModel>(result.TObject));           
        }



    }
}