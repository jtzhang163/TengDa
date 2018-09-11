using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using TengDa;
using TengDa.WF;

namespace Veken.Baking.App
{
    public partial class OutOvenForm : Form
    {
        int i = -1, j = -1;
        int clampNum = 0;

        ManageSystem ms = null;

        public OutOvenForm(int i, int j, ManageSystem ms)
        {
            InitializeComponent();
            this.i = i;
            this.j = j;
            this.ms = ms;
            this.Text = string.Format("腔体 {0} 出腔扫码", Current.ovens[i].Floors[j].Name);
            this.lbFloorNumber.Text = Current.ovens[i].Floors[j].Number;
            this.lvClampCodes.Columns.Clear();
            this.lvClampCodes.Items.Clear();

            this.lvClampCodes.Columns.Add("序号");
            this.lvClampCodes.Columns.Add("料盒条码");
            this.lvClampCodes.Columns.Add("电池数");
            this.lvClampCodes.Columns[0].Width = 50;
            this.lvClampCodes.Columns[1].Width = 140;
            this.lvClampCodes.Columns[2].Width = 60;

            clampCodes.Clear();
            //填充ListView
            foreach (Clamp c in Current.ovens[i].Floors[j].Clamps)
            {
                ListViewItem li = new ListViewItem();//创建行对象
                li.Text = (++clampNum).ToString();
                li.SubItems.Add(c.Code);
                li.SubItems.Add(c.Batteries.Count.ToString());
                this.lvClampCodes.Items.Add(li);
                clampCodes.Add(c.Code);
            }

            this.lbTip.Text = "等待扫码！";
            Current.IsOutOvenFormShow = true;
        }

        private void OutOvenForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.lvClampCodes.Dispose();
            Current.IsOutOvenFormShow = false;
        }

        private List<string> clampCodes = new List<string>();

        public void SetTbClampCode(string code)
        {
            this.tbClampCode.Text = code;
            this.tbClampCode.ForeColor = Color.Lime;

            if (!clampCodes.Contains(code))
            {
                this.lbTip.Text = "料盒已出，或腔体中没有该料盒：" + code;
                this.lbTip.ForeColor = Color.Red;
            }
            else
            {
                this.lbTip.Text = "获得条码 " + code;
                this.lbTip.ForeColor = Color.LightGreen;
                Thread thread = new Thread(new ParameterizedThreadStart(OutOven));
                thread.Start(code);
            }

            System.Timers.Timer t = new System.Timers.Timer(3000);
            t.Elapsed += new System.Timers.ElapsedEventHandler(ClearClampCode);
            t.AutoReset = false;
            t.Enabled = true;
        }

        private void ClearClampCode(object source, System.Timers.ElapsedEventArgs e)
        {
            if (this.IsHandleCreated)
            {
                this.BeginInvoke(new MethodInvoker(() => { this.tbClampCode.Text = string.Empty; }));
            }
        }

        private object locker = new object();

        private void tbClampCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                string code = tbClampCode.Text.Trim();
                if (Regex.IsMatch(code, Current.option.ClampCodeRegexStr))
                {
                    SetTbClampCode(code);
                }
                else
                {
                    Error.Alert("输入条码格式不正确！ " + code);
                }
            }
        }

        private void OutOven(object obj)
        {
            string code = obj.ToString();

            Clamp clamp = Current.ovens[i].Floors[j].Clamps.Single(c => c.Code == code);

            string msg = string.Empty;
            lock (locker)
            {
                try
                {
                    if (Current.mes.IsAlive)
                    {
                        if (Current.mes.IsOffline) { Current.mes.IsOffline = false; }

                        this.BeginInvoke(new MethodInvoker(() => { this.lbTip.Text = code + " 正在上传料盒信息..."; this.lbTip.ForeColor = Color.LightGreen; }));
                        if (!MES.UploadCellInfo(clamp))
                        {
                            this.BeginInvoke(new MethodInvoker(() => { this.lbTip.Text = string.Format("{0}: {1}", code, "上传失败"); this.lbTip.ForeColor = Color.Red; }));
                            LogHelper.WriteError(string.Format("{0}: {1}", code, "上传失败"));
                            return;
                        }
                        this.BeginInvoke(new MethodInvoker(() => { this.lbTip.Text = code + " 上传MES成功"; this.lbTip.ForeColor = Color.LightGreen; }));
                        LogHelper.WriteInfo(code + " 上传MES成功");
                        Thread.Sleep(100);
                    }
                    else
                    {

                        if (TengDa.WF.Current.user.Group.Level > 2 && !Current.mes.IsOffline)
                        {
                            Current.mes.IsOffline = true;
                        }

                        if (!Current.mes.IsOffline)
                        {
                            MesOfflineVerify m = new MesOfflineVerify();

                            DialogResult dr = m.ShowDialog();
                            if (dr == DialogResult.OK)
                            {
                                if (!Current.mes.IsOffline)
                                {
                                    Current.mes.IsOffline = true;
                                }
                            }
                            else
                            {
                                Error.Alert("无法连接至MES，无法入腔！");
                                return;
                            }
                        }

                    }

                    Yield.BlankingOK += clamp.Batteries.Count;
                    Current.ovens[i].Floors[j].Clamps.Remove(clamp);
                    string clampids = string.Empty;
                    foreach (Clamp c in Current.ovens[i].Floors[j].Clamps)
                    {
                        clampids += c.Id + ",";
                    }
                    Current.ovens[i].Floors[j].ClampIds = clampids.TrimEnd(',');

                    this.BeginInvoke(new MethodInvoker(() =>
                    {
                        foreach(ListViewItem li in this.lvClampCodes.Items)
                        {
                            foreach (ListViewItem.ListViewSubItem subli in li.SubItems)
                            {
                                if (subli.Text == code)
                                {
                                    this.lvClampCodes.Items.Remove(li);
                                }
                            }
                        }                      
                    }));

                    clampCodes.Remove(code);
                }
                catch (Exception ex)
                {
                    Error.Alert(ex.ToString());
                }
            }
        }
    }
}
