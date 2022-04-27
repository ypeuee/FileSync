using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileSync.Sync.Directories
{
    /// <summary>
    /// 同步重命名文件夹
    /// </summary>
    public class DirectoriesReName : DirectoriesBase
    {
        /// <summary>
        /// 文件夹重命名
        /// </summary>
        /// <param name="pathFrom"></param>
        /// <param name="pathTo"></param>
        public void SyncReName(string pathFrom, string pathTo, bool isExecute = true, Action<SyncType, List<string>> actionFileList = null, Action<SyncType, string> actionFile = null, Action<SyncType, string, int> actionFileProgress = null)
        {
            //只处理同级目录下，修改文件夹名称
            var fromFileList = DirectoriesLists(pathFrom);
            var toFileList = DirectoriesLists(pathTo);

            //add
            var tempAdds = new DirectoriesAdd().CalculateAddList(fromFileList, toFileList);

            List<string> dirList = new List<string>();
            foreach (var item in tempAdds)
            {
                string toPath = pathTo + item.FullName;// Path.Combine(pathTo, item.Path);
                string fromPath = pathFrom + item.FullName;
                List<string> fromList = new List<string>();
                List<string> toList = new List<string>();

                //同级目录列表
                string toPathParent = Directory.GetParent(toPath).FullName;
                if (!Directory.Exists(toPathParent))
                    continue;
                foreach (var d in Directory.GetDirectories(toPathParent))
                {
                    toList.Add(d.Replace(pathTo, ""));
                }

                string fromPathParent = Directory.GetParent(fromPath).FullName;
                foreach (var d in Directory.GetDirectories(fromPathParent))
                {
                    fromList.Add(d.Replace(pathFrom, ""));
                }

                //排除源目录已存在
                var difference = toList.Where(m => !fromList.Contains(m)).ToList();

                foreach (var dif in difference)
                {
                    SyncReName(pathTo, actionFile, actionFileProgress, item, dif);
                }
            }

        }

        /// <summary>
        /// 重命名文件夹
        /// </summary>
        /// <param name="pathTo"></param>
        /// <param name="actionFile"></param>
        /// <param name="actionFileProgress"></param>
        /// <param name="item"></param>
        /// <param name="dif"></param>
        public void SyncReName(string pathTo, Action<SyncType, string> actionFile, Action<SyncType, string, int> actionFileProgress, DirectorieM item, string dif)
        {
            Console.WriteLine($"rename {dif} --> {item.FullName}");
            //当前同步的文件回调
            actionFile?.Invoke(SyncType.DirReName, item.Name);

            Directory.Move(pathTo + dif, pathTo + item.FullName);
            //当前同步进度回调
            actionFileProgress?.Invoke(SyncType.DirReName, item.Name, 100);
        }

        /// <summary>
        /// 获取重命名文件夹
        /// </summary>
        /// <param name="pathFrom"></param>
        /// <param name="pathTo"></param>
        /// <returns></returns>
        public List<string> ReNameList(string pathFrom, string pathTo)
        {
            //只处理同级目录下，修改文件夹名称
            var fromFileList = DirectoriesLists(pathFrom);
            var toFileList = DirectoriesLists(pathTo);

            //add
            var tempAdds = new DirectoriesAdd().CalculateAddList(fromFileList, toFileList);

            List<string> dirList = new List<string>();
            foreach (var item in tempAdds)
            {
                string toPath = pathTo + item.FullName;// Path.Combine(pathTo, item.Path);
                string fromPath = pathFrom + item.FullName;
                List<string> fromList = new List<string>();
                List<string> toList = new List<string>();

                //同级目录列表
                string toPathParent = Directory.GetParent(toPath).FullName;
                if (!Directory.Exists(toPathParent))
                    continue;
                foreach (var d in Directory.GetDirectories(toPathParent))
                {
                    toList.Add(d.Replace(pathTo, ""));
                }

                string fromPathParent = Directory.GetParent(fromPath).FullName;
                foreach (var d in Directory.GetDirectories(fromPathParent))
                {
                    fromList.Add(d.Replace(pathFrom, ""));
                }

                //排除源目录已存在
                var difference = toList.Where(m => !fromList.Contains(m)).ToList();

                var res = difference.Where(m => IsPatch(fromPath, pathTo + m)).ToList();
                dirList.AddRange(res);

                //foreach (var dif in difference)
                //{
                //    if (IsPatch(fromPath, pathTo + dif))
                //    {   
                //    }
                //}
            }
            return dirList;
        }


        /// <summary>
        /// 验证现在目录是明细（文件+目录）是否一致
        /// </summary>
        /// <param name="pathFrom"></param>
        /// <param name="pathTo"></param>
        /// <returns></returns>
        bool IsPatch(string pathFrom, string pathTo)
        {
            var from = new DirectoryInfo(pathFrom);
            var fromDirs = from.GetDirectories();
            var to = new DirectoryInfo(pathTo);
            var toDirs = to.GetDirectories();
            //比较目录数
            if (fromDirs.Length != toDirs.Length)
                return false;
            //比较目录名称
            for (int i = 0; i < fromDirs.Length; i++)
            {
                //名称不一致
                if (fromDirs[i].Name != toDirs[i].Name)
                    return false;
            }

            var fromFiles = from.GetFiles();
            var toFiles = to.GetFiles();
            //比较文件数
            if (fromFiles.Length != toFiles.Length)
                return false;
            for (int i = 0; i < fromFiles.Length; i++)
            {
                //名称不一致
                if (fromFiles[i].Name != toFiles[i].Name)
                    return false;
                if (fromFiles[i].Length != toFiles[i].Length)
                    return false;
                if (fromFiles[i].LastWriteTime != toFiles[i].LastWriteTime)
                    return false;
            }
            return true;
        }
    }
}
