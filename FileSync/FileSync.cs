using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace FileSync
{
    public class FileSync
    {
        List<FileM> catchFIleList = new List<FileM>();
        Dictionary<string, int> screenFile = new Dictionary<string, int>();

        public FileSync()
        {
            screenFile.Add(FileAttributes.ReadOnly.ToString(), 1);
            screenFile.Add(FileAttributes.Hidden.ToString(), 1);
            screenFile.Add(FileAttributes.System.ToString(), 4);
            screenFile.Add(FileAttributes.Directory.ToString(), 16);
            screenFile.Add(FileAttributes.Device.ToString(), 32);
            screenFile.Add(FileAttributes.Temporary.ToString(), 256);
            screenFile.Add(FileAttributes.SparseFile.ToString(), 512);
            screenFile.Add(FileAttributes.ReparsePoint.ToString(), 1024);
            screenFile.Add(FileAttributes.Compressed.ToString(), 2048);
            screenFile.Add(FileAttributes.Offline.ToString(), 4096);
            screenFile.Add(FileAttributes.NotContentIndexed.ToString(), 8192);
            screenFile.Add(FileAttributes.Encrypted.ToString(), 16384);
            screenFile.Add(FileAttributes.IntegrityStream.ToString(), 32768);
            screenFile.Add(FileAttributes.NoScrubData.ToString(), 131072);
        }

        /// <summary>
        /// 执行文件同步
        /// </summary>
        /// <param name="config"></param>
        public void Exectue(IConfigurationRoot config)
        {
            //源路径
            string pathFrom = config["FileSync:PathFrom"];// @"D:\testFrom";
            Console.WriteLine($"pathFrom: {pathFrom}");
            if (!Directory.Exists(pathFrom))
            {
                Console.WriteLine($"源路径【FileSync:PathFrom】错误。{pathFrom}");
                return;
            }

            //目的路径
            string pathTo = config["FileSync:PathTo"];// @"D:\testTo";
            Console.WriteLine($"pathTo: {pathTo}");
            if (!Directory.Exists(pathTo))
            {
                Console.WriteLine($"目的路径【FileSync:PathTo】错误。{pathTo}");
                return;
            }


            var dirSync = new DirectoriesSync();
            var fileSync = new FileSync();

            do
            {

                dirSync.DirectoriesReName(pathFrom, pathTo);

                //文件
                fileSync.CopyAddFile(pathFrom, pathTo);
                fileSync.CopyUpdFile(pathFrom, pathTo);
                fileSync.CopyDelFile(pathFrom, pathTo);
                //文件夹
                dirSync.CopyAddDirectories(pathFrom, pathTo);
                dirSync.CopyDelDirectories(pathFrom, pathTo);

                System.Threading.Thread.Sleep(1000);
            } while (bool.Parse(config["FileSync:Repeatedly"]));

        }


        /// <summary>
        /// 获取所有文件
        /// </summary>
        /// <param name="path"></param>
        List<FileM> FileLists(string homePath, string path = null)
        {
            if (path == null)
                path = homePath;

            List<FileM> catchFIleList = new List<FileM>();

            var dirInfo = new DirectoryInfo(path);
            var dirs = dirInfo.GetDirectories();
            foreach (var dir in dirs)
            {
                if (dir.Attributes == FileAttributes.Directory)
                {
                    var items = FileLists(homePath, dir.FullName);
                    catchFIleList.AddRange(items);
                }
            }

            FileM catchFile;
            var files = dirInfo.GetFiles();
            foreach (var file in files)
            {
                catchFile = new FileM()
                {
                    Name = file.Name,
                    FullName = file.FullName.Replace(homePath, ""),
                    Path = file.DirectoryName.Replace(homePath, ""),
                    Extension = file.Extension,
                    CreationTime = file.CreationTime,
                    LastWriteTime = file.LastWriteTime,
                    LastAccessTime = file.LastAccessTime,
                    Attributes = file.Attributes,
                    Length = file.Length
                };
                catchFIleList.Add(catchFile);
            }
            return catchFIleList;
        }

        /// <summary>
        /// 文件重命名
        /// </summary>
        /// <param name="pathFrom"></param>
        /// <param name="pathTo"></param>
        public void FileReName(string pathFrom, string pathTo)
        {
            var fromFileList = FileLists(pathFrom);

            var toFileList = FileLists(pathTo);

            //add
            var tempAdds = CalculateAddFile(fromFileList, toFileList);
            foreach (var item in tempAdds)
            {
                Console.WriteLine($"add {item.FullName}");
                string toPath = pathTo + item.Path;// Path.Combine(pathTo, item.Path);
                if (!Directory.Exists(toPath))
                {
                    Directory.CreateDirectory(toPath);
                }
                string toFile = pathTo + item.FullName;//Path.Combine(pathTo, item.FullName);
                File.Copy(pathFrom + item.FullName, toFile);
            }

        }

        public void CopyAddFile(string pathFrom, string pathTo, Action<List<string>> actionFileList = null, Action<string> actionFile = null, Action<string, int> actionFileProgress = null)
        {
            var fromFileList = FileLists(pathFrom);

            var toFileList = FileLists(pathTo);

            //add
            var tempAdds = CalculateAddFile(fromFileList, toFileList);
            //查找到需要同步的文件回调
            actionFileList.Invoke(tempAdds.Select(m => m.Name).ToList());
            foreach (var item in tempAdds)
            {
                Console.WriteLine($"add {item.FullName}");
                //当前同步的文件回调
                actionFile?.Invoke(item.FullName);

                string toPath = pathTo + item.Path;// Path.Combine(pathTo, item.Path);
                if (!Directory.Exists(toPath))
                {
                    Directory.CreateDirectory(toPath);
                }
                string toFile = pathTo + item.FullName;//Path.Combine(pathTo, item.FullName);
                File.Copy(pathFrom + item.FullName, toFile);

                //当前同步进度回调
                actionFileProgress?.Invoke(item.FullName, 100);
            }

        }

        public void CopyUpdFile(string pathFrom, string pathTo, Action<List<string>> actionFileList = null, Action<string> actionFile = null, Action<string, int> actionFileProgress = null)
        {
            var fromFileList = FileLists(pathFrom);

            var toFileList = FileLists(pathTo);

            //upd
            var tempUpds = CalculateUpdFile(fromFileList, toFileList);
            //查找到需要同步的文件回调
            actionFileList?.Invoke(tempUpds.Select(m => m.ToFile.Name).ToList());
            foreach (var item in tempUpds)
            {
                Console.WriteLine($"upd {item.FromFile.FullName} {item.FromFile.LastAccessTime}:{item.ToFile.LastAccessTime}");
                //当前同步的文件回调
                actionFile?.Invoke(item.ToFile.Name);
                File.Copy(pathFrom + item.FromFile.FullName, pathTo + item.ToFile.FullName, true);
                //当前同步进度回调
                actionFileProgress?.Invoke(item.ToFile.Name, 100);
            }

        }

        public void CopyDelFile(string pathFrom, string pathTo, Action<List<string>> actionFileList = null, Action<string> actionFile = null, Action<string, int> actionFileProgress = null)
        {
            var fromFileList = FileLists(pathFrom);

            var toFileList = FileLists(pathTo);

            //del
            var tempDels = CalculateDelFile(fromFileList, toFileList);
            //查找到需要同步的文件回调
            actionFileList?.Invoke(tempDels.Select(m => m.Name).ToList());

            foreach (var item in tempDels)
            {
                Console.WriteLine($"del {item.FullName} ");
                //当前同步的文件回调
                actionFile?.Invoke(item.Name);

                File.Delete(pathTo + item.FullName);
                //当前同步进度回调
                actionFileProgress?.Invoke(item.Name, 100);
            }

        }

        public void CopyAddFile1(string pathFrom, string pathTo)
        {
            var fromFileList = FileLists(pathFrom);

            var toFileList = FileLists(pathTo);

            //add
            var tempAdds = CalculateAddFile(fromFileList, toFileList);
            foreach (var item in tempAdds)
            {
                Console.WriteLine($"add {item.FullName}");
                string toPath = pathTo + item.Path;// Path.Combine(pathTo, item.Path);
                if (!Directory.Exists(toPath))
                {
                    Directory.CreateDirectory(toPath);
                }
                string toFile = pathTo + item.FullName;//Path.Combine(pathTo, item.FullName);
                File.Copy(pathFrom + item.FullName, toFile);
            }

            //upd
            var tempUpds = CalculateUpdFile(fromFileList, toFileList);
            foreach (var item in tempUpds)
            {
                Console.WriteLine($"upd {item.FromFile.FullName} {item.FromFile.LastAccessTime}:{item.ToFile.LastAccessTime}");
                File.Copy(item.FromFile.FullName, item.ToFile.FullName, true);
            }

            //move
            //var tempMoves = CalculateMoveFile(fromFileList, toFileList);
            //foreach (var item in tempMoves)
            //{
            //    Console.WriteLine($"move {item.FromFile.FullName} {item.FromFile.Path}:{item.ToFile.Path}");
            //}

            //del
            var tempDels = CalculateDelFile(fromFileList, toFileList);
            foreach (var item in tempDels)
            {
                Console.WriteLine($"del {item.FullName} ");

                File.Delete(item.FullName);
            }

            //updTo
            var tempUpdTos = CalculateToUpdFile(fromFileList, toFileList);
            foreach (var item in tempUpdTos)
            {
                Console.WriteLine($"updTo {item.FromFile.FullName} {item.FromFile.LastAccessTime}:{item.ToFile.LastAccessTime}");

            }

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

        /// <summary>
        /// 计算目标被修改文件
        /// </summary>
        /// <param name="fromFileList"></param>
        /// <param name="toFileList"></param>
        /// <returns></returns>
        List<CalcuateFileM> CalculateToUpdFile(List<FileM> fromFileList, List<FileM> toFileList)
        {
            var temps = from t in toFileList
                        join f in fromFileList
                        on t.FullName equals f.FullName
                        //into  items
                        //from item in items.DefaultIfEmpty()
                        //where item.FileName == null
                        select new CalcuateFileM
                        {
                            FromFile = f,
                            ToFile = t
                        };

            return temps.Where(m => m.FromFile.Length != m.ToFile.Length ||
           m.FromFile.LastAccessTime != m.ToFile.LastAccessTime ||
           m.FromFile.LastWriteTime != m.ToFile.LastWriteTime).ToList();
        }




        /// <summary>
        /// 获取变动的文件
        /// </summary>
        public void GetLastWriteFile()
        {
            DateTime time = DateTime.Now;
            Console.WriteLine(time);
            //GetLastWriteFile(Path, time);

            foreach (var file in catchFIleList.Where(m => m.MonitorintTime != time && m.FilOperation != FilOperation.Delete))
            {
                file.FilOperation = FilOperation.Delete;
                file.MonitorintTime = time;
                Console.WriteLine($"删除文件--{file.Path}\\{file.FullName}");
            }
        }

        /// <summary>
        /// 获取变动的文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="time">上次检测的时间</param>
        void GetLastWriteFile(string path, DateTime time)
        {
            var dirInfo = new DirectoryInfo(path);
            var dirs = dirInfo.GetDirectories();
            foreach (var dir in dirs)
            {
                if (dir.Attributes == FileAttributes.Directory)
                {
                    GetLastWriteFile(dir.FullName, time);
                }
            }

            FileM catchFile;
            var files = dirInfo.GetFiles();

            foreach (var file in files)
            {
                catchFile = new FileM()
                {
                    FullName = file.FullName,
                    Path = file.DirectoryName,
                    CreationTime = file.CreationTime,
                    LastWriteTime = file.LastWriteTime,
                    LastAccessTime = file.LastAccessTime,
                    Attributes = file.Attributes,
                    MonitorintTime = time,
                };

                if (!IsScreenFileAttributes(file.Attributes))
                {
                    try
                    {
                        //var open = file.OpenRead();
                        //open.Close();
                    }
                    catch (System.UnauthorizedAccessException)
                    {
                        Console.WriteLine("System.IO.FileInfo.Name is read-only or is a directory.");
                        catchFile.FileStatus = FileStatus.ReadOnly;
                    }
                    catch (System.IO.DirectoryNotFoundException)
                    {
                        Console.WriteLine("The specified path is invalid, such as being on an unmapped drive.");
                        catchFile.FileStatus = FileStatus.Exception;
                    }
                    catch (System.IO.IOException)
                    {
                        Console.WriteLine("The file is already open.");
                        catchFile.FileStatus = FileStatus.Opened;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Exception");
                        catchFile.FileStatus = FileStatus.Exception;
                    }

                    var oldCatchFile = catchFIleList.FirstOrDefault(m => m.FullName == catchFile.FullName && m.Path == catchFile.Path);
                    if (oldCatchFile == null)
                    {
                        Console.WriteLine($"新加文件--{catchFile.Path}\\{catchFile.FullName}");

                        catchFile.FilOperation = FilOperation.Add;
                        catchFIleList.Add(catchFile);
                        continue;
                    }
                    if (oldCatchFile.LastWriteTime != catchFile.LastWriteTime)
                    {
                        oldCatchFile.FilOperation = FilOperation.Update;
                        oldCatchFile.LastWriteTime = catchFile.LastWriteTime;
                        Console.WriteLine($"修改文件--{catchFile.Path}\\{catchFile.FullName}");
                    }
                    //更新监控扫描时间
                    oldCatchFile.MonitorintTime = time;
                }
            }



        }

        /// <summary>
        /// 匹配文件属性是否过虑
        /// </summary>
        /// <param name="fileAttributes">文件属性</param>
        /// <returns>ture过虑，false不过虑</returns>
        bool IsScreenFileAttributes(FileAttributes fileAttributes)
        {
            string[] strs = fileAttributes.ToString().Split(", ");
            foreach (string str in strs)
            {
                if (screenFile.ContainsKey(str))
                    return true;
            }
            return false;
        }

    }







    /// <summary>
    /// 操作属性
    /// </summary>
    public enum FilOperation
    {
        Normal = 0,
        Add = 1,
        Update = 2,
        Delete = 3,
    }

    /// <summary>
    /// 文件当时状态
    /// </summary>
    public enum FileStatus
    {
        Normal = 0,
        ReadOnly = 1,
        Opened = 2,
        Exception = 3,
    }
}
