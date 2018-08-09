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
            if (UserViewModel.Logout())
            {
                AppCurrent.AppViewModel.UserName = string.Empty;
                AppCurrent.AppViewModel.UserGroupName = string.Empty;
                AppCurrent.AppViewModel.UserProfilePicture = string.Empty;
                AppCurrent.AppViewModel.UserNumber = string.Empty;
                AppCurrent.AppViewModel.UserPhoneNumber = string.Empty;
                AppCurrent.AppViewModel.UserEmail = string.Empty;

                AppCurrent.AppViewModel.CurrentUserNameTip = string.Empty;
                Current.ShowTips(Current.User.Name + "成功注销");

                AppCurrent.AppViewModel.IsLogin = false;

                LoginWindow window = new LoginWindow();
                window.Show();
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
