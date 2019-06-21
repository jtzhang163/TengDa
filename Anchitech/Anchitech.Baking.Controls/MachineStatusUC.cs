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
        private object Machine;

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
            this.Machine = machine;
            this.lbMachineName.Text = (string)GetProperty(machine, "Name");
            this.cbIsEnable.Checked = (bool)GetProperty(machine, "IsEnable");
        }

        /// <summary>
        /// 显示设备状态灯颜色
        /// </summary>
        public void SetLampColor(Color color)
        {
            if (color == Color.Green) { this.pbLamp.Image = Properties.Resources.Green_Round; }
            else if (color == Color.Red) { this.pbLamp.Image = Properties.Resources.Red_Round; }
            else if (color == Color.Gray) { this.pbLamp.Image = Properties.Resources.Gray_Round; }
            else if (color == Color.Yellow) { this.pbLamp.Image = Properties.Resources.Yellow_Round; }
        }

        public string GetStatusInfo()
        {
            return this.tbStatus.Text;
        }

        /// <summary>
        /// 显示设备状态
        /// </summary>
        public void SetStatusInfo(string info)
        {
            this.tbStatus.Text = info;
        }

        public void SetForeColor(Color color)
        {
            this.tbStatus.ForeColor = color;
        }

        public void SetBackColor(Color color)
        {
            this.tbStatus.BackColor = color;
        }

        public object GetProperty(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName).GetValue(obj);
        }

        private void CbIsEnable_CheckedChanged(object sender, EventArgs e)
        {
            var isEnable = (sender as CheckBox).Checked;
            this.Machine.GetType().GetProperty("IsEnable").SetValue(this.Machine, isEnable);
        }
    }
}
