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
    public partial class TransferUC : UserControl
    {
        public TransferUC()
        {
            InitializeComponent();
        }

        public void Init(Transfer transfer)
        {
            this.lbName.Text = transfer.Name;
            this.simpleClampUC1.Init(transfer.Station);
        }

        public void Update(Transfer transfer)
        {
            transfer.IsAlive = transfer.IsEnable && transfer.Plc.IsAlive;
            transfer.Station.IsAlive = transfer.IsAlive && transfer.Station.IsEnable;
            this.BackColor = transfer.IsAlive ? Color.White : Color.LightGray;
            this.simpleClampUC1.Update(transfer.Station);
        }
    }
}
