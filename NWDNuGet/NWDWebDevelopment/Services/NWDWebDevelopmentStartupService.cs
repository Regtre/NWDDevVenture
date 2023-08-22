using Microsoft.Extensions.Hosting;

namespace NWDWebDevelopment.Services
{
    public class NWDWebDevelopmentStartupService : IHostedService
    {
        private IServiceProvider _Services;
        
        public NWDWebDevelopmentStartupService(IServiceProvider sServices)
        {
            _Services = sServices;
        }

        public async Task StartAsync(CancellationToken sCancellationToken)
        {
            NWDWebDevelopmentRecursiveService.StartTimer();
            await Task.Delay(1, sCancellationToken);
        }

        public async Task StopAsync(CancellationToken sCancellationToken)
        {
            await Task.Delay(1, sCancellationToken);
        }
        
    }
}
