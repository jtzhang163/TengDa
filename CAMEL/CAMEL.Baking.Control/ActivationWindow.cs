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
            try
            {
                // IzIwMTktMDktMDYkU2hvd0FjdGl2ZU1zZ1RpbWU9MjAxOS0wOS0xOFQwMDowMDowMDtFeHBpcmF0aW9uVGltZT0yMDIwLTA5LTE4VDEyOjAwOjAwO0lzU2hvd01zZz1GYWxzZTtJc0V4cGlyZWQ9RmFsc2U7SXNBY3RpdmF0ZWQ9RmFsc2Uj
                var activeCode = this.tbActiveCode.Text.Trim();

                // #2019-09-06$ShowActiveMsgTime=2019-09-18T00:00:00;ExpirationTime=2020-09-18T12:00:00;IsShowMsg=False;IsExpired=False;IsActivated=False#
                var codeAfterDecode = TengDa.Encrypt.Base64.DecodeBase64(activeCode);

                // 2019-09-06
                var codeNowDate = codeAfterDecode.Trim('#').Split('$')[0];
                if (!DateTime.TryParse(codeNowDate, out DateTime date))
                {
                    throw new Exception("激活码格式错误");
                }
                if (date.ToString("yyyy-MM-dd") != DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    throw new Exception("激活码参数错误");
                }

                // ShowActiveMsgTime=2019-09-18T00:00:00;ExpirationTime=2020-09-18T12:00:00;IsShowMsg=False;IsExpired=False;IsActivated=False
                var codeParams = codeAfterDecode.Trim('#').Split('$')[1];

                var paramList = codeParams.Split(';');
                if (paramList.Length != 5)
                {
                    throw new Exception("激活码格式错误");
                }

                for (int i = 0; i < 5; i++)
                {
                    var propertyName = paramList[i].Split('=')[0];
                    if (!new string[] { "ShowActiveMsgTime", "ExpirationTime", "IsShowMsg", "IsExpired", "IsActivated" }.Contains(propertyName))
                    {
                        throw new Exception("激活码格式错误");
                    }

                    var sVal = paramList[i].Split('=')[1];

                    if (propertyName.EndsWith("Time"))
                    {
                        if (!DateTime.TryParse(sVal, out DateTime dVal))
                        {
                            throw new Exception("激活码格式错误");
                        }

                        if (!Activation.SetValue(propertyName, dVal))
                        {
                            throw new Exception("设置激活码出错");
                        }
                    }
                    else
                    {
                        if (!bool.TryParse(sVal, out bool bVal))
                        {
                            throw new Exception("激活码格式错误");
                        }
                        if (!Activation.SetValue(propertyName, bVal))
                        {
                            throw new Exception("设置激活码出错");
                        }
                    }
                }
                Tip.Alert("输入验证码成功");
            }
            catch (Exception ex)
            {
                Error.Alert(ex);
            }
        }
    }
}
