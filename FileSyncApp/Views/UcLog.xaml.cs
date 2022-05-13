using FIleSyncData;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Linq;

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

            InitData();
        }

        //初始化显示和历史记录
        private void InitData()
        {
            var list=new FIleSyncData.SyncLogDAL().Query();
            var OrgList = new ObservableCollection<OrgModel>();

            if(list!=null)
            foreach (var item in list.GroupBy(m => m.LogDate))
            {
                var model = new OrgModel()
                {
                    //IsGrouping = true,
                    DisplayName = $"同步于{item.Key}",
                    Children = new ObservableCollection<OrgModel>()
                };
                foreach (var i in item)
               {
                        model.Children.Add(new OrgModel()
                        {
                            SurName = i.Extension.ToLower(),
                            Name = i.Name,
                            Info = i.Path,
                            Count = i.LogTime.ToString("T")
                        }); ;
                }             

                OrgList.Add(model);
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
        }

        string GetExtension(string path, int num = 0)
        {
            int index = path.LastIndexOf(".");
            if (index < 0)
                return string.Empty;
            string extenstion = path.Substring(index, path.Length - index );

            if (num < 1)
                return extenstion;
            if (extenstion.Length < num)
                return extenstion;

            return extenstion.Substring(0, num);
        }
    }


    public class TreeviewDataInit
    {
        private static TreeviewDataInit dataInit;

        public static TreeviewDataInit Instance
        {
            get
            {
                if (dataInit == null)
                    dataInit = new TreeviewDataInit();
                return dataInit;
            }
        }
        private TreeviewDataInit()
        {
            OrgList = new ObservableCollection<OrgModel>()
            {
                new OrgModel()
                {
                    //IsGrouping = true,
                    DisplayName = "同步于2022/4/8 14:59:38",
                    Children = new ObservableCollection<OrgModel>()
                    {
                        new OrgModel(){
                            //IsGrouping=false,
                            SurName="刘",
                            Name="刘棒",
                            Info="我要走向天空！",
                            Count="1"
                        },
                            new OrgModel(){
                            //IsGrouping=false,
                            SurName="刘",
                            Name="刘棒",
                            Info="我要走向天空！",
                            Count="1"
                        },
                            new OrgModel(){
                            //IsGrouping=false,
                            SurName="刘",
                            Name="刘棒",
                            Info="我要走向天空！",
                            Count="1"

                    },
                            new OrgModel(){
                            //IsGrouping=false,
                            SurName="刘",
                            Name="刘棒",
                            Info="我要走向天空！",
                            Count="1"
                    },
                },
                },
                new OrgModel()
                {
                    //IsGrouping = true,
                    DisplayName = "同步于2022/4/8 14:59:38",
                    Children = new ObservableCollection<OrgModel>()
                    {
                        new OrgModel(){
                            //IsGrouping=false,
                            SurName="刘",
                            Name="刘棒",
                            Info="我要走向天空！",
                            Count="1"
                        },
                            new OrgModel(){
                            //IsGrouping=false,
                            SurName="刘",
                            Name="刘棒",
                            Info="我要走向天空！",
                            Count="1"
                        },
                            new OrgModel(){
                            //IsGrouping=false,
                            SurName="刘",
                            Name="刘棒",
                            Info="我要走向天空！",
                            Count="1"

                    },
                            new OrgModel(){
                            //IsGrouping=false,
                            SurName="刘",
                            Name="刘棒",
                            Info="我要走向天空！",
                            Count="1"
                    },
                },
                }
            };
        }

        public ObservableCollection<OrgModel> OrgList { get; set; }

    }

    public class OrgModel
    {
        public bool IsGrouping { get { return Children != null && Children.Count > 0; } }
        public ObservableCollection<OrgModel> Children { get; set; }
        public string DisplayName { get; set; }
        public string SurName { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public string Count { get; set; }
    }

    public class BoolToVisible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
