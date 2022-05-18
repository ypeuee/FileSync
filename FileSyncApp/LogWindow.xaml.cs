using FIleSyncData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MainApp
{
    /// <summary>
    /// LogWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LogWindow : BaseWindow
    {
        SyncLogDAL dal;
        public LogWindow(SyncLogDAL dal)
        {
            InitializeComponent();
            this.dal = dal;
            ucLog.dal = dal;
        }


        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            this.Close();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (mainWindow != null && logWindow != null)
            {
                mainWindow.LocationChanged -= mainWindow_EventHandler;
            }
            logWindow = null;
        }


        private static MainWindow mainWindow;
        private static LogWindow logWindow;
        /// <summary>
        /// 显示窗口，不可使用Show方法。
        /// </summary>
        public static void ShowWindow()
        {
            if (logWindow != null)
                return;

            if (mainWindow == null)
            {
                //历史记录容器跟随
                 mainWindow = (MainWindow)Application.Current.MainWindow;
            }
            if (logWindow == null)
            {
                logWindow = new LogWindow(new SyncLogDAL());
            }
            logWindow.Show();
            logWindow.Left = mainWindow.Left + mainWindow.Width + 5;
            logWindow.Top = mainWindow.Top;
            //logView.Activate();
            mainWindow.LocationChanged += mainWindow_EventHandler;
        }

        static void mainWindow_EventHandler(object? sender, EventArgs e)
        {
            if (mainWindow == null || logWindow == null)
                return;

            if (logWindow.Visibility == Visibility.Visible)
            {
                logWindow.Left = ((Window)sender).Left + mainWindow.Width + 5;
                logWindow.Top = ((Window)sender).Top;
                logWindow.Activate();
            }
        }





    }
}
