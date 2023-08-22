using Microsoft.Extensions.Hosting;

namespace NWDWebUploader.Services
{
    public class NWDWebUploaderStartupService : IHostedService
    {
        private IServiceProvider _Services;
        
        public NWDWebUploaderStartupService(IServiceProvider sServices)
        {
            _Services = sServices;
        }

        public async Task StartAsync(CancellationToken sCancellationToken)
        {
            NWDWebUploaderRecursiveService.StartTimer();
            await Task.Delay(1, sCancellationToken);
        }

        public async Task StopAsync(CancellationToken sCancellationToken)
        {
            await Task.Delay(1, sCancellationToken);
        }
        
    }
}
