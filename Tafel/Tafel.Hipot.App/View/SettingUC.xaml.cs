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

            ObjectTreeView.Items.Add(new TreeViewItem() { Header = "电阻测试仪", Name = "InsulationTester" });

            var treeViewItem = new TreeViewItem() { Header = "冷却机", Name = "Cooler" };
            treeViewItem.Items.Add(new TreeViewItem() { Header = "PLC", Name = "PLC" });
            ObjectTreeView.Items.Add(treeViewItem);

            ObjectTreeView.Items.Add(new TreeViewItem() { Header = "温度采集器", Name = "TemperatureCollector" });

            ObjectTreeView.Items.Add(new TreeViewItem() { Header = "扫码枪", Name = "Scaner" });

            ObjectTreeView.Items.Add(new TreeViewItem() { Header = "MES", Name = "Mes" });
        }

        private void ObjectTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            PropertyInfo propertyInfo = ObjectTreeView.SelectedItem.GetType().GetProperty("Name");
            string name = (string)propertyInfo.GetValue(ObjectTreeView.SelectedItem, null);
            Console.WriteLine(name);

            if (name == "AppViewModel")
            {
                ObjPropertySetter.SelectedObject = Current.App;
                ObjPropertySetter.IsReadOnly = AppCurrent.User.Id < 1 ? true : false; 
            }
            else if (name == "AppOption")
            {
                ObjPropertySetter.SelectedObject = Current.Option;
                ObjPropertySetter.IsReadOnly = AppCurrent.User.Role.Level < 2 ? true : false;
            }
            else if (name == "InsulationTester")
            {
                ObjPropertySetter.SelectedObject = Current.Tester;
                ObjPropertySetter.IsReadOnly = AppCurrent.User.Role.Level < 3 ? true : false;
            }
            else if (name == "Cooler")
            {
                ObjPropertySetter.SelectedObject = Current.Cooler;
                ObjPropertySetter.IsReadOnly = AppCurrent.User.Role.Level < 3 ? true : false;
            }
            else if (name == "PLC")
            {
                ObjPropertySetter.SelectedObject = Current.Cooler.PLC;
                ObjPropertySetter.IsReadOnly = AppCurrent.User.Role.Level < 3 ? true : false;
            }
            else if (name == "TemperatureCollector")
            {
                ObjPropertySetter.SelectedObject = Current.Collector;
                ObjPropertySetter.IsReadOnly = AppCurrent.User.Role.Level < 3 ? true : false;
            }
            else if (name == "Scaner")
            {
                ObjPropertySetter.SelectedObject = Current.Scaner;
                ObjPropertySetter.IsReadOnly = AppCurrent.User.Role.Level < 3 ? true : false;
            }
            else if(name == "Mes")
            {
                ObjPropertySetter.SelectedObject = Current.Mes;
                ObjPropertySetter.IsReadOnly = AppCurrent.User.Role.Level < 2 ? true : false;
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

            //if (type == typeof(MES))
            //{
            //    //System.Reflection.PropertyInfo propertyInfoId = type.GetProperty("Id"); //获取指定名称的属性
            //    //int Id = (int)propertyInfoId.GetValue(o, null); //获取属性值
            //    //Xceed.Wpf.Toolkit.PropertyGrid.PropertyItem p = (Xceed.Wpf.Toolkit.PropertyGrid.PropertyItem)(e.OldValue);

            //    //settingsStr = string.Format("将Id为 {0} 的 {1} 的 {2} 修改为 {3} ", Id, type.Name, p.PropertyName, p.Value);
            //    //Context.MesContext.SaveChangesAsync();

            //    //OperationHelper.ShowTips(settingsStr, true);
            //    Context.MesContext.SaveChangesAsync();
            //}

            if(type == typeof(InsulationTester))
            {
                Context.InsulationContext.SaveChangesAsync();
            }
            else if (type == typeof(TemperatureCollector))
            {
                Context.CollectorContext.SaveChangesAsync();
            }
            else if (type == typeof(Cooler))
            {
                Context.CoolerContext.SaveChangesAsync();
            }
            else if (type == typeof(PLC))
            {
                Context.CoolerContext.SaveChangesAsync();
            }
            else if (type == typeof(Scaner))
            {
                Context.ScanerContext.SaveChangesAsync();
            }
            else if (type == typeof(MES))
            {
                Context.MesContext.SaveChangesAsync();
            }

        }
    }
}