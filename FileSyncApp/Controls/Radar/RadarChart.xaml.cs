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

namespace MainApp.Controls.Radar
{
    /// <summary>
    /// RadarChart.xaml 的交互逻辑
    /// </summary>
    public partial class RadarChart : UserControl
    {
        #region 图形属性

        /// <summary>
        /// 文本边框
        /// </summary>
        private List<Ellipse> textCircles = new List<Ellipse>();

        /// <summary>
        /// 文本内容
        /// </summary>
        private List<TextBlock> textBlocks = new List<TextBlock>();

        /// <summary>
        /// 雷达托盘
        /// </summary>
        private Ellipse dialEllipse = new Ellipse();

        /// <summary>
        /// 雷达托盘
        /// </summary>
        private List<Polygon> dialPolygons = new List<Polygon>();

        /// <summary>
        /// 雷达指针
        /// </summary>
        private Polygon handPolygon = new Polygon();

        #endregion

        #region 雷达属性

        /// <summary>
        /// 画布半径
        /// </summary>
        private double canvasRadius = 0;

        /// <summary>
        /// 雷达半径（文本半径与托盘半径的和）
        /// </summary>
        private double totalRadius = 0;

        /// <summary>
        /// 文本半径
        /// </summary>
        private double textRadius = 0;

        /// <summary>
        /// 托盘半径
        /// </summary>
        private double dialRadius = 0;

        /// <summary>
        /// 平均角度
        /// </summary>
        private double degree = 0;

        /// <summary>
        /// 托盘层次
        /// </summary>
        private const int layerNumber = 5;

        /// <summary>
        /// 雷达属性信息集合
        /// </summary>
        private List<RadarAttribute> attributes = new List<RadarAttribute>();

        #endregion

        #region 外部接口

        /// <summary>
        /// 最小百分比
        /// </summary>
        private static double minPercent = 0.0;
        /// <summary>
        /// 获取最小百分比
        /// </summary>
        public static double MinPercent
        {
            get { return minPercent; }
        }

        /// <summary>
        /// 最大百分比
        /// </summary>
        private static double maxPercent = 1.0;
        /// <summary>
        /// 获取最大百分比
        /// </summary>
        public static double MaxPercent
        {
            get { return maxPercent; }
        }

        /// <summary>
        /// 雷达属性个数
        /// </summary>
        private int attributeCount = 4;
        /// <summary>
        /// 获取或设置雷达属性个数（不能小于3）
        /// </summary>
        public int AttributeCount
        {
            get { return attributeCount; }
            set
            {
                if (value >= 3)
                {
                    attributeCount = value;
                    drawRadar();
                }
            }
        }

        /// <summary>
        /// 获取雷达属性
        /// </summary>
        /// <param name="index">雷达属性索引（不能小于零，也不能大于等于雷达属性个数）</param>
        /// <returns>雷达属性信息</returns>
        public RadarAttribute this[int index]
        {
            get { return attributes[index]; }
        }

        /// <summary>
        /// 文本内容颜色
        /// </summary>
        private Brush textFill = new SolidColorBrush(Colors.White);
        /// <summary>
        /// 获取或设置文本内容颜色
        /// </summary>
        public Brush TextFill
        {
            get { return textFill; }
            set
            {
                textFill = value;
                foreach (TextBlock text in textBlocks)
                {
                    text.Foreground = textFill;
                }
            }
        }

        /// <summary>
        /// 文本边框颜色
        /// </summary>
        private Brush textStroke = new SolidColorBrush(Colors.White);
        /// <summary>
        /// 获取或设置文本边框颜色
        /// </summary>
        public Brush TextStroke
        {
            get { return textStroke; }
            set
            {
                textStroke = value;
                foreach (Ellipse circle in textCircles)
                {
                    circle.Stroke = textStroke;
                }
            }
        }

        /// <summary>
        /// 文本边框粗细
        /// </summary>
        private double textStrokeThickness = 1;
        /// <summary>
        /// 获取或设置文本边框粗细
        /// </summary>
        public double TextStrokeThickness
        {
            get { return textStrokeThickness; }
            set
            {
                textStrokeThickness = value;
                foreach (Ellipse circle in textCircles)
                {
                    circle.StrokeThickness = textStrokeThickness;
                }
            }
        }

        /// <summary>
        /// 托盘填充色
        /// </summary>
        private Brush dialFill = new SolidColorBrush(Colors.Black);
        /// <summary>
        /// 获取或设置托盘填充色
        /// </summary>
        public Brush DialFill
        {
            get { return dialFill; }
            set
            {
                dialFill = value;
                dialEllipse.Fill = dialFill;
            }
        }

        /// <summary>
        /// 托盘纹路颜色
        /// </summary>
        private Brush dialStroke = new SolidColorBrush(Colors.White);
        /// <summary>
        /// 获取或设置托盘纹路颜色
        /// </summary>
        public Brush DialStroke
        {
            get { return dialStroke; }
            set
            {
                dialStroke = value;
                dialEllipse.Stroke = dialStroke;
                foreach (Polygon polygon in dialPolygons)
                {
                    polygon.Stroke = dialStroke;
                }
            }
        }

        /// <summary>
        /// 托盘纹路粗细
        /// </summary>
        private double dialStrokeThickness = 1;
        /// <summary>
        /// 获取或设置托盘纹路粗细
        /// </summary>
        public double DialStrokeThickness
        {
            get { return dialStrokeThickness; }
            set
            {
                dialStrokeThickness = value;
                dialEllipse.StrokeThickness = dialStrokeThickness;
                foreach (Polygon polygon in dialPolygons)
                {
                    polygon.StrokeThickness = dialStrokeThickness;
                }
            }
        }

        /// <summary>
        /// 指针填充色
        /// </summary>
        private Brush handFill = new SolidColorBrush(Colors.White);
        /// <summary>
        /// 获取或设置指针填充色
        /// </summary>
        public Brush HandFill
        {
            get { return handFill; }
            set
            {
                handFill = value;
                handPolygon.Fill = handFill;
            }
        }

        /// <summary>
        /// 指针透明度
        /// </summary>
        private double handOpacity = 1;
        /// <summary>
        /// 获取或设置指针透明度
        /// </summary>
        public double HandOpacity
        {
            get { return handOpacity; }
            set
            {
                handOpacity = value;
                handPolygon.Opacity = handOpacity;
            }
        }

        /// <summary>
        /// 构造器
        /// </summary>
        public RadarChart()
        {
            InitializeComponent();
            //初始化托盘样式
            dialEllipse.StrokeThickness = dialStrokeThickness;
            dialEllipse.Stroke = dialStroke;
            dialEllipse.Fill = dialFill;
            for (int i = 0; i < layerNumber; i++)
            {
                Polygon polygon = new Polygon();
                polygon.StrokeThickness = dialStrokeThickness;
                polygon.Stroke = dialStroke;
                dialPolygons.Add(polygon);
            }
            //初始化指针样式
            handPolygon.Fill = handFill;
            handPolygon.Opacity = handOpacity;
            handPolygon.StrokeThickness = 0;
            //根据控件实际大小缩放雷达图
            canvasRadius = Math.Min(canvas.Height, canvas.Width) / 2;
            textRadius = canvasRadius * 1 / 6;
            dialRadius = canvasRadius * 4 / 6;
            totalRadius = textRadius + dialRadius;
            textRadius *= 0.8; //实际文本半径偏小
        }

        #endregion

        #region 绘图接口

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            drawRadar();
        }

        private void drawPolygonArea()
        {
            //外圆形
            dialEllipse.Width = canvasRadius * 2;
            dialEllipse.Height = canvasRadius * 2;
            canvas.Children.Add(dialEllipse);
            //内多边形
            Point point = new Point();
            for (int dial = 0; dial < dialPolygons.Count; dial++)
            {
                Polygon polygon = dialPolygons[dial];
                polygon.Points.Clear();
                for (int i = 0; i < attributes.Count; i++)
                {
                    double radius = dialRadius * (1 - (double)dial / layerNumber);
                    point.X = -radius * Math.Sin(i * degree * Math.PI / 180);
                    point.Y = -radius * Math.Cos(i * degree * Math.PI / 180);
                    dialPolygons[dial].Points.Add(point);
                }
                canvas.Children.Add(polygon);
                Canvas.SetLeft(polygon, canvasRadius);
                Canvas.SetTop(polygon, canvasRadius);
            }
        }

        private void drawTextArea()
        {
            textCircles.Clear();
            textBlocks.Clear();
            Point point = new Point();
            for (int i = 0; i < attributes.Count; i++)
            {
                point.X = -totalRadius * Math.Sin(i * degree * Math.PI / 180);
                point.Y = -totalRadius * Math.Cos(i * degree * Math.PI / 180);
                //文本边框
                Ellipse circle = new Ellipse();
                circle.StrokeThickness = textStrokeThickness;
                circle.Stroke = textStroke;
                circle.Width = textRadius * 2;
                circle.Height = textRadius * 2;
                canvas.Children.Add(circle);
                Canvas.SetLeft(circle, canvasRadius + point.X - textRadius);
                Canvas.SetTop(circle, canvasRadius + point.Y - textRadius);
                //文本框
                TextBlock textBlock = new TextBlock();
                textBlock.Text = attributes[i].Name;
                textBlock.FontSize = textRadius >= 1 ? textRadius : 1;
                textBlock.Foreground = textFill;
                textBlock.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                textBlock.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                //文本边框
                Border border = new Border();
                border.Width = circle.Width;
                border.Height = circle.Height;
                border.Child = textBlock;
                canvas.Children.Add(border);
                Canvas.SetLeft(border, canvasRadius + point.X - textRadius);
                Canvas.SetTop(border, canvasRadius + point.Y - textRadius);
                //添加到队列中
                textCircles.Add(circle);
                textBlocks.Add(textBlock);
            }
        }

        private void drawRadarArea()
        {
            Point point = new Point();
            handPolygon.Points.Clear();
            for (int i = 0; i < attributes.Count; i++)
            {
                double percent = attributes[i].Percent;
                point.X = -dialRadius * percent * Math.Sin(i * degree * Math.PI / 180);
                point.Y = -dialRadius * percent * Math.Cos(i * degree * Math.PI / 180);
                handPolygon.Points.Add(point);
            }
            canvas.Children.Add(handPolygon);
            Canvas.SetLeft(handPolygon, canvasRadius);
            Canvas.SetTop(handPolygon, canvasRadius);
        }

        private void drawRadar()
        {
            degree = -360 / attributeCount;
            //初始化雷达属性
            if (attributes.Count != attributeCount)
            {
                attributes.Clear();
                for (int i = 0; i < attributeCount; i++)
                {
                    RadarAttribute attribute = new RadarAttribute(string.Format("A{0}", i + 1), 0.5);
                    attribute.NameChangeEvent += new NameChangeDelegate(attribute_NameChangeEvent);
                    attribute.PercentChangeEvent += new PercentChangeDelegate(attribute_PercentChangeEvent);
                    attributes.Add(attribute);
                }
            }
            //重绘雷达图
            canvas.Children.Clear();
            drawPolygonArea();
            drawTextArea();
            drawRadarArea();
        }

        private void attribute_NameChangeEvent(RadarAttribute attribute)
        {
            int index = attributes.IndexOf(attribute);
            textBlocks[index].Text = attribute.Name;
        }

        private void attribute_PercentChangeEvent(RadarAttribute attribute)
        {
            int index = attributes.IndexOf(attribute);
            Point point = new Point();
            point.X = -dialRadius * attribute.Percent * Math.Sin(index * degree * Math.PI / 180);
            point.Y = -dialRadius * attribute.Percent * Math.Cos(index * degree * Math.PI / 180);
            handPolygon.Points[index] = point;
        }

        #endregion
    }
}
