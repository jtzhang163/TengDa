using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TengDa.WF;
using TengDa;
using System.Threading;

namespace BYD.Scan.Controls
{
    public partial class MesDebugUC : UserControl
    {

        private MES mes;

        public MesDebugUC()
        {
            InitializeComponent();
        }

        public void Init()
        {
            this.mes = Current.mes;
            this.tbFlag.Text = "1";
            this.tbTerminal.Text = Current.mes.Terminal;
            this.tbUserId.Text = Current.mes.UserId;
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            var barcode = this.tbBarcode.Text.Trim();
            var flag = this.tbFlag.Text.Trim();
            var terminal = this.tbTerminal.Text.Trim();
            var userId = this.tbUserId.Text.Trim();

            if (string.IsNullOrEmpty(barcode))
            {
                Tip.Alert("输入电芯条码为空！");
                return;
            }
            if (string.IsNullOrEmpty(flag))
            {
                Tip.Alert("输入调用类型为空！");
                return;
            }
            if (string.IsNullOrEmpty(terminal))
            {
                Tip.Alert("输入调用工位为空！");
                return;
            }
            if (string.IsNullOrEmpty(userId))
            {
                Tip.Alert("输入调用者为空！");
                return;
            }

            MesRequest request = new MesRequest()
            {
                Barcode = barcode,
                Flag = flag,
                Terminal = terminal,
                UserId = userId
            };

            Thread t = new Thread(() =>
            {
                var response = MES.UploadBatteryInfo(request);
                this.BeginInvoke(new MethodInvoker(() =>
                {
                    this.tbRtCode.Text = response.Code.ToString();
                    this.tbRtMsg.Text = response.RtMsg;
                    Color fore = response.Code == 0 ? Color.Green : Color.Red;
                    this.tbRtCode.ForeColor = fore;
                    this.tbRtMsg.ForeColor = fore;
                }));
            });
            t.Start();
        }
    }
}
