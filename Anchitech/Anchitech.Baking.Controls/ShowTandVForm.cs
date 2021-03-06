﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TengDa;

namespace Anchitech.Baking.Controls
{
    public partial class ShowTandVForm : Form
    {
        private Floor floor;
        private TorVUC[] showTempers = new TorVUC[48];
        public ShowTandVForm(Floor floor)
        {
            InitializeComponent();

            this.floor = floor;

            this.Text = this.floor.Name + " 温度真空显示";

            this.showVacuum.Init("真空度(Pa)：");

            for (int i = 0; i < Option.TemperaturePointCount; i++)
            {
                showTempers[i] = (TorVUC)(this.Controls.Find(string.Format("showTemper01{0}", (i + 1).ToString("D2")), true)[0]);
                showTempers[i].Init("左" + Current.option.TemperNames[i] + "(℃):");
                showTempers[i + 24] = (TorVUC)(this.Controls.Find(string.Format("showTemper02{0}", (i + 1).ToString("D2")), true)[0]);
                showTempers[i + 24].Init("右" + Current.option.TemperNames[i] + "(℃):");
            }
            BtnFreshUI_Click(null, null);
        }

        private void BtnFreshUI_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Option.TemperaturePointCount; i++)
            {
                var isExTPoint0 = IsExTPoint(this.floor.Stations[0], this.floor.Stations[0].Temperatures[i]);
                showTempers[i].UpdateValue(this.floor.Stations[0].Temperatures[i], isExTPoint0);

                var isExTPoint1 = IsExTPoint(this.floor.Stations[1], this.floor.Stations[1].Temperatures[i]);
                showTempers[i + 24].UpdateValue(this.floor.Stations[1].Temperatures[i], isExTPoint1);
            }

            this.showVacuum.UpdateValue(floor.Vacuum, false);
        }

        private bool IsExTPoint(Station station, float temperature)
        {
            var tExParams = Current.option.TExParams;
            if (station.Temperatures.Count(t => t > tExParams.NL && t < tExParams.NH) >= tExParams.NC)
            {
                if (temperature < tExParams.EL || temperature > tExParams.EH)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
