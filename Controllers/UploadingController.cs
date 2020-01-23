using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using VueExample.Entities;
using VueExample.Models.SRV6.Uploader;
using VueExample.Providers.Srv6.Interfaces;
using VueExample.ViewModels;

namespace VueExample.Controllers
{
    [Route("api/[controller]")]
    public class UploadingController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUploaderService _uploaderService;
        private readonly ISRV6GraphicService _graphicService;
        private readonly IElementService _elementService;
        private readonly IMeasurementRecordingService _measurementRecordingService;
        private readonly IFolderService _folderService;
        public UploadingController(IFolderService folderService, IUploaderService uploaderService, IMeasurementRecordingService measurementRecordingService, ISRV6GraphicService graphicService, IElementService elementService, IMapper mapper)
        {
            _uploaderService = uploaderService;
            _measurementRecordingService = measurementRecordingService;
            _elementService = elementService;
            _folderService = folderService;
            _graphicService = graphicService;
            _mapper = mapper;
        }
        
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Upload([FromBody] JObject uploadingFileJObject)
        {
            var uploadingFile = uploadingFileJObject.ToObject<UploadingFile>();
            var bigMeasurementRecording = await _measurementRecordingService.GetOrAddBigMeasurement(uploadingFile.BigMeasurementName, uploadingFile.WaferId);
            uploadingFile.MeasurementRecordingId = (await _measurementRecordingService.GetByNameAndWaferId("оп." + uploadingFile.OperationName, uploadingFile.WaferId))?.Id;
            if (uploadingFile.MeasurementRecordingId is null)
            {
                uploadingFile.MeasurementRecordingId = (await _measurementRecordingService.GetOrCreate(uploadingFile.OperationName, 2, bigMeasurementRecording.Id, uploadingFile.StageId)).Id;              
            } 
            else
            {
                uploadingFile.IsNewMeasurement = false;
            }         
            await _measurementRecordingService.GetOrCreateFkMrP((int)uploadingFile.MeasurementRecordingId, 247, uploadingFile.WaferId);
            foreach (var graphicName in uploadingFile.GraphicNames)
            {
                uploadingFile.Graphics.Add(await _graphicService.GetByCodeProductAndName(uploadingFile.CodeProductId, graphicName));
            }
            var type = uploadingFile.Graphics.FirstOrDefault().Type;
            if(type == 1)
            {
                uploadingFile.Data = _folderService.GetDataFromLNRFile(uploadingFile.Path);
            }
            if(type == 2)
            {
                uploadingFile.Data = _folderService.GetDataFromHSTGFile(uploadingFile.Path);
            }
            var link = await _uploaderService.Uploading(uploadingFile, type);          
            await _elementService.UpdateElementOnIdmr((int) uploadingFile.MeasurementRecordingId, uploadingFile.ElementId);
            return Ok(link);
        }

        [HttpPost]
        [Route("checkUploadingStatus")]
        public async Task<IActionResult> CheckUploadingStatus([FromBody] JObject uploadingFileJObject)
        {
            var uploadingFile = uploadingFileJObject.ToObject<UploadingFile>();
            var measurementRecording = await _measurementRecordingService.GetByNameAndWaferId("оп." + uploadingFile.OperationName, uploadingFile.WaferId);
            var fkMrGraphicsList = new List<FkMrGraphic>();
            foreach (var graphicName in uploadingFile.GraphicNames)
            {
                var graphic = await _graphicService.GetByCodeProductAndName(uploadingFile.CodeProductId, graphicName);
                var fkMrGraphics = await _measurementRecordingService.GetFkMrGraphics(measurementRecording?.Id, graphic.Id);
                fkMrGraphicsList.Add(fkMrGraphics);
            }                 
            return Ok(_mapper.Map<List<FkMrGraphic>, List<FKMRGraphicsViewModel>>(fkMrGraphicsList));         
        }
    }
}