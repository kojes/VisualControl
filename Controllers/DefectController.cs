using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VueExample.Models;
using VueExample.Providers;
using VueExample.ResponseObjects;
using VueExample.Services;
using VueExample.ViewModels;

namespace VueExample.Controllers
{
    [Route("api/[controller]/[action]")]
    public class DefectController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IDefectProvider _defectProvider;
        private readonly IPhotoProvider _photoProvider;
        public DefectController(IMapper mapper, IDefectProvider defectProvider, IPhotoProvider photoProvider)
        {
            _mapper = mapper;
            _defectProvider = defectProvider;
            _photoProvider = photoProvider;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetById(int defectId)
        {
            var defect = await _defectProvider.GetByIdAsync(defectId);
            return CreatedAtAction(nameof(GetById), defect);

        }

        [HttpGet]
        public IActionResult GetByWaferId(string waferId)
        {
            return Ok(_defectProvider.GetByWaferId(waferId));
        }

        [HttpGet]
        public IActionResult GetByDieId(long dieId)
        {
            return Ok(_defectProvider.GetByDieId(dieId));
        }

        [HttpPost]
        public IActionResult SaveNewDefect([FromBody]DefectViewModel defectViewModel)
        {

            var emptyPhotos = new List<string>();
            var ims = new ImageManipulationService();
            defectViewModel.Date = DateTime.Now;
            var defectId = _defectProvider.GetDuplicate(defectViewModel.DieId, defectViewModel.StageId, defectViewModel.DefectTypeId);
            var response = new StandardResponseObject { ResponseType = "success", Message = $"Обнаружен идентичный дефект в БД, загруженные фото добавлены к существующему дефекту, кристалл №{defectViewModel.DieCode} на пластине {defectViewModel.WaferId}"};


            if (defectId == 0)
            {
                defectId = _defectProvider.InsertNewDefect(_mapper.Map<Defect>(defectViewModel));
                response = new StandardResponseObject
                {
                    ResponseType = "success",
                    Message = $"Дефект успешно загружен, кристалл №{defectViewModel.DieCode} на пластине {defectViewModel.WaferId}"
                };
            }
            
         
            var photoStorageFolder = FileSystemService.CreateNewFolder(ExtraConfiguration.PhotoStoragePath, defectViewModel.WaferId);
            foreach (var photoGuid in defectViewModel.LoadedPhotosList)
                if (!string.IsNullOrEmpty(FileSystemService.FindFolderInTemporaryFolder(photoGuid)))
                {
                    System.IO.File.Move(FileSystemService.GetFirstFilePathFromFolderInTemporaryFolder(photoGuid), photoStorageFolder + "\\" + photoGuid + ".jpg");
                    ims.ResizeImage(256, 256, 75, photoStorageFolder + "\\" + photoGuid + ".jpg", photoStorageFolder + "\\" + photoGuid + "_MINI.jpg");
                    var photo = new Photo
                    {
                        DefectId = defectId,
                        Guid = photoGuid
                      
                    };
                    _photoProvider.InsertPhoto(photo);
                    FileSystemService.DeleteFolderInTemporaryFolder(photoGuid);
                }
                else
                {
                    emptyPhotos.Add(photoGuid);
                }

            if (emptyPhotos.Count == defectViewModel.LoadedPhotosList.Count)
            {
                if (_photoProvider.GetPhotosByDefectId(defectId).Count == 0) _defectProvider.DeleteById(defectId);
                response = new StandardResponseObject {ResponseType = "error", Message = "Загрузите фото дефекта"};
            }

          

            return Ok(response);

        }
    }
}
