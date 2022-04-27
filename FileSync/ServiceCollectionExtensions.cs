using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FileSync
{

    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        ///  添加应用
        /// </summary>
        /// <param name="services">服务容器</param>
        /// <param name="configuration">配置信息</param>
        public static void AddApplication<TEntryModule>(this IServiceCollection services, IConfiguration configuration)
            where TEntryModule : IDataModule
        {
            var instance = (IDataModule)Activator.CreateInstance(typeof(TEntryModule));
            instance.Load(services, configuration);
        }

    }
}
