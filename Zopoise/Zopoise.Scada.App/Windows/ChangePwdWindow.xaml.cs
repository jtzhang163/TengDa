using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TengDa.Wpf;

namespace Zopoise.Scada.App
{
    /// <summary>
    /// ChangePwdWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ChangePwdWindow : Window
    {
        public ChangePwdWindow()
        {
            InitializeComponent();
            this.DataContext = Current.App;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var originPwsInput = this.OriginPwsInput.Password.Trim();
            var newPwsInput = this.NewPwsInput.Password.Trim();
            var newPwsReInput = this.NewPwsReInput.Password.Trim();
            if (string.IsNullOrEmpty(originPwsInput))
            {
                Tip.Alert("请输入当前密码！");
                return;
            }
            if (string.IsNullOrEmpty(newPwsInput))
            {
                Tip.Alert("请输入新密码！");
                return;
            }
            if (string.IsNullOrEmpty(newPwsReInput))
            {
                Tip.Alert("请重新输入新密码！");
                return;
            }

            if (newPwsInput != newPwsReInput)
            {
                Tip.Alert("新密码两次输入不一致！");
                return;
            }

            if (newPwsInput == originPwsInput)
            {
                Tip.Alert("输入的新密码和原密码相同！");
                return;
            }

            if (AppCurrent.User.Password != TengDa.Encrypt.Base64.EncodeBase64(originPwsInput))
            {
                Tip.Alert("当前密码输入错误！");
                return;
            }

            AppCurrent.User.Password = TengDa.Encrypt.Base64.EncodeBase64(newPwsInput);

            try
            {
                TengDa.Wpf.Context.UserContext.SaveChanges();
                OperationHelper.ShowTips("修改密码成功！",true);
            }
            catch(Exception ex)
            {
                Error.Alert(ex);
            }

        }
    }
}
