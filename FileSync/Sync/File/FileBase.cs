using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileSync.Sync.File
{
    public class FileBase
    {
        List<FileM> catchFIleList = new List<FileM>();
        Dictionary<string, int> screenFile = new Dictionary<string, int>();

        public FileBase()
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
        /// 获取所有文件
        /// </summary>
        /// <param name="path"></param>
        public List<FileM> FileLists(string homePath, string path = null)
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
