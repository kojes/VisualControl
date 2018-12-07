using System;
using System.Collections.Generic;
using System.Linq;
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
        public IActionResult GetById(int defectId)
        {
            return Ok(_defectProvider.GetById(defectId));
        }

        [HttpGet]
        public IActionResult GetByWaferId(string waferId)
        {
            return Ok(_defectProvider.GetByWaferId(waferId));
        }

        [HttpPost]
        public IActionResult SaveNewDefect([FromBody]DefectViewModel defectViewModel)
        {

            var emptyPhotos = new List<string>();
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
            {
                if (!String.IsNullOrEmpty(FileSystemService.FindFolderInTemporaryFolder(photoGuid)))
                {
                    System.IO.File.Move(FileSystemService.GetFirstFilePathFromFolderInTemporaryFolder(photoGuid), photoStorageFolder + "\\" + photoGuid + ".jpg");
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
            }

            if (emptyPhotos.Count == defectViewModel.LoadedPhotosList.Count)
            {
                if (_photoProvider.GetPhotosByDefectId(defectId).Count == 0)
                {
                    _defectProvider.DeleteById(defectId);
                }
                response = new StandardResponseObject {ResponseType = "error", Message = "Загрузите фото дефекта"};
            }

          

            return Ok(response);

        }
    }
}