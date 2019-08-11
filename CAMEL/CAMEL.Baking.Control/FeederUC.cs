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
using TengDa.WF;

namespace CAMEL.Baking.Control
{
    public partial class FeederUC : UserControl
    {
        public FeederUC()
        {
            InitializeComponent();

            if (System.Windows.SystemParameters.PrimaryScreenHeight > 800)
            {
                this.lbName.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            }
        }

        public void Init(Feeder feeder)
        {
            this.lbName.Text = feeder.Name;

            this.lbStationName1.Text = feeder.Stations[0].Name;
            this.lbStationName2.Text = feeder.Stations[1].Name;

            //this.lbFromStationName1.Text = blanker.Stations[0].FromStation.Name;
            //this.lbFromStationName2.Text = blanker.Stations[1].FromStation.Name;

            this.simpleClampUC1.Init(feeder.Stations[0]);
            this.simpleClampUC2.Init(feeder.Stations[1]);
        }

        public void Update(Feeder feeder)
        {
            feeder.IsAlive = feeder.IsEnable && feeder.Plc.IsAlive;
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

            //this.lbFromStationName1.Text = blanker.Stations[0].FromStation.Name;
            //this.lbFromStationName2.Text = blanker.Stations[1].FromStation.Name;

            this.simpleClampUC1.Update(feeder.Stations[0]);
            this.simpleClampUC2.Update(feeder.Stations[1]);
        }
    }
}
