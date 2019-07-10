using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BYD.Scan.Controls
{
    public partial class TouchscreenUC : UserControl
    {
        private Touchscreen touchscreen;
        public TouchscreenUC()
        {
            InitializeComponent();
        }

        public void Init(Touchscreen touchscreen)
        {
            this.touchscreen = touchscreen;
            this.lbName.Text = this.touchscreen.Name;
            this.lbIsReadyScan1.Text = "";
            this.lbIsReadyScan2.Text = "";
        }

        public void UpdateUI()
        {
            this.BackColor = this.touchscreen.IsAlive ? Color.White : SystemColors.Control;

            if (this.touchscreen.IsReadyScan1)
            {
                this.lbIsReadyScan1.Text = "请求扫码";
                this.lbIsReadyScan1.BackColor = Color.White;
                this.lbIsReadyScan1.ForeColor = Color.Green;
            }
            else
            {
                this.lbIsReadyScan1.Text = "";
                this.lbIsReadyScan1.BackColor = Color.Transparent;
                this.lbIsReadyScan1.ForeColor = Color.Transparent;
            }

            if (this.touchscreen.IsReadyScan2)
            {
                this.lbIsReadyScan2.Text = "请求扫码";
                this.lbIsReadyScan2.BackColor = Color.White;
                this.lbIsReadyScan2.ForeColor = Color.Green;
            }
            else
            {
                this.lbIsReadyScan2.Text = "";
                this.lbIsReadyScan2.BackColor = Color.Transparent;
                this.lbIsReadyScan2.ForeColor = Color.Transparent;
            }
        }
    }
}
