using Microsoft.Extensions.Hosting;

namespace NWDWebTreat.Services
{
    public class NWDWebTreatStartupService : IHostedService
    {
        private IServiceProvider _Services;
        
        public NWDWebTreatStartupService(IServiceProvider sServices)
        {
            _Services = sServices;
        }

        public async Task StartAsync(CancellationToken sCancellationToken)
        {
            NWDWebTreatRecursiveService.StartTimer();
            await Task.Delay(1, sCancellationToken);
        }

        public async Task StopAsync(CancellationToken sCancellationToken)
        {
            await Task.Delay(1, sCancellationToken);
        }
        
    }
}
