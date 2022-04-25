using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSync
{
    /// <summary>
    /// 相同文件查找
    /// </summary>
    class SameFile
    {
        public void Execute(IConfigurationRoot config)
        {
            //源路径
            string pathFrom = config["SameFile:PathFrom"];// @"D:\testFrom";
            //目的路径
            string pathTo = config["SameFile:PathTo"];// @"D:\testTo";
            //配置规则
            string rule = config["SameFile:Rule"];

            Console.WriteLine($"pathFrom: {pathFrom}");
            Console.WriteLine($"pathTo: {pathTo}");
            Console.WriteLine($"Rule: {rule}");

            var res = GetSameFile(pathFrom, pathTo, rule.Split(','));
            foreach (var item in res)
            {
                Console.WriteLine($"{item.FromFile.FullName}");
                foreach (var toFile in item.ToFile)
                {
                    Console.WriteLine($"\t{toFile.FullName}");
                }
                Console.WriteLine();
            }

        }



        public List<SameFileM> GetSameFile(string pathFrom, string pathTo = null, string[] rules = null)
        {
            var fromFileList = FileLists(pathFrom);
            List<FileM> toFileList;
            if (string.IsNullOrEmpty(pathTo))
            {
                pathTo = pathFrom;
                toFileList = fromFileList;
            }
            else
            {
                toFileList = FileLists(pathTo);
            }

            if (rules == null || rules.Length == 0)
                rules = new string[] { "文件名称", "文件大小", "修改时间" };

            var items = CulcuteRule(rules, toFileList, fromFileList);
            var result = items.GroupBy(m => m.FromFile).Select(m => new SameFileM() { FromFile = m.Key, ToFile = m.Select(n => n.ToFile).ToList() }).ToList();

            return result;
        }

        /// <summary>
        /// 按规则查找相同文件
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="toFileList"></param>
        /// <param name="fromFileList"></param>
        /// <returns></returns>
        List<CalcuateFileM> CulcuteRule(string[] rules, List<FileM> toFileList, List<FileM> fromFileList)
        {
            List<CalcuateFileM> items;
            //首规则
            switch (rules[0])
            {
                case "文件名称":
                    //按名称匹配
                    items = MatchName(fromFileList, toFileList);
                    break;
                case "文件大小":
                    //按文件大小
                    items = MatchLength(fromFileList, toFileList);
                    break;
                case "修改时间":
                    //按修改时间
                    items = MatchWriteTime(fromFileList, toFileList);
                    break;
                default:
                    //按名称匹配
                    items = MatchName(fromFileList, toFileList);
                    break;

            }
            //其它项规则
            for (int i = 1; i < rules.Length; i++)
            {
                switch (rules[i])
                {
                    case "文件名称":
                        //按名称匹配
                        items = items.Where(m => m.FromFile.Name == m.ToFile.Name).ToList();
                        break;
                    case "文件大小":
                        //按文件大小
                        items = items.Where(m => m.FromFile.Length == m.ToFile.Length).ToList();
                        break;
                    case "修改时间":
                        //按修改时间
                        items = items.Where(m => m.FromFile.LastWriteTime == m.ToFile.LastWriteTime).ToList();
                        break;
                }
            }

            return items;
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
                    FullName = file.FullName,//.Replace(homePath, ""),
                    Path = file.DirectoryName,//.Replace(homePath, ""),
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
        /// 按名称匹配相同文件
        /// </summary>
        /// <param name="fromFileList"></param>
        /// <param name="toFileList"></param>
        /// <returns></returns>
        List<CalcuateFileM> MatchName(List<FileM> fromFileList, List<FileM> toFileList)
        {
            var temps = from f in fromFileList
                        join t in toFileList
                        on f.Name equals t.Name into
                        items
                        from item in items.DefaultIfEmpty()
                        where item.Path != f.Path
                        select new CalcuateFileM
                        {
                            FromFile = f,
                            ToFile = item
                        };

            var list = temps.ToList();
            return list;//.ToList().Where(m => m.ToFile == null).ToList();
        }

        /// <summary>
        /// 按文件大小匹配相同文件
        /// </summary>
        /// <param name="fromFileList"></param>
        /// <param name="toFileList"></param>
        /// <returns></returns>
        List<CalcuateFileM> MatchLength(List<FileM> fromFileList, List<FileM> toFileList)
        {
            var temps = from f in fromFileList
                        join t in toFileList
                        on f.Length equals t.Length into
                        items
                        from item in items.DefaultIfEmpty()
                        where item.Path != f.Path
                        select new CalcuateFileM
                        {
                            FromFile = f,
                            ToFile = item
                        };

            var list = temps.ToList();
            return list;//.ToList().Where(m => m.ToFile == null).ToList();
        }

        /// <summary>
        /// 按文件最后写入时间匹配相同文件
        /// </summary>
        /// <param name="fromFileList"></param>
        /// <param name="toFileList"></param>
        /// <returns></returns>
        List<CalcuateFileM> MatchWriteTime(List<FileM> fromFileList, List<FileM> toFileList)
        {
            var temps = from f in fromFileList
                        join t in toFileList
                        on f.LastWriteTime equals t.LastWriteTime into
                        items
                        from item in items.DefaultIfEmpty()
                        where item.Path != f.Path
                        select new CalcuateFileM
                        {
                            FromFile = f,
                            ToFile = item
                        };

            var list = temps.ToList();
            return list;//.ToList().Where(m => m.ToFile == null).ToList();
        }
    }

    public enum SameFuleRule
    {
        文件名称 = 0, 文件大小 = 1, 修改时间 = 2
    }
}
