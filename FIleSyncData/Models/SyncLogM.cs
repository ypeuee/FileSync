using System;

namespace FIleSyncData.Models
{

    /// <summary>
    /// 同步日志表
    /// </summary>
    public class SyncLogM
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 扩展名
        /// </summary>
        public string Extension { get; set; }
        /// <summary>
        /// 全名
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 最后写入时间
        /// </summary>
        public DateTime LastWriteTime { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public string FilOperation { get; set; }
        /// <summary>
        /// 同步结果信息
        /// </summary>
        public string LogMsg { get; set; }
        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime LogTime { get; set; }

        public string LogDate { get { return LogTime.ToString("yyyy-MM-dd"); } }

    }
}
