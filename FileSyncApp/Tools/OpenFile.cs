using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Tools
{
    public static class Open
    {
        /*
语法
EXPLORER.EXE [/n][/e][,/root,<object>][[,/select],<sub object>]
/n: 默认选项，用我的电脑视图为每个选中的item打开一个单独的窗口,  即使该窗口已经被打开。
/e: 使用资源管理器视图。资源管理器视图和Windows 3.x的文件管理器非常相似。
/root,<object>: 指定视图目录根，默认使用桌面作为根目录。
/select,<sub object>: 选中指定对象。如果使用"/select" , 则父目录被打开，并选中指定对象。

例子
一般用法
explorer C:\Windows
打开资源管理器视图并以C:\Windows为目录根浏览
explorer /e,/root,C:\Windows
打开资源管理器视图并选中Calc.exe
explorer /e,/select,c:\windows\system32\calc.exe

注意
/root和/select最好不要同时使用
文件夹参数必须使用反斜杠-\
文件夹参数如果包含空格请用""包裹
        */

        static string explorerPath = @"C:\Windows\explorer.exe";

        /// <summary>
        /// 打开文件夹
        /// </summary>
        /// <param name="path"></param>
        public static void OpenDir(string path)
        {
            //判断操作系统是否为 Linux OSX Windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                System.Diagnostics.Process.Start(explorerPath, path.ReplaceSprit(false).AddQuotation());
            }
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="path"></param>
        public static void OpenFile(string path)
        {   //判断操作系统是否为 Linux OSX Windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // 打开文件夹并选中文件
                System.Diagnostics.Process.Start(explorerPath, "/select," + path.ReplaceSprit(false).AddQuotation());
            }
        }


        /// <summary>
        /// 替换斜杠
        /// </summary>
        public static string ReplaceSprit(this string text, bool positive = true)
        {
            if (positive)
                return text.Replace('\\', '/');
            return text.Replace('/', '\\');
        }

        /// <summary>
        /// 添加双引号
        /// </summary>
        public static string AddQuotation(this string text)
        {
            if (text.StartsWith('"'))
                return text;
            return string.Format("\"{0}\"", text);
        }
    }
}
