using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LazyCache;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VueExample.Providers.Srv6.Interfaces;
using VueExample.ViewModels;

namespace VueExample.Controllers
{
    [Route("api/[controller]")]
    public class GradientController : Controller
    {
        private readonly IGradientService _gradientService;
        private readonly IAppCache _cache; 
        public GradientController(IAppCache cache, IGradientService gradientService)
        {
            _gradientService = gradientService;
            _cache = cache;
        }

        [HttpGet]
        [ProducesResponseType (typeof(GradientViewModel), StatusCodes.Status200OK)]
        [Route("statparameter")]
        public async Task<IActionResult> GetGradientStatParameter([FromQuery] string gradientViewModelJSON)
        {
            var gradientViewModel = JsonConvert.DeserializeObject<GradientStatViewModel>(gradientViewModelJSON);
            string measurementRecordingIdAsKey = Convert.ToString(gradientViewModel.MeasurementRecordingId);
            var singleParameterStatisticList = 
                (await _cache.GetAsync<Dictionary<string, List<VueExample.StatisticsCore.SingleParameterStatistic>>>($"S_{measurementRecordingIdAsKey}_KF_{gradientViewModel.K*10}"))[gradientViewModel.KeyGraphicState];
            var gradient = _gradientService.GetGradient(singleParameterStatisticList, gradientViewModel.StepsQuantity, gradientViewModel.Divider, gradientViewModel.StatParameter, gradientViewModel.SelectedDiesId);
            return gradient.GradientSteps.Count > 0 ? Ok(gradient) : (IActionResult)BadRequest(gradient);
        }

    }
}