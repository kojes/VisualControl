using System.Collections.Generic;
using VueExample.Models.SRV6.Export;
using VueExample.ViewModels;

namespace VueExample.StatisticsCore.Abstract
{
    public interface IExportProvider
    {
        List<Dictionary<string, string>> Export(int idmr, string statNames, string delimeter);
        void PopulateKurbatovXLSByValues(KurbatovXLS kurbatovXLS);
    }
}