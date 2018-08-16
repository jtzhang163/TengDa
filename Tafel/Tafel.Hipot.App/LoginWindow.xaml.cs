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

            new DbInitializer().Initialize();

            this.DataContext = Current.App;
            myMediaTimeline.Source = new Uri(AppDomain.CurrentDomain.BaseDirectory + "Videos/LoginHead.mp4");
            if (Current.Option.IsRememberMe)
            {
                var user = TengDa.Wpf.Context.UserContext.Users.SingleOrDefault(u => u.Id == Current.Option.LastLoginUserId);
                if (user != null)
                {
                    this.loginUserNameTextBox.Text = user.Name;
                    this.loginPasswordBox.Password = Base64.DecodeBase64(user.Password);
                }
            }
        }
        private void OnLogin(object sender, ExecutedRoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.loginUserNameTextBox.Text))
            {
                Tip.Alert("请输入用户名");
                return;
            }
            if (string.IsNullOrEmpty(this.loginPasswordBox.Text))
            {
                Tip.Alert("请输入密码");
                return;
            }

            string msg = string.Empty;
            if (Current.Option.IsMesLogin)
            {
                //MES登录              
                if (MES.Login(this.loginUserNameTextBox.Text, this.loginPasswordBox.Password, out msg))
                {
                    AfterLogin();
                }
                else
                {
                    Tip.Alert(msg);
                }
            }
            else
            {
                //普通登录
                if (User.Login(this.loginUserNameTextBox.Text, this.loginPasswordBox.Password, out msg))
                {
                    AfterLogin();
                }
                else
                {
                    Tip.Alert(msg);
                }
            }

        }

        private void AfterLogin()
        {
            Current.Option.LastLoginUserId = AppCurrent.User.Id;
            OperationHelper.ShowTips(AppCurrent.User.Name + "成功登录");
            btnLogin.Content = "正在登录...";
            Thread t = new Thread(() =>
            {
                Thread.Sleep(2000);
                Dispatcher.Invoke(new Action(() =>
                {
                    //登录成功，关闭窗口          
                    Current.App.MainWindowsBackstageIsOpen = false;
                    new MainWindow().Show();
                    this.Close();
                }));
            });
            t.Start();
        }

        private void BtnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
