using System;
using System.Threading;
using System.Windows;

namespace TengDa.Wpf
{

    /// <summary>
    /// 提示信息弹窗提示公用类
    /// </summary>
    public class Tip
    {
        public static void Alert(string msg)
        {
            if (!Current.isMessageBoxShow)
            {
                Current.isMessageBoxShow = true;
                if (Xceed.Wpf.Toolkit.MessageBox.Show(msg, "提示", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
                {
                    Current.isMessageBoxShow = false;
                }
            }
            LogHelper.WriteInfo(msg);
        }
    }

    /// <summary>
    /// 异常处理公用类
    /// </summary>
    public class Error
    {
        /// <summary>
        /// 弹窗显示异常内容
        /// </summary>
        /// <param name="ex"></param>
        public static void Alert(Exception ex)
        {
            Alert(ex.Message);
        }
        /// <summary>
        /// 弹窗显示异常字符串
        /// </summary>
        /// <param name="ex"></param>
        public static void Alert(string str)
        {
            if (!Current.isMessageBoxShow)
            {
                Current.isMessageBoxShow = true;
                if (Xceed.Wpf.Toolkit.MessageBox.Show(str, "异常提示", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
                {
                    Current.isMessageBoxShow = false;
                }
            }
            LogHelper.WriteError(str);
        }
    }

    /// <summary>
    /// 确认框公用类
    /// </summary>
    public class Verify
    {
        /// <summary>
        /// 弹窗确认
        /// </summary>
        public static bool Show(string caption, string messageText)
        {
            bool isOK = false;

            if (Xceed.Wpf.Toolkit.MessageBox.Show(messageText, caption, MessageBoxButton.OKCancel, MessageBoxImage.Information) == MessageBoxResult.OK)
            {
                isOK = true;
            }

            return isOK;
        }
    }
}
