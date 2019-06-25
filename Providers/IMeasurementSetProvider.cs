using System.Collections.Generic;
using VueExample.Models;
using VueExample.ViewModels;

namespace VueExample.Providers
{
    public interface IMeasurementSetProvider
    {
         List<MeasurementSet> GetAllSets();
         List<AtomicMeasurementExtendedViewModel> GetAtomicsById(int measurementSetId);
         MeasurementSet Create(string name);
         bool Delete(int id);
    }
}