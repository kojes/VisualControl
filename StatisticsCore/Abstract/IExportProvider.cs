using System.Collections.Generic;
using System.Threading.Tasks;
using VueExample.Models.SRV6.Export;
using VueExample.ViewModels;

namespace VueExample.StatisticsCore.Abstract
{
    public interface IExportProvider
    {
        List<Dictionary<string, string>> Export(int idmr, string statNames, string delimeter);
        Task<List<string>> GetStatisticsNameByMeasurementId(int measurementRecordingId);
        void PopulateKurbatovXLSByValues(KurbatovXLS kurbatovXLS);
        
    }
}