using FIleSyncData;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Linq;
using FileSyncApp.Controls.Convers;
using MainApp.Tools;

namespace MainApp.Views
{
    /// <summary>
    /// UcLogs.xaml 的交互逻辑
    /// </summary>
    public partial class UcLog : UserControl
    {
        public UcLog()
        {
            InitializeComponent();

            InitData(CbxError.IsChecked == true);
        }


        //初始化显示和历史记录
        private void InitData(bool isFailure )
        {
            var list = new FIleSyncData.SyncLogDAL().Query(isFailure);
            var OrgList = new ObservableCollection<TreeViewModel>();

            if (list != null)
            {
                foreach (var item in list.GroupBy(m => m.LogDate))
                {
                    var model = new TreeViewModel()
                    {
                        //IsGrouping = true,
                        DisplayName = $"同步于{item.Key}",
                        Children = new ObservableCollection<TreeViewModel>()
                    };
                    foreach (var i in item)
                    {
                        model.Children.Add(new TreeViewModel()
                        {
                            SurName = i.Extension.ToLower(),
                            Name = i.Name,
                            Path = i.Path,
                            Date = i.LogTime.ToString("T")
                        }); ;
                    }

                    OrgList.Add(model);
                }
            }
            TreeViewOrg.ItemsSource = OrgList;// TreeviewDataInit.Instance.OrgList;

            if (OrgList.Count == 0)
            {
                StackPanel1.Visibility = Visibility.Visible;
                TreeViewOrg.Visibility = Visibility.Hidden;
            }
            else
            {
                StackPanel1.Visibility = Visibility.Hidden;
                TreeViewOrg.Visibility = Visibility.Visible;
            }
        }

        public SyncLogDAL dal { get; set; }

        /// <summary>
        /// 清空历史记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnHistory(object sender, RoutedEventArgs e)
        {
            StackPanel1.Visibility = Visibility.Visible;
            TreeViewOrg.Visibility = Visibility.Hidden;

            LblMsg.Content = $"{(CbxError.IsChecked == true ? "错误" : "历史")}历史记录为空";

            dal.Delete();

        }

        /// <summary>
        /// 显示错误记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxError_Click(object sender, RoutedEventArgs e)
        {
            LblMsg.Content = $"{(CbxError.IsChecked == true ? "错误" : "历史")}历史记录为空";

            InitData(CbxError.IsChecked==true);
        }

        string GetExtension(string path, int num = 0)
        {
            int index = path.LastIndexOf(".");
            if (index < 0)
                return string.Empty;
            string extenstion = path.Substring(index, path.Length - index);

            if (num < 1)
                return extenstion;
            if (extenstion.Length < num)
                return extenstion;

            return extenstion.Substring(0, num);
        }
    
    
        
    }

}
