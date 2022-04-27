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
        public StartupService(SyncStart syncStart)
        {
            this.syncStart = syncStart;
        }
     
        private readonly IHostApplicationLifetime appLifetime;

        public StartupService(IHostApplicationLifetime appLifetime)
        {           
            this.appLifetime = appLifetime;

            syncStart.Main();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

     
    }
}
