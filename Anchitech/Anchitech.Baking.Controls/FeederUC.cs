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
    public partial class FeederUC : UserControl
    {
        public FeederUC()
        {
            InitializeComponent();
        }

        public void Init(Feeder feeder)
        {
            this.lbName.Text = feeder.Name;
            this.lbFeederStationName1.Text = feeder.Stations[0].Name;
            this.lbFeederStationName2.Text = feeder.Stations[1].Name;
            this.lbFeederStationName3.Text = feeder.Stations[2].Name;

            this.feedClampUC1.Init(feeder.Stations[0]);
            this.feedClampUC2.Init(feeder.Stations[1]);
            this.feedClampUC3.Init(feeder.Stations[2]);
        }

        public void Update(Feeder feeder)
        {
            feeder.IsAlive = Current.Feeder.IsEnable && feeder.Plc.IsAlive;
            feeder.Stations.ForEach(s => s.IsAlive = s.IsEnable && feeder.IsAlive);
            //两次离线再变灰（避免一直闪烁）
            this.BackColor = feeder.IsAlive || feeder.PreIsAlive ? Color.White : Color.LightGray;
            feeder.PreIsAlive = feeder.IsAlive;

            switch (feeder.TriLamp)
            {
                case TriLamp.Green: this.pbTriLamp.Image = Properties.Resources.Green_Round; break;
                case TriLamp.Yellow: this.pbTriLamp.Image = Properties.Resources.Yellow_Round; break;
                case TriLamp.Red: this.pbTriLamp.Image = Properties.Resources.Red_Round; break;
                case TriLamp.Unknown: this.pbTriLamp.Image = Properties.Resources.Gray_Round; break;
            }


            if (!string.IsNullOrEmpty(feeder.AlarmStr) && feeder.IsAlive)
            {
                if (feeder.PreAlarmStr != feeder.AlarmStr)
                {
                    this.lbName.Text = feeder.AlarmStr.TrimEnd(',') + "...";
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
                this.lbName.Text = feeder.Name;
                this.lbName.ForeColor = SystemColors.WindowText;
                this.lbName.BackColor = Color.Transparent;
            }
            feeder.PreAlarmStr = feeder.AlarmStr;

            this.feedClampUC1.Update(feeder.Stations[0]);
            this.feedClampUC2.Update(feeder.Stations[1]);
            this.feedClampUC3.Update(feeder.Stations[2]);
        }
    }
}
