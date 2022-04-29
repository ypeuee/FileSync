using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MainApp.Controls.Radar
{
    internal delegate void SignalCollectionDelegate(RadarSignal signal);

    public class RadarSignalCollection : IEnumerable
    {
        /// <summary>
        /// 信号源添加事件
        /// </summary>
        internal SignalCollectionDelegate SignalAddEvent = null;

        /// <summary>
        /// 信号源删除事件
        /// </summary>
        internal SignalCollectionDelegate SignalDelEvent = null;

        /// <summary>
        /// 信号源列表
        /// </summary>
        private List<RadarSignal> signals = new List<RadarSignal>();

        /// <summary>
        /// 构造器
        /// </summary>
        private RadarSignalCollection()
        {
        }

        /// <summary>
        /// 带参数的构造器
        /// </summary>
        /// <param name="signalAdd">信号源添加事件</param>
        /// <param name="signalDel">信号源删除事件</param>
        internal RadarSignalCollection(SignalCollectionDelegate signalAdd, SignalCollectionDelegate signalDel)
        {
            SignalAddEvent = signalAdd;
            SignalDelEvent = signalDel;
        }

        /// <summary>
        /// 获取信号源实例
        /// </summary>
        /// <param name="index">信号源索引</param>
        /// <returns>返回信号源实例</returns>
        public RadarSignal this[int index]
        {
            get { return signals[index]; }
        }

        /// <summary>
        /// 获取信号源个数
        /// </summary>
        public int Count
        {
            get { return signals.Count; }
        }

        /// <summary>
        /// 添加信号源
        /// </summary>
        /// <param name="signal">信号源实例</param>
        public void Add(RadarSignal signal)
        {
            signals.Add(signal);
            if (SignalAddEvent != null)
            {
                SignalAddEvent(signal);
            }
        }

        /// <summary>
        /// 删除信号源
        /// </summary>
        /// <param name="signal">信号源实例</param>
        public void Remove(RadarSignal signal)
        {
            signals.Remove(signal);
            if (SignalDelEvent != null)
            {
                SignalDelEvent(signal);
            }
        }

        /// <summary>
        /// 删除信号源
        /// </summary>
        /// <param name="index">信号源索引</param>
        public void RemoveAt(int index)
        {
            RadarSignal signal = signals[index];
            signals.RemoveAt(index);
            if (SignalDelEvent != null)
            {
                SignalDelEvent(signal);
            }
        }

        /// <summary>
        /// 清空信号源
        /// </summary>
        public void Clear()
        {
            if (SignalDelEvent != null)
            {
                foreach (RadarSignal signal in signals)
                {
                    SignalDelEvent(signal);
                }
            }
            signals.Clear();
        }

        /// <summary>
        /// 获取循环访问 RadarSignalCollection 的枚举器
        /// </summary>
        /// <returns>返回循环访问 RadarSignalCollection 的枚举器</returns>
        public IEnumerator GetEnumerator()
        {
            return signals.GetEnumerator();
        }
    }
}
