using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSync
{
   public class DirectoriesSync
    {

        /// <summary>
        /// 文件夹重命名
        /// </summary>
        /// <param name="pathFrom"></param>
        /// <param name="pathTo"></param>
        public void DirectoriesReName(string pathFrom, string pathTo,bool isExecute=true, Action<SyncType,List< string>> actionFileList = null, Action<SyncType, string> actionFile = null, Action<SyncType,string, int> actionFileProgress = null)
        {
            //只处理同级目录下，修改文件夹名称
            var fromFileList = DirectoriesLists(pathFrom);
            var toFileList = DirectoriesLists(pathTo);

            //add
            var tempAdds = CalculateAddDirectoriesList(fromFileList, toFileList);
            //查找到需要同步的文件回调
            actionFileList?.Invoke(SyncType.DirReName, tempAdds.Select(m => m.Name).ToList());

            //不执行
            if (!isExecute)
                return;

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
                    if (IsPatch(fromPath, pathTo + dif))
                    {
                        Console.WriteLine($"rename {dif} --> {item.FullName}");
                      //当前同步的文件回调
                        actionFile?.Invoke(SyncType.DirReName, item.Name);

                        Directory.Move(pathTo + dif, pathTo + item.FullName);
                        //当前同步进度回调
                        actionFileProgress?.Invoke(SyncType.DirReName, item.Name, 100);
                    }
                }
            }
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


        /// <summary>
        /// 获取所有文件夹
        /// </summary>
        /// <param name="homePath"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        List<DirectorieM> DirectoriesLists(string homePath, string path = null)
        {
            if (path == null)
                path = homePath;

            List<DirectorieM> catchFIleList = new List<DirectorieM>();

            var dirInfo = new DirectoryInfo(path);
            var dirs = dirInfo.GetDirectories();
            DirectorieM catchFile;
            foreach (var dir in dirs)
            {
                if (dir.Attributes == FileAttributes.Directory)
                {
                    var items = DirectoriesLists(homePath, dir.FullName);
                    catchFIleList.AddRange(items);

                    catchFile = new DirectorieM()
                    {
                        Name = dir.Name,
                        FullName = dir.FullName.Replace(homePath, ""),
                        Extension = dir.Extension,
                        CreationTime = dir.CreationTime,
                        LastWriteTime = dir.LastWriteTime,
                        LastAccessTime = dir.LastAccessTime,
                        Attributes = dir.Attributes,
                    };
                    catchFIleList.Add(catchFile);
                }
            }

            return catchFIleList;
        }

        public void CopyAddDirectories(string pathFrom, string pathTo, 
            bool isExecute = true, Action<SyncType, List<string>> actionFileList = null, Action<SyncType, string> actionFile = null, Action<SyncType, string, int> actionFileProgress = null)
        {
            var fromFileList = DirectoriesLists(pathFrom);

            var toFileList = DirectoriesLists(pathTo);

            //add
            var tempAdds = CalculateAddDirectoriesList(fromFileList, toFileList);
            //查找到需要同步的文件回调
            actionFileList?.Invoke(SyncType.DirAdd, tempAdds.Select(m => m.Name).ToList());

            //不执行
            if (!isExecute)
                return;

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

        public void CopyDelDirectories(string pathFrom, string pathTo, 
            bool isExecute = true, Action<SyncType, List<string>> actionFileList = null, Action<SyncType, string> actionFile = null, Action<SyncType, string, int> actionFileProgress = null)
        {
            var fromFileList = DirectoriesLists(pathFrom);

            var toFileList = DirectoriesLists(pathTo);

            //del
            var tempDels = CalculateDelDirectories(fromFileList, toFileList);
            //查找到需要同步的文件回调
            actionFileList?.Invoke(SyncType.DirDel, tempDels.Select(m => m.Name).ToList());

            //不执行
            if (!isExecute)
                return; 
            
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
        /// 计算新增文件
        /// </summary>
        /// <param name="fromFileList"></param>
        /// <param name="toFileList"></param>
        /// <returns></returns>
        List<DirectorieM> CalculateAddDirectoriesList(List<DirectorieM> fromFileList, List<DirectorieM> toFileList)
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

        /// <summary>
        /// 计算删除文件
        /// </summary>
        /// <param name="fromFileList"></param>
        /// <param name="toFileList"></param>
        /// <returns></returns>
        List<DirectorieM> CalculateDelDirectories(List<DirectorieM> fromFileList, List<DirectorieM> toFileList)
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
