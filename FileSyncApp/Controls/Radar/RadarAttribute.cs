using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MainApp.Controls.Radar
{
    internal delegate void NameChangeDelegate(RadarAttribute attribute);
    internal delegate void PercentChangeDelegate(RadarAttribute attribute);

    public class RadarAttribute
    {
        /// <summary>
        /// 属性名称更改事件
        /// </summary>
        internal event NameChangeDelegate NameChangeEvent = null;
        /// <summary>
        /// 属性百分比更改事件
        /// </summary>
        internal event PercentChangeDelegate PercentChangeEvent = null;

        /// <summary>
        /// 属性名称
        /// </summary>
        private string name = string.Empty;
        /// <summary>
        /// 获取或设置属性名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                name = value.Length > 2 ? value.Substring(0, 2) : value;
                if (NameChangeEvent != null)
                {
                    NameChangeEvent(this);
                }
            }
        }

        /// <summary>
        /// 属性百分比
        /// </summary>
        private double percent = 0.0;
        /// <summary>
        /// 获取或设置属性百分比
        /// </summary>
        public double Percent
        {
            get { return percent; }
            set
            {
                if (value >= RadarChart.MaxPercent)
                {
                    percent = RadarChart.MaxPercent;
                }
                else if (value <= RadarChart.MinPercent)
                {
                    percent = RadarChart.MinPercent;
                }
                else
                {
                    percent = value;
                }
                if (PercentChangeEvent != null)
                {
                    PercentChangeEvent(this);
                }
            }
        }

        /// <summary>
        /// 构造器
        /// </summary>
        private RadarAttribute()
        { 
        }

        /// <summary>
        /// 带参数的构造器
        /// </summary>
        /// <param name="text">属性名称</param>
        /// <param name="percent">属性百分比</param>
        internal RadarAttribute(string name, double percent)
        {
            Name = name;
            Percent = percent;
        }
    }
}
