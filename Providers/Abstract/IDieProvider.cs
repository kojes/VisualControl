using System.Collections.Generic;
using System.Threading.Tasks;
using VueExample.Models;
using VueExample.Repository;

namespace VueExample.Providers
{
    public interface IDieProvider : IRepository<Die>
    {
        List<Die> GetDiesByWaferId(string waferId);
        Task<Die> GetByWaferIdAndCode(string waferId, string code);
        Task CreateDieParameter(int dieId, int measurementRecordingId, int parameterId = 247, string value = "0");
    }
}
