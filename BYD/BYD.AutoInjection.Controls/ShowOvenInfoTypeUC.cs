using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BYD.AutoInjection.Controls
{
    public partial class ShowOvenInfoTypeUC : UserControl
    {
        public ShowOvenInfoTypeUC()
        {
            InitializeComponent();
            this.cbShowType.SelectedIndex = 0;
        }

        private void CbShowType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TengDa.WF.Current.IsRunning)
            {
                Current.option.FloorShowInfoType = this.cbShowType.SelectedItem.ToString();
            }
        }
    }
}
