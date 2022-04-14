﻿using System;
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
        }

        #region 开始按钮改变格式
        private void btnStart_MouseEnter(object sender, MouseEventArgs e)
        {
            pathStop.Visibility = Visibility.Hidden;
            pathStart.Visibility = Visibility.Visible;
        }
        private void btnStart_MouseLeave(object sender, MouseEventArgs e)
        {
            pathStop.Visibility = Visibility.Visible;
            pathStart.Visibility = Visibility.Hidden;
        }
        #endregion

        /// <summary>
        /// 开始按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {

        }



        private void ButHistory_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            var logView = new LogWindow();
            logView.Show();
            logView.Left = mainWindow.Left + mainWindow.Width + 5;
            logView.Top = mainWindow.Top;
            //logView.Activate();
            mainWindow.LocationChanged += (dnO, dmE) =>
            {
                if (logView.Visibility == Visibility.Visible)
                {
                    logView.Left = ((Window)dnO).Left + mainWindow.Width + 5;
                    logView.Top = ((Window)dnO).Top;
                    logView.Activate();
                }
            };         

        }

       



    }
}
