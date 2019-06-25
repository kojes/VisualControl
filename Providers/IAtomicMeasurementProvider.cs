using VueExample.ViewModels;

namespace VueExample.Providers
{
    public interface IAtomicMeasurementProvider
    {
        int AddToMeasurementSet(AtomicMeasurementViewModel atomicMeasurementViewModel);
        void DeleteFromMeasurementSet(int measurementSetId, int atomicId);


    }
}