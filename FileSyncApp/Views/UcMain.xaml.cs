using FIleSyncData;
using MainApp.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
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

            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 10);
            timer.Tick += (sender, e) =>
            {
                RunScan(sender, null);
            };
            timer.Start();

        }

        bool timeIsRun = false;
        System.Windows.Threading.DispatcherTimer timer = null;

        /// <summary>
        /// 单击打开按钮
        /// </summary>
        [Category("Behavior")]
        public event RoutedEventHandler StartClick;

        /// <summary>
        /// 开始按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            RunScan(sender, e);
        }

        void RunScan(object sender, RoutedEventArgs e)
        {
            if (timeIsRun == true) return;        
            timeIsRun = true;

            if (StartClick != null)
                StartClick(sender, e);

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

            ////历史记录容器跟随
            //MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            //var logView = new LogWindow(new SyncLogDAL());
            //logView.Show();
            //logView.Left = mainWindow.Left + mainWindow.Width + 5;
            //logView.Top = mainWindow.Top;
            ////logView.Activate();
            //mainWindow.LocationChanged += (dnO, dmE) =>
            //{
            //    if (logView.Visibility == Visibility.Visible)
            //    {
            //        logView.Left = ((Window)dnO).Left + mainWindow.Width + 5;
            //        logView.Top = ((Window)dnO).Top;
            //        logView.Activate();
            //    }
            //};

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
