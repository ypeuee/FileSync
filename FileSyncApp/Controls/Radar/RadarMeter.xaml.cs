using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Collections;

namespace MainApp.Controls.Radar
{
    /// <summary>
    /// RadarMeter.xaml 的交互逻辑
    /// </summary>
    public partial class RadarMeter : UserControl
    {
        #region 雷达属性

        /// <summary>
        /// 扫描故事板
        /// </summary>
        private Storyboard scanStory = null;

        /// <summary>
        /// 托盘纹路
        /// </summary>
        private List<Shape> dialShape = new List<Shape>();

        #endregion

        #region 外部接口

        /// <summary>
        /// 雷达扫描最近距离
        /// </summary>
        private static double minDistance = 50.0;
        /// <summary>
        /// 获取雷达扫描最近距离
        /// </summary>
        public static double MinDistance
        {
            get { return minDistance; }
        }

        /// <summary>
        /// 雷达扫描最远距离
        /// </summary>
        private static double maxDistance = 300.0;
        /// <summary>
        /// 获取或设置雷达扫描最远距离
        /// </summary>
        public static double MaxDistance
        {
            get { return maxDistance; }
        }

        /// <summary>
        /// 获取或设置托盘填充色
        /// </summary>
        public Brush DialFill
        {
            get { return stroke1.Fill; }
            set { stroke1.Fill = value; }
        }

        /// <summary>
        /// 获取或设置托盘纹路颜色
        /// </summary>
        public Brush DialStroke
        {
            get { return stroke1.Stroke; }
            set 
            {
                foreach (Shape shape in dialShape)
                {
                    shape.Stroke = value;
                }
            }
        }

        /// <summary>
        /// 获取或设置托盘纹路粗细
        /// </summary>
        public double DialStrokeThickness
        {
            get { return stroke1.StrokeThickness; }
            set
            {
                foreach (Shape shape in dialShape)
                {
                    shape.StrokeThickness = value;
                }
            }
        }

        /// <summary>
        /// 获取或设置扫描区域颜色
        /// </summary>
        public Brush ScanFill
        {
            get { return ScanArea.Fill; }
            set { ScanArea.Fill = value; }
        }

        /// <summary>
        /// 获取或设置扫描区域透明度
        /// </summary>
        public double ScanOpacity
        {
            get { return ScanArea.Opacity; }
            set { ScanArea.Opacity = value; }
        }

        /// <summary>
        /// 信号源集合
        /// </summary>
        private RadarSignalCollection signalCollection = null;
        /// <summary>
        /// 获取信号源集合
        /// </summary>
        public RadarSignalCollection SignalCollection
        {
            get { return signalCollection; }
        }

        /// <summary>
        /// 是否启动雷达扫描
        /// </summary>
        private bool isStarted = false;
        /// <summary>
        /// 获取是否启动雷达扫描
        /// </summary>
        public bool IsStarted
        {
            get { return isStarted; }
        }

        /// <summary>
        /// 构造器
        /// </summary>
        public RadarMeter()
        {
            InitializeComponent();
            scanStory = (Storyboard)Resources["scanStory"];
            dialShape.Add(stroke1);
            dialShape.Add(stroke2);
            dialShape.Add(stroke3);
            dialShape.Add(stroke4);
            dialShape.Add(stroke5);
            dialShape.Add(stroke6);
            dialShape.Add(stroke7);
            dialShape.Add(stroke8);
            dialShape.Add(stroke9);
            
            signalCollection = new RadarSignalCollection(onSignalAdd, onSignalDel);
        }

        /// <summary>
        /// 启动雷达扫描
        /// </summary>
        public void Start()
        {
            if (isStarted) return;
            scanStory.Begin();
            isStarted = true;
        }

        /// <summary>
        /// 停止雷达扫描
        /// </summary>
        public void Stop()
        {
            if (isStarted == false) return;
            scanStory.Stop();
            isStarted = false;
        }

        #endregion

        #region 事件接口

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            scanStory.Begin();
        }

        private void onSignalAdd(RadarSignal signal)
        {
            scanStory.Children.Add(signal.Animation);
            DotContainer.Children.Add(signal.Model);
        }

        private void onSignalDel(RadarSignal signal)
        {
            scanStory.Children.Remove(signal.Animation);
            DotContainer.Children.Remove(signal.Model);
        }

        #endregion
    }
}
