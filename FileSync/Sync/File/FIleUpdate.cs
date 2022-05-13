using System;
using System.Collections.Generic;
using System.Linq;

namespace FileSync.Sync.File
{
    /// <summary>
    /// 同步修改文件
    /// </summary>
    public class FIleUpdate:FileBase
    {
        public void SyncUpdate(string pathFrom, string pathTo,
           bool isExecute = true, Action<SyncType, List<string>> actionFileList = null, Action<SyncType, string> actionFile = null, Action<SyncType, string, int> actionFileProgress = null)
        {
            List<CalcuateFileM> tempUpds = UpdateList(pathFrom, pathTo);//查找到需要同步的文件回调
            actionFileList?.Invoke(SyncType.FileUpd, tempUpds.Select(m => m.ToFile.Name).ToList());

            //不执行
            if (!isExecute)
                return;

            SyncUpdate(pathFrom, pathTo, actionFile, actionFileProgress, tempUpds);

        }

        /// <summary>
        /// 修改文件列表
        /// </summary>
        /// <param name="pathFrom"></param>
        /// <param name="pathTo"></param>
        /// <returns></returns>
        public List<CalcuateFileM> UpdateList(string pathFrom, string pathTo)
        {
            var fromFileList = FileLists(pathFrom);

            var toFileList = FileLists(pathTo);

            //upd
            var tempUpds = CalculateUpdFile(fromFileList, toFileList);
            return tempUpds;
        }

        /// <summary>
        /// 同步修改文件
        /// </summary>
        /// <param name="pathFrom"></param>
        /// <param name="pathTo"></param>
        /// <param name="actionFile"></param>
        /// <param name="actionFileProgress"></param>
        /// <param name="tempUpds"></param>
        public  void SyncUpdate(string pathFrom, string pathTo, Action<SyncType, string> actionFile, Action<SyncType, string, int> actionFileProgress, List<CalcuateFileM> tempUpds)
        {
            foreach (var item in tempUpds)
            {
                Console.WriteLine($"upd {item.FromFile.FullName} {item.FromFile.LastAccessTime}:{item.ToFile.LastAccessTime}");
                //当前同步的文件回调
                actionFile?.Invoke(SyncType.FileUpd, item.ToFile.Name);
                System.IO.File.Copy(pathFrom + item.FromFile.FullName, pathTo + item.ToFile.FullName, true);
                //当前同步进度回调
                actionFileProgress?.Invoke(SyncType.FileUpd, pathFrom + item.FromFile.FullName, 100);
            }
        }

        /// <summary>
        /// 修改文件
        /// </summary>
        /// <param name="fromFileList"></param>
        /// <param name="toFileList"></param>
        /// <returns></returns>
        List<CalcuateFileM> CalculateUpdFile(List<FileM> fromFileList, List<FileM> toFileList)
        {
            var temps = from f in fromFileList
                        join t in toFileList
                        on f.FullName equals t.FullName
                        // into items
                        //from item in items.DefaultIfEmpty()
                        //where item.FileName == null
                        select new CalcuateFileM
                        {
                            FromFile = f,
                            ToFile = t
                        };

            return temps.Where(m => m.FromFile.Length != m.ToFile.Length ||
            //m.FromFile.LastAccessTime != m.ToFile.LastAccessTime ||
            m.FromFile.LastWriteTime != m.ToFile.LastWriteTime).ToList();
        }


    }
}
