using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using VueExample.Contexts;
using VueExample.Models;
using VueExample.ViewModels;

namespace VueExample.Providers
{
    public class SimpleMeasurementProvider : IMeasurementProvider
    {
        private readonly IMapper _mapper;
        public SimpleMeasurementProvider(IMapper mapper) => _mapper = mapper;      
        public (List<Process>, List<CodeProduct>, List<MeasuredDevice>, List<Measurement>) GetAllMeasurementInfo()
        {
            var codeProductIdList = new List<int>();
            var codeProductList = new List<CodeProduct>();
            var processList = new List<Process>();
            var measuredDeviceList = new List<MeasuredDevice>();
            var measurementList = new List<Measurement>();
            var processIdList = new List<int>();
            using (ApplicationContext db = new ApplicationContext())
            {
                foreach (var deviceId in db.Measurement.ToList().Select(x => x.MeasuredDeviceId).Distinct().Where(x => x != null).ToList())
                {
                    codeProductIdList.Add(db.MeasuredDevice.FirstOrDefault(x => x.MeasuredDeviceId == deviceId).CodeProductId);
                    measuredDeviceList.Add(db.MeasuredDevice.FirstOrDefault(x=>x.MeasuredDeviceId == deviceId));
                }

                measurementList = db.Measurement.ToList();
            }

            using (Srv6Context db = new Srv6Context())
            {
                foreach (var codeproductid in codeProductIdList.Distinct().ToList())
                {
                    processIdList.Add(db.CodeProducts.FirstOrDefault(x=>x.IdCp == codeproductid).ProcessId);
                    codeProductList.Add(db.CodeProducts.FirstOrDefault(x => x.IdCp == codeproductid));
                }

                foreach (var processid in processIdList)
                {
                    processList.Add(db.Processes.FirstOrDefault(x=>x.ProcessId == processid));
                }
            }

            measurementList.Reverse();
            return (processList.Distinct().ToList(), codeProductList.Distinct().ToList(), measuredDeviceList.Distinct().OrderBy(_ => _.Name).ToList(), measurementList);
        }

        public Object GetPointsByMeasurementId(int measurementId)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var points = db.Point.Where(x => x.MeasurementId == measurementId).ToList();
                var availableDevices = points.Select(x => x.DeviceId).Distinct();
                var portsArray = points.Select(x => x.PortNumber).Distinct().ToArray().OrderBy(x=>x);
                var availableGraphics = points.Select(x => x.GraphicId).Distinct();
                var devicesArray = db.Device.Where(x => availableDevices.Contains(x.DeviceId)).ToArray();
                var graphicsArray = db.Graphic.Where(x => availableGraphics.Contains(x.GraphicId)).ToArray();
                var result = new
                {
                    Ports = portsArray,
                    Devices = devicesArray,
                    Graphic = graphicsArray
                };
                return result;
            }
        }

        public List<PointViewModel> GetPoints(int measurementId, int deviceId, int graphicId, int port)
        {
            using (ApplicationContext applicationContext = new ApplicationContext())
            {
                return applicationContext.Point.Where(x => x.MeasurementId == measurementId 
                                               && x.DeviceId == deviceId 
                                               && x.GraphicId == graphicId 
                                               && x.PortNumber == port).
                                               OrderBy(x => x.PointId).
                                               AsNoTracking().
                                               ProjectTo<PointViewModel>(_mapper.ConfigurationProvider).
                                               ToList();
            }
            
        }

        public Measurement GetById(int measurementId)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.Measurement.Find(measurementId);
            }
        }

        public MaterialViewModel GetMaterial(int measurementId)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
               return _mapper.Map<MaterialViewModel>(db.Measurement.Include(_ => _.Material).FirstOrDefault(_ => _.MeasurementId == measurementId).Material);
            }
        }

        public MeasurementOnlineStatus GetMeasurementOnlineStatus(int measurementId)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var pointsList = db.Point.Where(x => x.MeasurementId == measurementId).OrderBy(_ => _.PointId).ToList();
                var measurement = db.Measurement.Find(measurementId);
                var measurementOnlineStatus = new MeasurementOnlineStatus(measurement.IntervalInSeconds, pointsList.First().Time, pointsList.Last().Time);
                return measurementOnlineStatus;
            }
        }

        public bool IsMeasurementOnline(int measurementId)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var measurement = db.Measurement.Find(measurementId);
                if (measurement.IntervalInSeconds != null)
                {                   
                    return db.Point.Where(_ => _.MeasurementId == measurementId).OrderByDescending(_ => _.PointId).FirstOrDefault().Time.AddSeconds((double)2 * measurement.IntervalInSeconds.GetValueOrDefault()) > DateTime.Now; 
                }
                return false;
               
            }
        }

        
    }
}
