using Microsoft.Extensions.Hosting;
using NWDCrucial.Configuration;
using NWDHub.Configuration;

namespace NWDHub.Services
{
    public class NWDHubStartupService : IHostedService
    {
        private IServiceProvider _Services;
        
        public NWDHubStartupService(IServiceProvider sServices)
        {
            _Services = sServices;
        }

        public async Task StartAsync(CancellationToken sCancellationToken)
        {
            NWDHubRecursiveService.StartTimer();
            await Task.Delay(1, sCancellationToken);
        }

        public async Task StopAsync(CancellationToken sCancellationToken)
        {
            await Task.Delay(1, sCancellationToken);
        }
        
    }
}
