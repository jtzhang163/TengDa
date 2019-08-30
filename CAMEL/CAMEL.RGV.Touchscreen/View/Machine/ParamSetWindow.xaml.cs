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

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (Current.Option.IsPad) return;   //运行在平板时不触发
            ChangeParam();
        }

        private void BtnOK_TouchEnter(object sender, TouchEventArgs e)
        {
            if (!Current.Option.IsPad) return;
            ChangeParam();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            if (Current.Option.IsPad) return;   //运行在平板时不触发
            CloseIWindow();
        }

        private void BtnClose_TouchLeave(object sender, TouchEventArgs e)
        {
            if (!Current.Option.IsPad) return;
            CloseIWindow();
        }

        private void ChangeParam()
        {
            if (!int.TryParse(this.tbNewValue.Text.Trim(), out int result))
            {
                this.lbTip.Content = "输入有误！";
                return;
            }

            if (result < Parameter.GetMinvalue(this.paramName) || result > Parameter.GetMaxvalue(this.paramName))
            {
                this.lbTip.Content = "请输入有效范围内的值";
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

        private void CloseIWindow()
        {
            this.Close();
        }
    }
}
