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

namespace BYD.Scan.Controls
{
    public partial class ScanerUC : UserControl
    {
        private Scaner scaner;
        private TextBox[] tbCodes = new TextBox[2];
        public ScanerUC()
        {
            InitializeComponent();
        }

        public void Init(Scaner scaner)
        {
            this.scaner = scaner;
            this.lbName.Text = this.scaner.Name;
            for (int x = 0; x < 2; x++)
            {
                tbCodes[x] = (TextBox)(this.Controls.Find(string.Format("tbCode{0}", x + 1), true)[0]);
                tbCodes[x].Text = this.scaner.Codes[x];
            }
        }

        public void UpdateUI()
        {
            this.BackColor = this.scaner.IsAlive ? Color.White : SystemColors.Control;

            for (int x = 0; x < 2; x++)
            {
                tbCodes[x].Text = this.scaner.Codes[x];
                tbCodes[x].ForeColor = GetForeColor(x);
            }
        }

        private Color GetForeColor(int x)
        {
            if (this.scaner.Codes[x].Contains("ERROR"))
            {
                return Color.Red;
            }
            else if (this.scaner.Codes[0].Length > 18 && this.scaner.Codes[1].Length > 18)
            {
                if (!this.scaner.MES_RESULTs[x].ToLower().Contains("ok"))
                {
                    return Color.Blue;
                }
                return Color.LimeGreen;
            }
            return Color.Black;
        }

        private void TbCode_DoubleClick(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要清除该处的电池条码", "清除条码确认", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if ((sender as TextBox).Name == "tbCode1")
                {
                    this.scaner.Code1 = "";
                    LogHelper.WriteInfo(string.Format("手动清除 {0} 条码1", this.scaner.Name));
                }
                else
                {
                    this.scaner.Code2 = "";
                    LogHelper.WriteInfo(string.Format("手动清除 {0} 条码2", this.scaner.Name));
                }
            }
        }
    }
}
