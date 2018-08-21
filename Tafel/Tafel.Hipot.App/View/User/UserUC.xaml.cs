﻿using System.Windows.Controls;
using TengDa.Wpf;

namespace Tafel.Hipot.App.View
{
    /// <summary>
    /// AboutUC.xaml 的交互逻辑
    /// </summary>
    public partial class UserUC : UserControl
    {
        public UserUC()
        {
            InitializeComponent();
        }

        private void LogoutHyberlink_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (AppCurrent.IsRunning)
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

        private void ChangeProfileHyberlink_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void ChangePwdHyberlink_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var win = new ChangePwdWindow();
            win.ShowDialog();
        }
    }
}
