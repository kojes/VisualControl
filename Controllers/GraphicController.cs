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
    public class GraphicController : Controller
    {
        private readonly IGraphicProvider _graphicProvider;

        public GraphicController(IGraphicProvider graphicProvider)
        {
            _graphicProvider = graphicProvider;
        }

        [HttpGet]
        [ProducesResponseType(typeof(GraphicViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<Error>), StatusCodes.Status404NotFound)]
        [Route(("get/{id:int}"))]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _graphicProvider.GetGraphicById(id);
            return result.HasErrors ? (IActionResult)NotFound(result.GetErrors()) : (IActionResult)Ok(result.TObject);
        }

        [HttpGet]
        [ProducesResponseType(typeof(GraphicViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(List<Error>), StatusCodes.Status404NotFound)]
        [Route(("getbynameandtype"))]
        public async Task<IActionResult> GetByNameAndType([FromQuery] string name, [FromQuery] string type)
        {
            var result = await _graphicProvider.GetGraphicByNameAndType(name, type);
            return result.HasErrors ? (IActionResult)NotFound(result.GetErrors()) : (IActionResult)Ok(result.TObject);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GraphicViewModel>), StatusCodes.Status200OK)]
        [Route("av/measurementid/{measurementId:int}")]
        public async Task<IActionResult> GetAvailiableByMeasurementId([FromRoute] int measurementId)
        {
            var graphic = await _graphicProvider.GetAvailiableByMeasurementId(measurementId);
            return Ok(graphic);
        }
    }
}
