using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileSync
{

    using FileSync.Sync;
    using FileSync.Sync.Directories;
    using FileSync.Sync.File;

    /// <summary>
    /// 数据库连接对象注入
    /// </summary>
    public class FileSyncModule : IDataModule
    {
        public void Load(IServiceCollection Services, IConfiguration Configuration)
        {
            //Scoped(作用域) Transient(瞬时)  Singleton(单例)
            //Dir
            Services.AddTransient<DirectorieM>();
            Services.AddTransient<CalcuateDirectorieM>();
            Services.AddTransient<DirectoriesBase>();
            Services.AddTransient<DirectoriesAdd>();
            Services.AddTransient<DirectoriesDelete>();
            Services.AddTransient<DirectoriesReName>();
            Services.AddTransient<DirectoriesAll>();

            //File
            Services.AddTransient<FileM>();
            Services.AddTransient<CalcuateFileM>();
            Services.AddTransient<FileBase>();
            Services.AddTransient<FileAdd>();
            Services.AddTransient<FIleMove>();
            Services.AddTransient<FileReName>();
            Services.AddTransient<FileDelete>();
            Services.AddTransient<FIleUpdate>();
            Services.AddTransient<FileAll>();

            Services.AddTransient<SyncStart>();
            
            // 注册数据库会话工厂
            //Services.AddTransient<IDbConnectionFactory, DbConnectionFactory>();            
        }
    }
}
