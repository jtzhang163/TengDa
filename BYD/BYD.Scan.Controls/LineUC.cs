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
        private LineChildUC[] lineChildUCs = new LineChildUC[Option.ChildLineCount];

        public LineUC()
        {
            InitializeComponent();
        }

        public void Init(Line line)
        {
            this.line = line;
            this.gbLine.Text = this.line.Name;
            this.touchscreenUC1.Init(this.line.Touchscreen);
            for (int j = 0; j < Option.ChildLineCount; j++)
            {
                lineChildUCs[j] = (LineChildUC)(this.Controls.Find(string.Format("lineChildUC{0}", j + 1), true)[0]);
                lineChildUCs[j].Init(this.line.ChildLines[j]);
            }
        }

        public void UpdateUI()
        {
            this.BackColor = this.line.Touchscreen.IsAlive ? Color.White : SystemColors.Control;
            this.touchscreenUC1.UpdateUI();
            for (int j = 0; j < Option.ChildLineCount; j++)
            {
                lineChildUCs[j].UpdateUI();
            }
        }
    }
}
