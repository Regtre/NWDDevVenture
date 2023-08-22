using Microsoft.Extensions.Hosting;

namespace NWDWebPlayerDemo.Services
{
    public class NWDWebPlayerDemoStartupService : IHostedService
    {
        private IServiceProvider _Services;
        
        public NWDWebPlayerDemoStartupService(IServiceProvider sServices)
        {
            _Services = sServices;
        }

        public async Task StartAsync(CancellationToken sCancellationToken)
        {
            NWDWebPlayerDemoRecursiveService.StartTimer();
            await Task.Delay(1, sCancellationToken);
        }

        public async Task StopAsync(CancellationToken sCancellationToken)
        {
            await Task.Delay(1, sCancellationToken);
        }
        
    }
}
