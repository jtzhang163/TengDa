using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TengDa.Wpf;

namespace Zopoise.Scada.App.View
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
            //gridUserDetail.DataContext = Current.User;
        }


        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow window = new LoginWindow();
            window.ShowDialog();
        }

        private void hyberlinkLogout_Click(object sender, RoutedEventArgs e)
        {
            if (User.Logout())
            {
                //Tip.Alert("成功登录");
                AppCurrent.AppViewModel.UserName = string.Empty;
                AppCurrent.AppViewModel.UserGroupName = string.Empty;
                AppCurrent.AppViewModel.UserProfilePicture = string.Empty;
                AppCurrent.AppViewModel.UserNumber = string.Empty;
                AppCurrent.AppViewModel.UserPhoneNumber = string.Empty;
                AppCurrent.AppViewModel.UserEmail = string.Empty;

                AppCurrent.AppViewModel.CurrentUserNameTip = string.Empty;
                AppCurrent.AppViewModel.ShowTips(Current.User.Name + "成功注销");

                //Thread t = new Thread(() =>
                //{
                //  Thread.Sleep(2000);
                //  Dispatcher.Invoke(new Action(() =>
                //  {
                //    //登录成功，关闭窗口
                //    this.Close();
                AppCurrent.AppViewModel.IsLogin = false;
                AppCurrent.AppViewModel.MainWindowsBackstageIsOpen = true;
                //  }));
                //});
                //t.Start();

            }
        }

        private void hyberlinkSetting_Click(object sender, RoutedEventArgs e)
        {

        }

        private void hyberlinkManage_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    public class ShowLoginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Hidden : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ShowUserDetailConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
