using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileSync.Sync.Directories
{
    /// <summary>
    /// 同步添加文件夹
    /// </summary>
    public class DirectoriesAdd : DirectoriesBase
    {
        /// <summary>
        /// 同步新增的文件夹
        /// </summary>
        /// <param name="pathFrom"></param>
        /// <param name="pathTo"></param>
        /// <param name="isExecute"></param>
        /// <param name="actionFileList"></param>
        /// <param name="actionFile"></param>
        /// <param name="actionFileProgress"></param>
        public void SyncAdd(string pathFrom, string pathTo,
         bool isExecute = true, Action<SyncType, List<string>> actionFileList = null, Action<SyncType, string> actionFile = null, Action<SyncType, string, int> actionFileProgress = null)
        {
            List<DirectorieM> tempAdds = AddList(pathFrom, pathTo);//查找到需要同步的文件回调
            actionFileList?.Invoke(SyncType.DirAdd, tempAdds.Select(m => m.Name).ToList());

            //不执行
            if (!isExecute)
                return;

            SyncAdd(pathTo, actionFile, actionFileProgress, tempAdds);
        }

        /// <summary>
        /// 同步新增的文件夹
        /// </summary>
        /// <param name="pathTo"></param>
        /// <param name="actionFile"></param>
        /// <param name="actionFileProgress"></param>
        /// <param name="tempAdds"></param>
        public void SyncAdd(string pathTo, Action<SyncType, string> actionFile, Action<SyncType, string, int> actionFileProgress, List<DirectorieM> tempAdds)
        {
            foreach (var item in tempAdds)
            {
                Console.WriteLine($"add {item.FullName}");
                //当前同步的文件回调
                actionFile?.Invoke(SyncType.DirAdd, item.Name);

                string toPath = pathTo + item.FullName;// Path.Combine(pathTo, item.Path);
                if (!Directory.Exists(toPath))
                {
                    Directory.CreateDirectory(toPath);
                }
                //当前同步进度回调
                actionFileProgress?.Invoke(SyncType.DirAdd, item.Name, 100);
            }
        }

        /// <summary>
        /// 获取添加文件
        /// </summary>
        /// <param name="pathFrom"></param>
        /// <param name="pathTo"></param>
        /// <returns></returns>
        public List<DirectorieM> AddList(string pathFrom, string pathTo)
        {
            var fromFileList = DirectoriesLists(pathFrom);

            var toFileList = DirectoriesLists(pathTo);

            //add
            var tempAdds = CalculateAddList(fromFileList, toFileList);
            return tempAdds;
        }



        /// <summary>
        /// 计算新增文件
        /// </summary>
        /// <param name="fromFileList"></param>
        /// <param name="toFileList"></param>
        /// <returns></returns>
        internal List<DirectorieM> CalculateAddList(List<DirectorieM> fromFileList, List<DirectorieM> toFileList)
        {
            var temps = from f in fromFileList
                        join t in toFileList
                        on f.FullName equals t.FullName into
                        items
                        from item in items.DefaultIfEmpty()
                            //where item.FileName == null
                        select new
                        {
                            fromFile = f,
                            toFile = item
                        };
            //var list = temps.ToList();
            return temps.ToList().Where(m => m.toFile == null).Select(m => m.fromFile).ToList();
        }

    }
}
