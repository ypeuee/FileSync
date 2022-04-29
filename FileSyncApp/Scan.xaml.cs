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
    /// Scan.xaml 的交互逻辑
    /// </summary>
    public partial class Scan : UserControl
    {
        public Scan()
        {
            InitializeComponent();
        }

        private void meter_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (meter.IsStarted)
            {
                meter.Stop();
            }
            else
            {
                meter.Start();
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random(DateTime.Now.Millisecond);
            //添加信号源
            RadarSignal rs = new RadarSignal(30, new SolidColorBrush(Colors.Red),
                    random.Next((int)RadarMeter.MinDistance, (int)RadarMeter.MaxDistance + 1), random.Next(0, 360));
            meter.SignalCollection.Add(rs);
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            //删除最后一个信号源
            if (meter.SignalCollection.Count > 0)
            {
                meter.SignalCollection.RemoveAt(meter.SignalCollection.Count - 1);
            }
        }
        private void Red_Click(object sender, RoutedEventArgs e)
        {
            //设置红色信号源
            foreach (RadarSignal signal in meter.SignalCollection)
            {
                signal.Fill = new SolidColorBrush(Colors.Red);
            }
        }

        private void Yellow_Click(object sender, RoutedEventArgs e)
        {
            //设置黄色信号源
            foreach (RadarSignal signal in meter.SignalCollection)
            {
                signal.Fill = new SolidColorBrush(Colors.Yellow);
            }
        }

    }
}
