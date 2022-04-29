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
        public LogWindow()
        {
            InitializeComponent();

        }

        public LogWindow(double top, double left):this()
        {
            Top = top;
            Left = left;
        }


        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            this.Close();
        }
    }
}
