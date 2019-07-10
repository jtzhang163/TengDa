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
    public partial class ScanerDebugUC : UserControl
    {

        private Scaner scaner;

        public ScanerDebugUC()
        {
            InitializeComponent();
        }

        public void Init()
        {
            Current.Lines.ForEach(o1 => o1.ChildLines.ForEach(o2 =>
            {
                this.cbScanerList.Items.Add(string.Format("{0}[ID:{1}]", o2.AutoScaner.Name, o2.AutoScaner.Id));
            }));
            this.cbScanerList.SelectedIndex = 0;
        }

        private void BtnStartScan_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(() =>
            {
                var result = this.scaner.StartScan(out string code, out string msg);
                if (result == TengDa.ScanResult.OK)
                {
                    Tip.Alert(code);
                }
                else
                {
                    Error.Alert(code);
                }
            });
            t.Start();
        }

        private void BtnStopScan_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(() =>
            {
                this.scaner.StopScan();
            });
            t.Start();
        }

        private void CbScanerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.scaner = Scaner.ScanerList.First(s => s.Id.ToString() == this.cbScanerList.SelectedItem.ToString().Split(':', ']')[1]);
        }
    }
}
