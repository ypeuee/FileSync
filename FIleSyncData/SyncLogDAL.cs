using FIleSyncData.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using Dapper;

namespace FIleSyncData
{     /// <summary>
      /// 格子数据库访问
      /// </summary>
    public class SyncLogDAL
    {
        public void InsLog(SyncLogM log)
        {
            string sqlCreate = @"
CREATE TABLE if not exists SyncLog (
  Id integer PRIMARY KEY autoincrement,
  Name varchar(255) not null,
  Extension varchar(255),
  FullName varchar(5000) not null,
  Path varchar(5000) not null,
  TypeName varchar(10),
  CreateTime datetime,
  LastWriteTime datatime,
  FilOperation int,
  LogMsg varchar(500),
  LogTime datatime
);";

            string sqlIns = @"
insert into synclog
(Name,Extension,FullName,Path, TypeName,CreateTime,LastWriteTime,FilOperation,LogTime )
values
(@Name,@Extension,@FullName,@Path, @TypeName,@CreateTime,@LastWriteTime,@FilOperation,@LogTime);

";
            try
            {
                using (var con = SqlitedContext.NewConnection())
                {
                    var v1 = con.Execute(sqlCreate);
                    var v2 = con.Execute(sqlIns, log);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 查询全部记录
        /// </summary>
        /// <returns></returns>
        public List<SyncLogM> Query()
        {
            string sql =
@"
select * from synclog
";

            try
            {
                using (var conn = SqlitedContext.NewConnection())
                {
                    var list = conn.Query<SyncLogM>(sql)?.AsList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }


            /// <summary>
        /// 查询全部记录
        /// </summary>
        /// <returns></returns>
        public void Delete()
        {
            string sql =
@"
delete from synclog
";

            try
            {
                using (var conn = SqlitedContext.NewConnection())
                {
                    var list = conn.Execute(sql);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }


    }

}
