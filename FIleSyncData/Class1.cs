using FIleSyncData.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace FIleSyncData
{     /// <summary>
      /// 格子数据库访问
      /// </summary>
    public class TM_PickingGridDAL
    {
        SqlitedContext context;
        public TM_PickingGridDAL(SqlitedContext context)
        {
            this.context = context;
        }
        public TM_PickingGridDAL()
        {
        }

        /// <summary>
        /// 初始化格子。原理先清空，后放入新数据
        /// </summary>
        /// <param name="list"></param>
        public void InitGrid(List<TM_PickingGridM> list)
        {

            //using (var context = new SqlitedContext())
            //{
            StringBuilder sql = new StringBuilder();

            //删除
            sql.AppendFormat(@" DELETE FROM TM_PickingGrid;");

            //插入
            foreach (var m in list)
            {
                sql.AppendFormat($"INSERT INTO TM_PickingGrid(Id,AreaId,GridName,EquipmentID,ChannelID,TPLID) VALUES('{m.Id}', '{m.AreaId}','{m.GridName}','{m.EquipmentID}','{m.ChannelID}','{m.TPLID}'); ");
            }
            context.Database.ExecuteSqlCommand(sql.ToString());
            //}

        }

        /// <summary>
        /// 查询全部
        /// </summary>
        /// <returns></returns>
        public List<TM_PickingGridM> Query()
        {
            string connString = "Data Source=Cloud.ClientApp.db3;Version = 3";
            string sql =
@"drop table if exists userInfo;
create table userInfo(
    userInfo int primary key,
    userName nvarchar(50)
);
insert into userInfo values(1,'小明');
insert into userInfo values(2,'小红');
";
            string sql2 = "select * from userInfo";

            try
            {
                using (var conn = new SQLiteConnection(connString))
                {
                    conn.Open();
                    var cmd = new SQLiteCommand(sql, conn);
                    var v1 = cmd.ExecuteNonQuery();

                    cmd.CommandText = sql2;
                    SQLiteDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Console.WriteLine("UserId:{0}\tUserName:{1}", dr[0], dr[1]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            using (var context = new SqlitedContext())
            {
                return context.TM_PickingGrid.ToList();
            }
        }

    }
    public class TM_PickingAreaDAL
    {

        /// <summary>
        /// 初始化格子。原理先清空，后放入新数据
        /// </summary>
        /// <param name="list"></param>
        public static void InitArea(List<TM_PickingAreaM> list)
        {
            using (var context = new SqlitedContext())
            {
                StringBuilder sql = new StringBuilder();

                //删除
                sql.AppendFormat(@" DELETE FROM TM_PickingArea;");

                //插入
                foreach (var m in list)
                {
                    sql.AppendFormat($"INSERT INTO TM_PickingArea(Id,AreaName,EquipmentID,ChannelID,TPLID) VALUES('{m.Id}','{m.AreaName}','{m.EquipmentID}','{m.ChannelID}','{m.TPLID}'); ");
                }
                context.Database.ExecuteSqlCommand(sql.ToString());
            }
        }

        /// <summary>
        /// 查询全部
        /// </summary>
        /// <returns></returns>
        public static List<TM_PickingAreaM> Query()
        {
            using (var context = new SqlitedContext())
            {
                return context.TM_PickingArea.ToList();
            }
        }

    }
}
