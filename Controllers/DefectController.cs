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

        [HttpPost]
        public IActionResult SaveNewDefect([FromBody]DefectViewModel defectViewModel)
        {

            var emptyPhotos = new List<string>();
            defectViewModel.Date = DateTime.Now;
            
            var defectId = _defectProvider.InsertNewDefect(_mapper.Map<Defect>(defectViewModel));

            var photoStorageFolder = FileSystemService.CreateNewFolder(ExtraConfiguration.PhotoStoragePath, defectViewModel.WaferId);
            foreach (var photoGuid in defectViewModel.LoadedPhotosList)
            {
                if (!String.IsNullOrEmpty(FileSystemService.FindFolderInTemporaryFolder(photoGuid)))
                {
                    System.IO.File.Move(FileSystemService.GetFirstFilePathFromFolderInTemporaryFolder(photoGuid), photoStorageFolder + "\\" + photoGuid + ".jpg");
                    var photo = new Photo
                    {
                        DefectId = defectId,
                        Guid = photoGuid,
                        WaferId = defectViewModel.WaferId
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
                _defectProvider.DeleteById(defectId);
                return Ok(new StandardResponseObject("Нет фото дефекта"));
            }

            return Ok(new StandardResponseObject());

        }
    }
}
