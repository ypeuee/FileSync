using System.Collections.Generic;
using System.Linq;

namespace FileSync.Sync.File
{
    public class FIleMove:FileBase
    {
        /// <summary>
        /// 计算移动文件
        /// </summary>
        /// <param name="fromFileList"></param>
        /// <param name="toFileList"></param>
        /// <returns></returns>
        List<CalcuateFileM> CalculateMoveFile(List<FileM> fromFileList, List<FileM> toFileList)
        {
            var temps = from t in toFileList
                        join f in fromFileList
                        on new { t.Name, t.Length } equals new { f.Name, f.Length } into
                        items
                        from item in items.DefaultIfEmpty()
                            //where item.FileName == null
                        select new CalcuateFileM
                        {
                            FromFile = item,
                            ToFile = t
                        };

            return temps.Where(m => m.FromFile == null).ToList();
        }


    }
}
