using FileSync;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainApp.Views
{
    /// <summary>
    /// UcScan.xaml 的交互逻辑
    /// </summary>
    public partial class UcScan : UserControl
    {
        public UcScan()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 扫描结束事件
        /// </summary>
        [Category("Behavior")]
        public event RoutedEventHandler StopClick;


        private void meter_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //if (meter.IsStarted)
            //{
            //    meter.Stop();
            //    this.Visibility = Visibility.Hidden;

            //    if (StopClick != null)
            //        StopClick(sender, e);

            //}
            //else
            //{
            //    meter.Start();
            //}
        }

        //显示时，自动开启动画
        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!meter.IsStarted)
            {
                if ((bool)e.NewValue) //== Visibility.Visible)
                {
                    Reset();
                    meter.Start();
                    Main();
                }
            }
        }

        void Main()
        {
            Console.WriteLine("开始执行");
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            var config = builder.Build();

            //文件同步

            Console.WriteLine("文件同步");
            Exectue(config);

            Console.WriteLine("完成执行，按任意键退出。");

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

            //线程执行      
            Task task = Task.Factory.StartNew(() =>
             {
                 var dirSync = new DirectoriesSync();
                 var fileSync = new FileSync.FileSync();

                 //只计算
                 bool isExecute = false;


                 dirSync.DirectoriesReName(pathFrom, pathTo
                    , isExecute, ActionFileList);

                 //文件
                 fileSync.CopyAddFile(pathFrom, pathTo
                      , isExecute, ActionFileList);
                 fileSync.CopyUpdFile(pathFrom, pathTo
                    , isExecute, ActionFileList);
                 fileSync.CopyDelFile(pathFrom, pathTo
                     , isExecute, ActionFileList);
                 //文件夹
                 dirSync.CopyAddDirectories(pathFrom, pathTo
                      , isExecute, ActionFileList);
                 dirSync.CopyDelDirectories(pathFrom, pathTo
                  , isExecute, ActionFileList);

                 //执行
                 isExecute = true;
                 dirSync.DirectoriesReName(pathFrom, pathTo
                   , actionFileProgress: ActionFileProgress);

                 //文件
                 fileSync.CopyAddFile(pathFrom, pathTo
                     , actionFile: ActionFile, actionFileProgress: ActionFileProgress);
                 fileSync.CopyUpdFile(pathFrom, pathTo
                    , actionFile: ActionFile, actionFileProgress: ActionFileProgress);
                 fileSync.CopyDelFile(pathFrom, pathTo
                     , actionFile: ActionFile, actionFileProgress: ActionFileProgress);
                 //文件夹
                 dirSync.CopyAddDirectories(pathFrom, pathTo
                       , actionFile: ActionFile, actionFileProgress: ActionFileProgress);
                 dirSync.CopyDelDirectories(pathFrom, pathTo
                  , actionFile: ActionFile, actionFileProgress: ActionFileProgress);

             });


            task.ContinueWith((obj) =>
            {
                End();
                System.Threading.Thread.Sleep(1000);

                meter.Stop();
                this.Dispatcher.Invoke(() => { Visibility = Visibility.Hidden; });
            });


        }

        /// <summary>
        /// 重置进度
        /// </summary>
        void Reset()
        {
            FileTotalNum = 0;
            FIleIndex = 0;
            ShowNum("0");
            ShowMsg("正在比较云端本地目录...");
        }

        /// <summary>
        /// 同步结束
        /// </summary>
        void End()
        {
            FileTotalNum = 0;
            FIleIndex = 0;
            ShowNum("100");
            ShowMsg("同步完成");
        }

        int FileTotalNum = 0;
        int FIleIndex = 0;
        /// <summary>
        /// 回调函数，当前同步的文件列表
        /// </summary>
        /// <param name="fileList"></param>
        void ActionFileList(List<string> fileList)
        {
            FileTotalNum += fileList.Count;
        }

        /// <summary>
        /// 回调函数，当前同步的文件
        /// </summary>
        /// <param name="file"></param>
        private void ActionFile(string file)
        {
            ShowMsg(file);
        }

        /// <summary>
        /// 回调函数，同前同步文件进度
        /// </summary>
        /// <param name="file"></param>
        /// <param name="progress"></param>
        private void ActionFileProgress(string file, int progress)
        {
            FIleIndex++;
            ShowNum(((int)(FIleIndex * 100.00 / FileTotalNum)).ToString());
            System.Threading.Thread.Sleep(500);
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="msg"></param>
        void ShowMsg(string msg)
        {
            if (LblMsg.CheckAccess())
                LblMsg.Content = msg;
            else
                lblNum.Dispatcher.Invoke(new Action<string>((m) => ShowMsg(m)), msg);
        }

        /// <summary>
        /// 显示进度值
        /// </summary>
        /// <param name="value"></param>
        void ShowNum(string value)
        {
            if (lblNum.CheckAccess())
                lblNum.Content = value;
            else
                lblNum.Dispatcher.Invoke(new Action<string>((m) => ShowNum(m)), value);
        }

    }
}
