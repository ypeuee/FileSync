using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSync
{
    public enum SyncType
    {
        DirDel = 0,
        DirAdd = 1,
        DirReName = 2,
        FileDel = 3,
        FileAdd = 4,
        FileUpd = 5,
    }
}
