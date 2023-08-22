using Microsoft.Extensions.Hosting;

namespace NWDWebStandard.Services
{
    public class NWDWebStandardStartupService : IHostedService
    {
        private IServiceProvider _Services;
        
        public NWDWebStandardStartupService(IServiceProvider sServices)
        {
            _Services = sServices;
        }

        public async Task StartAsync(CancellationToken sCancellationToken)
        {
            NWDWebStandardRecursiveService.StartTimer();
            await Task.Delay(1, sCancellationToken);
        }

        public async Task StopAsync(CancellationToken sCancellationToken)
        {
            await Task.Delay(1, sCancellationToken);
        }
        
    }
}
