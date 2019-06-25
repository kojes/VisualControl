using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VueExample.Providers;
using VueExample.ViewModels;

namespace VueExample.Controllers
{
    [Route("api/[controller]/[action]")]
    public class MeasurementSetController : Controller
    {
        private readonly IMeasurementSetProvider _measurementSetProvider;
        private readonly IAtomicMeasurementProvider _atomicMeasurementProvider;
        public MeasurementSetController(IMeasurementSetProvider measurementsetProvider, IAtomicMeasurementProvider atomicMeasurementProvider)
        {
            _measurementSetProvider = measurementsetProvider;     
            _atomicMeasurementProvider = atomicMeasurementProvider;        
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_measurementSetProvider.GetAllSets());
        }
        

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult AddNewSet([FromQuery(Name = "name")] string setName)
        {
            var id = _measurementSetProvider.Create(setName);
            
            if(id == null)
            {
                return Ok();
            }
         
            return CreatedAtAction("Add", id);
        }
  
        [HttpDelete]
        public IActionResult DeleteSet([FromQuery(Name = "measurementsetid")] int measurementSetId)
        {
            _measurementSetProvider.Delete(measurementSetId);
            return Ok(); 
           
        }


        [HttpGet]
        public IActionResult GetAtomicsById([FromQuery(Name = "measurementsetid")] int measurementSetId)
        {
            return Ok(_measurementSetProvider.GetAtomicsById(measurementSetId));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult AddNewAtomicToSet([FromBody] AtomicMeasurementViewModel atomicMeasurementViewModel)
        {
            var id = _atomicMeasurementProvider.AddToMeasurementSet(atomicMeasurementViewModel);
            if(id == 0)
            {
                return Ok();
            }
            return CreatedAtAction("Add", id);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        
        public IActionResult DeleteAtomicFromSet([FromBody] AtomicMeasurementMeasurementSetViewModel atomicMeasurementMeasurementSetViewModel)
        {
            _atomicMeasurementProvider.DeleteFromMeasurementSet(atomicMeasurementMeasurementSetViewModel.MeasurementSetId, atomicMeasurementMeasurementSetViewModel.AtomicId);
            return Ok();
        }
    }
}