using CAMEL.RGV.Touchscreen.View;
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
    public partial class ParamBox : UserControl
    {
        public ParamBox()
        {
            InitializeComponent();
        }



        public static readonly DependencyProperty ParamValueProperty = DependencyProperty.Register("ParamValue", typeof(string), typeof(ParamBox), new PropertyMetadata(ParamValueChanged));
        public static readonly DependencyProperty ParamUnitProperty = DependencyProperty.Register("ParamUnit", typeof(string), typeof(ParamBox), new PropertyMetadata(ParamUnitChanged));
        public static readonly DependencyProperty ParamNameProperty = DependencyProperty.Register("ParamName", typeof(string), typeof(ParamBox), new PropertyMetadata(ParamNameChanged));

        public string ParamValue
        {
            get { return (string)GetValue(ParamValueProperty); }
            set { SetValue(ParamValueProperty, value); }
        }
        public string ParamUnit
        {
            get { return (string)GetValue(ParamUnitProperty); }
            set { SetValue(ParamUnitProperty, value); }
        }

        public string ParamName
        {
            get { return (string)GetValue(ParamNameProperty); }
            set { SetValue(ParamNameProperty, value); }
        }


        private static void ParamValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var paramBox = (ParamBox)d;
            paramBox.tbParamValue.Content = (string)e.NewValue;
        }

        private static void ParamUnitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var paramBox = (ParamBox)d;
            paramBox.lbParamUnit.Content = (string)e.NewValue;
        }

        private static void ParamNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var paramBox = (ParamBox)d;
            paramBox.lbParamName.Content = (string)e.NewValue;
        }

        private void TbParamValue_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!Current.RGV.IsConnected)
            {
                MessageBox.Show("尚未连接RGV PLC！","异常提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            new ParamSetWindow(ParamName, int.Parse(ParamValue)).ShowDialog();
        }
    }
}
