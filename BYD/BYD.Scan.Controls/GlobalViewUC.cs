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
    public partial class GlobalViewUC : UserControl
    {
        public GlobalViewUC()
        {
            InitializeComponent();
        }

        public void Init()
        {
            for (int i = 0; i < 4; i++)
            {
                var lineUC = (LineUC)(this.Controls.Find(string.Format("lineUC{0}", i + 1), true)[0]);
                lineUC.Init(Current.Lines[i]);
            }
        }
    }
}
