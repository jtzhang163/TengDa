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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TengDa.Wpf
{
    /// <summary>
    /// YieldUC.xaml 的交互逻辑
    /// </summary>
    public partial class YieldUC : UserControl
    {
        public YieldUC()
        {
            InitializeComponent();
            this.DataContext = Current.RealtimeYieldViewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Current.RealtimeYieldViewModel.FeedingOK++;
        }

        private void BtnClearYield_Click(object sender, RoutedEventArgs e)
        {
            Current.RealtimeYieldViewModel.ClearYield();
        }
    }
}
