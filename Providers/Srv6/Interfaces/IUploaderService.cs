using System.Threading.Tasks;
using VueExample.Models.SRV6.Uploader;

namespace VueExample.Providers.Srv6.Interfaces
{
    public interface IUploaderService
    {
        Task<string> Uploading(UploadingFile uploadingFile, int type);
    }
}