using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using TengDa.Wpf;

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

            ObjectTreeView.Items.Add(new TreeViewItem() { Header = "测试仪", Name = "InsulationTester" });

            ObjectTreeView.Items.Add(new TreeViewItem() { Header = "MES", Name = "Mes"});
        }

        private void ObjectTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            PropertyInfo propertyInfo = ObjectTreeView.SelectedItem.GetType().GetProperty("Name");
            string name = (string)propertyInfo.GetValue(ObjectTreeView.SelectedItem, null);
            Console.WriteLine(name);

            if (name == "AppViewModel")
            {
                ObjPropertySetter.SelectedObject = AppCurrent.AppViewModel;
                ObjPropertySetter.IsReadOnly = Current.User.Id < 1 ? true : false; 
            }
            else if (name == "AppOption")
            {
                ObjPropertySetter.SelectedObject = AppCurrent.Option;
                ObjPropertySetter.IsReadOnly = Current.User.Role.Level < 2 ? true : false;
            }
            else if (name == "InsulationTester")
            {
                ObjPropertySetter.SelectedObject = AppCurrent.InsulationTester;
                ObjPropertySetter.IsReadOnly = Current.User.Role.Level < 3 ? true : false;
            }
            else if(name == "Mes")
            {
                ObjPropertySetter.SelectedObject = AppCurrent.Mes;
                ObjPropertySetter.IsReadOnly = Current.User.Role.Level < 2 ? true : false;
            }

        }


        private void ObjPropertySetter_SelectedPropertyItemChanged(object sender, RoutedPropertyChangedEventArgs<Xceed.Wpf.Toolkit.PropertyGrid.PropertyItemBase> e)
        {
            if (e.NewValue == null || e.OldValue == null)
            {
                return;
            }

            object o = this.ObjPropertySetter.SelectedObject;
            Type type = o.GetType();
            string settingsStr = string.Empty;

            if (type == typeof(MES))
            {
                //System.Reflection.PropertyInfo propertyInfoId = type.GetProperty("Id"); //获取指定名称的属性
                //long Id = (long)propertyInfoId.GetValue(o, null); //获取属性值
                //Xceed.Wpf.Toolkit.PropertyGrid.PropertyItem p = (Xceed.Wpf.Toolkit.PropertyGrid.PropertyItem)(e.OldValue);

                //settingsStr = string.Format("将Id为 {0} 的 {1} 的 {2} 修改为 {3} ", Id, type.Name, p.PropertyName, p.Value);
                //AppContext.MesContext.SaveChangesAsync();

                //Current.ShowTips(settingsStr, true);
                AppContext.MesContext.SaveChangesAsync();
            }
            else if(type == typeof(InsulationTester))
            {
                AppContext.InsulationContext.SaveChangesAsync();
            }

        }
    }
}