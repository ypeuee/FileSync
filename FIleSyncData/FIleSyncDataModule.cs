using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FIleSyncData
{
    public static partial class ServiceCollectionExtensions
    {
        /// <summary>
        ///  添加应用
        /// </summary>
        /// <param name="services">服务容器</param>
        /// <param name="configuration">配置信息</param>
        public static void AddData<TEntryModule>(this IServiceCollection services, IConfiguration configuration)
        {
            var instance = (FIleSyncDataModule)Activator.CreateInstance(typeof(FIleSyncDataModule));
            instance.Load(services, configuration);
        }

    }
    public class FIleSyncDataModule
    {
        public void Load(IServiceCollection Services, IConfiguration Configuration)
        {

            //SqlitedContext
            Services.AddTransient<SqlitedContext>();
            Services.AddTransient<SyncLogDAL>();
 
            // 注册数据库会话工厂
            //Services.AddTransient<IDbConnectionFactory, DbConnectionFactory>();            
        }
    }
}
