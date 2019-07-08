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

        private LineUC[] lineUCs = new LineUC[Option.LineCount];

        public GlobalViewUC()
        {
            InitializeComponent();
        }

        public void Init()
        {
            for (int i = 0; i < Option.LineCount; i++)
            {
                lineUCs[i] = (LineUC)(this.Controls.Find(string.Format("lineUC{0}", i + 1), true)[0]);
                lineUCs[i].Init(Current.Lines[i]);
            }
        }

        public void UpdateUI()
        {
            for (int i = 0; i < Option.LineCount; i++)
            {
                lineUCs[i].UpdateUI();
            }
        }
    }
}
