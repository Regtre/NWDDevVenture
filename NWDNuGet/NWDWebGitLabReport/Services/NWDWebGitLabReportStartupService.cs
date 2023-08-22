using Microsoft.Extensions.Hosting;

namespace NWDWebGitLabReport.Services
{
    public class NWDWebGitLabReportStartupService : IHostedService
    {
        private IServiceProvider _Services;
        
        public NWDWebGitLabReportStartupService(IServiceProvider sServices)
        {
            _Services = sServices;
        }

        public async Task StartAsync(CancellationToken sCancellationToken)
        {
            NWDWebGitLabReportRecursiveService.StartTimer();
            await Task.Delay(1, sCancellationToken);
        }

        public async Task StopAsync(CancellationToken sCancellationToken)
        {
            await Task.Delay(1, sCancellationToken);
        }
        
    }
}
