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
    public partial class MachinesStatusUC : UserControl
    {
        public MachinesStatusUC()
        {
            InitializeComponent();
        }

        public void Init()
        {
            this.machines.Add(new MachineIndex { Machine = Current.Feeder, MsUC = this.machineStatusUC1 });
            this.machines.Add(new MachineIndex { Machine = Current.ClampScaner, MsUC = this.machineStatusUC2 });
            this.machines.Add(new MachineIndex { Machine = Current.BatteryScaner, MsUC = this.machineStatusUC3 });
            this.machines.Add(new MachineIndex { Machine = Current.Robot, MsUC = this.machineStatusUC4 });
            int machineindex = 5;
            Current.ovens.ForEach(o => { this.machines.Add(new MachineIndex { Machine = o, MsUC = (MachineStatusUC)(this.Controls.Find(string.Format("machineStatusUC{0}", machineindex++), true)[0]) }); });
            this.machines.Add(new MachineIndex { Machine = Current.Blanker, MsUC = this.machineStatusUC17 });
            this.machines.Add(new MachineIndex { Machine = Current.mes, MsUC = this.machineStatusUC18 });
            this.machines.ForEach(o => o.MsUC.Init(o.Machine));
        }

        public void SetCheckBoxEnabled(bool isEnabled)
        {
            this.machines.ForEach(o => { o.MsUC.SetCheckBoxEnabled(isEnabled); });
        }
    }
}
