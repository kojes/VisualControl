using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VueExample.Providers.ChipVerification.Abstract;
using VueExample.ViewModels;

namespace VueExample.Controllers
{
    [Route("api/[controller]")]
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
        [Route("getall/{facilityId:int}")]
        public IActionResult GetAll([FromRoute] int facilityId)
        {
            return Ok(_measurementSetProvider.GetAllSets(facilityId));
        }
        

        [HttpPut]
        [Route("addnewset")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult AddNewSet([FromQuery(Name = "name")] string setName)
        {
            var createdMeasurementSet = _measurementSetProvider.Create(setName);
            
            if(createdMeasurementSet.Item1 == null)
            {
                return Ok(createdMeasurementSet.Item2.Message);
            }
         
            return CreatedAtAction("Add", createdMeasurementSet.Item1);
        }
  
        [HttpDelete]
        [Route("deleteset")]
        public IActionResult DeleteSet([FromQuery(Name = "measurementsetid")] Guid measurementSetId)
        {
            _measurementSetProvider.Delete(measurementSetId);
            return Ok(); 
           
        }

        [Route("getatomics/{measurementSetId:guid}/{facilityId}")]
        [HttpGet]
        public IActionResult GetAtomicsById([FromRoute] Guid measurementSetId, [FromServices] IMeasurementProvider measurementProvider)
        {
            return Ok(_measurementSetProvider.GetAtomicsById(measurementSetId, measurementProvider));
        }
        
        [Route("getatomics/online/{facilityId}")]
        [HttpGet]
        public IActionResult GetAtomicsOnline([FromServices] IMeasurementProvider measurementProvider, [FromRoute] int facilityId)
        {
            return Ok(_measurementSetProvider.GetAtomicsOnline(measurementProvider, facilityId));
        }

        [Route("getatomics/material/{materialId}/{facilityId}")]
        [HttpGet]
        public IActionResult GetAtomicsByMaterial([FromRoute] int materialId, [FromServices] IMeasurementProvider measurementProvider, [FromRoute] int facilityId)
        {
            return Ok(_measurementSetProvider.GetAtomicsByMaterial(materialId, measurementProvider, facilityId));
        }

        [HttpPost]
        [Route("addnewatomictoset")]
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
        [Route("deleteatomicfromset")]
        [ProducesResponseType(StatusCodes.Status200OK)]        
        public IActionResult DeleteAtomicFromSet([FromBody] AtomicMeasurementMeasurementSetViewModel atomicMeasurementMeasurementSetViewModel)
        {
            _atomicMeasurementProvider.DeleteFromMeasurementSet(atomicMeasurementMeasurementSetViewModel.MeasurementSetId, atomicMeasurementMeasurementSetViewModel.AtomicId);
            return Ok();
        }
    }
}