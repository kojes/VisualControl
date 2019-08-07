using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using VueExample.ViewModels;
using VueExample.Providers.ChipVerification.Abstract;

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