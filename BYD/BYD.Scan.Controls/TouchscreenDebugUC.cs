using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TengDa;
using TengDa.WF;
using System.Threading;

namespace BYD.Scan.Controls
{
    public partial class TouchscreenDebugUC : UserControl
    {
        private Touchscreen touchscreen;
        private string addr;
        public TouchscreenDebugUC()
        {
            InitializeComponent();
        }

        public void Init()
        {
            Current.Lines.ForEach(o1 =>
            {
                this.cbTouchscreenList.Items.Add(string.Format("{0}[ID:{1}]", o1.Touchscreen.Name, o1.Touchscreen.Id));
            });
            this.cbTouchscreenList.SelectedIndex = 0;
            this.cbAddr.SelectedIndex = 0;
        }

        private void CbTouchscreenList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.touchscreen = Touchscreen.TouchscreenList.First(s => s.Id.ToString() == this.cbTouchscreenList.SelectedItem.ToString().Split(':', ']')[1]);
        }

        private void CbAddr_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.addr = this.cbAddr.SelectedItem.ToString().Split(' ')[0];
        }

        private void BtnRead_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(() =>
            {
                if (!this.touchscreen.GetInfo(this.addr, 1, out ushort[] output, out string msg))
                {
                    Error.Alert(msg);
                    return;
                }
                if (output.Length < 1)
                {
                    LogHelper.WriteError(string.Format("与PLC通信出现错误，msg：{0}", msg));
                    return;
                }
                this.BeginInvoke(new MethodInvoker(() =>
                {
                    this.tbReadResult.Text = output[0].ToString();
                }));
            });
            t.Start();
        }

        private void BtnWrite_Click(object sender, EventArgs e)
        {
            if (this.tbWriteValue.Text.Trim() == "")
            {
                Error.Alert("输入为空！");
                return;
            }

            int writeValue = TengDa._Convert.StrToInt(this.tbWriteValue.Text.Trim(), -1);
            if (writeValue == -1)
            {
                Error.Alert("输入有误！");
                return;
            }

            Thread t = new Thread(() =>
            {
                if (this.touchscreen.SetInfo(this.addr, (ushort)writeValue, out string msg))
                {
                    Tip.Alert("成功写入！");
                }
                else
                {
                    Error.Alert(msg);
                }
            });
            t.Start();
        }
    }
}
