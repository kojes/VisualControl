using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VueExample.Providers.Srv6;

namespace VueExample.Controllers
{
    [Route("api/[controller]/[action]")]
    public class GraphicSrv6Controller : Controller
    {
        GraphicService graphicService = new GraphicService();

        [HttpGet]
        public IActionResult GetGraphicNameByKeyGraphicState(string keyGraphicState)
        {
            var graphicId = Convert.ToInt32(keyGraphicState.Split('_').FirstOrDefault());
            return Ok(graphicService.GetById(graphicId).Name);
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
                graphicWithKeyGraphicStateViewModel.GraphicName = graphicService.GetById(graphicId).Name;
                graphicWithKeyGraphicStateViewModel.KeyGraphicState = kgs;
                availiableGraphicList.Add(graphicWithKeyGraphicStateViewModel);
            }

           return Ok(availiableGraphicList);
            
        }
    }
}