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
    public partial class LineUC : UserControl
    {
        private Line line;
        public LineUC()
        {
            InitializeComponent();
        }

        public void Init(Line line)
        {
            this.line = line;
            this.gbLine.Text = this.line.Name;
            this.touchscreenUC1.Init(this.line.Touchscreen);
            for (int i = 0; i < 2; i++)
            {
                var lineChildUC = (LineChildUC)(this.Controls.Find(string.Format("lineChildUC{0}", i + 1), true)[0]);
                lineChildUC.Init(this.line.ChildLines[i]);
            }
        }
    }
}
