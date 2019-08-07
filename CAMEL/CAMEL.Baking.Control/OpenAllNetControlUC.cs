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

namespace CAMEL.Baking.Control
{
    /// <summary>
    /// 打开全部网控
    /// </summary>
    public partial class OpenAllNetControlUC : UserControl
    {
        public OpenAllNetControlUC()
        {
            InitializeComponent();
        }

        private void BtnOpenAllNetControl_Click(object sender, EventArgs e)
        {
            Operation.Add("点击打开全部网控");
            Current.ovens.Where(o => o.IsAlive).ToList().ForEach(o =>
            {
                o.Floors.ForEach(f =>
                {
                    if (!f.IsNetControlOpen)
                    {
                        f.toOpenNetControl = true;
                    }
                });
            });
        }
    }
}
