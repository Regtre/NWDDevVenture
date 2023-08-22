using Microsoft.Extensions.Hosting;

namespace NWDWebDownloader.Services
{
    public class NWDWebDownloaderStartupService : IHostedService
    {
        private IServiceProvider _Services;
        
        public NWDWebDownloaderStartupService(IServiceProvider sServices)
        {
            _Services = sServices;
        }

        public async Task StartAsync(CancellationToken sCancellationToken)
        {
            NWDWebDownloaderRecursiveService.StartTimer();
            await Task.Delay(1, sCancellationToken);
        }

        public async Task StopAsync(CancellationToken sCancellationToken)
        {
            await Task.Delay(1, sCancellationToken);
        }
        
    }
}
