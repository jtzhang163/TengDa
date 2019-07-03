using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BYD.Scan.Controls
{
    public partial class OvenParamUC : UserControl
    {
        public OvenParam ovenParam;

        public OvenParamUC()
        {
            InitializeComponent();
        }
        public void Init(OvenParam ovenParam)
        {
            this.ovenParam = ovenParam;
            this.lbContent.Text = string.Format("{0}({1})", ovenParam.Content, ovenParam.Unit);
        }

        public void SetOldValue(int val)
        {
            this.tbOldValue.Text = val.ToString();
        }

        public void SetNewValue(int val)
        {
            this.tbNewValue.Text = val.ToString();
        }

        public int GetOldValue()
        {
            return TengDa._Convert.StrToInt(this.tbOldValue.Text.Trim(), -1);
        }

        public int GetNewValue()
        {
            return TengDa._Convert.StrToInt(this.tbNewValue.Text.Trim(), -1);
        }

        private void TbNewValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b' && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
