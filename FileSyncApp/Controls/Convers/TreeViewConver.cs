using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Controls;


namespace FileSyncApp.Controls.Convers
{
    using MainApp.Tools;

    public class TreeViewConver
    {
    }

    public partial class TreeViewEvent : System.Windows.ResourceDictionary
    {
        /// <summary>
        /// 单击path打开资源管理器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TreeViewEvent_TextBlock_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var txt = sender as TextBlock;
            if (txt == null) return;
            if (string.IsNullOrEmpty(txt.Text) ||!Directory.Exists(txt.Text)) return;

            Open.OpenDir(txt.Text);
        }
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
