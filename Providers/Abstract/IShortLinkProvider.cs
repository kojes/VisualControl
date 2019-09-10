using System.Threading.Tasks;
using VueExample.ResponseObjects;
using VueExample.ViewModels;

namespace VueExample.Providers.Abstract
{
    public interface IShortLinkProvider
    {
         Task<AfterDbManipulationObject<ShortLinkInfoViewModel>> GetElementExportDetails(string shortLink);
    }
}