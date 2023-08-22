using Microsoft.Extensions.Hosting;

namespace NWDWebTrackException.Services
{
    public class NWDWebTrackExceptionStartupService : IHostedService
    {
        private IServiceProvider _Services;
        
        public NWDWebTrackExceptionStartupService(IServiceProvider sServices)
        {
            _Services = sServices;
        }

        public async Task StartAsync(CancellationToken sCancellationToken)
        {
            NWDWebTrackExceptionRecursiveService.StartTimer();
            await Task.Delay(1, sCancellationToken);
        }

        public async Task StopAsync(CancellationToken sCancellationToken)
        {
            await Task.Delay(1, sCancellationToken);
        }
        
    }
}
