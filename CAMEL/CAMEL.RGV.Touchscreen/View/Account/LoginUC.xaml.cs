using CAMEL.RGV.Touchscreen.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CAMEL.RGV.Touchscreen.View
{
    /// <summary>
    /// LoginUC.xaml 的交互逻辑
    /// </summary>
    public partial class LoginUC : UserControl
    {
        public LoginUC()
        {
            InitializeComponent();

            this.password.Password = "admin";
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var username = this.username.Text.Trim();
            var password = this.password.Password.Trim();
           
            if (username == "admin" && password == "admin")
            {    
                
                Speech.Voice("登录成功");
                Current.Option.Username = username;

                Window parentWindow = Window.GetWindow(this);
                Type type = parentWindow.GetType();
                MethodInfo mi = type.GetMethod("LoginSuccessInvoke");
                mi.Invoke(parentWindow, new object[] { });
            }
            else
            {
                Speech.Voice("登录失败");
                lbTip.Content = "用户名或密码错误";
                lbTip.Visibility = Visibility.Visible;
            }
        }

        private void Input_GotFocus(object sender, RoutedEventArgs e)
        {
            lbTip.Visibility = Visibility.Hidden;
        }
    }
}
