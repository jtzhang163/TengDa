using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TengDa;

namespace Anchitech.Baking.Controls
{
    public partial class OvenUC : UserControl
    {
        public OvenUC()
        {
            InitializeComponent();
        }

        public void Init(Oven oven)
        {
            this.lbName.Text = oven.Name;
        }

        public void Update(Oven oven)
        {
            oven.IsAlive = oven.IsEnable && oven.Plc.IsAlive;
            oven.Floors.ForEach(f => f.IsAlive = f.IsEnable && (oven.IsAlive || oven.PreIsAlive));
            oven.Floors.ForEach(f => f.Stations.ForEach(s => s.IsAlive = s.IsEnable && f.IsAlive));

            //两次离线再变灰（避免一直闪烁）
            this.BackColor = oven.IsAlive || oven.PreIsAlive ? Color.White : Color.LightGray;
            oven.PreIsAlive = oven.IsAlive;

            switch (oven.TriLamp)
            {
                case TriLamp.Green: this.pbTriLamp.Image = Properties.Resources.Green_Round; break;
                case TriLamp.Yellow: this.pbTriLamp.Image = Properties.Resources.Yellow_Round; break;
                case TriLamp.Red: this.pbTriLamp.Image = Properties.Resources.Red_Round; break;
                case TriLamp.Unknown: this.pbTriLamp.Image = Properties.Resources.Gray_Round; break;
            }

            if (!string.IsNullOrEmpty(oven.AlarmStr) && oven.Plc.IsAlive)
            {
                if (oven.PreAlarmStr != oven.AlarmStr)
                {
                    this.lbName.Text = oven.AlarmStr.TrimEnd(',') + "...";
                }
                else
                {
                    string alarmStr = this.lbName.Text;
                    this.lbName.Text = alarmStr.Substring(1, alarmStr.Length - 1) + alarmStr.Substring(0, 1);
                }

                this.lbName.ForeColor = Color.White;
                this.lbName.BackColor = Color.Red;
            }
            else
            {
                this.lbName.Text = oven.Name;
                this.lbName.ForeColor = SystemColors.WindowText;
                this.lbName.BackColor = Color.Transparent;
            }

            oven.PreIsAlive = oven.IsAlive;
            oven.Floors.ForEach(f => f.PreIsAlive = f.IsAlive);
            oven.Floors.ForEach(f => f.Stations.ForEach(s => s.PreIsAlive = s.IsAlive));

        }
    }
}
