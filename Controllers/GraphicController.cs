using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VueExample.Providers.ChipVerification.Abstract;

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

        [HttpGet("get/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var graphic = await _graphicProvider.GetGraphicById(id);
            return Ok(graphic);
        }

        [HttpGet("av/measurementid/{measurementId:int}")]
        public async Task<IActionResult> GetAvailiableByMeasurementId([FromRoute] int measurementId)
        {
            var graphic = await _graphicProvider.GetAvailiableByMeasurementId(measurementId);
            return Ok(graphic);
        }
    }
}
