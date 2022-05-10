using System.Data.Entity;
using System.Diagnostics;
using FIleSyncData.Models;

namespace FIleSyncData
{
    public class SqlitedContext : DbContext
    {
        /// <summary>
        /// 用指定的数据库连接枚举创建MasterContext对像 
        /// </summary>

        public SqlitedContext() :
            base(new System.Data.SQLite.SQLiteConnection("data source=Cloud.ClientApp.db3"),true)
        {
#if DEBUG
            base.Database.Log = (info) =>
            {
                Debug.WriteLine(info);
            };
#endif
        }
     
        /// <summary>
        /// 摘果机工位信息
        /// </summary>
        public virtual DbSet<TM_PickingAreaM> TM_PickingArea { get; set; }


        /// <summary>
        /// 摘果机格子信息
        /// </summary>
        public virtual DbSet<TM_PickingGridM> TM_PickingGrid { get; set; }

    }
}
