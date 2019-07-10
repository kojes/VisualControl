using System;
using System.Collections.Generic;
using VueExample.ResponseObjects;
using VueExample.ViewModels;

namespace VueExample.Providers
{
    public interface IMeasurementSetProvider
    {
         List<MeasurementSetViewModel> GetAllSets();
         List<AtomicMeasurementExtendedViewModel> GetAtomicsById(Guid measurementSetId, IMeasurementProvider measurementProvider);
         (MeasurementSetViewModel, Error) Create(string name);
         bool Delete(Guid id);
         List<AtomicMeasurementExtendedViewModel> GetAtomicsOnline(IMeasurementProvider measurementProvider);
         List<AtomicMeasurementExtendedViewModel> GetAtomicsByMaterial(int materialId, IMeasurementProvider measurementProvider);
    }
}