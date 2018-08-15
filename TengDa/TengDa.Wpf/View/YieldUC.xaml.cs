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
            this.DataContext = Current.YieldNow;
        }


        private void BtnClearYield_Click(object sender, RoutedEventArgs e)
        {
            Current.YieldNow.ClearYield();
        }
    }
}
