using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TengDa.WF;
using TengDa;

namespace Anchitech.Baking.Controls
{
    /// <summary>
    /// 打开全部网控
    /// </summary>
    public partial class BatteryBindInitUC : UserControl
    {
        public BatteryBindInitUC()
        {
            InitializeComponent();
        }

        private void BtnBatteryBindInit_Click(object sender, EventArgs e)
        {
            if (Battery.BatteryBindInit(out string msg))
            {
                Tip.Alert("电池绑定初始化成功！");
            }
            else
            {
                Error.Alert("电池绑定初始化失败！msg:" + msg);
            }
        }
    }
}
