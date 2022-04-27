using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileSync.Sync.Directories
{
    /// <summary>
    /// 删除文件夹
    /// </summary>
    public class DirectoriesDelete : DirectoriesBase
    {
        /// <summary>
        /// 同步删除的文件夹
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
            List<DirectorieM> tempDels = DeleteList(pathFrom, pathTo, actionFileList);

            //不执行
            if (!isExecute)
                return;

            SyncDelete(pathTo, actionFile, actionFileProgress, tempDels);

        }

        /// <summary>
        /// 同步删除文件夹
        /// </summary>
        /// <param name="pathTo"></param>
        /// <param name="actionFile"></param>
        /// <param name="actionFileProgress"></param>
        /// <param name="tempDels"></param>
        public void SyncDelete(string pathTo, Action<SyncType, string> actionFile, Action<SyncType, string, int> actionFileProgress, List<DirectorieM> tempDels)
        {
            foreach (var item in tempDels)
            {
                Console.WriteLine($"del {item.FullName} ");
                //当前同步的文件回调
                actionFile?.Invoke(SyncType.DirDel, item.Name);

                Directory.Delete(pathTo + item.FullName);
                //当前同步进度回调
                actionFileProgress?.Invoke(SyncType.DirDel, item.Name, 100);
            }
        }

        /// <summary>
        /// 获取删除的文件夹
        /// </summary>
        /// <param name="pathFrom"></param>
        /// <param name="pathTo"></param>
        /// <param name="actionFileList"></param>
        /// <returns></returns>
        public List<DirectorieM> DeleteList(string pathFrom, string pathTo, Action<SyncType, List<string>> actionFileList)
        {
            var fromFileList = DirectoriesLists(pathFrom);

            var toFileList = DirectoriesLists(pathTo);

            //del
            var tempDels = CalculateDelete(fromFileList, toFileList);
            //查找到需要同步的文件回调
            actionFileList?.Invoke(SyncType.DirDel, tempDels.Select(m => m.Name).ToList());
            return tempDels;
        }


        /// <summary>
        /// 计算删除文件
        /// </summary>
        /// <param name="fromFileList"></param>
        /// <param name="toFileList"></param>
        /// <returns></returns>
        List<DirectorieM> CalculateDelete(List<DirectorieM> fromFileList, List<DirectorieM> toFileList)
        {
            var temps = from t in toFileList
                        join f in fromFileList
                        on t.FullName equals f.FullName into
                        items
                        from item in items.DefaultIfEmpty()
                            //where item.FileName == null
                        select new CalcuateDirectorieM
                        {
                            FromDirectories = item,
                            ToDirectories = t
                        };

            return temps.Where(m => m.FromDirectories == null).Select(m => m.ToDirectories).ToList();
        }
    }
}
