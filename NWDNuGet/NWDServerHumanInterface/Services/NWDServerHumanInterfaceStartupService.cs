using Microsoft.Extensions.Hosting;

namespace NWDServerHumanInterface.Services
{
    public class NWDServerHumanInterfaceStartupService : IHostedService
    {
        private IServiceProvider _Services;
        
        public NWDServerHumanInterfaceStartupService(IServiceProvider sServices)
        {
            _Services = sServices;
        }

        public async Task StartAsync(CancellationToken sCancellationToken)
        {
            NWDServerHumanInterfaceRecursiveService.StartTimer();
            await Task.Delay(1, sCancellationToken);
        }

        public async Task StopAsync(CancellationToken sCancellationToken)
        {
            await Task.Delay(1, sCancellationToken);
        }
        
    }
}
