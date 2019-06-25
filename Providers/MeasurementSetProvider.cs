using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using VueExample.Contexts;
using VueExample.Models;
using VueExample.ViewModels;

namespace VueExample.Providers
{
    public class MeasurementSetProvider : IMeasurementSetProvider
    {
        public MeasurementSet Create(string name)
        {
            if(IsExistedDuplicate(name))
            {
                return null;
            }
            using(ApplicationContext applicationContext = new ApplicationContext())
            {
               var measurementSet = new MeasurementSet {Name = name};
               applicationContext.MeasurementSet.Add(measurementSet);
               applicationContext.SaveChanges();
               return measurementSet;
            }
        }

        public bool Delete(int id)
        {

            using(ApplicationContext applicationContext = new ApplicationContext())
            {
                if(!applicationContext.MeasurementSet.Any(x => x.MeasurementSetId == id))
                {
                    return false;
                }
                
                var deletedAtomicInSet = applicationContext.MeasurementSetAtomicMeasurement.Where(x => x.MeasurementSetId == id);
                applicationContext.MeasurementSetAtomicMeasurement.RemoveRange(deletedAtomicInSet);
                var deleted = applicationContext.MeasurementSet.Find(id);
                applicationContext.Remove(deleted);
                applicationContext.SaveChanges();
                return true;
            }
        }

        public List<MeasurementSet> GetAllSets()
        {
            using(ApplicationContext applicationContext = new ApplicationContext())
            {
                return applicationContext.MeasurementSet.ToList();
            }
        }

        public List<AtomicMeasurementExtendedViewModel> GetAtomicsById(int measurementSetId)
        {
            var atomicViewModelList = new List<AtomicMeasurementExtendedViewModel>();
            using(ApplicationContext applicationContext = new ApplicationContext())
            {
         
                var atomicList = applicationContext.AtomicMeasurement.Include(_ => _.MeasurementSetAtomicMeasurement).ThenInclude(_ => _.MeasurementSet).Include(_ => _.Device).Include(_ => _.Graphic).Include(_ => _.Measurement).Where( x => x.MeasurementSetAtomicMeasurement.Any(_ => _.MeasurementSetId == measurementSetId)).ToList();
                atomicViewModelList.AddRange(from atomic in atomicList
                                             select new AtomicMeasurementExtendedViewModel { AtomicMeasurementId = atomic.Id, DeviceId = atomic.DeviceId, DeviceName = atomic.Device.Name,
                                             MeasurementId = atomic.MeasurementId, MeasurementName = atomic.Measurement.Name, GraphicId = atomic.GraphicId, GraphicName = atomic.Graphic.RussianName,
                                             PortNumber = atomic.PortNumber, MeasurementSetId = measurementSetId});
                
                                            
            }
            return atomicViewModelList;
        }

        private bool IsExistedDuplicate(string setName)
        {
            using(ApplicationContext applicationContext = new ApplicationContext())
            {
             
               return applicationContext.MeasurementSet.Any(x => x.Name == setName);
            }
        }
    }
}