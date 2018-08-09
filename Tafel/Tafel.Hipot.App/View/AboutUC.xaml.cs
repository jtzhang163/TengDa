using System.Windows.Controls;

namespace Tafel.Hipot.App.View
{
    /// <summary>
    /// AboutUC.xaml 的交互逻辑
    /// </summary>
    public partial class AboutUC : UserControl
    {
        public AboutUC()
        {
            InitializeComponent();
            this.DataContext = AppCurrent.AppViewModel;
        }
    }
}
