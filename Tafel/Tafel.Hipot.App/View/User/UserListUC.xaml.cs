using System.Linq;
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
            Tip.Alert("保存修改成功，重启软件即可生效！");
        }


        private void UserListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var user = (User)e.AddedItems[0];
                var index = new RoleFactory().GetLowAuthorityRoles().ToList().IndexOf(user.Role);
                if (UserRolesComboBox != null)
                {
                    UserRolesComboBox.SelectedIndex = index;
                    var isVisible = user.Role.Level <= AppCurrent.User.Role.Level && user != AppCurrent.User;
                   // UserRolesComboBox.IsHitTestVisible = !isVisible;
                    var visible = isVisible ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
                    UserRoleLabel.Visibility = visible;
                    UserRolesComboBox.Visibility = visible;
                    SaveChangesButton.Visibility = visible;
                }

            }
        }

        private void UserRolesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var user = (User)UserListBox.SelectedItem;

            if (e.AddedItems.Count > 0)
            {
                user.Role = (Role)e.AddedItems[0];
            }           
        }
    }
}
