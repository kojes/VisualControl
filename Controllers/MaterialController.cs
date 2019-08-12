using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VueExample.Providers.ChipVerification.Abstract;
using VueExample.ResponseObjects;
using VueExample.ViewModels;

namespace VueExample.Controllers
{
    [Route("api/[controller]")]
    
    public class MaterialController : Controller
    {
        private readonly IMaterialProvider _materialProvider;
        public MaterialController(IMaterialProvider materialProvider)
        {
            _materialProvider = materialProvider;
        }      

        [HttpGet]
        [ProducesResponseType(typeof(MaterialViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<Error>), StatusCodes.Status404NotFound)]
        [Route("getbymeasurementid/{measurementId:int}")]
        public async Task<IActionResult> GetByMeasurementId([FromRoute] int measurementId)
        {
            var result = await _materialProvider.GetMaterialByMeasurementId(measurementId);           
             return result.HasErrors ? (IActionResult)NotFound(result.GetErrors()) : (IActionResult)Ok(result.TObject);
        }

        [HttpPost]
        [ProducesResponseType(typeof(MaterialViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<Error>), StatusCodes.Status404NotFound)]
        [Route("changematerial")]
        public async Task<IActionResult> ChangeMaterial([FromBody] ChangeMaterialViewModel changeMaterialViewModel)
        {
            var result = await _materialProvider.ChangeMaterialOnMeasurement(changeMaterialViewModel.MeasurementId, changeMaterialViewModel.MaterialId);
            return result.HasErrors ? (IActionResult)NotFound(result.GetErrors()) : (IActionResult)Ok(result.TObject);
        }        
        
        [HttpGet]
        [ProducesResponseType(typeof(List<MaterialViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<Error>), StatusCodes.Status404NotFound)]
        [Route("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _materialProvider.GetAll();
            return result.HasErrors ? (IActionResult)NotFound(result.GetErrors()) : (IActionResult)Ok(result.TObject);
            
        }

    }
}