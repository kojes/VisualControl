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
        private readonly ApplicationContext _applicationContext;
        public MeasurementSetProvider(IOptions<AppSettings> appSettings, ApplicationContext applicationContext)
        {
            _appSettings = appSettings.Value;
            _applicationContext = applicationContext;

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
            
            var measurementSet = new MeasurementSet {Name = name};
            _applicationContext.MeasurementSet.Add(measurementSet);
            _applicationContext.SaveChanges();
            return (new MeasurementSetViewModel{MeasurementSetId = measurementSet.MeasurementSetId, 
                                                Name = measurementSet.Name, 
                                                Route = Convert.ToString(measurementSet.MeasurementSetId), 
                                                IsGenerated = false}, new Error());            
        }

        public bool Delete(Guid id)
        {           
            if(!_applicationContext.MeasurementSet.Any(x => x.MeasurementSetId == id))
            {
                 return false;
            }
                
            var deletedAtomicInSet = _applicationContext.MeasurementSetAtomicMeasurement.Where(x => x.MeasurementSetId == id);
            _applicationContext.MeasurementSetAtomicMeasurement.RemoveRange(deletedAtomicInSet);
            var deleted = _applicationContext.MeasurementSet.Find(id);
            _applicationContext.Remove(deleted);
            _applicationContext.SaveChanges();
            return true;
            
        }

        public List<MeasurementSetViewModel> GetAllSets()
        {
            var measurementSetsViewModelList = new List<MeasurementSetViewModel>{GenerateOnlineMeasurementSet()};
            measurementSetsViewModelList.AddRange(GenerateMaterialBasedMeasurementSets());
                           
            var measurementSetsFromDb = _applicationContext.MeasurementSet.ToList();
            measurementSetsViewModelList.AddRange(from measurementSet in measurementSetsFromDb
                                                  select new MeasurementSetViewModel 
                                                  {MeasurementSetId = measurementSet.MeasurementSetId, 
                                                   Name = measurementSet.Name, IsGenerated = false, 
                                                   Route = Convert.ToString(measurementSet.MeasurementSetId)});          

           return measurementSetsViewModelList;
           
        }

      

        public List<AtomicMeasurementExtendedViewModel> GetAtomicsById(Guid measurementSetId, IMeasurementProvider measurementProvider)
        {
            var atomicViewModelList = new List<AtomicMeasurementExtendedViewModel>();
           
         
            var atomicList = _applicationContext.AtomicMeasurement.Include(_ => _.MeasurementSetAtomicMeasurement).
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
                                             GraphicId = atomic.GraphicId, GraphicUnit = atomic.Graphic.Unit,
                                             PortNumber = atomic.PortNumber, MeasurementSetId = measurementSetId, 
                                             IsOnline = measurementProvider.IsMeasurementOnline(atomic.MeasurementId)});
                
                                            
            
            return atomicViewModelList;
        }

        public List<AtomicMeasurementExtendedViewModel> GetAtomicsByMaterial(int materialId, IMeasurementProvider measurementProvider)
        {
            var atomicViewModelList = new List<AtomicMeasurementExtendedViewModel>();
           

            var atomicList = _applicationContext.AtomicMeasurement.Include(_ => _.Device).
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
                                                                    GraphicId = atomic.GraphicId, GraphicUnit = atomic.Graphic.Unit,
                                                                    PortNumber = atomic.PortNumber, 
                                                                    IsOnline = measurementProvider.IsMeasurementOnline(atomic.MeasurementId)});

           
            return atomicViewModelList;
        }

        public List<AtomicMeasurementExtendedViewModel> GetAtomicsOnline(IMeasurementProvider measurementProvider)
        {
            var atomicViewModelList = new List<AtomicMeasurementExtendedViewModel>();
            
            var atomicList = _applicationContext.AtomicMeasurement.Include(_ => _.Device).
                                                                    Include(_ => _.Graphic).
                                                                    Include(_ => _.Measurement).
                                                                    Where(x => x.Measurement.Points.
                                                                    OrderByDescending(_ => _.PointId).
                                                                    FirstOrDefault().Time.AddSeconds((double) x.Measurement.IntervalInSeconds * 2) > 
                                                                                                     DateTime.Now.AddHours(-1.0)).
                                                                    AsNoTracking().
                                                                    ToList();
             atomicViewModelList.AddRange(from atomic in atomicList
                                             select new AtomicMeasurementExtendedViewModel 
                                             { AtomicMeasurementId = atomic.Id, DeviceId = atomic.DeviceId, 
                                             DeviceName = atomic.Device.Name,
                                             MeasurementId = atomic.MeasurementId, MeasurementName = atomic.Measurement.Name, 
                                             GraphicId = atomic.GraphicId, GraphicUnit = atomic.Graphic.Unit,
                                             PortNumber = atomic.PortNumber, IsOnline = true});
            
            return atomicViewModelList;
        }

        private bool IsExistedDuplicate(string setName)
        {            
            return _applicationContext.MeasurementSet.AsNoTracking()
                                                       .Any(x => x.Name == setName);           
        }

        private bool IsNameReservedToGenerated(string setName)
        {
            if(setName == _appSettings.OnlineName)
            {
                return true;
            }
          
            if(_applicationContext.Material.Select(_ => _.Name).Distinct().Contains(setName))
            {
                return true;
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
           
            foreach (var material in _applicationContext.Material.Where(_ => _.Name != "Неизвестно").AsNoTracking().ToList())
            {
                materialBasedMeasurementSetsList.Add(new MeasurementSetViewModel { MeasurementSetId = Guid.NewGuid(), 
                                                                                        Name = material.Name, IsGenerated = true, 
                                                                                        Route = $"material/{material.MaterialId}" });
            }                    
            
            return materialBasedMeasurementSetsList;
        }

      
    }
}