using System;
using System.Collections.Generic;
using VueExample.ResponseObjects;
using VueExample.ViewModels;

namespace VueExample.Providers.ChipVerification.Abstract
{
    public interface IMeasurementSetProvider
    {
         List<MeasurementSetViewModel> GetAllSets(int facilityId);
         List<AtomicMeasurementExtendedViewModel> GetAtomicsById(Guid measurementSetId, IMeasurementProvider measurementProvider);
         (MeasurementSetViewModel, Error) Create(string name);
         bool Delete(Guid id);
         List<AtomicMeasurementExtendedViewModel> GetAtomicsOnline(IMeasurementProvider measurementProvider, int facilityId);
         List<AtomicMeasurementExtendedViewModel> GetAtomicsByMaterial(int materialId, IMeasurementProvider measurementProvider, int facilityId);
    }
}