using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace MainApp
{
    public class BaseWindow : Window
    {
        public BaseWindow() : base()
        {
             //Icon = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/cloud-256.ico"));


            // 鼠标左键拖动
            MouseLeftButtonDown += (sender, e) =>
            {
                try
                {
                    this.DragMove();

                }
                catch (Exception)
                {
                    return;
                }
                int minNum = 10;

                //当前窗体位置
                //this.Left  his.Top 

                //左
                if (this.Left < minNum)
                    DoMove((int)Left, minNum, "Left");
                //上
                if (this.Top < minNum)
                    DoMove((int)Top, minNum, "Top");
                //右                
                if (SystemParameters.PrimaryScreenWidth - Width - minNum < this.Left)
                    //当前屏幕分辨率  
                    DoMove((int)Left, (int)(SystemParameters.PrimaryScreenWidth - Width - minNum), "Left");
                //下
                if (SystemParameters.PrimaryScreenHeight - Height - minNum < this.Top)
                    //当前屏幕分辨率  
                    DoMove((int)Top, (int)(SystemParameters.PrimaryScreenHeight - Height - minNum), "Top");

                /// <summary>
                /// 移动动画
                /// </summary>
                /// <param name="from"></param>
                /// <param name="to"></param>
                /// <param name="path"></param>
                void DoMove(int from, int to, string path)
                {
                    var sb = new Storyboard();
                    var da = new DoubleAnimation()
                    {
                        From = from,
                        To = to,
                        Duration = TimeSpan.FromSeconds(0.2)
                    };
                    Storyboard.SetTarget(da, this);
                    Storyboard.SetTargetProperty(da, new PropertyPath($"({path})"));
                    sb.Children.Add(da);
                    sb.Completed += (comO, ComE) => { sb.Remove(this); };
                    sb.Begin(this, true);
                }
            };
        }

        void StlyInit()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;
            ResizeMode = ResizeMode.NoResize;
            Height = 524; Width = 340;
            MinHeight = 524; MinWidth = 340;
            MaxHeight = 524; MaxWidth = 340;
        }
    }
}
