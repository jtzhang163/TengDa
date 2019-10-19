using CAMEL.RGV.Touchscreen.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CAMEL.RGV.Touchscreen
{
    public static class MouseOrTouchDownOrUp
    {
        public static void MouseOrTouchDown(object sender)
        {
            if (!Current.RGV.IsConnected)
            {
                Speech.Voice("尚未连接RGV PLC");
                MessageBox.Show("尚未连接RGV PLC！", "异常提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var msg = string.Empty;
            var button = sender as Label;
            var btnContent = button.Content.ToString();
            Speech.Voice(btnContent);
            var addr = Parameter.GetAddr(btnContent);

            if (btnContent == "参数写入")
            {
                if (MessageBox.Show("确定写入参数？", "提示信息", MessageBoxButton.OKCancel, MessageBoxImage.Information) != MessageBoxResult.OK)
                {
                    return;
                }
            }

            var val = 0;

            if (Parameter.GetType(btnContent) == "点动")
            {
                addr = Parameter.GetAddr(btnContent);
                Current.RGV.Write(addr, (short)1, out msg);
                button.Background = Brushes.LightGray;
            }
            else if (Parameter.GetType(btnContent) == "单次写入")
            {
                if (Current.RGV.Write(addr, (short)1, out msg))
                {
                    button.Background = Brushes.LightGray;
                }
            }
            else if (Parameter.GetType(btnContent) == "使能")
            {
                Current.RGV.Read(addr, out short oldVal, out msg);
                val = oldVal == 1 ? 2 : 1;
                if (Current.RGV.Write(addr, (short)val, out msg))
                {
                    Util.Tool.SetValue(Current.RGV, btnContent, val == 1);
                }
            }
            else if (Parameter.GetType(btnContent) == "状态使能")
            {
                if (btnContent == "调度无效")
                {
                    val = Current.RGV.调度无效 ? 1 : 2;
                    if (Current.RGV.Write(addr, (short)val, out msg))
                    {
                        Current.RGV.调度无效 = val == 2;
                    }
                }
                else
                {
                    val = Current.RGV.手动状态 ? 2 : 1;
                    if (Current.RGV.Write(addr, (short)val, out msg))
                    {
                        Current.RGV.手动状态 = val == 1;
                    }
                }

            }
            else
            {
                button.Background = Brushes.LightGray;
            }
        }

        public static void MouseOrTouchUp(object sender)
        {
            if (!Current.RGV.IsConnected)
            {
                return;
            }

            var button = sender as Label;
            var btnContent = button.Content.ToString();

            if (Parameter.GetType(btnContent) == "点动")
            {
                var addr = Parameter.GetAddr(btnContent);
                Current.RGV.Write(addr, (short)2, out string msg);
                button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF367BB5"));
            }
            else if (Parameter.GetType(btnContent) == "单次写入")
            {
                button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF367BB5"));
            }
        }

    }
}
