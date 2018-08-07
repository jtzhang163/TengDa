using System.Windows.Controls;

namespace TengDa.Wpf
{
    /// <summary>
    /// TipUC.xaml 的交互逻辑
    /// </summary>
    public partial class TipUC : UserControl
    {
        public TipUC()
        {
            InitializeComponent();
            this.DataContext = Current.TipViewModel;
        }
    }
}
