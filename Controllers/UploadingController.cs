using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using VueExample.Models.SRV6.Uploader;
using VueExample.Providers.Srv6;
using VueExample.Providers.Srv6.Interfaces;

namespace VueExample.Controllers
{
    public class UploadingController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUploaderService _uploaderService;
        private readonly ISRV6GraphicService _graphicService;
        private readonly MeasurementRecordingService _measurementRecordingService = new MeasurementRecordingService();
        private readonly IFolderService _folderService;
        public UploadingController(IFolderService folderService, IUploaderService uploaderService, ISRV6GraphicService graphicService, IMapper mapper)
        {
            _uploaderService = uploaderService;
            _folderService = folderService;
            _graphicService = graphicService;
            _mapper = mapper;
        }
        
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Upload([FromBody] JObject uploadingFileJObject)
        {
            var uploadingFile = uploadingFileJObject.ToObject<UploadingFile>();
            uploadingFile.MeasurementRecordingId = (await _measurementRecordingService.GetByNameAndWaferId("оп." + uploadingFile.OperationName, uploadingFile.WaferId))?.Id;
            if (uploadingFile.MeasurementRecordingId is null)
            {
                uploadingFile.MeasurementRecordingId = (await _measurementRecordingService.Create(uploadingFile.OperationName, 2)).Id;
                uploadingFile.IsNewMeasurement = true;
            }
            var type = uploadingFile.Graphics.FirstOrDefault().Type;
            await _measurementRecordingService.CreateFkMrP((int)uploadingFile.MeasurementRecordingId, 247, uploadingFile.WaferId);
            foreach (var graphicName in uploadingFile.GraphicNames)
            {
                uploadingFile.Graphics.Add(await _graphicService.GetByCodeProductAndName(uploadingFile.CodeProductId, graphicName));
            }
            if(type == 1)
            {
                uploadingFile.Data = _folderService.GetDataFromLNRFile(uploadingFile.Path);
            }
            if(type == 2)
            {
                uploadingFile.Data = _folderService.GetDataFromHSTGFile(uploadingFile.Path);
            }
            var link = await _uploaderService.Uploading(uploadingFile, type);
            return Ok(link);
        }
        
    }
}