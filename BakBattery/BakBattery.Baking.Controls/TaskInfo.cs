﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BakBattery.Baking.Controls
{
    public partial class TaskInfo : UserControl
    {
        Label[] state = new Label[7];

        public TaskInfo()
        {
            InitializeComponent();
            for (int i = 0; i < state.Length; i++)
            {
                state[i] = (Label)(this.Controls.Find("state" + (i + 1), true)[0]);
            }
        }

        public void UpdateInfo()
        {
            for (int i = 0; i < state.Length; i++)
            {
                if (state[i].Text == Current.Task.Status.ToString())
                {
                    state[i].BackColor = Color.Black;
                    state[i].ForeColor = Color.White;
                }
                else
                {
                    state[i].BackColor = Color.LightGray;
                    state[i].ForeColor = Color.DarkGreen;
                }
            }
            lbTaskName.Text = Current.Task.TaskName;
            lbFromStation.Text = Current.Task.FromStationName;
            lbToStation.Text = Current.Task.ToStationName;


        }
    }
}
