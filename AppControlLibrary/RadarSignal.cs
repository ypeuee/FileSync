using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Windows.Input;

namespace RadarControl
{
    public class RadarSignal
    {
        private string toolTip;

        public string ToolTip
        {
            get { return toolTip; }
            set { toolTip = value; }
        }


        /// <summary>
        /// 用户自定义数据
        /// </summary>
        private object tag = null;
        /// <summary>
        /// 获取或设置用户自定义数据
        /// </summary>
        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        /// <summary>
        /// 信号源半径
        /// </summary>
        private double radius = 0;
        /// <summary>
        /// 获取或设置信号源半径
        /// </summary>
        public double Radius
        {
            get { return radius; }
            set
            {
                radius = value <= 0 ? 0 : value;
                onChangeRadius();
            }
        }

        /// <summary>
        /// 填充色
        /// </summary>
        private Brush fill = null;
        /// <summary>
        /// 获取或设置填充色
        /// </summary>
        public Brush Fill
        {
            get { return fill; }
            set 
            {
                fill = value;
                onChangeFill();
            }
        }

        /// <summary>
        /// 信号源离中心距离
        /// </summary>
        private double distance = 0;
        /// <summary>
        /// 获取或设置信号源离中心距离
        /// </summary>
        public double Distance
        {
            get { return distance; }
            set
            {
                distance = value <= RadarMeter.MinDistance ? RadarMeter.MinDistance : value;
                onChangeDistance();
            }
        }

        /// <summary>
        /// 信号源沿中轴线偏移角度
        /// </summary>
        private double angle = 0;
        /// <summary>
        /// 获取或设置信号源沿中轴线偏移角度
        /// </summary>
        public double Angle
        {
            get { return angle; }
            set
            {
                angle = value % 360;
                onChangeAngle();
            }
        }

        /// <summary>
        /// 信号源动画
        /// </summary>
        private DoubleAnimation animation = new DoubleAnimation(1.0, 0.5, new Duration(TimeSpan.FromSeconds(1.0)));
        /// <summary>
        /// 获取信号源动画
        /// </summary>
        internal DoubleAnimation Animation { get { return animation; } }

        /// <summary>
        /// 信号源图形
        /// </summary>
        private Ellipse model = new Ellipse();
        /// <summary>
        /// 获取信号源图形
        /// </summary>
        internal Ellipse Model { get { return model; } }

        /// <summary>
        /// 半径变化处理
        /// </summary>
        private void onChangeRadius()
        {
            model.Width = radius * 2;
            model.Height = radius * 2;
            TransformGroup group = model.RenderTransform as TransformGroup;
            RotateTransform rt = group.Children[1] as RotateTransform;
            rt.CenterX = radius;
            rt.CenterY = radius;
        }

        /// <summary>
        /// 填充色变化处理
        /// </summary>
        private void onChangeFill()
        {
            model.Fill = fill;
        }

        /// <summary>
        /// 距离变化处理
        /// </summary>
        private void onChangeDistance()
        {
            TransformGroup group = model.RenderTransform as TransformGroup;
            TranslateTransform tt = group.Children[0] as TranslateTransform;
            tt.X = 0;
            tt.Y = 0 - distance;
        }

        /// <summary>
        /// 角度变化处理
        /// </summary>
        private void onChangeAngle()
        {
            TransformGroup group = model.RenderTransform as TransformGroup;
            RotateTransform rt = group.Children[1] as RotateTransform;
            rt.Angle = angle;
        }

        /// <summary>
        /// 在鼠标指针进入此元素的边界时发生
        /// </summary>
        private void model_mouseEnter(object sender, MouseEventArgs e)
        {
            if (MouseEnter != null)
            {
                MouseEnter(this, e);
            }
        }

        /// <summary>
        /// 在鼠标指针离开此元素的边界时发生
        /// </summary>
        private void model_mouseLeave(object sender, MouseEventArgs e)
        {
            if (MouseLeave != null)
            {
                MouseLeave(this, e);
            }
        }

        /// <summary>
        /// 在鼠标指针悬停于此元素上并且用户移动该鼠标指针时发生
        /// </summary>
        private void model_mouseMove(object sender, MouseEventArgs e)
        {
            if (MouseMove != null)
            {
                MouseMove(this, e);
            }
        }

        /// <summary>
        /// 在指针悬停于此元素上并且用户按下任意鼠标按钮时发生
        /// </summary>
        private void model_mouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MouseDown != null)
            {
                MouseDown(this, e);
            }
        }

        /// <summary>
        /// 在用户在此元素上释放任意鼠标按钮时发生
        /// </summary>
        private void model_mouseUp(object sender, MouseButtonEventArgs e)
        {
            if (MouseUp != null)
            {
                MouseUp(this, e);
            }
        }

        /// <summary>
        /// 初始化模型
        /// </summary>
        private void initializeModel()
        {
            model.MouseEnter += new MouseEventHandler(model_mouseEnter);
            model.MouseLeave += new MouseEventHandler(model_mouseLeave);
            model.MouseMove += new MouseEventHandler(model_mouseMove);
            model.MouseDown += new MouseButtonEventHandler(model_mouseDown);
            model.MouseUp += new MouseButtonEventHandler(model_mouseUp);
            model.Width = radius * 2;
            model.Height = radius * 2;
            model.Fill = fill;
            TransformGroup group = new TransformGroup();
            group.Children.Add(new TranslateTransform(0, 0 - distance));
            group.Children.Add(new RotateTransform(angle, radius, radius));
            model.RenderTransform = group;
            Storyboard.SetTarget(animation, model);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(Ellipse.Opacity)"));

            model.ToolTip = toolTip;
        }

        /// <summary>
        /// 构造器
        /// </summary>
        private RadarSignal()
        { 
        }

        /// <summary>
        /// 带参数构造器
        /// </summary>
        /// <param name="radius">信号源半径</param>
        /// <param name="fill">信号源填充色</param>
        /// <param name="distance">信号源离中心距离</param>
        /// <param name="angle">信号源沿中轴线偏移角度</param>
        public RadarSignal(double radius, Brush fill, double distance, double angle)
        {
            this.radius = radius;
            this.fill = fill;
            this.distance = distance;
            this.angle = angle;
            initializeModel();
        }

        /// <summary>
        /// 在鼠标指针进入此元素的边界时发生
        /// </summary>
        public event MouseEventHandler MouseEnter = null;

        /// <summary>
        /// 在鼠标指针离开此元素的边界时发生
        /// </summary>
        public event MouseEventHandler MouseLeave = null;

        /// <summary>
        /// 在鼠标指针悬停于此元素上并且用户移动该鼠标指针时发生
        /// </summary>
        public event MouseEventHandler MouseMove = null;

        /// <summary>
        /// 在指针悬停于此元素上并且用户按下任意鼠标按钮时发生
        /// </summary>
        public event MouseButtonEventHandler MouseDown = null;

        /// <summary>
        /// 在用户在此元素上释放任意鼠标按钮时发生
        /// </summary>
        public event MouseButtonEventHandler MouseUp = null;
    }
}
