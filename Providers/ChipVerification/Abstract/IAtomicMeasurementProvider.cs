using System;
using VueExample.ViewModels;

namespace VueExample.Providers.ChipVerification.Abstract
{
    public interface IAtomicMeasurementProvider
    {
        int AddToMeasurementSet(AtomicMeasurementViewModel atomicMeasurementViewModel);
        void DeleteFromMeasurementSet(Guid measurementSetId, int atomicId);


    }
}