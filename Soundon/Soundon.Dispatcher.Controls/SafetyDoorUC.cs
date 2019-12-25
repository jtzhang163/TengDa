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
        public SafetyDoorUC()
        {
            InitializeComponent();
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