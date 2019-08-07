using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace VueExample.BackgroundTasks
{
    public class OnlineTestingService : BackgroundService
    {
        private readonly ILogger<OnlineTestingService> _logger;
        public OnlineTestingService(ILogger<OnlineTestingService> logger)
        {
            _logger = logger;    
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => OnStopping());
            _logger.LogInformation($"OnlineTesting task start.");
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"OnlineTesting task doing background work.");
                await Task.Delay(1000, stoppingToken);
            }

        }

        private void OnStopping() 
        {
             _logger.LogInformation($"Stopped.");
        }
    }
}