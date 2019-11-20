using System.Linq;
using VueExample.Providers.Srv6.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace VueExample.Providers.Srv6
{
    public class FolderService : IFolderService
    {
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



    }
}