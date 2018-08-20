using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TengDa.Wpf;

namespace Tafel.Hipot.App.View
{
    /// <summary>
    /// AboutUC.xaml 的交互逻辑
    /// </summary>
    public partial class CurrentUserUC : UserControl
    {
        public CurrentUserUC()
        {
            InitializeComponent();
            this.DataContext = Current.App;

        }

        private void hyberlinkSetting_Click(object sender, RoutedEventArgs e)
        {

        }

        private void hyberlinkManage_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
