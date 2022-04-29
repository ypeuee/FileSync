using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSync
{
    public static class ConfigurationFile
    {
        private static IConfigurationRoot configuration;

        /// <summary>
        /// 读取appsettings.json配置文件
        /// </summary>
        public static IConfigurationRoot Configuration
        {
            get
            {
                var builder = new ConfigurationBuilder()
                     .SetBasePath(Directory.GetCurrentDirectory())
                     .AddJsonFile("appsettings.json");
                configuration = builder.Build();

                return configuration;
            }

        }

    }
}
