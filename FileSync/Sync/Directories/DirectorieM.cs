using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSync.Sync.Directories
{
    public class DirectorieM
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 文件全称
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 扩展名
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 最后写入时间
        /// </summary>
        public DateTime LastWriteTime { get; set; }

        /// <summary>
        /// 最后访问时间
        /// </summary>
        public DateTime LastAccessTime { get; set; }

        /// <summary>
        /// 文件当前状态
        /// </summary>
        public FileStatus FileStatus { get; set; } = FileStatus.Normal;

        /// <summary>
        /// 操作属性
        /// </summary>
        public FilOperation FilOperation { get; set; } = FilOperation.Normal;

        /// <summary>
        /// 监控扫描时间
        /// </summary>
        public DateTime MonitorintTime { get; set; }

        /// <summary>
        /// 获取或设置当前文件或目录的属性。
        /// </summary>
        public FileAttributes Attributes { get; set; }

        /// <summary>
        /// true空文件夹
        /// </summary>
        public bool IsEmpty { get; set; }
    }
}
