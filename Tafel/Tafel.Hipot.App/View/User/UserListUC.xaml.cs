using System.Windows.Controls;
using TengDa.Wpf;

namespace Tafel.Hipot.App.View
{
    /// <summary>
    /// AboutUC.xaml 的交互逻辑
    /// </summary>
    public partial class UserListUC : UserControl
    {
        public UserListUC()
        {
            InitializeComponent();
        }

        private void ButtonSaveChanges_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            TengDa.Wpf.Context.UserContext.SaveChanges();
            Tip.Alert("修改成功！");
        }
    }
}
