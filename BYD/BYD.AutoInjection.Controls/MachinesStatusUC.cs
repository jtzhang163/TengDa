using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BYD.AutoInjection.Controls
{
    public partial class MachinesStatusUC : UserControl
    {
        public MachinesStatusUC()
        {
            InitializeComponent();
        }

        public void Init()
        {
            this.machineIndexs.Add(new MachineIndex { Machine = Current.Feeder, MsUC = this.machineStatusUC1 });
            this.machineIndexs.Add(new MachineIndex { Machine = Current.ClampScaner, MsUC = this.machineStatusUC2 });
            this.machineIndexs.Add(new MachineIndex { Machine = Current.BatteryScaner, MsUC = this.machineStatusUC3 });
            this.machineIndexs.Add(new MachineIndex { Machine = Current.Robot, MsUC = this.machineStatusUC4 });
            int machineindex = 5;
            Current.ovens.ForEach(o => { this.machineIndexs.Add(new MachineIndex { Machine = o, MsUC = (MachineStatusUC)(this.Controls.Find(string.Format("machineStatusUC{0}", machineindex++), true)[0]) }); });
            this.machineIndexs.Add(new MachineIndex { Machine = Current.Blanker, MsUC = this.machineStatusUC17 });
            this.machineIndexs.Add(new MachineIndex { Machine = Current.mes, MsUC = this.machineStatusUC18 });
            this.machineIndexs.ForEach(o => o.MsUC.Init(o.Machine));
        }

        public void SetCheckBoxEnabled(bool isEnabled)
        {
            this.machineIndexs.ForEach(o => { o.MsUC.SetCheckBoxEnabled(isEnabled); });
        }

        /// <summary>
        /// 显示设备状态灯颜色
        /// </summary>
        /// <param name="machine"></param>
        /// <param name="color">红、黄、绿、灰</param>
        public void SetLampColor(object machine,Color color)
        {
            this.machineIndexs.FirstOrDefault(o => o.Machine == machine).MsUC.SetLampColor(color);
        }

        /// <summary>
        /// 显示设备状态
        /// </summary>
        public void SetStatusInfo(object machine, string info)
        {
            this.machineIndexs.FirstOrDefault(o => o.Machine == machine).MsUC.SetStatusInfo(info);
        }

        public void SetForeColor(object machine, Color color)
        {
            this.machineIndexs.FirstOrDefault(o => o.Machine == machine).MsUC.SetForeColor(color);
        }

        public void SetBackColor(object machine, Color color)
        {
            this.machineIndexs.FirstOrDefault(o => o.Machine == machine).MsUC.SetBackColor(color);
        }

        public string GetStatusInfo(object machine)
        {
            return this.machineIndexs.FirstOrDefault(o => o.Machine == machine).MsUC.GetStatusInfo();
        }
    }
}
