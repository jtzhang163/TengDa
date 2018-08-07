using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Zopoise.Scada.App.View
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

            ObjectTreeView.Items.Add(new TreeViewItem() { Header = AppCurrent.Plc.Name, Name = AppCurrent.Plc.Name });

            ObjectTreeView.Items.Add(new TreeViewItem() { Header = AppCurrent.Communicator.Name, Name = AppCurrent.Communicator.Name });

            var TreeViewCommunicator = new TreeViewItem() { Header = "工装板", Name = "工装板", IsExpanded = true };
            AppCurrent.Testers.ForEach(t => TreeViewCommunicator.Items.Add(new TreeViewItem() { Header = t.Name, Name = t.Name }));
            ObjectTreeView.Items.Add(TreeViewCommunicator);

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
            else if (name == AppCurrent.Plc.Name)
            {
                ObjPropertySetter.SelectedObject = AppCurrent.Plc;
            }
            else if (name == AppCurrent.Communicator.Name)
            {
                ObjPropertySetter.SelectedObject = AppCurrent.Communicator;
            }
            else
            {

                var tester = AppCurrent.Testers.FirstOrDefault(t => t.Name == name);
                if (tester != null)
                {
                    ObjPropertySetter.SelectedObject = AppCurrent.Testers.FirstOrDefault(t => t.Name == name);
                }
            }

        }

        private void ObjPropertySetter_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            AppContext.PlcContext.SaveChanges();
            AppContext.CommunicatorContext.SaveChanges();
            AppContext.TesterContext.SaveChanges();
        }
    }
}