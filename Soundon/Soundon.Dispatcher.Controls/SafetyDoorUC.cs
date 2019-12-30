using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Soundon.Dispatcher.Controls
{
    public partial class SafetyDoorUC : UserControl
    {

        private string[] pbNames =
        {
            "大机器人就绪",
            "大机器人运行中",
            "安全防护",
            "电箱实际急停状态",
            "3号上料门打开",
            "3号下料门打开",
            "4号上料门打开",
            "4号下料门打开",
            "4号小门打开",
            "3号上料急停",
            "3号下料急停",
            "4号上料急停",
            "4号下料急停"
        };

        public SafetyDoorUC()
        {
            InitializeComponent();
        }

        public void Init()
        {
            var isLine34 = Current.feeders[0].Name.Contains("3");
            this.toolTip.SetToolTip(this.pbD1105, "电箱实际急停状态");
            for (int i = 0; i < 13; i++)
            {
                PictureBox pb = (PictureBox)this.Controls.Find("pbD" + (i + 1102), true)[0];
                var toolTip = isLine34 ? pbNames[i] : pbNames[i].Replace('3', '5').Replace('4', '6');
                this.toolTip.SetToolTip(pb, toolTip);
            }
        }

        public void UpdateUI()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(SafetyDoorUC));
            Type robotType = typeof(Robot);
            Type safetyDoorType = typeof(SafetyDoor);
            Type pictBoxType = typeof(PictureBox);
            for (int i = 1102; i < 1115; i++)
            {
                var flag = false;
                if (i < 1105)
                {
                    flag = (bool)robotType.GetProperty("D" + i).GetValue(Current.Robot);
                }
                else
                {
                    flag = (bool)safetyDoorType.GetProperty("D" + i).GetValue(Current.SafetyDoor);
                }
                var img = flag ? Properties.Resources.Green_Round : Properties.Resources.Red_Round;
                pictBoxType.GetProperty("BackgroundImage").SetValue(this.Controls.Find("pbD" + i, true)[0], img);
            }
        }
    }
}