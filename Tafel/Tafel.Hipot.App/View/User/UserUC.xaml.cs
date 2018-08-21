using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Controls;
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
            OpenFileDialog op = new Microsoft.Win32.OpenFileDialog();
            op.Title = "选择新头像的图片";
            op.RestoreDirectory = true;          
            op.Filter = "图片文件|*.jpg;*.jpeg;*.bmp;*.png;*.gif";
            op.ShowDialog();

            var fromFlieName = op.FileName;

            if (string.IsNullOrEmpty(fromFlieName))
            {
                //未选择文件
                return;
            }

            var extension = fromFlieName.Substring(fromFlieName.LastIndexOf('.'));
            var toFlieName = string.Format("/Images/{0}{1}", 
                TengDa.Encrypt.MD5.MD5Encrypt(DateTime.Now.ToString()),//加密后的文件名
                extension//.文件扩展名
                );

            try
            {
                File.Copy(fromFlieName, System.Environment.CurrentDirectory + toFlieName, true);
                Current.App.UserProfilePicture = toFlieName;
                TengDa.Wpf.Context.UserContext.SaveChanges();
                OperationHelper.ShowTips("更新头像成功",true);
            }
            catch(Exception ex)
            {
                Error.Alert(ex);
            } 

        }

        private void ChangePwdHyberlink_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var win = new ChangePwdWindow();
            win.ShowDialog();
        }
    }
}
