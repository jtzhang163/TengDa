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

namespace CAMEL.Baking.Control
{
    public partial class ScanerDebugUC : UserControl
    {
        public ScanerDebugUC()
        {
            InitializeComponent();

        }

        public void Init()
        {
            //cbBatteryScaner.Items.Add(Current.BatteryScaner.Name);
            cbClampScaner.Items.Add(Current.ClampScaner.Name);

            //cbBatteryScaner.SelectedIndex = 0;
            cbClampScaner.SelectedIndex = 0;
        }

        private void BtnClampScanStart_Click(object sender, EventArgs e)
        {
            string code = string.Empty;
            string msg = string.Empty;
            ScanResult scanResult = Current.ClampScaner.StartScan(out code, out msg);
            if (scanResult == ScanResult.OK)
            {
                Tip.Alert(code);
            }
            else if (scanResult == ScanResult.NG)
            {
                Tip.Alert("扫码返回NG！");
            }
            else
            {
                Error.Alert(msg);
            }
        }

        private void BtnClampScanOkBackToFeeder_Click(object sender, EventArgs e)
        {
            string msg = string.Empty;
            if (!Current.Feeder.SetScanClampResult(ScanResult.OK, out msg))
            {
                Error.Alert(msg);
            }
            else
            {
                Tip.Alert("OK");
            }
        }

        private void BtnClampScanNgBackToFeeder_Click(object sender, EventArgs e)
        {
            string msg = string.Empty;
            if (!Current.Feeder.SetScanClampResult(ScanResult.NG, out msg))
            {
                Error.Alert(msg);
            }
            else
            {
                Tip.Alert("OK");
            }
        }
    }
}
