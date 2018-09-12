using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Outstanding.Dispatcher.Controls
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
            if (btnYieldClearClick != null)
            {
                btnYieldClearClick(sender, new EventArgs());
            }
        }

        public void YieldUpdate()
        {
            lbShowFeedingOK2.Text = Yield.FeedingOK.ToString();
            lbShowBlankingOK2.Text = Yield.BlankingOK.ToString();
        }

        public void SetClearYieldTime(DateTime dateTime)
        {
            lbClearYieldTime.Text = dateTime.ToString("yyyy/M/d H:mm");
        }
    }
}
