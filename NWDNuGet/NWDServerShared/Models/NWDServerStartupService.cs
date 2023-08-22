using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NWDServerShared.Configuration;

namespace NWDServerShared.Models
{
    public class NWDServerStartupService : IHostedService
    {
        #region properties

        private IServiceProvider _Services;

        #endregion

        #region constructor

        public NWDServerStartupService(IServiceProvider sServices)
        {
            _Services = sServices;
        }

        #endregion

        #region methods

        public async Task StartAsync(CancellationToken sCancellationToken)
        {
            await Task.Delay(1, sCancellationToken);
        }

        public async Task StopAsync(CancellationToken sCancellationToken)
        {
            await Task.Delay(1, sCancellationToken);
        }

        #endregion
    }
}