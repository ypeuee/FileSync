using FileSync.Sync.Directories;
using FileSync.Sync.File;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace FileSync.Sync
{
    public class SyncStart
    {
        DirectoriesAdd dirAdd;
        DirectoriesDelete dirDel;
        DirectoriesReName dirReName;

        FileAdd fileAdd;
        FIleUpdate fileUpd;
        FileDelete fileDel;
        public SyncStart(DirectoriesAdd dirAdd, DirectoriesDelete dirDel, DirectoriesReName dirReName
            , FileAdd fileAdd, FIleUpdate fileUpd, FileDelete fileDel)
        {
            this.dirAdd = dirAdd;
            this.dirDel = dirDel;
            this.dirReName = dirReName;

            this.fileAdd = fileAdd;
            this.fileUpd = fileUpd;
            this.fileDel = fileDel;
        }
        public void Main()
        {
            Console.WriteLine("开始执行");

            IConfigurationRoot config = ConfigurationFile.Configuration;

            //查询相同
            //if (bool.Parse(config["SameFile:Execute"]))
            //{
            //    Console.WriteLine("查询相同");
            //    new SameFile().Execute(config);
            //}

            //文件同步
            if (bool.Parse(config["FileSync:Execute"]))
            {
                Console.WriteLine("文件同步");

                Exectue(config);
            }

            Console.WriteLine("完成执行，按任意键退出。");
            Console.ReadKey();
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


            do
            {

                dirReName.SyncReName(pathFrom, pathTo);

                //文件
                fileAdd.SyncAdd(pathFrom, pathTo);
                fileUpd.SyncUpdate(pathFrom, pathTo);
                fileDel.SyncDelete(pathFrom, pathTo);
                //文件夹
                dirAdd.SyncAdd(pathFrom, pathTo);
                dirDel.SyncDelete(pathFrom, pathTo);

                System.Threading.Thread.Sleep(1000);
            } while (bool.Parse(config["FileSync:Repeatedly"]));

        }
    }
}
