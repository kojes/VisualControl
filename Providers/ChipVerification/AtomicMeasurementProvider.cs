using System;
using System.Linq;
using VueExample.Contexts;
using VueExample.Models;
using VueExample.Providers.ChipVerification.Abstract;
using VueExample.ViewModels;

namespace VueExample.Providers .ChipVerification
{
    public class AtomicMeasurementProvider : IAtomicMeasurementProvider 
    {
        private readonly ApplicationContext _applicationContext;
        public AtomicMeasurementProvider(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        public int AddToMeasurementSet(AtomicMeasurementViewModel atomicMeasurementViewModel) 
        {
            var measurementSetId = atomicMeasurementViewModel.MeasurementSetId;
            int atomicMeasurementId = FindOrCreate(atomicMeasurementViewModel.MeasurementId,
                atomicMeasurementViewModel.DeviceId, atomicMeasurementViewModel.PortNumber,
                atomicMeasurementViewModel.GraphicId);
                
            if (FindDuplicateMeasurementSetAtomicMeasurement(measurementSetId, atomicMeasurementId)) 
            {
                return 0;
            }
           
            var measurementSetAtomicMeasurement = new MeasurementSetAtomicMeasurement
                                                        {MeasurementSetId = measurementSetId,
                                                          AtomicMeasurementId = atomicMeasurementId};
            _applicationContext.MeasurementSetAtomicMeasurement.Add(measurementSetAtomicMeasurement);
            _applicationContext.SaveChanges();
            return measurementSetAtomicMeasurement.Id;          

        }

        public void DeleteFromMeasurementSet(Guid measurementSetId, int atomicId) 
        {
           
            var measurementSetAtomicMeasurement = new MeasurementSetAtomicMeasurement {
                                                MeasurementSetId = measurementSetId,
                                                AtomicMeasurementId = atomicId};
            _applicationContext.MeasurementSetAtomicMeasurement.Remove(measurementSetAtomicMeasurement);
            _applicationContext.SaveChanges();

            
        }

        private bool FindDuplicateMeasurementSetAtomicMeasurement (Guid measurementSetId, int atomicId) 
        {
            return _applicationContext.MeasurementSetAtomicMeasurement.Count(_ => _.AtomicMeasurementId == atomicId && _.MeasurementSetId == measurementSetId) > 0;
        }

        private int FindOrCreate(int measurementId, int deviceId, int portNumber, int graphicId) 
        {
          
            var atomic = _applicationContext.AtomicMeasurement.FirstOrDefault(_ => _.DeviceId == deviceId &&
                                                                                       _.GraphicId == graphicId &&
                                                                                       _.MeasurementId == measurementId &&
                                                                                       _.PortNumber == portNumber);
            if (atomic != null) {
                return atomic.Id;
            }

            return Create(measurementId, deviceId, portNumber, graphicId);
           
        }

        private int Create(int measurementId, int deviceId, int portNumber, int graphicId) 
        {
            var atomicMeasurement = new AtomicMeasurement { MeasurementId = measurementId, DeviceId = deviceId, PortNumber = portNumber, GraphicId = graphicId };
            _applicationContext.AtomicMeasurement.Add(atomicMeasurement);
            _applicationContext.SaveChanges();
            return atomicMeasurement.Id;
            
        }

    }
}