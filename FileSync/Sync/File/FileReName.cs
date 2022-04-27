using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSync.Sync.File
{
  public  class FileReName:FileBase
    {
        /// <summary>
        /// 文件重命名
        /// </summary>
        /// <param name="pathFrom"></param>
        /// <param name="pathTo"></param>
        public void ReName(string pathFrom, string pathTo)
        {
            //var fromFileList = FileLists(pathFrom);

            //var toFileList = FileLists(pathTo);

            ////add
            //var tempAdds = CalculateAddFile(fromFileList, toFileList);
            //foreach (var item in tempAdds)
            //{
            //    Console.WriteLine($"add {item.FullName}");
            //    string toPath = pathTo + item.Path;// Path.Combine(pathTo, item.Path);
            //    if (!System.IO.Directory.Exists(toPath))
            //    {
            //        System.IO.Directory.CreateDirectory(toPath);
            //    }
            //    string toFile = pathTo + item.FullName;//Path.Combine(pathTo, item.FullName);
            //    System.IO.File.Copy(pathFrom + item.FullName, toFile);
            //}

        }

    }
}
