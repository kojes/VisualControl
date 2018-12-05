using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VueExample.Providers;

namespace VueExample.Controllers
{
    public class GraphicController : Controller
    {
        private readonly IGraphicProvider graphicProvider;

        public GraphicController(IGraphicProvider graphicProvider)
        {
            this.graphicProvider = graphicProvider;
        }

        [HttpGet("[action]")]
        public IActionResult GetById(int id)
        {
            var graphic = graphicProvider.GetById(id);
            return Ok(graphic);
        }
    }
}
