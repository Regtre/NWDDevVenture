using Microsoft.Extensions.Hosting;

namespace NWDWebEditor.Services
{
    public class NWDWebEditorStartupService : IHostedService
    {
        private IServiceProvider _Services;
        
        public NWDWebEditorStartupService(IServiceProvider sServices)
        {
            _Services = sServices;
        }

        public async Task StartAsync(CancellationToken sCancellationToken)
        {
            NWDWebEditorRecursiveService.StartTimer();
            await Task.Delay(1, sCancellationToken);
        }

        public async Task StopAsync(CancellationToken sCancellationToken)
        {
            await Task.Delay(1, sCancellationToken);
        }
        
    }
}
