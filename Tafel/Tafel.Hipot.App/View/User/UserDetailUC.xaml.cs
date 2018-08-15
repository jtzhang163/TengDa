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
            this.DataContext = Current.App;

        }

        private void hyberlinkLogout_Click(object sender, RoutedEventArgs e)
        {
            if (TengDa.Wpf.Current.IsRunning)
            {
                Tip.Alert("系统正在运行，请先停止！");
                return;
            }

            if (User.Logout())
            {
                new LoginWindow().Show();
                Current.MainWindow.Close();
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
