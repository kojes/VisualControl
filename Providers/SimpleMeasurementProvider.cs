using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VueExample.Contexts;
using VueExample.Models;

namespace VueExample.Providers
{
    public class SimpleMeasurementProvider : IMeasurementProvider
    {
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
                foreach (var deviceId in db.Measurement.ToList().Select(x => x.MeasuredDeviceId).Distinct().ToList())
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
            return (processList.Distinct().ToList(), codeProductList.Distinct().ToList(), measuredDeviceList.Distinct().ToList(), measurementList);
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

        public List<Point> GetPoints(int measurementId, int deviceId, int graphicId, int port)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var points = db.Point.Where(x => x.MeasurementId == measurementId).ToList();
                return points.Where(x => x.DeviceId == deviceId && x.GraphicId == graphicId && x.PortNumber == port)
                    .OrderBy(x => x.Time).ToList();
            }
            
        }
    }
}
