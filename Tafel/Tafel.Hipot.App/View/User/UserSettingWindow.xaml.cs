using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using TengDa.Wpf;

namespace Tafel.Hipot.App
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UserSettingWindow : Window
    {
        public UserSettingWindow()
        {
            InitializeComponent();
            this.DataContext = AppCurrent.AppViewModel;
            myMediaTimeline.Source = new Uri(AppDomain.CurrentDomain.BaseDirectory + "Videos/LoginHead.mp4");
        }
        private void OnLogin(object sender, ExecutedRoutedEventArgs e)
        {
            if (UserViewModel.Login(this.LoginUserNameCombobox.Text, this.LoginUserPasswordBox.Password))
            {

                //Tip.Alert("成功登录");
                AppCurrent.AppViewModel.UserName = Current.User.Name;
                AppCurrent.AppViewModel.UserGroupName = Current.Role.Name;
                AppCurrent.AppViewModel.UserProfilePicture = Current.User.ProfilePicture;
                AppCurrent.AppViewModel.UserNumber = Current.User.Number;
                AppCurrent.AppViewModel.UserPhoneNumber = Current.User.PhoneNumber;
                AppCurrent.AppViewModel.UserEmail = Current.User.Email;

                AppCurrent.AppViewModel.CurrentUserNameTip = Current.User.Name;
                Current.ShowTips(Current.User.Name + "成功登录");

                btnLogin.Content = "正在登录...";

                Thread t = new Thread(() =>
                {
                    Thread.Sleep(2000);
                    Dispatcher.Invoke(new Action(() =>
                    {
                        //登录成功，关闭窗口
                        this.Close();
                        AppCurrent.AppViewModel.IsLogin = true;
                        AppCurrent.AppViewModel.MainWindowsBackstageIsOpen = false;
                    }));
                });
                t.Start();

            }
        }
    }
}
