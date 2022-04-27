using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FileSync
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                 //.UseSerilog((hostingContext, logger) =>
                 //{
                 //    logger.ReadFrom.Configuration(hostingContext.Configuration, "Logging");
                 //})              
               .ConfigureServices((hostingContext, services) =>
               {
                   services.AddHostedService<StartupService>();
                   services.AddApplication<FileSyncModule>(hostingContext.Configuration);
             
                   //注入弹性数据库
                   //services.AddData<ElasticDatabaseModule>(hostingContext.Configuration);
               });
        }

    



    }
}
