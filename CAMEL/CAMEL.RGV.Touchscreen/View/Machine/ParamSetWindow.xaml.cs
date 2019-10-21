using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using CAMEL.RGV.Touchscreen.Util;

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
                Speech.Voice("输入有误");
                this.lbTip.Content = "输入有误！";
                return;
            }

            if (result < Parameter.GetMinvalue(this.paramName) || result > Parameter.GetMaxvalue(this.paramName))
            {
                Speech.Voice("请输入有效范围内的值");
                this.lbTip.Content = "请输入有效范围内的值";
                return;
            }

            if (result != currentValue)
            {
                if (Parameter.GetDataType(this.paramName) == "ushort")
                {
                    if (!Current.RGV.Write(Parameter.GetAddr(this.paramName), (ushort)result, out string msg))
                    {
                        Speech.Voice("ERROR:" + msg);
                        this.lbTip.Content = "ERROR:" + msg;
                        return;
                    }
                }
                else
                {
                    if (!Current.RGV.Write(Parameter.GetAddr(this.paramName), (short)result, out string msg))
                    {
                        Speech.Voice("ERROR:" + msg);
                        this.lbTip.Content = "ERROR:" + msg;
                        return;
                    }
                }
            }
            Speech.Voice("设置成功");
            this.Close();
        }

        private void CloseIWindow()
        {
            this.Close();
        }

        [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
        public static extern void keybd_event(byte bvk, byte bScan, uint dwFlags, uint dwExtraInfo);
        private  Process process;

        private void TbNewValue_GotFocus(object sender, RoutedEventArgs e)
        {
            //StartProcess();
        }
        private void TbNewValue_LostFocus(object sender, RoutedEventArgs e)
        {
            //TopProcess();
        }

        private void TbNewValue_KeyDown(object sender, KeyEventArgs e)
        {

        }
        public void StartProcess()
        {

            Thread thread = new Thread(o =>
            {
                try
                {
                    string path = string.Format(System.IO.Directory.GetCurrentDirectory() + @"\APP.exe");
                    if (!System.IO.File.Exists(path))
                        return;
                    if (process != null)
                    {
                        process.Close();
                    }
                    process = Process.Start(path);
                      
                }
                catch (Exception ex)
                {
                    process.Kill();

                }
            });
            thread.IsBackground = true;
            thread.Start();
        }
        public void TopProcess()
        {
            if (process != null && !process.HasExited)
            {
                process.Kill();
            }
        }

    }
}
