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
            if (meter.IsStarted)
            {
                meter.Stop();
                this.Visibility = Visibility.Hidden;

                if (StopClick != null)
                    StopClick(sender, e);

            }
            else
            {
                meter.Start();
            }
        }

        //显示时，自动开启动画
        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!meter.IsStarted)
            {
                if ((Visibility)e.NewValue == Visibility.Visible)
                    meter.Start();
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
            if (bool.Parse(config["FileSync:Execute"]))
            {
                Console.WriteLine("文件同步");
                var fileSync = new FileSync.FileSync();
                fileSync.Exectue(config);
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

            //线程执行
            Task.Factory.StartNew(() =>
           {
               var dirSync = new DirectoriesSync();
               var fileSync = new FileSync.FileSync();

               dirSync.DirectoriesReName(pathFrom, pathTo
                   , ActionFileList, ActionFile, ActionFileProgress);

               //文件
               fileSync.CopyAddFile(pathFrom, pathTo
                  , ActionFileList, ActionFile, ActionFileProgress);
               fileSync.CopyUpdFile(pathFrom, pathTo
                   , ActionFileList, ActionFile, ActionFileProgress);
               fileSync.CopyDelFile(pathFrom, pathTo
                   , ActionFileList, ActionFile, ActionFileProgress);
               //文件夹
               dirSync.CopyAddDirectories(pathFrom, pathTo
                  , ActionFileList, ActionFile, ActionFileProgress);
               dirSync.CopyDelDirectories(pathFrom, pathTo
                   , ActionFileList, ActionFile, ActionFileProgress);
           });

        }



        int NumValue = 0;
        /// <summary>
        /// 回调函数，当前同步的文件列表
        /// </summary>
        /// <param name="fileList"></param>
        void ActionFileList(List<string> fileList)
        {
            if (NumValue >= 100)
                NumValue = 0;

            NumValue += 10;
            ShowNum(NumValue.ToString());
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
                lblNum.Dispatcher.Invoke(new Action<string>((m) => ShowMsg(m)), value);
        }

    }
}
