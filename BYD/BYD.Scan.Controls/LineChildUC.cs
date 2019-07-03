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
        public LineChildUC()
        {
            InitializeComponent();
        }

        public void Init(Line line)
        {
            this.line = line;
            this.scanerUC1.Init(this.line.AutoScaner);
            this.scanerUC2.Init(this.line.ManuScaner);
        }
    }
}
