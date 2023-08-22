using Microsoft.Extensions.Hosting;

namespace NWDWebStudioDemo.Services
{
    public class NWDWebStudioDemoStartupService : IHostedService
    {
        private IServiceProvider _Services;
        
        public NWDWebStudioDemoStartupService(IServiceProvider sServices)
        {
            _Services = sServices;
        }

        public async Task StartAsync(CancellationToken sCancellationToken)
        {
            NWDWebStudioDemoRecursiveService.StartTimer();
            await Task.Delay(1, sCancellationToken);
        }

        public async Task StopAsync(CancellationToken sCancellationToken)
        {
            await Task.Delay(1, sCancellationToken);
        }
        
    }
}
