using System.Collections.Generic;
using System.Threading.Tasks;
using VueExample.Models.SRV6.Uploader;
using VueExample.ViewModels;

namespace VueExample.Providers.Srv6.Interfaces
{
    public interface IFileGraphicUploaderService
    {
        Task<IList<FileName>> GetAllFileNamesByProcessId(int processId);
        Task<IList<GraphicName>> GetGraphicsByFileName(int graphicId);
        Task<FileName> CreateFileName(FileNameUploaderViewModel fileNameViewModel);
        Task<GraphicName> AddGraphicToFileName(int fileNameId, GraphicNameUploaderViewModel graphicNameUploaderViewModel);
        Task DeleteGraphicFromFileName(int fileNameId, GraphicNameUploaderViewModel graphicNameUploaderViewModel);
    }
}