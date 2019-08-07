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

        public void UpdateValue(float val)
        {
            this.lbValue.Text = val.ToString();
        }
    }
}
