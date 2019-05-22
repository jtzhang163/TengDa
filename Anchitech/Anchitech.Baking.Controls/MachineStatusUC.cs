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
    public partial class MachineStatusUC : UserControl
    {
        public MachineStatusUC()
        {
            InitializeComponent();
        }

        public void SetCheckBoxEnabled(bool isEnabled)
        {
            this.cbIsEnable.Enabled = isEnabled;
        }

        public void Init(object machine)
        {
            this.lbMachineName.Text = (string)GetProperty(machine, "Name");
        }

        public object GetProperty(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName).GetValue(obj);
        }
    }
}
