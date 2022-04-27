using System.Collections.Generic;
using System.IO;

namespace FileSync.Sync.Directories
{
    public class DirectoriesBase
    {
        public DirectoriesBase() { }

        /// <summary>
        /// 获取所有文件夹
        /// </summary>
        /// <param name="homePath"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        protected List<DirectorieM> DirectoriesLists(string homePath, string path = null)
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
    }
}
