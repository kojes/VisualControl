using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VueExample.Providers.Srv6;
using VueExample.Providers.Srv6.Interfaces;

namespace VueExample.Controllers
{
    [Route("api/[controller]/[action]")]
    public class GraphicSrv6Controller : Controller
    {
        private readonly ISRV6GraphicService _graphicService;
        public GraphicSrv6Controller(ISRV6GraphicService graphicService)
        {
            _graphicService = graphicService;
        }

        [HttpGet]
        public IActionResult GetGraphicNameByKeyGraphicState(string keyGraphicState)
        {
            var graphicId = Convert.ToInt32(keyGraphicState.Split('_').FirstOrDefault());
            return Ok(_graphicService.GetById(graphicId).Name);
        }

        [HttpGet]
        public IActionResult GetAvailiableGraphicsByKeyGraphicStateList(string keyGraphicStateJSON)
        {
            var keyGraphicStateList = JsonConvert.DeserializeObject<List<string>>(keyGraphicStateJSON);
            var availiableGraphicList = new List<ViewModels.GraphicWithKeyGraphicStateViewModel>();
            foreach (var kgs in keyGraphicStateList)
            {
                var graphicWithKeyGraphicStateViewModel = new ViewModels.GraphicWithKeyGraphicStateViewModel();
                var graphicId = Convert.ToInt32(kgs.Split('_').FirstOrDefault());
                graphicWithKeyGraphicStateViewModel.GraphicName = _graphicService.GetById(graphicId).Name;
                graphicWithKeyGraphicStateViewModel.KeyGraphicState = kgs;
                availiableGraphicList.Add(graphicWithKeyGraphicStateViewModel);
            }

           return Ok(availiableGraphicList);
            
        }
    }
}