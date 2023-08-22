using Microsoft.Extensions.Hosting;

namespace NWDWebHttpErrorSimulator.Services
{
    public class NWDWebHttpErrorSimulatorStartupService : IHostedService
    {
        private IServiceProvider _Services;
        
        public NWDWebHttpErrorSimulatorStartupService(IServiceProvider sServices)
        {
            _Services = sServices;
        }

        public async Task StartAsync(CancellationToken sCancellationToken)
        {
            NWDWebHttpErrorSimulatorRecursiveService.StartTimer();
            await Task.Delay(1, sCancellationToken);
        }

        public async Task StopAsync(CancellationToken sCancellationToken)
        {
            await Task.Delay(1, sCancellationToken);
        }
        
    }
}
