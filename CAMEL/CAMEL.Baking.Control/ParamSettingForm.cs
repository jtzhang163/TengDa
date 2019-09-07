using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TengDa;

namespace CAMEL.Baking.Control
{
    public partial class ParamSettingForm : Form
    {
        private Floor floor;
        private OvenParamUC[] ovenParamUCs = new OvenParamUC[20];
        public ParamSettingForm(Floor floor)
        {
            InitializeComponent();

            this.floor = floor;

            this.Text = this.floor.Name + " 参数设置";

            this.lbGetStatus.Text = "";
            this.lbSetStatus.Text = "";
            this.btnSetParam.Enabled = false;

            for (int i = 0; i < ovenParamUCs.Length; i++)
            {
                ovenParamUCs[i] = (OvenParamUC)(this.Controls.Find(string.Format("ovenParamUC{0}", (i + 1).ToString("D3")), true)[0]);
                ovenParamUCs[i].Init(OvenParam.OvenParamList[i]);
            }

        }

        private void BtnGetParam_Click(object sender, EventArgs e)
        {
            try
            {
                Thread t = new Thread(() =>
                {
                    this.BeginInvoke(new MethodInvoker(() =>
                    {
                        this.btnGetParam.Enabled = false;
                        this.lbGetStatus.Text = "获取设备参数...";
                    }));
                    Thread.Sleep(1000);

                    var isSuccess = true;
                    var msg = "";

                    var oven = this.floor.GetOven();

                    for (int i = 0; i < this.ovenParamUCs.Length; i++)
                    {
                        var ii = i;
                        var addr = "";
                        var j = oven.Floors.IndexOf(this.floor);

                        addr =
                        j == 0 ? this.ovenParamUCs[ii].ovenParam.Floor1Addr :
                        j == 1 ? this.ovenParamUCs[ii].ovenParam.Floor2Addr :
                        j == 2 ? this.ovenParamUCs[ii].ovenParam.Floor3Addr :
                        j == 3 ? this.ovenParamUCs[ii].ovenParam.Floor4Addr :
                        j == 4 ? this.ovenParamUCs[ii].ovenParam.Floor5Addr : "";

                        if (oven.GetParam(addr, out int val, out msg))
                        {
                            if (this.ovenParamUCs[ii].ovenParam.Unit == "℃")
                            {
                                val /= 10;
                            }
                            this.BeginInvoke(new MethodInvoker(() =>
                            {
                                this.ovenParamUCs[ii].SetOldValue(val);
                                this.ovenParamUCs[ii].SetNewValue(val);
                            }));
                        }
                        else
                        {
                            isSuccess = false;
                            break;
                        }

                    }

                    if (isSuccess)
                    {
                        this.BeginInvoke(new MethodInvoker(() =>
                        {
                            this.lbGetStatus.ForeColor = Color.LimeGreen;
                            this.lbGetStatus.Text = "成功获取完设备参数";
                            this.btnSetParam.Enabled = true;
                        }));
                    }
                    else
                    {
                        this.BeginInvoke(new MethodInvoker(() =>
                        {
                            this.lbGetStatus.Text = msg;
                            this.lbGetStatus.ForeColor = Color.Red;
                        }));
                    }

                    this.BeginInvoke(new MethodInvoker(() =>
                    {
                        this.btnGetParam.Enabled = true;
                    }));

                });
                t.Start();
            }
            catch (Exception ex)
            {
                Error.Alert(ex);
            }
        }

        private void BtnSetParam_Click(object sender, EventArgs e)
        {
            try
            {
                Thread t = new Thread(() =>
                {
                    this.BeginInvoke(new MethodInvoker(() =>
                    {
                        this.btnSetParam.Enabled = false;
                        this.lbSetStatus.Text = "更新设备参数...";
                    }));
                    Thread.Sleep(1000);

                    var isSuccess = true;
                    var msg = "";

                    var oven = this.floor.GetOven();

                    for (int i = 0; i < this.ovenParamUCs.Length; i++)
                    {
                        var addr = "";
                        var j = oven.Floors.IndexOf(this.floor);
                        addr =
                        j == 0 ? this.ovenParamUCs[i].ovenParam.Floor1Addr :
                        j == 1 ? this.ovenParamUCs[i].ovenParam.Floor2Addr :
                        j == 2 ? this.ovenParamUCs[i].ovenParam.Floor3Addr :
                        j == 3 ? this.ovenParamUCs[i].ovenParam.Floor4Addr :
                        j == 4 ? this.ovenParamUCs[i].ovenParam.Floor5Addr : "";

                        if (this.ovenParamUCs[i].GetNewValue() == this.ovenParamUCs[i].GetOldValue())
                        {
                            continue;
                        }

                        if (this.ovenParamUCs[i].GetNewValue() < 0)
                        {
                            isSuccess = false;
                            msg = "输入参数有误";
                            break;
                        }

                        var val = this.ovenParamUCs[i].GetNewValue();
                        if (this.ovenParamUCs[i].ovenParam.Unit == "℃")
                        {
                            val *= 10;
                        }
                        if (!oven.SetParam(addr, val, out msg))
                        {
                            isSuccess = false;
                            break;
                        }
                    }

                    if (isSuccess)
                    {
                        this.BeginInvoke(new MethodInvoker(() =>
                        {
                            this.lbSetStatus.ForeColor = Color.LimeGreen;
                            this.lbSetStatus.Text = "成功更新完设备参数";
                            this.btnSetParam.Enabled = true;
                        }));
                    }
                    else
                    {
                        this.BeginInvoke(new MethodInvoker(() =>
                        {
                            this.lbSetStatus.Text = msg;
                            this.lbSetStatus.ForeColor = Color.Red;
                        }));
                    }

                    this.BeginInvoke(new MethodInvoker(() =>
                    {
                        this.btnSetParam.Enabled = true;
                    }));

                });
                t.Start();
            }
            catch (Exception ex)
            {
                Error.Alert(ex);
            }
        }

        private void BtnGetDefaultValue_Click(object sender, EventArgs e)
        {
            this.btnGetDefaultValue.Enabled = false;
            for (int i = 0; i < this.ovenParamUCs.Length; i++)
            {
                this.ovenParamUCs[i].SetNewValue(this.ovenParamUCs[i].ovenParam.DefaultValue);
            }
            this.btnGetDefaultValue.Enabled = true;
        }

        private void BtnSetDefaultValue_Click(object sender, EventArgs e)
        {
            this.btnSetDefaultValue.Enabled = false;
            for (int i = 0; i < this.ovenParamUCs.Length; i++)
            {
                this.ovenParamUCs[i].ovenParam.DefaultValue = this.ovenParamUCs[i].GetNewValue();
            }
            this.btnSetDefaultValue.Enabled = true;
        }
    }
}
