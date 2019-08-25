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

namespace CAMEL.RGV.Touchscreen.Control
{
    /// <summary>
    /// ParamBox.xaml 的交互逻辑
    /// </summary>
    public partial class SignalBox : UserControl
    {
        public SignalBox()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ParamNameProperty = DependencyProperty.Register("ParamName", typeof(string), typeof(SignalBox), new PropertyMetadata(ParamNameChanged));
        public static readonly DependencyProperty ParamValueProperty = DependencyProperty.Register("ParamValue", typeof(bool), typeof(SignalBox), new PropertyMetadata(ParamValueChanged));

        public string ParamName
        {
            get { return (string)GetValue(ParamNameProperty); }
            set { SetValue(ParamNameProperty, value); }
        }

        public bool ParamValue
        {
            get { return (bool)GetValue(ParamValueProperty); }
            set { SetValue(ParamValueProperty, value); }
        }


        private static void ParamNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var paramBox = (SignalBox)d;
            paramBox.lbParamName.Content = (string)e.NewValue;
        }

        private static void ParamValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var paramBox = (SignalBox)d;
            paramBox.elSignal.Fill = (bool)e.NewValue ? Brushes.LimeGreen: Brushes.LightGray;
        }
    }
}
