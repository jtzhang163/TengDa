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
    public partial class ScanerUC : UserControl
    {
        private Scaner scaner;
        public ScanerUC()
        {
            InitializeComponent();
        }

        public void Init(Scaner scaner)
        {
            this.scaner = scaner;
            this.lbName.Text = this.scaner.Name;
        }
    }
}
