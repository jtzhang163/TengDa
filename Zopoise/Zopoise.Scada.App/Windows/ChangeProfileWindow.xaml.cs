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
    public partial class ChangeProfileWindow : Window
    {
        public ChangeProfileWindow()
        {
            InitializeComponent();

            for (int i = 0; i < 12; i++)
            {
                ProfileListBox.Items.Add(
                    new Image()
                    {
                        Source = new BitmapImage(new Uri(string.Format("/Images/Profiles/{0}.jpg", (i + 1).ToString("D3")), UriKind.Relative)),
                        Clip = new EllipseGeometry() { Center = new Point(24, 24), RadiusX = 24, RadiusY = 24 },
                        Width = 48,
                        Height = 48
                    });
            }

            this.DataContext = Current.App;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((BitmapImage)NewProfileImage.Source == null)
                {
                    Tip.Alert("尚未选中新头像");
                    return;
                }

                var newProfilePicture = ((BitmapImage)NewProfileImage.Source).UriSource.ToString();
                if (Current.App.UserProfilePicture == newProfilePicture)
                {
                    Tip.Alert("请选择不同的头像");
                    return;
                }

                Current.App.UserProfilePicture = ((BitmapImage)NewProfileImage.Source).UriSource.ToString();
                TengDa.Wpf.Context.UserContext.SaveChanges();
                OperationHelper.ShowTips("更换头像成功！",true);
            }
            catch(Exception ex)
            {
                Error.Alert(ex);
            }
        }

        private void ProfileListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                NewProfileImage.Source = ((Image)e.AddedItems[0]).Source;
            }
        }
    }
}
