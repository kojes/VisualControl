using System.Threading.Tasks;

namespace VueExample.Providers.Srv6.Interfaces
{
    public interface IStandartWaferService
    {
        Task<string> GetCodeFromStandartWafer(string code, string map);
    }
}