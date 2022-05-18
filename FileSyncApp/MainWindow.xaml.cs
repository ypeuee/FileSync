using FileSync.Sync.Directories;
using FileSync.Sync.File;
using MainApp.Views;
using MainApp.Controls.Radar;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MainApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : BaseWindow
    {  
        public MainWindow(DirectoriesAll directoriesAll, FileAll fileAll)
        {
            InitializeComponent();

            #region 自动关闭已打开的菜单
            //窗体失去焦点自动关闭菜单
            this.Deactivated += (o, e) =>
            {
                if (ucMenu.Visibility == Visibility.Visible)
                    ucMenu.Visibility = Visibility.Hidden;
            };

            //单击窗体自动关闭菜单
            this.MouseDown += (o, e) =>
            {
                if (ucMenu.Visibility == Visibility.Visible)
                    ucMenu.Visibility = Visibility.Hidden;
            };
            #endregion

            ucMain.ScanStart += (o, e) =>
            {
                ucScan.Visibility = Visibility.Visible;
                Canvas.SetZIndex(ucScan, 100);//置于最顶层
            };
       
            ucScan.DirectoriesAll = directoriesAll;
            ucScan.FileAll = fileAll;
            //扫描结束
            ucScan.ScanEnd += ucMain.MainScanEnd;
        }


        #region 标题栏

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
            ShowInTaskbar = false;
            Close();
        }

        /// <summary>
        /// 最大化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_max_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
            ShowOrhide();
        }

        /// <summary>
        /// 正常大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_normal_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Normal;
            ShowOrhide();
        }

        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_min_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 双击最大化或者是还原
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                ShowOrhide();
            }
        }

        /// <summary>
        /// 显示或者隐藏按钮
        /// </summary>
        private void ShowOrhide()
        {
            switch (WindowState)
            {
                case WindowState.Normal:
                    //btn_normal.Visibility = Visibility.Collapsed;
                    //btn_max.Visibility = Visibility.Visible;
                    break;
                case WindowState.Minimized:
                    break;
                case WindowState.Maximized:
                    //btn_max.Visibility = Visibility.Collapsed;
                    //btn_normal.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 打开或关闭菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_menu_Click(object sender, RoutedEventArgs e)
        {
            //单击工具栏，打开或关闭菜单
            if (ucMenu.Visibility == Visibility.Hidden)
            {
                ucMenu.Visibility = Visibility.Visible;
                Canvas.SetZIndex(ucMenu, 101);//置于最顶层
            }
            else
                ucMenu.Visibility = Visibility.Hidden;
        }
        #endregion


    }
}
