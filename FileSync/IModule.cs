using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileSync
{
    /// <summary>
    ///  引用模块入口
    ///  用来帮助模块化对象注入和模块自身对象的申明
    /// </summary>
    public interface IDataModule
    {
        /// <summary>
        ///  加载模块
        ///  跟Asp.Net Core Startup 中ConfigureServices的作用一样。
        ///  帮助对应模块的对象注入和模块自身对象的申明。
        /// </summary>
        /// <param name="services">配置服务上下文</param>
        /// <param name="configuration">所以可以直接使用context.Services、context.Configuration 进行服务注册和配置相关的操作。</param>
        void Load(IServiceCollection services, IConfiguration configuration);
    }
}
