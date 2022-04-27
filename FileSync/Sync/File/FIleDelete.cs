using System;
using System.Collections.Generic;
using System.Linq;

namespace FileSync.Sync.File
{
    /// <summary>
    /// 同步修改的文件
    /// </summary>
    public class FileDelete : FileBase
    {
        /// <summary>
        /// 同步删除文件
        /// </summary>
        /// <param name="pathFrom"></param>
        /// <param name="pathTo"></param>
        /// <param name="isExecute"></param>
        /// <param name="actionFileList"></param>
        /// <param name="actionFile"></param>
        /// <param name="actionFileProgress"></param>
        public void SyncDelete(string pathFrom, string pathTo,
        bool isExecute = true, Action<SyncType, List<string>> actionFileList = null, Action<SyncType, string> actionFile = null, Action<SyncType, string, int> actionFileProgress = null)
        {
            List<FileM> tempDels = DeleteList(pathFrom, pathTo);//查找到需要同步的文件回调
            actionFileList?.Invoke(SyncType.FileDel, tempDels.Select(m => m.Name).ToList());

            //不执行
            if (!isExecute)
                return;

            SyncDelete(pathTo, actionFile, actionFileProgress, tempDels);

        }

        /// <summary>
        /// 删除文件列表
        /// </summary>
        /// <param name="pathFrom"></param>
        /// <param name="pathTo"></param>
        /// <returns></returns>
        public List<FileM> DeleteList(string pathFrom, string pathTo)
        {
            var fromFileList = FileLists(pathFrom);

            var toFileList = FileLists(pathTo);

            //del
            var tempDels = CalculateDelFile(fromFileList, toFileList);
            return tempDels;
        }

        /// <summary>
        /// 同步删除文件
        /// </summary>
        /// <param name="pathTo"></param>
        /// <param name="actionFile"></param>
        /// <param name="actionFileProgress"></param>
        /// <param name="tempDels"></param>
        public  void SyncDelete(string pathTo, Action<SyncType, string> actionFile, Action<SyncType, string, int> actionFileProgress, List<FileM> tempDels)
        {
            foreach (var item in tempDels)
            {
                Console.WriteLine($"del {item.FullName} ");
                //当前同步的文件回调
                actionFile?.Invoke(SyncType.FileDel, item.Name);

                System.IO.File.Delete(pathTo + item.FullName);
                //当前同步进度回调
                actionFileProgress?.Invoke(SyncType.FileDel, item.Name, 100);
            }
        }


        /// <summary>
        /// 计算删除文件
        /// </summary>
        /// <param name="fromFileList"></param>
        /// <param name="toFileList"></param>
        /// <returns></returns>
        List<FileM> CalculateDelFile(List<FileM> fromFileList, List<FileM> toFileList)
        {
            var temps = from t in toFileList
                        join f in fromFileList
                        on t.FullName equals f.FullName into
                        items
                        from item in items.DefaultIfEmpty()
                            //where item.FileName == null
                        select new CalcuateFileM
                        {
                            FromFile = item,
                            ToFile = t
                        };

            return temps.Where(m => m.FromFile == null).Select(m => m.ToFile).ToList();
        }

    }
}
