using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TengDa.WF
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
                Thread t = new Thread(() =>
                {
                    Current.isMessageBoxShow = true;
                    DialogResult dr = MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    if (dr == DialogResult.OK)
                    {
                        Current.isMessageBoxShow = false;
                    }
                });
                t.Start();
            }
            LogHelper.WriteInfo(msg);
        }
    }
}
