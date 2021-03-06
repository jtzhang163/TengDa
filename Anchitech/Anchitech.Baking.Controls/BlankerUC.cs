﻿using System;
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

namespace Anchitech.Baking.Controls
{
    public partial class BlankerUC : UserControl
    {
        public BlankerUC()
        {
            InitializeComponent();
        }

        public void Init(Blanker blanker)
        {
            this.lbName.Text = blanker.Name;

            this.lbStationName1.Text = blanker.Stations[0].Name;
            this.lbStationName2.Text = blanker.Stations[1].Name;

            //this.lbFromStationName1.Text = blanker.Stations[0].FromStation.Name;
            //this.lbFromStationName2.Text = blanker.Stations[1].FromStation.Name;

            this.simpleClampUC2.Init(blanker.Stations[0]);
            this.simpleClampUC1.Init(blanker.Stations[1]);

            this.tsmPutFinished1.Text = blanker.Stations[0].Name + "放盘完成";
            this.tsmPutFinished2.Text = blanker.Stations[1].Name + "放盘完成";
        }

        public void Update(Blanker blanker)
        {
            blanker.IsAlive = blanker.IsEnable && blanker.Plc.IsAlive;
            blanker.Stations.ForEach(s => s.IsAlive = s.IsEnable && blanker.IsAlive);
            //两次离线再变灰（避免一直闪烁）
            this.BackColor = blanker.IsAlive || blanker.PreIsAlive ? Color.White : Color.LightGray;
            blanker.PreIsAlive = blanker.IsAlive;

            switch (blanker.TriLamp)
            {
                case TriLamp.Green: this.pbTriLamp.Image = Properties.Resources.Green_Round; break;
                case TriLamp.Yellow: this.pbTriLamp.Image = Properties.Resources.Yellow_Round; break;
                case TriLamp.Red: this.pbTriLamp.Image = Properties.Resources.Red_Round; break;
                case TriLamp.Unknown: this.pbTriLamp.Image = Properties.Resources.Gray_Round; break;
            }


            if (!string.IsNullOrEmpty(blanker.AlarmStr) && blanker.IsAlive)
            {
                if (blanker.PreAlarmStr != blanker.AlarmStr)
                {
                    this.lbName.Text = blanker.AlarmStr.TrimEnd(',') + "...";
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
                this.lbName.Text = blanker.Name;
                this.lbName.ForeColor = SystemColors.WindowText;
                this.lbName.BackColor = Color.Transparent;
            }
            blanker.PreAlarmStr = blanker.AlarmStr;

            //this.lbFromStationName1.Text = blanker.Stations[0].FromStation.Name;
            //this.lbFromStationName2.Text = blanker.Stations[1].FromStation.Name;

            this.simpleClampUC1.Update(blanker.Stations[0]);
            this.simpleClampUC2.Update(blanker.Stations[1]);
        }

        private void CmsBlanker_Opening(object sender, CancelEventArgs e)
        {
            this.tsmPutFinished1.Enabled = Current.Blanker.IsAlive && Current.Blanker.Stations[0].ClampStatus == ClampStatus.满夹具;
            this.tsmPutFinished2.Enabled = Current.Blanker.IsAlive && Current.Blanker.Stations[1].ClampStatus == ClampStatus.满夹具;
        }

        private void TsmPutFinished_Click(object sender, EventArgs e)
        {
            if ((sender as ToolStripMenuItem).Name == "tsmPutFinished1")
            {
                Operation.Add(string.Format("手动点击{0}放盘完成", Current.Blanker.Stations[0].Name));
                Current.Blanker.SetPutClampFinish(0);
            }
            else
            {
                Operation.Add(string.Format("手动点击{0}放盘完成", Current.Blanker.Stations[1].Name));
                Current.Blanker.SetPutClampFinish(1);
            }
        }
    }
}
