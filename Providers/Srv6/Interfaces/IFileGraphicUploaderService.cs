using System.Collections.Generic;
using System.Threading.Tasks;
using VueExample.Models.SRV6.Uploader;
using VueExample.ViewModels;

namespace VueExample.Providers.Srv6.Interfaces
{
    public interface IFileGraphicUploaderService
    {
        Task<IList<FileName>> GetAllFileNamesByProcessId(int processId);
        Task<IList<GraphicNameUploaderViewModel>> GetGraphicsByFileName(int fileNameId);
        Task<FileName> CreateFileName(FileNameUploaderViewModel fileNameViewModel);
        Task<GraphicName> AddGraphicToFileName(int fileNameId, GraphicNameUploaderViewModel graphicNameUploaderViewModel);
        Task DeleteFileName(int fileNameId, int processId);
        Task DeleteGraphicFromFileName(int fileNameId, GraphicNameUploaderViewModel graphicNameUploaderViewModel);
    }
}