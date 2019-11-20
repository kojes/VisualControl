using System.Collections.Generic;

namespace VueExample.Providers.Srv6.Interfaces
{
    public interface IFolderService
    {
        List<string> GetAllCodeProductInUploaderDirectory(string directoryPath);
        List<string> GetAllWaferInCodeProductFolder(string directoryPath, string codeProductFolderName);
        List<string> GetAllMeasurementRecordingFolder(string directoryPath, string codeProductFolderName, string waferFolderName);
    }
}