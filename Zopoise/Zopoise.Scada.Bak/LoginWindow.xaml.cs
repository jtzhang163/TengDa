using System;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TengDa.Encrypt;
using TengDa.Wpf;

namespace Zopoise.Scada.Bak
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
                    this.userNameTextBox.Text = user.Name;
                    this.passwordBox.Password = Base64.DecodeBase64(user.Password);
                    this.ProfileImage.Source = new BitmapImage(new Uri(user.ProfilePicture, UriKind.Relative)); ;
                }
            }

            //MES.GetInfo();

        }
        private void btnLoginOrRegister_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.userNameTextBox.Text))
            {
                Tip.Alert("请输入用户名");
                return;
            }

            if (string.IsNullOrEmpty(this.passwordBox.Text))
            {
                Tip.Alert("请输入密码");
                return;
            }

            if (!Current.App.IsLoginWindow && string.IsNullOrEmpty(this.confirmPasswordBox.Text))
            {
                Tip.Alert("请输入确认密码");
                return;
            }

            if (!Current.App.IsLoginWindow && this.confirmPasswordBox.Text != this.passwordBox.Text)
            {
                Tip.Alert("两次输入密码不相同");
                return;
            }

            string msg = string.Empty;
            //登录
            if (Current.App.IsLoginWindow)
            {
                //if (Current.Option.IsMesLogin)
                //{
                //    if (!Current.Mes.IsPingSuccess)
                //    {
                //        Error.Alert("无法连接至MES服务器，登录失败！");
                //        return;
                //    }
                //    //MES登录
                //    if (MES.Login(this.userNameTextBox.Text, this.passwordBox.Password, out msg))
                //    {
                //        AfterLogin();
                //    }
                //    else
                //    {
                //        Tip.Alert(msg);
                //    }
                //}
                //else
                //{
                //普通登录
                if (User.Login(this.userNameTextBox.Text, this.passwordBox.Password, out msg))
                {
                    AfterLogin();
                }
                else
                {
                    Tip.Alert(msg);
                }
                //}
            }
            else //注册
            {
                if (User.Register(this.userNameTextBox.Text, this.passwordBox.Password, out msg))
                {
                    AfterRegister();
                }
                else
                {
                    Tip.Alert(msg);
                }
            }


        }

        private void AfterRegister()
        {
            Current.Option.LastLoginUserId = AppCurrent.User.Id;
            OperationHelper.ShowTips(this.userNameTextBox.Text + "成功注册");
            btnLoginOrRegister.Content = "正在注册...";
            Thread t = new Thread(() =>
            {
                Thread.Sleep(2000);
                Dispatcher.Invoke(new Action(() =>
                {
                    //登录成功，关闭窗口          
                    Current.App.IsLoginWindow = true;
                    btnLoginOrRegister.Content = "登 录";
                    Tip.Alert(this.userNameTextBox.Text + "成功注册，请让管理员审核后登录");
                }));
            });
            t.Start();
        }

        private void AfterLogin()
        {
            OperationHelper.ShowTips(AppCurrent.User.Name + "成功登录");
            btnLoginOrRegister.Content = "正在登录...";
            Thread t = new Thread(() =>
            {
                Thread.Sleep(2000);
                Dispatcher.Invoke(new Action(() =>
                {
                    //登录成功，关闭窗口          
                    Current.App.MainWindowsBackstageIsOpen = false;
                    Current.Option.LastLoginUserId = AppCurrent.User.Id;
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


        private void loginHyperlink_Click(object sender, RoutedEventArgs e)
        {
            Current.App.IsLoginWindow = true;
        }

        private void registerHyperlink_Click(object sender, RoutedEventArgs e)
        {
            Current.App.IsLoginWindow = false;
        }

        private void userNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var user = TengDa.Wpf.Context.UserContext.Users.FirstOrDefault(u => u.Name == this.userNameTextBox.Text.Trim());
            if (user != null)
            {
                this.ProfileImage.Source = new BitmapImage(new Uri(user.ProfilePicture, UriKind.Relative)); ;
            }
        }
    }
}

