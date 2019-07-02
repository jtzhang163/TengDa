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

namespace BYD.AutoInjection.Controls
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
                Operation.Add("手动确认任务结束");
            }
            else
            {
                TengDa.Error.Alert("密码错误！");
            }
        }
    }
}
