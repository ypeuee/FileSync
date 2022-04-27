using FileSync.Sync.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSync.Sync.Same
{
    public class SameFileM
    {   
        /// <summary>
            /// 源
            /// </summary>
        public FileM FromFile { get; set; }

        /// <summary>
        /// 目标
        /// </summary>
        public List<FileM> ToFile { get; set; }
    }
}
