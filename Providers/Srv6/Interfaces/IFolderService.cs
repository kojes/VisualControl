using System.Collections.Generic;
using System.Threading.Tasks;
using VueExample.ViewModels;

namespace VueExample.Providers.Srv6.Interfaces
{
    public interface IFolderService
    {
        List<string> GetAllCodeProductInUploaderDirectory(string directoryPath);
        List<string> GetAllWaferInCodeProductFolder(string directoryPath, string codeProductFolderName);
        List<string> GetAllMeasurementRecordingFolder(string directoryPath, string codeProductFolderName, string waferFolderName);
        Task<List<SimpleOperationUploaderViewModel>> GetSimpleOperations(string directoryPath, string codeProductName, string waferName, int dieTypeId, List<string> measurementRecordings);
    }
}