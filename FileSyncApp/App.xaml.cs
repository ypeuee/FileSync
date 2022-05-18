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
            // 窗口化应用需要在 STA 模式下运行，设置当前
            if (!Thread.CurrentThread.TrySetApartmentState(ApartmentState.STA))
            {
                Thread.CurrentThread.SetApartmentState(ApartmentState.Unknown);
                Thread.CurrentThread.SetApartmentState(ApartmentState.STA);
            }
            //方式 一
            // 构建默认主机
            //var builder = Host.CreateDefaultBuilder();
            //CreateHostBuilder().Build().RunAsync();

            //方式二
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
                   services.AddData<FIleSyncDataModule>(hostingContext.Configuration);

                   services.AddLogging();

                   services.AddSingleton<MainWindow>();

                   //方式一
                   // 注入 WPF 启动类 App
                   services.AddSingleton<App>();
                   // 启动 WPF 服务
                   services.AddHostedService<WPFRunner<App, MainWindow>>();

                   //注入弹性数据库
                   //services.AddData<ElasticDatabaseModule>(hostingContext.Configuration);
               });
        }
    }
    public class WPFRunner<TApplication, TWindow>
        : IHostedService where TApplication
        : System.Windows.Application where TWindow
        : System.Windows.Window
    {
        private readonly TApplication application;
        private readonly TWindow window;
        public WPFRunner(TWindow window, TApplication application)
        {
            this.window = window;
            this.application = application;
        }
        // 参考 ASP.NET 中的后台服务（见下方参考）
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // 启动
            application.Run(window);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // 结束
            application.Shutdown();
            return Task.CompletedTask;
        }
    }

}
