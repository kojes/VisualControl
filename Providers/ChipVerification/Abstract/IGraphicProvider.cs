using System.Collections.Generic;
using System.Threading.Tasks;
using VueExample.Models;

namespace VueExample.Providers.ChipVerification.Abstract
{
    public interface IGraphicProvider
    {
        Task<Graphic> GetGraphicById(int id);
        Task<List<Graphic>> GetAvailiableByMeasurementId(int measurementId);
    }
}
