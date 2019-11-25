using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using VueExample.Models.SRV6.Uploader;
using VueExample.Providers.Srv6.Interfaces;
using VueExample.ViewModels;

namespace VueExample.Controllers
{
    [Route("api/[controller]")]
    public class FileGraphicUploaderController : Controller
    {
        private readonly IFileGraphicUploaderService _fileGraphicUploaderService;
        public FileGraphicUploaderController(IFileGraphicUploaderService fileGraphicUploaderService)
        {
            _fileGraphicUploaderService = fileGraphicUploaderService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(FileName), StatusCodes.Status200OK)]
        [ProducesResponseType (StatusCodes.Status404NotFound)]
        [Route("process/{processId:int}")]
        public async Task<IActionResult> GetAllFileNamesByProcessId([FromRoute] int processId)
        {
            var fileName =  await _fileGraphicUploaderService.GetAllFileNamesByProcessId(processId);
            return fileName.Count > 0 ? Ok(fileName) : (IActionResult)NotFound();
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<GraphicName>), StatusCodes.Status200OK)]
        [ProducesResponseType (StatusCodes.Status404NotFound)]
        [Route("graphics/{fileNameId:int}")]
        public async Task<IActionResult> GetGraphicsByFileName(int fileNameId)
        {
            var graphicsList = await _fileGraphicUploaderService.GetGraphicsByFileName(fileNameId);
            return graphicsList.Count > 0 ? Ok(graphicsList) : (IActionResult)NotFound();
        }

        [HttpPut]
        [ProducesResponseType (typeof(FileName), StatusCodes.Status201Created)]
        [Route("filename/create")]
        public async Task<IActionResult> CreateFileName([FromBody] JObject fileNameViewModelJObject)
        {
            var fileName = await _fileGraphicUploaderService.CreateFileName(fileNameViewModelJObject.ToObject<FileNameUploaderViewModel>());
            return CreatedAtAction("", fileName);
        }

        [HttpPut]
        [ProducesResponseType (typeof(GraphicName), StatusCodes.Status201Created)]
        [Route("graphicname/create/{fileNameId:int}")]
        public async Task<IActionResult> AddGraphicToFileName([FromRoute] int fileNameId, [FromBody] JObject graphicNameJObject)
        {
            var graphic = await _fileGraphicUploaderService.AddGraphicToFileName(fileNameId, graphicNameJObject.ToObject<GraphicNameUploaderViewModel>());
            return CreatedAtAction("", graphic);
        }

        [HttpDelete]
        [ProducesResponseType (StatusCodes.Status204NoContent)]
        [Route("graphicname/delete/{fileNameId:int}")]
        public async Task<IActionResult> Delete([FromRoute] int fileNameId, [FromBody] JObject graphicNameJObject)
        {
            await _fileGraphicUploaderService.DeleteGraphicFromFileName(fileNameId, graphicNameJObject.ToObject<GraphicNameUploaderViewModel>());
            return NoContent();
        }

    }
}