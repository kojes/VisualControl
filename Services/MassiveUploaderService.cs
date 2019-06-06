using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VueExample.Models;
using VueExample.Providers;
using VueExample.ResponseObjects;
using VueExample.ViewModels;

namespace VueExample.Services
{
    public class MassiveUploaderService
    {
        public ResponseObjects.FullResponseObject<List<DefectViewModel>> FindDefectsInFolder(string path)
        {
            var response = new FullResponseObject<List<DefectViewModel>>();
            WaferProvider waferProvider = new WaferProvider();
            DieProvider dieProvider = new DieProvider();
            var dies = new List<Die>();
            

            if (!Directory.Exists(path))
            {
                response.ErrorList.Add(new Error("Директория не существует", "NEF001"));
                return response;
            }

            var wafer = waferProvider.GetByWaferId(path.Split("\\").Last().Split('_').FirstOrDefault());

            if (wafer == null)
            {
                response.ErrorList.Add(new Error("Пластина не найдена", "WNE001"));
                return response;
            }
            else
            {
                dies = dieProvider.GetDiesByWaferId(wafer.WaferId).ToList();
            }

            var foldersBadGoodList = System.IO.Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly).ToList();

            if (!foldersBadGoodList.Contains($"{path}\\Брак"))
            {
                response.ErrorList.Add(new Error("Директория <<Брак>> не найдена", "NEF002"));
                return response;
            }

            if (!foldersBadGoodList.Contains($"{path}\\Годен"))
            {
                response.ErrorList.Add(new Error("Директория <<Годен>> не найдена", "NEF003"));
                return response;
            }

            DefectTypeProvider defectTypeProvider = new DefectTypeProvider();
            var defectTypes = defectTypeProvider.GetAll();

            var diesCodes = dies.Select(x => x.Code).ToList();


            foreach (var typeFolderPath in Directory.GetDirectories($"{path}\\Брак", "*", SearchOption.TopDirectoryOnly).ToList())
            {
                var typeFolder = typeFolderPath.Split("\\").Last();
                if (!defectTypes.Select(x => x.Description).ToList().Contains(typeFolder))
                {
                    response.WarningList.Add(new Warning($"Найдена папка с неизвестном типом дефекта: {typeFolder} в папке Брак", "UTD001"));
                }
                else
                {
                    var images = FileSystemService.GetFilesFrom($"{path}\\Брак\\{typeFolder}", new[] {"jpg", "png"}, false);
                    foreach (var image in images)
                    {
                        var imageCode = image.Split('_').Last().Split('.').First();
                        if (!diesCodes.Contains(imageCode))
                        {
                            response.WarningList.Add(new Warning($"Найдено изображение с неизвестным кодом кристалла: {imageCode} в папке {typeFolder}", "UCD001"));
                        }
                        else
                        {
                            var defectTypeId = defectTypes.FirstOrDefault(x => x.Description == typeFolder)
                                .DefectTypeId;
                            var dieId = dies.FirstOrDefault(x => x.Code == imageCode).DieId;

                            if (response.Body.Count(x => x.DefectTypeId == defectTypeId && x.DieId == dieId) == 0)
                            {
                                var defect = new DefectViewModel
                                {
                                    WaferId = wafer.WaferId,
                                    DangerLevelId = 1,
                                    Date = DateTime.Now,
                                    DefectTypeId = defectTypeId,
                                    DieId = dieId
                                };
                                defect.LoadedPhotosList.Add(image);
                                defect.DieCode = imageCode;
                                response.Body.Add(defect);
                            }
                            else
                            {
                                response.Body.FirstOrDefault(x => x.DefectTypeId == defectTypeId && x.DieId == dieId).LoadedPhotosList.Add(image);
                            }
                            
                        }
                    }
                }
            }





            return response;
        }
    }
}
