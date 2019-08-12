using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using VueExample.Contexts;
using VueExample.Providers.ChipVerification.Abstract;
using VueExample.ResponseObjects;
using VueExample.Models;
using VueExample.ViewModels;

namespace VueExample.Providers.ChipVerification
{
    public class PointProvider : IPointProvider
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IMapper _mapper;              
        public PointProvider(ApplicationContext applicationContext, IMapper mapper) 
        {
            _mapper = mapper;  
            _applicationContext = applicationContext;
        }

        public async Task<AfterDbManipulationObject<List<long>>> CreatePointSet(PointSetViewModel pointSetViewModel)
        {
            var pointsList = new List<Point>();
            foreach (var atomicPoint in pointSetViewModel.AtomicPointList)
            {
                var point = new Point{  MeasurementId = pointSetViewModel.MeasurementId, 
                                        DeviceId = pointSetViewModel.DeviceId,
                                        PortNumber = pointSetViewModel.PortNumber,
                                        GraphicId = atomicPoint.GraphicId,
                                        Value = atomicPoint.Value,
                                        Time = atomicPoint.Time};
                _applicationContext.Point.Add(point);
                pointsList.Add(point);
            }
            await _applicationContext.SaveChangesAsync();
            var obj = new AfterDbManipulationObject<List<long>>(pointsList.Select(x => x.PointId).ToList());
            return obj;

        }

        public async Task<AfterDbManipulationObject<PointViewModel>> CreateSinglePoint(PointViewModel pointViewModel)
        {
            var point = _mapper.Map<Point>(pointViewModel);
            _applicationContext.Point.Add(point);
            await _applicationContext.SaveChangesAsync();
            var obj = new AfterDbManipulationObject<PointViewModel>(_mapper.Map<PointViewModel>(point));
            return obj;
        }

        public async Task<AfterDbManipulationObject<List<LivePointViewModel>>> GetLivePoints(List<AtomicMeasurementExtendedViewModel> atomicMeasurementViewModelList)
        {
           var livePointViewModelList = new List<LivePointViewModel>();
           var obj = new AfterDbManipulationObject<List<LivePointViewModel>>(livePointViewModelList);         

           for(int i = 0;  i < atomicMeasurementViewModelList.Count(); i++) 
           {
                var measurementIdSqlParameter = new SqlParameter("@MeasurementID", atomicMeasurementViewModelList[i].MeasurementId);
                var deviceIdSqlParameter = new SqlParameter("@DeviceID", atomicMeasurementViewModelList[i].DeviceId);
                var graphicIdSqlParameter = new SqlParameter("@GraphicID", atomicMeasurementViewModelList[i].GraphicId);
                var portNumberSqlParameter = new SqlParameter("@PortNumber", atomicMeasurementViewModelList[i].PortNumber);
                var lastPoint = await _applicationContext.Point.FromSql("EXECUTE GetLastPoint @MeasurementID, @DeviceID, @GraphicID, @PortNumber", measurementIdSqlParameter, deviceIdSqlParameter, 
                                                                                          graphicIdSqlParameter, portNumberSqlParameter).
                                                                                          FirstOrDefaultAsync();
                livePointViewModelList.Add(new LivePointViewModel{Value = lastPoint.Value, MeasurementId = atomicMeasurementViewModelList[i].MeasurementId});
           }

           if(livePointViewModelList.Count == 0)
           {
               obj.AddError(new Error(@"Точки не найдены"));
           }

           return obj;
        }

        public async Task<AfterDbManipulationObject<List<PointViewModel>>> GetPoints(int measurementId, int deviceId, int graphicId, int port)
        {
            var pointsList = await _applicationContext.Point.Where(x => x.MeasurementId == measurementId 
                                               && x.DeviceId == deviceId 
                                               && x.GraphicId == graphicId 
                                               && x.PortNumber == port).
                                               OrderBy(x => x.PointId).
                                               AsNoTracking().
                                               ProjectTo<PointViewModel>(_mapper.ConfigurationProvider).
                                               ToListAsync();      
            var obj = new AfterDbManipulationObject<List<PointViewModel>>();
            if(pointsList.Count == 0)
            {
                obj.AddError(new Error(@"Точки не найдены"));
                return obj;
            }
            obj.SetObject(pointsList);
            return obj;
        }
    }
}