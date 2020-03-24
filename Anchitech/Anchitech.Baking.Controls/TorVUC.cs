using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Anchitech.Baking.Controls
{
    public partial class TorVUC : UserControl
    {

        public TorVUC()
        {
            InitializeComponent();
        }
        public void Init(string subject)
        {    
            this.lbSubject.Text = subject;
        }

        public void UpdateValue(float val, bool isExTPoint)
        {
            this.lbValue.Text = val.ToString();

            this.lbValue.ForeColor = isExTPoint ? System.Drawing.Color.White : System.Drawing.Color.Green;
            this.lbValue.BackColor = isExTPoint ? System.Drawing.Color.Red : SystemColors.Control;
        }
    }
}
