using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TengDa.Wpf;

namespace Tafel.Hipot.App
{
    /// <summary>
    /// MachineListUC.xaml 的交互逻辑
    /// </summary>
    public partial class CommunicateStatusUC : UserControl
    {
        public CommunicateStatusUC()
        {
            InitializeComponent();

        }
    }

    public class CommStatusTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null && item is CommunicateObject)
            {
                //var comm = item as CommunicateObject;
                //if (comm.IsAlive)
                //{
                    return (container as FrameworkElement).FindResource("commTemplate") as DataTemplate;
                //}
                //else
                //{
                //    return (container as FrameworkElement).FindResource("commIsNotAliveTemplate") as DataTemplate;
                //}
            }
            return null;
        }
    }
}
