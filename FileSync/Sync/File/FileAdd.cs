using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileSync.Sync.File
{
    /// <summary>
    /// 同步新增文件
    /// </summary>
    public class FileAdd : FileBase
    {
        /// <summary>
        /// 同步新增文件
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
            List<FileM> tempAdds = AddList(pathFrom, pathTo);

            //查找到需要同步的文件回调
            actionFileList?.Invoke(SyncType.FileAdd, tempAdds.Select(m => m.Name).ToList());

            //不执行
            if (!isExecute)
                return;

            SyncAdd(pathFrom, pathTo, actionFile, actionFileProgress, tempAdds);
        }

        /// <summary>
        /// 同步新增文件
        /// </summary>
        /// <param name="pathFrom"></param>
        /// <param name="pathTo"></param>
        /// <param name="actionFile"></param>
        /// <param name="actionFileProgress"></param>
        /// <param name="tempAdds"></param>
        public static void SyncAdd(string pathFrom, string pathTo, Action<SyncType, string> actionFile, Action<SyncType, string, int> actionFileProgress, List<FileM> tempAdds)
        {
            foreach (var item in tempAdds)
            {
                Console.WriteLine($"add {item.FullName}");
                //当前同步的文件回调
                actionFile?.Invoke(SyncType.FileAdd, item.FullName);

                string toPath = pathTo + item.Path;// Path.Combine(pathTo, item.Path);
                if (!Directory.Exists(toPath))
                {
                    Directory.CreateDirectory(toPath);
                }
                string toFile = pathTo + item.FullName;//Path.Combine(pathTo, item.FullName);
                System.IO.File.Copy(pathFrom + item.FullName, toFile);

                //当前同步进度回调
                actionFileProgress?.Invoke(SyncType.FileAdd, pathFrom + item.FullName, 100);
            }
        }

        /// <summary>
        /// 新增文件列表
        /// </summary>
        /// <param name="pathFrom"></param>
        /// <param name="pathTo"></param>
        /// <returns></returns>
        public List<FileM> AddList(string pathFrom, string pathTo)
        {
            var fromFileList = FileLists(pathFrom);

            var toFileList = FileLists(pathTo);

            //add
            var tempAdds = CalculateAddFile(fromFileList, toFileList);
            return tempAdds;
        }



        /// <summary>
        /// 计算新增文件
        /// </summary>
        /// <param name="fromFileList"></param>
        /// <param name="toFileList"></param>
        /// <returns></returns>
        List<FileM> CalculateAddFile(List<FileM> fromFileList, List<FileM> toFileList)
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
