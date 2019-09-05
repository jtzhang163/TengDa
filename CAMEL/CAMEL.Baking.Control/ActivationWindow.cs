using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TengDa;
using TengDa.WF;

namespace CAMEL.Baking.Control
{
    public partial class ActivationWindow : Form
    {
        public ActivationWindow()
        {
            InitializeComponent();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            var activeCode = this.tbActiveCode.Text.Trim();
            var codeAfterDecode = TengDa.Encrypt.Base64.DecodeBase64(activeCode);
            var codes = codeAfterDecode.Split('_');
            if (codes.Length < 3)
            {
                Error.Alert("激活码格式错误！");
                return;
            }

            DateTime.TryParse(codes[0], out DateTime date);
            if (date.ToString("yyyy-MM-dd") != DateTime.Now.ToString("yyyy-MM-dd"))
            {
                Error.Alert("激活码参数错误！");
                return;
            }

            var propertyName = codes[1];
            if (!new string[] { "ShowActiveMsgTime", "ExpirationTime", "IsShowMsg", "IsExpired", "IsActivated" }.Contains(propertyName))
            {
                Error.Alert("激活码参数错误！");
                return;
            }

            var sVal = codes[2];

            if (propertyName.EndsWith("Time"))
            {
                if (DateTime.TryParse(sVal, out DateTime dVal))
                {
                    if(Activation.SetValue(propertyName, dVal))
                    {
                        Tip.Alert("设置激活码成功！");
                        return;
                    }
                    else
                    {
                        Error.Alert("设置激活码出错！");
                        return;
                    }
                }
                else
                {
                    Error.Alert("激活码参数错误！");
                    return;
                }
            }
            else
            {
                if (bool.TryParse(sVal, out bool bVal))
                {
                    if (Activation.SetValue(propertyName, bVal))
                    {
                        Tip.Alert("设置激活码成功！");
                        return;
                    }
                    else
                    {
                        Error.Alert("设置激活码出错！");
                        return;
                    }
                }
                else
                {
                    Error.Alert("激活码参数错误！");
                    return;
                }
            }     
        }
    }
}
