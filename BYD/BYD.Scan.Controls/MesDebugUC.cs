using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TengDa.WF;
using TengDa;
using System.Threading;

namespace BYD.Scan.Controls
{
    public partial class MesDebugUC : UserControl
    {

        private MES mes;

        public MesDebugUC()
        {
            InitializeComponent();
        }

        public void Init()
        {
            this.mes = Current.mes;
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(() =>
            {

            });
            t.Start();
        }
    }
}
