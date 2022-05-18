using MainApp.Tools;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MainApp.Views
{
    /// <summary>
    /// UcMain.xaml 的交互逻辑
    /// </summary>
    public partial class UcMain : UserControl
    {
        public UcMain()
        {
            InitializeComponent();

            BindPath();

            #region 开始按钮改变格式
            btnStart.MouseEnter += (sender, e) =>
              {
                  pathStop.Visibility = Visibility.Hidden;
                  pathStart.Visibility = Visibility.Visible;
              };
            btnStart.MouseLeave += (sender, e) =>
              {
                  pathStop.Visibility = Visibility.Visible;
                  pathStart.Visibility = Visibility.Hidden;
              };
            #endregion

            //同步定时器         
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 10);
            timer.Tick += (sender, e) =>
            {
                RunScan(sender, null);
            };
            timer.Start();
        }

        /// <summary>
        /// 扫描开始事件
        /// </summary>
        [Category("Behavior")]
        public event RoutedEventHandler ScanStart;

        /// <summary>
        /// 扫描结束事件
        /// </summary>
        [Category("Behavior")]
        public event Action<string, bool> ScanEnd;

        bool timeIsRun = false;
        System.Windows.Threading.DispatcherTimer timer = null;
        DateTime dateTime = DateTime.Now;

        /// <summary>
        /// 扫描结束通知方法
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="agr2"></param>
        public void MainScanEnd(string arg1, bool arg2)
        {
            if (arg2)
            {
                dateTime = DateTime.Now;
            }
            var timeLag = (DateTime.Now - dateTime);
            string msg;
            if (timeLag.TotalDays >= 1)
                msg = $"{Math.Ceiling(timeLag.TotalDays) }天";
            else if (timeLag.TotalHours >= 1)
                msg = $"{Math.Ceiling(timeLag.TotalHours) }小时";
            else if (timeLag.TotalMinutes >= 1)
                msg = $"{Math.Ceiling(timeLag.TotalMinutes) }分钟";
            else
                msg = $"{ (timeLag.Seconds > 0 ? Math.Ceiling(timeLag.TotalSeconds) : 1) }秒";
            lblSyncMsg.Content = $"上次同步于{msg}前无目录文件更新";


            ScanEnd?.Invoke(arg1, arg2);

            if (arg2)
            {
                Task.Factory.StartNew(() =>
                {
                    LogWindow.LoadLogs();
                });
            }
        }

        /// <summary>
        /// 开始按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            RunScan(sender, e);
        }

        /// <summary>
        /// 执行扫描
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void RunScan(object sender, RoutedEventArgs e)
        {
            if (timeIsRun == true) return;
            timeIsRun = true;
            try
            {
                ScanStart?.Invoke(sender, e);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                //throw;
            }

            timeIsRun = false;
        }

        /// <summary>
        /// 打开历史记录窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButHistory_Click(object sender, RoutedEventArgs e)
        {
            LogWindow.ShowWindow();
        }

        /// <summary>
        /// 打开源文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFrom_Click(object sender, RoutedEventArgs e)
        {
            Open.OpenDir(lblFrom.Content.ToString());
        }

        /// <summary>
        /// 打开目标文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTo_Click(object sender, RoutedEventArgs e)
        {
            Open.OpenDir(lblTo.Content.ToString());
        }

        /// <summary>
        /// 绑定源和目标路径
        /// </summary>
        void BindPath()
        {
            lblFrom.Content = FileSync.ConfigurationFile.Configuration["FileSync:PathFrom"];
            lblTo.Content = FileSync.ConfigurationFile.Configuration["FileSync:PathTo"];
        }
    }
}
