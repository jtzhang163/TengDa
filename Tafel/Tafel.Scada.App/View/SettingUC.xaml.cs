using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Tafel.Hipot.App.View
{
    /// <summary>
    /// AboutUC.xaml 的交互逻辑
    /// </summary>
    public partial class SettingUC : UserControl
    {
        public SettingUC()
        {
            InitializeComponent();

            ObjectTreeView.Items.Add(new TreeViewItem() { Header = "应用程序", Name = "AppViewModel" });

            ObjectTreeView.Items.Add(new TreeViewItem() { Header = "基本配置", Name = "AppOption" });

            ObjectTreeView.Items.Add(new TreeViewItem() { Header = AppCurrent.InsulationTester.Name, Name = AppCurrent.InsulationTester.Name });

        }

        private void ObjectTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            PropertyInfo propertyInfo = ObjectTreeView.SelectedItem.GetType().GetProperty("Name");
            string name = (string)propertyInfo.GetValue(ObjectTreeView.SelectedItem, null);
            Console.WriteLine(name);

            if (name == "AppViewModel")
            {
                ObjPropertySetter.SelectedObject = AppCurrent.AppViewModel;
            }
            else if (name == "AppOption")
            {
                ObjPropertySetter.SelectedObject = AppCurrent.Option;
            }
            else if (name == AppCurrent.InsulationTester.Name)
            {
                ObjPropertySetter.SelectedObject = AppCurrent.InsulationTester;
            }
            else
            {

            }

        }

        private void ObjPropertySetter_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            AppContext.InsulationContext.SaveChanges();
        }
    }
}