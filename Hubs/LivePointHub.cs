using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using VueExample.ViewModels;
using VueExample.Providers.ChipVerification.Abstract;

namespace VueExample.Hubs
{
    public class LivePointHub : Hub
    {
        private readonly IPointProvider _pointProvider;
        public LivePointHub(IPointProvider pointProvider)
        {
            _pointProvider = pointProvider;            
        }
        public async Task GetLastValues(List<AtomicMeasurementExtendedViewModel> atomicList)
        {
             await Clients.Caller.SendAsync("lastValues", (await _pointProvider.GetLivePoints(atomicList)).TObject);
        }
    }
}