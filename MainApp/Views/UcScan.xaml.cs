using FileSync;
using FileSync.Sync.Directories;
using FileSync.Sync.File;
using Microsoft.Extensions.Configuration;
using RadarControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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

        public DirectoriesAll DirectoriesAll { get; set; }
        public FileAll FileAll { get; set; }


        /// <summary>
        /// 扫描结束事件
        /// </summary>
        [Category("Behavior")]
        public event Action<string, bool> StopClick;


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
                    //文件同步
                    Exectue(ConfigurationFile.Configuration);
                }
            }

            if (!(bool)e.NewValue)
            {
                if (StopClick != null)
                    StopClick("执行完成!", true);
            }
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
            Task task = Task.Factory.StartNew(
                    //new Action<string, string>((pathTo, pathFrom)
                    () =>
             {
                 ////只计算
                 //var dirReNameList = DirectoriesAll.DirectoriesReName.ReNameList(pathFrom, pathTo);

                 ////文件
                 //var fileAddList = FileAll.FileAdd.AddList(pathFrom, pathTo);
                 //var fileUpdList = FileAll.FIleUpdate.UpdateList(pathFrom, pathTo);
                 //var fileDelList = FileAll.FileDelete.DeleteList(pathFrom, pathTo);
                 ////文件夹
                 //var dirAddList = DirectoriesAll.DirectoriesAdd.AddList(pathFrom, pathTo);
                 ////var dirDelList = DirectoriesAll.DirectoriesDelete.DeleteList(pathFrom, pathTo);


                 //var dirSync = new DirectoriesSync();
                 //var fileSync = new FileSync.FileSync();

                 //只计算
                 bool isExecute = false;


                 DirectoriesAll.DirectoriesReName.SyncReName(pathFrom, pathTo
                    , isExecute, ActionFileList);

                 //文件
                 FileAll.FileAdd.SyncAdd(pathFrom, pathTo
                      , isExecute, ActionFileList);
                 FileAll.FIleUpdate.SyncUpdate(pathFrom, pathTo
                    , isExecute, ActionFileList);
                 FileAll.FileDelete.SyncDelete(pathFrom, pathTo
                     , isExecute, ActionFileList);
                 //文件夹
                 DirectoriesAll.DirectoriesAdd.SyncAdd(pathFrom, pathTo
                      , isExecute, ActionFileList);
                 DirectoriesAll.DirectoriesDelete.SyncDelete(pathFrom, pathTo
                  , isExecute, ActionFileList);

                 //执行
                 isExecute = true;
                 DirectoriesAll.DirectoriesReName.SyncReName(pathFrom, pathTo
                   , actionFileProgress: ActionFileProgress);

                 //文件
                 FileAll.FileAdd.SyncAdd(pathFrom, pathTo
                     , actionFile: ActionFile, actionFileProgress: ActionFileProgress);
                 FileAll.FIleUpdate.SyncUpdate(pathFrom, pathTo
                    , actionFile: ActionFile, actionFileProgress: ActionFileProgress);
                 FileAll.FileDelete.SyncDelete(pathFrom, pathTo
                     , actionFile: ActionFile, actionFileProgress: ActionFileProgress);
                 //文件夹
                 DirectoriesAll.DirectoriesAdd.SyncAdd(pathFrom, pathTo
                       , actionFile: ActionFile, actionFileProgress: ActionFileProgress);
                 DirectoriesAll.DirectoriesDelete.SyncDelete(pathFrom, pathTo
                  , actionFile: ActionFile, actionFileProgress: ActionFileProgress);

             }
             //), pathTo, pathFrom
                );

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
            //清空信号源
            meter.Dispatcher.Invoke(() =>
            {
                meter.SignalCollection.Clear();
            });

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
        void ActionFileList(SyncType syncType, List<string> fileList)
        {
            FileTotalNum += fileList.Count;

            foreach (var file in fileList)
            {
                AddSignal(syncType, file);
            }
        }

        /// <summary>
        /// 回调函数，当前同步的文件
        /// </summary>
        /// <param name="file"></param>
        private void ActionFile(SyncType syncType, string file)
        {
            ShowMsg(file);
        }

        /// <summary>
        /// 回调函数，同前同步文件进度
        /// </summary>
        /// <param name="file"></param>
        /// <param name="progress"></param>
        private void ActionFileProgress(SyncType syncType, string file, int progress)
        {
            DelSignal();
            FIleIndex++;
            var value = ((int)(FIleIndex * 100.00 / FileTotalNum));
            if (value > 100)
                value = 100;
            ShowNum(value.ToString());
            System.Threading.Thread.Sleep(500);
        }

        void AddSignal(SyncType syncType, string path)
        {
            Color color;
            switch (syncType)
            {
                case SyncType.DirDel:
                    color = Colors.DarkRed;
                    break;
                case SyncType.DirAdd:
                    color = Colors.DarkGreen;
                    break;
                case SyncType.DirReName:
                    color = Colors.GreenYellow;
                    break;
                case SyncType.FileDel:
                    color = Colors.Red;
                    break;
                case SyncType.FileAdd:
                    color = Colors.Green;
                    break;
                case SyncType.FileUpd:
                    color = Colors.Blue;
                    break;
                default:
                    color = Colors.Blue;
                    break;
            }


            //添加信号源
            meter.Dispatcher.Invoke(new Action<string, Color>((p, c) =>
            {
                Random random = new Random(DateTime.Now.Millisecond);
                RadarSignal rs = new RadarSignal(30, new SolidColorBrush(color),
                  random.Next((int)RadarMeter.MinDistance,
                  (int)RadarMeter.MaxDistance + 1),
                  random.Next(0, 300))
                { ToolTip = path };
                meter.SignalCollection.Add(rs);
            }), path, color);

            // Random random = new Random(DateTime.Now.Millisecond);
            //RadarSignal rs = new RadarSignal(30, new SolidColorBrush(color),
            //        random.Next((int)RadarMeter.MinDistance, (int)RadarMeter.MaxDistance + 1), random.Next(0, 360));
            //rs.ToolTip = path;
            //meter.SignalCollection.Add(rs);
        }

        //删除第一个信号源
        void DelSignal()
        {
            meter.Dispatcher.Invoke(new Action(() =>
            {
                //删除第一个信号源
                if (meter.SignalCollection.Count > 0)
                    meter.SignalCollection.RemoveAt(0);
            }));
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
