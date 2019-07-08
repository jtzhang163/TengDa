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
        }

        public void UpdateUI()
        {
            this.BackColor = this.touchscreen.IsAlive ? Color.White : SystemColors.Control;
        }
    }
}
