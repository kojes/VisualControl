using System.Globalization;
using System.Collections.Immutable;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using VueExample.Contexts;
using VueExample.Models;
using VueExample.ViewModels;
using System.Data.SqlClient;
using VueExample.Providers.ChipVerification.Abstract;
using System.Threading.Tasks;

namespace VueExample.Providers.ChipVerification
{
    public class SimpleMeasurementProvider : IMeasurementProvider
    {
        private readonly IMapper _mapper;
        private readonly ApplicationContext _applicationContext;
        public SimpleMeasurementProvider(IMapper mapper, ApplicationContext applicationContext) 
        {
            _mapper = mapper;  
            _applicationContext = applicationContext;
        }    
        public (List<Process>, List<CodeProduct>, List<MeasuredDevice>, List<Measurement>) GetAllMeasurementInfo(int facilityId)
        {
          
            var codeProductIdList = new List<int>();
            var codeProductList = new List<CodeProduct>();
            var processList = new List<Process>();
            var measuredDeviceList = new List<MeasuredDevice>();
            var measurementList = new List<Measurement>();
            var processIdList = new List<int>();
           
            foreach (var deviceId in _applicationContext.Measurement.Where(x => x.FacilityId == facilityId).ToList().Select(x => x.MeasuredDeviceId).Distinct().Where(x => x != null).ToList())
            {
                codeProductIdList.Add(_applicationContext.MeasuredDevice.FirstOrDefault(x => x.MeasuredDeviceId == deviceId).CodeProductId);
                measuredDeviceList.Add(_applicationContext.MeasuredDevice.FirstOrDefault(x=>x.MeasuredDeviceId == deviceId));
            }

            measurementList = _applicationContext.Measurement.ToList();
            

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
            var points = _applicationContext.Point.Where(x => x.MeasurementId == measurementId).ToList();
            var availableDevices = points.Select(x => x.DeviceId).Distinct();
            var portsArray = points.Select(x => x.PortNumber).Distinct().ToArray().OrderBy(x=>x);
            var availableGraphics = points.Select(x => x.GraphicId).Distinct();
            var devicesArray = _applicationContext.Device.Where(x => availableDevices.Contains(x.DeviceId)).ToArray();
            var graphicsArray = _applicationContext.Graphic.Where(x => availableGraphics.Contains(x.GraphicId)).ToArray();
            var result = new
            {
                Ports = portsArray,
                Devices = devicesArray,
                Graphic = graphicsArray
            };
            return result;            
        }

        public List<PointViewModel> GetPoints(int measurementId, int deviceId, int graphicId, int port)
        {            
            return _applicationContext.Point.Where(x => x.MeasurementId == measurementId 
                                               && x.DeviceId == deviceId 
                                               && x.GraphicId == graphicId 
                                               && x.PortNumber == port).
                                               OrderBy(x => x.PointId).
                                               AsNoTracking().
                                               ProjectTo<PointViewModel>(_mapper.ConfigurationProvider).
                                               ToList();           
            
        }

        public Measurement GetById(int measurementId)
        {           
            return _applicationContext.Measurement.Find(measurementId);            
        }

        public MaterialViewModel GetMaterial(int measurementId)
        {           
            return _mapper.Map<MaterialViewModel>(_applicationContext.Measurement.
                                                                        Include(_ => _.Material).
                                                                        FirstOrDefault(_ => _.MeasurementId == measurementId).Material);           
        }

        public MeasurementOnlineStatus GetMeasurementOnlineStatus(int measurementId)
        {         
            var measurementIdSqlParameter = new SqlParameter("measurementID", measurementId);
            var measurementOnlineStatus = _applicationContext.MeasurementOnlineStatus.FromSql("EXECUTE GetMeasurementOnlineStatus @measurementID", measurementIdSqlParameter).FirstOrDefault();
            return measurementOnlineStatus;            
        }

        public bool IsMeasurementOnline(int measurementId) => GetMeasurementOnlineStatus(measurementId).IsOnline;
        
        public List<LivePointViewModel> GetLivePoints1(List<AtomicMeasurementExtendedViewModel> atomicMeasurementViewModelList)
        {         

            return (from _ in atomicMeasurementViewModelList
                                select new LivePointViewModel
                                {
                                    Value = _applicationContext.Point.
                                    OrderByDescending(x => x.PointId).
                                    FirstOrDefault(x => 
                                    x.MeasurementId == _.MeasurementId &&
                                    x.DeviceId == _.DeviceId &&
                                    x.GraphicId == _.GraphicId &&
                                    x.PortNumber == _.PortNumber).Value,
                                    MeasurementId = _.MeasurementId
                                }).ToList();
            
        }

        public List<LivePointViewModel> GetLivePoints(List<AtomicMeasurementExtendedViewModel> atomicMeasurementViewModelList)
        {         
           var livePointViewModelList = new List<LivePointViewModel>();
           for (int i = 0; i < atomicMeasurementViewModelList.Count(); i++)
           {
                var measurementIdSqlParameter = new SqlParameter("@MeasurementID", atomicMeasurementViewModelList[i].MeasurementId);
                var deviceIdSqlParameter = new SqlParameter("@DeviceID", atomicMeasurementViewModelList[i].DeviceId);
                var graphicIdSqlParameter = new SqlParameter("@GraphicID", atomicMeasurementViewModelList[i].GraphicId);
                var portNumberSqlParameter = new SqlParameter("@PortNumber", atomicMeasurementViewModelList[i].PortNumber);
                var lastPoint = _applicationContext.Point.FromSql("EXECUTE GetLastPoint @MeasurementID, @DeviceID, @GraphicID, @PortNumber", measurementIdSqlParameter, deviceIdSqlParameter, 
                                                                                          graphicIdSqlParameter, portNumberSqlParameter).
                                                                                          FirstOrDefault();
                livePointViewModelList.Add(new LivePointViewModel{Value = lastPoint.Value, MeasurementId = atomicMeasurementViewModelList[i].MeasurementId});
           }

           return livePointViewModelList;
            
        }


        public List<MeasurementStatisticsViewModel> GetMeasurementStatistics(List<AtomicMeasurementExtendedViewModel> atomicMeasurementViewModelList)
        {
            var measurementStatisticsViewModelList = new List<MeasurementStatisticsViewModel>();            
            var measurementStatisticsList = _applicationContext.Point.GroupBy(x => new {MeasurementId = x.MeasurementId, DeviceId = x.DeviceId, GraphicId = x.GraphicId, PortNumber = x.PortNumber})
                                                        .Select(x => new MeasurementStatisticsViewModel{Maximum = Convert.ToString(x.Max(_ => _.Value)), Minimum = Convert.ToString(x.Min(_ => _.Value)), 
                                                         MeasurementId = x.Key.MeasurementId, GraphicId = x.Key.GraphicId, PortNumber = x.Key.PortNumber, DeviceId = x.Key.DeviceId, 
                                                         LastValue = x.OrderByDescending(_ => _.PointId).FirstOrDefault().Value, After300Value = x.OrderBy(_ => _.PointId).Skip(300).FirstOrDefault().Value,
                                                         FirstPointValue = x.OrderBy(_ => _.PointId).FirstOrDefault().Value }).ToList();
         
            foreach (var atomic in atomicMeasurementViewModelList)
            {
                var atomicStatistics = measurementStatisticsList.FirstOrDefault(x => x.MeasurementId == atomic.MeasurementId && x.DeviceId == atomic.DeviceId && x.GraphicId == atomic.GraphicId && x.PortNumber == atomic.PortNumber);
                atomicStatistics.MeasurementName = atomic.MeasurementName;
                atomicStatistics.DeviceName = atomic.DeviceName;
                atomicStatistics.GraphicUnit = atomic.GraphicUnit;
                measurementStatisticsViewModelList.Add(atomicStatistics);
            }                                                
            return measurementStatisticsViewModelList;
        }

        public async Task<List<int>> GetAvailablePorts(int measurementId)
        {
            return await _applicationContext.Point.Where(_ => _.MeasurementId == measurementId).Select(x => x.PortNumber).Distinct().ToListAsync();
        }
    }
}
