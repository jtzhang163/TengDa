using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CAMEL.Baking.Control
{
    public partial class ActivationUC : UserControl
    {
        public ActivationUC()
        {
            InitializeComponent();
        }

        private void LbActivationMsg_Click(object sender, EventArgs e)
        {
            new ActivationWindow().ShowDialog();
        }
    }
}
