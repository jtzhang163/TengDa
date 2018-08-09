using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using TengDa.Encrypt;
using TengDa.Wpf;

namespace Tafel.Hipot.App
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            this.DataContext = AppCurrent.AppViewModel;
            myMediaTimeline.Source = new Uri(AppDomain.CurrentDomain.BaseDirectory + "Videos/LoginHead.mp4");
            if (AppCurrent.Option.IsRememberMe)
            {
                var user = Context.UserContext.Users.SingleOrDefault(u => u.Id == AppCurrent.Option.LastLoginUserId);
                if (user != null)
                {
                    this.loginUserNameTextBox.Text = user.Name;
                    this.loginPasswordBox.Password = Base64.DecodeBase64(user.Password);
                }
            }
        }
        private void OnLogin(object sender, ExecutedRoutedEventArgs e)
        {
            if (UserViewModel.Login(this.loginUserNameTextBox.Text, this.loginPasswordBox.Password))
            {

                AppCurrent.Option.LastLoginUserId = Current.User.Id;
                Current.ShowTips(Current.User.Name + "成功登录");
                btnLogin.Content = "正在登录...";
                Thread t = new Thread(() =>
                {
                    Thread.Sleep(2000);
                    Dispatcher.Invoke(new Action(() =>
                    {
                        //登录成功，关闭窗口          
                        AppCurrent.AppViewModel.MainWindowsBackstageIsOpen = false;
                        new MainWindow().Show();
                        this.Close();
                    }));
                });
                t.Start();

            }
        }

        private void BtnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
