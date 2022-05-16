using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSyncApp.Controls.Convers
{
    public class TreeViewConver
    {
    }

    public class TreeViewModel
    {

        public bool IsGrouping { get { return Children != null && Children.Count > 0; } }
        /// <summary>
        /// 明细行
        /// </summary>
        public ObservableCollection<TreeViewModel> Children { get; set; }
        /// <summary>
        /// 组名称
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string SurName { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 同步日期
        /// </summary>
        public string Date { get; set; }
    }
}
