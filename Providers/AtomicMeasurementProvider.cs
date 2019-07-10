using System;
using System.Linq;
using VueExample.Contexts;
using VueExample.Models;
using VueExample.ViewModels;

namespace VueExample.Providers {
    public class AtomicMeasurementProvider : IAtomicMeasurementProvider {
        public int AddToMeasurementSet (AtomicMeasurementViewModel atomicMeasurementViewModel) {
            var measurementSetId = atomicMeasurementViewModel.MeasurementSetId;
            int atomicMeasurementId = FindOrCreate(atomicMeasurementViewModel.MeasurementId,
                atomicMeasurementViewModel.DeviceId, atomicMeasurementViewModel.PortNumber,
                atomicMeasurementViewModel.GraphicId);
                
            if (FindDuplicateMeasurementSetAtomicMeasurement (measurementSetId, atomicMeasurementId)) {
                return 0;
            }

            using (ApplicationContext applicationContext = new ApplicationContext ()) {

                var measurementSetAtomicMeasurement = new MeasurementSetAtomicMeasurement
                                                         {MeasurementSetId = measurementSetId,
                                                          AtomicMeasurementId = atomicMeasurementId};
                applicationContext.MeasurementSetAtomicMeasurement.Add(measurementSetAtomicMeasurement);
                applicationContext.SaveChanges();
                return measurementSetAtomicMeasurement.Id;

            }

        }

        public void DeleteFromMeasurementSet (Guid measurementSetId, int atomicId) {
            using (ApplicationContext applicationContext = new ApplicationContext ()) {
                var measurementSetAtomicMeasurement = new MeasurementSetAtomicMeasurement {
                MeasurementSetId = measurementSetId,
                AtomicMeasurementId = atomicId
                };
                applicationContext.MeasurementSetAtomicMeasurement.Remove (measurementSetAtomicMeasurement);
                applicationContext.SaveChanges ();

            }
        }

        private bool FindDuplicateMeasurementSetAtomicMeasurement (Guid measurementSetId, int atomicId) {
            var isDuplicateExist = false;
            using (ApplicationContext applicationContext = new ApplicationContext ()) {
                if (applicationContext.MeasurementSetAtomicMeasurement.Count (_ => _.AtomicMeasurementId == atomicId && _.MeasurementSetId == measurementSetId) > 0) {
                    isDuplicateExist = true;
                }

            }

            return isDuplicateExist;
        }

        private int FindOrCreate (int measurementId, int deviceId, int portNumber, int graphicId) {
            using (ApplicationContext applicationContext = new ApplicationContext ()) {
                var atomic = applicationContext.AtomicMeasurement.FirstOrDefault (_ => _.DeviceId == deviceId &&
                    _.GraphicId == graphicId &&
                    _.MeasurementId == measurementId &&
                    _.PortNumber == portNumber);
                if (atomic != null) {
                    return atomic.Id;
                }
                return Create (measurementId, deviceId, portNumber, graphicId);

            }
        }

        private int Create (int measurementId, int deviceId, int portNumber, int graphicId) {
            using (ApplicationContext applicationContext = new ApplicationContext ()) {
                var atomicMeasurement = new AtomicMeasurement { MeasurementId = measurementId, DeviceId = deviceId, PortNumber = portNumber, GraphicId = graphicId };
                applicationContext.AtomicMeasurement.Add (atomicMeasurement);
                applicationContext.SaveChanges ();
                return atomicMeasurement.Id;
            }
        }

    }
}