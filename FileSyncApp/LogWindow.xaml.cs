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
        TM_PickingGridDAL dal;
        public LogWindow(TM_PickingGridDAL dal)
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
    }
}
