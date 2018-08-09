using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TengDa.Wpf;

namespace Tafel.Hipot.App.View
{
    /// <summary>
    /// AboutUC.xaml 的交互逻辑
    /// </summary>
    public partial class UserDetailUC : UserControl
    {
        public UserDetailUC()
        {
            InitializeComponent();
            this.DataContext = AppCurrent.AppViewModel;

        }

        private void hyberlinkLogout_Click(object sender, RoutedEventArgs e)
        {
            if (Current.IsRunning)
            {
                Tip.Alert("系统正在运行，请先停止！");
                return;
            }

            if (UserViewModel.Logout())
            {

                Current.ShowTips(Current.User.Name + "成功注销");
                new LoginWindow().Show();
                AppCurrent.MainWindow.Close();
            }
        }

        private void hyberlinkSetting_Click(object sender, RoutedEventArgs e)
        {

        }

        private void hyberlinkManage_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
