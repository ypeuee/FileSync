using FileSync.Sync;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileSync
{
    public class StartupService : IHostedService
    {
        SyncStart syncStart;
        private readonly IHostApplicationLifetime appLifetime;
        public StartupService(SyncStart syncStart, IHostApplicationLifetime appLifetime)
        {
            this.appLifetime = appLifetime;
            this.syncStart = syncStart;
        }
     

        public Task StartAsync(CancellationToken cancellationToken)
        {
            syncStart.Main();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

     
    }
}
