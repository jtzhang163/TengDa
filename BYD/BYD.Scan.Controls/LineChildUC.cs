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
    public partial class LineChildUC : UserControl
    {
        private Line line;
        private ScanerUC[] scanerUCs = new ScanerUC[Option.ChildLineScanerCount];
        public LineChildUC()
        {
            InitializeComponent();
        }

        public void Init(Line line)
        {
            this.line = line;
            for (int k = 0; k < Option.ChildLineScanerCount; k++)
            {
                scanerUCs[k] = (ScanerUC)(this.Controls.Find(string.Format("scanerUC{0}", k + 1), true)[0]);
                scanerUCs[k].Init(this.line.Scaners[k]);
            }
        }

        public void UpdateUI()
        {
            for (int k = 0; k < Option.ChildLineScanerCount; k++)
            {
                scanerUCs[k].UpdateUI();
            }
        }
    }
}
