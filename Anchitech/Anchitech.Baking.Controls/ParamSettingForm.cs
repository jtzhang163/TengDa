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

namespace Anchitech.Baking.Controls
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
                    var addr = 0;
                    var j = oven.Floors.IndexOf(this.floor);
                    if (j == 0)
                    {
                        addr = this.ovenParamUCs[i].ovenParam.Floor1Addr;
                    }
                    else if (j == 1)
                    {
                        addr = this.ovenParamUCs[i].ovenParam.Floor2Addr;
                    }
                    else if (j == 2)
                    {
                        addr = this.ovenParamUCs[i].ovenParam.Floor3Addr;
                    }

                    if (oven.GetParam(addr, out int val, out msg))
                    {
                        this.BeginInvoke(new MethodInvoker(() =>
                        {
                            this.ovenParamUCs[i].SetOldValue(val);
                            this.ovenParamUCs[i].SetNewValue(val);
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
                    Thread.Sleep(1000);
                    this.BeginInvoke(new MethodInvoker(() => { this.lbGetStatus.Text = ""; }));
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

        private void BtnSetParam_Click(object sender, EventArgs e)
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
                    var addr = 0;
                    var j = oven.Floors.IndexOf(this.floor);
                    if (j == 0)
                    {
                        addr = this.ovenParamUCs[i].ovenParam.Floor1Addr;
                    }
                    else if (j == 1)
                    {
                        addr = this.ovenParamUCs[i].ovenParam.Floor2Addr;
                    }
                    else if (j == 2)
                    {
                        addr = this.ovenParamUCs[i].ovenParam.Floor3Addr;
                    }

                    if(this.ovenParamUCs[i].GetNewValue() == this.ovenParamUCs[i].GetOldValue())
                    {
                        continue;
                    }

                    if (this.ovenParamUCs[i].GetNewValue() < 0)
                    {
                        isSuccess = false;
                        msg = "输入参数有误";
                        break;
                    }

                    if (!oven.SetParam(addr, this.ovenParamUCs[i].GetNewValue(), out msg))
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
                    Thread.Sleep(1000);
                    this.BeginInvoke(new MethodInvoker(() => { this.lbSetStatus.Text = ""; }));
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
    }
}
