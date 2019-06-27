using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TengDa.WF;

namespace Anchitech.Baking.Controls
{
    public partial class FinishTaskConfirmForm : Form
    {
        public FinishTaskConfirmForm()
        {
            InitializeComponent();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (this.tbPwd.Text.Trim() == DateTime.Now.ToString("HHmm"))
            {
                Current.option.TaskIsFinished = true;
                Tip.Alert("确认任务结束OK！");
            }
            else
            {
                
            }
        }
    }
}
