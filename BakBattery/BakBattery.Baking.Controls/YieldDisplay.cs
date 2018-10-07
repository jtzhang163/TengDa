using System;
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
    public partial class YieldDisplay: UserControl
    {
        public YieldDisplay()
        {
            InitializeComponent();

        }


        public delegate void BtnClick(object sender, EventArgs e);

        public event BtnClick btnYieldClearClick;

        private void btnYieldClear_Click(object sender, EventArgs e)
        {
            btnYieldClearClick?.Invoke(sender, new EventArgs());
        }

        public void YieldUpdate()
        {
            lbShowFeedingOK.Text = Current.Yields[0].FeedingOK.ToString();
            lbShowBlankingOK.Text = Current.Yields[0].BlankingOK.ToString();
        }

        public void SetYieldType()
        {

        }

        public void SetClearYieldTime(DateTime dateTime)
        {
            lbClearYieldTime.Text = dateTime.ToString("yyyy/M/d H:mm");
        }
    }
}
