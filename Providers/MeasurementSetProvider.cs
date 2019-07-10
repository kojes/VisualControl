using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VueExample.Contexts;
using VueExample.Helpers;
using VueExample.Models;
using VueExample.ResponseObjects;
using VueExample.ViewModels;

namespace VueExample.Providers
{
    public class MeasurementSetProvider : IMeasurementSetProvider
    {
        private readonly AppSettings _appSettings;
        public MeasurementSetProvider(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        
        public (MeasurementSetViewModel, Error) Create(string name)
        {
            if(String.IsNullOrEmpty(name))
            {
                return (null, new Error("Невозможно создать пустую серию"));
            }
            if(IsExistedDuplicate(name))
            {
                return (null, new Error("Такое имя серии уже существует"));
            }
            if(IsNameReservedToGenerated(name))
            {
                return (null, new Error("Такое имя серии зарезервировано"));
            }
            using(ApplicationContext applicationContext = new ApplicationContext())
            {
               var measurementSet = new MeasurementSet {Name = name};
               applicationContext.MeasurementSet.Add(measurementSet);
               applicationContext.SaveChanges();
               return (new MeasurementSetViewModel {MeasurementSetId = measurementSet.MeasurementSetId, 
                                                  Name = measurementSet.Name, 
                                                  Route = Convert.ToString(measurementSet.MeasurementSetId), 
                                                  IsGenerated = false}, new Error());
            }
        }

        public bool Delete(Guid id)
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

        public List<MeasurementSetViewModel> GetAllSets()
        {
            var measurementSetsViewModelList = new List<MeasurementSetViewModel>{GenerateOnlineMeasurementSet()};
            measurementSetsViewModelList.AddRange(GenerateMaterialBasedMeasurementSets());
            using(ApplicationContext applicationContext = new ApplicationContext())
            {                
                var measurementSetsFromDb = applicationContext.MeasurementSet.ToList();
                measurementSetsViewModelList.AddRange(from measurementSet in measurementSetsFromDb
                                                      select new MeasurementSetViewModel 
                                                      { MeasurementSetId = measurementSet.MeasurementSetId, 
                                                      Name = measurementSet.Name, IsGenerated = false, 
                                                      Route = Convert.ToString(measurementSet.MeasurementSetId)});
            }

           return measurementSetsViewModelList;
           
        }

      

        public List<AtomicMeasurementExtendedViewModel> GetAtomicsById(Guid measurementSetId, IMeasurementProvider measurementProvider)
        {
            var atomicViewModelList = new List<AtomicMeasurementExtendedViewModel>();
            using(ApplicationContext applicationContext = new ApplicationContext())
            {
         
                var atomicList = applicationContext.AtomicMeasurement.Include(_ => _.MeasurementSetAtomicMeasurement).
                                                                      ThenInclude(_ => _.MeasurementSet).
                                                                      Include(_ => _.Device).
                                                                      Include(_ => _.Graphic).
                                                                      Include(_ => _.Measurement).
                                                                      Where( x => x.MeasurementSetAtomicMeasurement.Any(_ => _.MeasurementSetId == measurementSetId)).
                                                                      AsNoTracking().
                                                                      ToList();
                atomicViewModelList.AddRange(from atomic in atomicList
                                             select new AtomicMeasurementExtendedViewModel 
                                             { AtomicMeasurementId = atomic.Id, DeviceId = atomic.DeviceId, DeviceName = atomic.Device.Name,
                                             MeasurementId = atomic.MeasurementId, MeasurementName = atomic.Measurement.Name, 
                                             GraphicId = atomic.GraphicId, GraphicName = atomic.Graphic.RussianName,
                                             PortNumber = atomic.PortNumber, MeasurementSetId = measurementSetId, 
                                             IsOnline = measurementProvider.IsMeasurementOnline(atomic.MeasurementId)});
                
                                            
            }
            return atomicViewModelList;
        }

        public List<AtomicMeasurementExtendedViewModel> GetAtomicsByMaterial(int materialId, IMeasurementProvider measurementProvider)
        {
            var atomicViewModelList = new List<AtomicMeasurementExtendedViewModel>();
            using(ApplicationContext applicationContext = new ApplicationContext())
            {

                var atomicList = applicationContext.AtomicMeasurement.Include(_ => _.Device).
                                                                      Include(_ => _.Graphic).
                                                                      Include(_ => _.Measurement).
                                                                      ThenInclude(_ => _.Material).
                                                                      Where(_ => _.Measurement.Material.MaterialId == materialId).
                                                                      AsNoTracking().
                                                                      ToList();
                atomicViewModelList.AddRange(from atomic in atomicList
                                                                    select new AtomicMeasurementExtendedViewModel 
                                                                    { AtomicMeasurementId = atomic.Id, DeviceId = atomic.DeviceId, DeviceName = atomic.Device.Name,
                                                                    MeasurementId = atomic.MeasurementId, MeasurementName = atomic.Measurement.Name, 
                                                                    GraphicId = atomic.GraphicId, GraphicName = atomic.Graphic.RussianName,
                                                                    PortNumber = atomic.PortNumber, 
                                                                    IsOnline = measurementProvider.IsMeasurementOnline(atomic.MeasurementId)});

            }
            return atomicViewModelList;
        }

        public List<AtomicMeasurementExtendedViewModel> GetAtomicsOnline(IMeasurementProvider measurementProvider)
        {
            var atomicViewModelList = new List<AtomicMeasurementExtendedViewModel>();
            using(ApplicationContext applicationContext = new ApplicationContext())
            {
                var atomicList = applicationContext.AtomicMeasurement.Include(_ => _.Device).
                                                                      Include(_ => _.Graphic).
                                                                      Include(_ => _.Measurement).
                                                                      Where(x => x.Measurement.Points.
                                                                      OrderByDescending(_ => _.PointId).
                                                                      FirstOrDefault().Time.AddSeconds((double) x.Measurement.IntervalInSeconds * 2) > DateTime.Now.AddHours(-1.0)).
                                                                      AsNoTracking().
                                                                      ToList();
                atomicViewModelList.AddRange(from atomic in atomicList
                                             select new AtomicMeasurementExtendedViewModel 
                                             { AtomicMeasurementId = atomic.Id, DeviceId = atomic.DeviceId, 
                                             DeviceName = atomic.Device.Name,
                                             MeasurementId = atomic.MeasurementId, MeasurementName = atomic.Measurement.Name, 
                                             GraphicId = atomic.GraphicId, GraphicName = atomic.Graphic.RussianName,
                                             PortNumber = atomic.PortNumber, IsOnline = true});
            }
            return atomicViewModelList;
        }

        private bool IsExistedDuplicate(string setName)
        {
            using(ApplicationContext applicationContext = new ApplicationContext())
            {
             
               return applicationContext.MeasurementSet.AsNoTracking()
                                                       .Any(x => x.Name == setName);
            }
        }

        private bool IsNameReservedToGenerated(string setName)
        {
            if(setName == _appSettings.OnlineName)
            {
                return true;
            }

            using(ApplicationContext applicationContext = new ApplicationContext())
            {
                if(applicationContext.Material.Select(_ => _.Name).Distinct().Contains(setName))
                {
                    return true;
                }
            }

            return false;
        }

        private MeasurementSetViewModel GenerateOnlineMeasurementSet()
        {
            return new MeasurementSetViewModel { MeasurementSetId = Guid.NewGuid(),  
                                                 Name = _appSettings.OnlineName, IsGenerated = true, 
                                                 Route = "online" };
        }

        private List<MeasurementSetViewModel> GenerateMaterialBasedMeasurementSets()
        {
            var materialBasedMeasurementSetsList = new List<MeasurementSetViewModel>();
            using(ApplicationContext applicationContext = new ApplicationContext())
            {
                 foreach (var material in applicationContext.Material.Where(_ => _.Name != "Неизвестно").AsNoTracking().ToList())
                 {
                     materialBasedMeasurementSetsList.Add(new MeasurementSetViewModel { MeasurementSetId = Guid.NewGuid(), 
                                                                                        Name = material.Name, IsGenerated = true, 
                                                                                        Route = $"material/{material.MaterialId}" });
                 }
                     
            }
            return materialBasedMeasurementSetsList;
        }

      
    }
}