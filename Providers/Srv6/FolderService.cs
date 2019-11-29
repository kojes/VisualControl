using System.Linq;
using VueExample.Providers.Srv6.Interfaces;
using System.Collections.Generic;
using System.IO;
using VueExample.ViewModels;
using System.Threading.Tasks;
using System;

namespace VueExample.Providers.Srv6
{
    public class FolderService : IFolderService
    {
        private readonly IElementService _elementService;
        private readonly IFileGraphicUploaderService _fileGraphicUploaderService;
        private readonly ProcessProvider processProvider = new ProcessProvider();
        private readonly CodeProductProvider codeProductProvider = new CodeProductProvider();
        public FolderService(IElementService elementService, IFileGraphicUploaderService fileGraphicUploaderService)
        {
            _elementService = elementService;
            _fileGraphicUploaderService = fileGraphicUploaderService;
        }
        public List<string> GetAllCodeProductInUploaderDirectory(string directoryPath)
        {
            var directoriesNameList = new List<string>();
            var directoriesArray = System.IO.Directory.GetDirectories(directoryPath);
            foreach (var directory in directoriesArray)
            {
                var dir = new DirectoryInfo(directory);
                var dirName = dir.Name;
                directoriesNameList.Add(dirName);
            }           
            return directoriesNameList;
        }

        public List<string> GetAllWaferInCodeProductFolder(string directoryPath, string codeProductFolderName)
        {
            var directoriesNameList = new List<string>();
            var directoriesArray = System.IO.Directory.GetDirectories($"{directoryPath}\\{codeProductFolderName}\\meas");
            foreach (var directory in directoriesArray)
            {
                var dir = new DirectoryInfo(directory);
                var dirName = dir.Name;
                directoriesNameList.Add(dirName);
            }           
            return directoriesNameList;
        }

        public List<string> GetAllMeasurementRecordingFolder(string directoryPath, string codeProductFolderName, string waferFolderName)
        {
            var directoriesNameList = new List<string>();
            var directoriesArray = System.IO.Directory.GetDirectories($"{directoryPath}\\{codeProductFolderName}\\meas\\{waferFolderName}");
            foreach (var directory in directoriesArray)
            {
                var dir = new DirectoryInfo(directory);
                var dirName = dir.Name;
                directoriesNameList.Add(dirName);
            }           
            return directoriesNameList;
        }

        public async Task<List<SimpleOperationUploaderViewModel>> GetSimpleOperations(string directoryPath, string codeProductName, string waferName, int dieTypeId, List<string> measurementRecordings)
        {
            var simpleOperationList = new List<SimpleOperationUploaderViewModel>(); 
            var fileNames = await _fileGraphicUploaderService.GetAllFileNamesByProcessId(processProvider.GetProcessIdByCodeProductId((await codeProductProvider.GetByWaferId(waferName)).IdCp));
            foreach (var meas in measurementRecordings)
            {
                var directoriesArray = System.IO.Directory.GetDirectories($"{directoryPath}\\{codeProductName}\\meas\\{waferName}\\{meas}");
                foreach (var directory in directoriesArray)
                {
                    var dirElementName = new DirectoryInfo(directory).Name;
                    var simpleOperationArray = Directory.EnumerateFiles($"{directoryPath}\\{codeProductName}\\meas\\{waferName}\\{meas}\\{dirElementName}", "*.*", SearchOption.TopDirectoryOnly)
                                                        .Where(s => s.EndsWith(".csv") || s.EndsWith(".s2p"));                    
                    foreach (var simpleOperationFileName in simpleOperationArray)
                    {
                        var simpleOperation = new SimpleOperationUploaderViewModel{Guid = Guid.NewGuid().ToString()};
                        simpleOperation.Name = $"{meas}";
                        var element = (await _elementService.GetByDieType(dieTypeId)).FirstOrDefault(x => x.Name == dirElementName);
                        simpleOperation.Element = new ElementUploading{Name = dirElementName, ElementId = element?.ElementId, Comment = element?.Comment};
                        simpleOperation.FileName = new FileNameUploaderViewModel{Name = Path.GetFileName(simpleOperationFileName), Path = simpleOperationFileName};
                        var fileName = fileNames.FirstOrDefault(f => f.Name == simpleOperationFileName);
                        if(fileName != null)
                        {
                            simpleOperation.FileName.Id = fileName.Id;
                            simpleOperation.FileName.ProcessId = fileName.ProcessId;
                            simpleOperation.FileName.GraphicNames = (await _fileGraphicUploaderService.GetGraphicsByFileName(fileName.Id)).ToList();
                        }
                        simpleOperationList.Add(simpleOperation);
                    }
                    
                }           
            }
            return simpleOperationList;
        }
    }
}