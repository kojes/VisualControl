using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using VueExample.Providers;
using VueExample.ViewModels;

namespace VueExample.Hubs
{
    public class LivePointHub : Hub
    {
        private readonly IMeasurementProvider _measurementProvider;
        public LivePointHub(IMeasurementProvider measurementProvider)
        {
            _measurementProvider = measurementProvider;            
        }
        public async Task GetLastValue(int measurementId, int deviceId, int graphicId, int port)
        {
            await Clients.Caller.SendAsync("lastValue", _measurementProvider.GetPoints(measurementId, deviceId, graphicId, port).LastOrDefault().Value);
        }

        public async Task GetLastValues(List<AtomicMeasurementExtendedViewModel> atomicList)
        {
            await Clients.Caller.SendAsync("lastValues", _measurementProvider.GetLivePoints(atomicList));
        }
    }
}