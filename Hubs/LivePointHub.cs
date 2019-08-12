using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using VueExample.ViewModels;
using VueExample.Providers.ChipVerification.Abstract;
using VueExample.ResponseObjects;

namespace VueExample.Hubs
{
    public class LivePointHub : Hub
    {
        private readonly IPointProvider _pointProvider;
        public LivePointHub(IPointProvider pointProvider)
        {
            _pointProvider = pointProvider;            
        }
        public async Task GetLastValue(int measurementId, int deviceId, int graphicId, int port)
        {
            await Clients.Caller.SendAsync("lastValue", (await _pointProvider.GetPoints(measurementId, deviceId, graphicId, port)).TObject.LastOrDefault().Value);
        }

        public async Task GetLastValues(List<AtomicMeasurementExtendedViewModel> atomicList)
        {
            await Clients.Caller.SendAsync("lastValues", await _pointProvider.GetLivePoints(atomicList));
        }
    }
}