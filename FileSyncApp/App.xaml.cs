using FileSync;
using FIleSyncData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MainApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {

        }
        private void OnStartup(object sender, StartupEventArgs e)
        {
            CreateHostBuilder().Build()//.Run();
           .Services.GetRequiredService<MainWindow>().Show();
        }

        public static IHostBuilder CreateHostBuilder(string[] args = null)
        {
            return Host.CreateDefaultBuilder(args)
               //.UseSerilog((hostingContext, logger) =>
               //{
               //    logger.ReadFrom.Configuration(hostingContext.Configuration, "Logging");
               //})              
               .ConfigureServices((hostingContext, services) =>
               {
                   //services.AddHostedService<StartupService>();
                   services.AddApplication<FileSyncModule>(hostingContext.Configuration);
                   services.AddSingleton<MainWindow>();
                   services.AddData<FIleSyncDataModule>(hostingContext.Configuration);
                   
                   //注入弹性数据库
                   //services.AddData<ElasticDatabaseModule>(hostingContext.Configuration);
               });
        }
    }

    public class StartupService : IHostedService
    {
        MainWindow start;
        private readonly IHostApplicationLifetime appLifetime;
        public StartupService(MainWindow start, IHostApplicationLifetime appLifetime)
        {
            this.appLifetime = appLifetime;
            this.start = start;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            start.Show();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
