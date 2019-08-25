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
using System.Windows.Shapes;

namespace CAMEL.RGV.Touchscreen.View
{
    /// <summary>
    /// ParamSetWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ParamSetWindow : Window
    {
        private string paramName;
        private int currentValue;
        public ParamSetWindow(string paramName, int currentValue)
        {
            InitializeComponent();
            this.paramName = paramName;
            this.currentValue = currentValue;
            this.lbParamName.Content = this.paramName;
            this.lbCurrentValue.Content = this.currentValue;
            this.tbNewValue.Text = "";
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(this.tbNewValue.Text.Trim(), out int result))
            {
                this.lbTip.Content = "输入有误！";
                return;
            }

            if (result != currentValue)
            {
                if (!Current.RGV.Write(Parameter.GetAddr(this.paramName), (short)result, out string msg))
                {
                    this.lbTip.Content = "ERROR:" + msg;
                    return;
                }
            }

            this.Close();
        }
    }
}
