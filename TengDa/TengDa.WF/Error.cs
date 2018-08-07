using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using TengDa.WF;

namespace TengDa
{
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
            if (!Current.isMessageBoxShow)
            {
                Thread t = new Thread(() =>
                {
                    Current.isMessageBoxShow = true;
                    DialogResult dr = MessageBox.Show(ex.Message, "异常提示", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    if (dr == DialogResult.OK)
                    {
                        Current.isMessageBoxShow = false;
                    }
                });
                t.Start();
            }
            LogHelper.WriteError(ex);
        }
        /// <summary>
        /// 弹窗显示异常字符串
        /// </summary>
        /// <param name="ex"></param>
        public static void Alert(string str)
        {
            if (!Current.isMessageBoxShow)
            {
                Thread t = new Thread(() =>
                {
                    Current.isMessageBoxShow = true;
                    DialogResult dr = MessageBox.Show(str, "异常提示", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    if (dr == DialogResult.OK)
                    {
                        Current.isMessageBoxShow = false;
                    }
                });
                t.Start();
            }
            LogHelper.WriteError(str);
        }
    }
}
