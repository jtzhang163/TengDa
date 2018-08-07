using System.Windows;
using System.Windows.Controls;

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
