using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TengDa.WF;

namespace TengDa.WF.Controls
{
    public partial class YieldDisplay : UserControl
    {
        public YieldDisplay()
        {
            InitializeComponent();
        }

        public delegate void BtnClick(object sender, EventArgs e);

        public event BtnClick btnYieldClearClick;

        private void btnYieldClear_Click(object sender, EventArgs e)
        {
            if (btnYieldClearClick != null)
            {
                btnYieldClearClick(sender, new EventArgs());
            }
        }

        public void YieldUpdate()
        {
            lbShowFeedingOK.Text = Yield.FeedingOK.ToString();
            lbShowBlankingOK.Text = Yield.BlankingOK.ToString();
        }

        public void SetClearYieldTime(DateTime dateTime)
        {
            lbClearYieldTime.Text = dateTime.ToString("yyyy/M/d H:mm");
        }
    }
}