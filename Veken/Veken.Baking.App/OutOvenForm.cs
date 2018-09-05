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

namespace Veken.Baking.App
{
    public partial class OutOvenForm : Form
    {
        int i = -1, j = -1;
        static int clampNum = 0;

        ManageSystem ms = null;

        public OutOvenForm(int i, int j, ManageSystem ms)
        {
            InitializeComponent();
            this.i = i;
            this.j = j;
            this.ms = ms;
            this.Text = string.Format("腔体 {0} 入腔扫码", Current.ovens[i].Floors[j].Name);
            this.lbFloorNumber.Text = Current.ovens[i].Floors[j].Number;
            this.lvClampCodes.Columns.Clear();
            this.lvClampCodes.Items.Clear();

            this.lvClampCodes.Columns.Add("序号");
            this.lvClampCodes.Columns.Add("料盒条码");
            this.lvClampCodes.Columns.Add("电池数");
            this.lvClampCodes.Columns[0].Width = 50;
            this.lvClampCodes.Columns[1].Width = 140;
            this.lvClampCodes.Columns[2].Width = 60;

            this.lbTip.Text = "等待扫码！";
            clampCodes.Clear();
            Current.IsInOvenFormShow = true;
        }

        private void InOvenForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.lvClampCodes.Dispose();
            Current.IsInOvenFormShow = false;
        }

        private List<string> clampCodes = new List<string>();

        public void SetTbClampCode(string code)
        {
            this.tbClampCode.Text = code;
            this.tbClampCode.ForeColor = Color.Lime;

            if (Current.ovens[i].Floors[j].Clamps.Count(c => c.Code == code) > 0)
            {
                this.lbTip.Text = "腔体中已有料盒：" + code;
                this.lbTip.ForeColor = Color.Red;
            }
            else if (!clampCodes.Contains(code))
            {
                this.lbTip.Text = "获得条码 " + code;
                this.lbTip.ForeColor = Color.LightGreen;

                Thread thread = new Thread(new ParameterizedThreadStart(InOven));
                thread.Start(code + "," + clampNum);
            }
            else
            {
                this.lbTip.Text = "重复扫码";
                this.lbTip.ForeColor = Color.Red;
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

        private void InOven(object obj)
        {
            string code = obj.ToString().Split(',')[0];
            int index = TengDa._Convert.StrToInt(obj.ToString().Split(',')[1], -1);

            string msg = string.Empty;
            string techNo = string.Empty;
            lock (locker)
            {
                try
                {
                    List<Cell> cells = new List<Cell>();
                    List<TechStandard> techStandards = new List<TechStandard>();
                    if (Current.mes.IsAlive)
                    {
                        if (Current.mes.IsOffline) { Current.mes.IsOffline = false; }

                        this.BeginInvoke(new MethodInvoker(() => { this.lbTip.Text = code + " 正在获取电芯数据和工艺参数..."; this.lbTip.ForeColor = Color.LightGreen; }));
                        if (!MES.GetInfo(Current.ovens[i].Floors[j].Number, code, out techNo, out cells, out techStandards, out msg))
                        {
                            this.BeginInvoke(new MethodInvoker(() => { this.lbTip.Text = string.Format("{0}: {1}", code, msg); this.lbTip.ForeColor = Color.Red; }));
                            LogHelper.WriteError(string.Format("{0}: {1}", code, msg));
                            return;
                        }
                        this.BeginInvoke(new MethodInvoker(() => { this.lbTip.Text = code + " 获取数据成功"; this.lbTip.ForeColor = Color.LightGreen; }));
                        LogHelper.WriteInfo(code + " 获取数据成功");
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

                    //料盒/托盘保存到数据库
                    Clamp clamp = new Clamp();
                    clamp.Code = code;
                    clamp.EnterTime = DateTime.Now;
                    clamp.FloorId = Current.ovens[i].Floors[j].Id;
                    clamp.Location = index.ToString();
                    clamp.OutTime = TengDa.Common.DefaultTime;
                    clamp.BakingStartTime = TengDa.Common.DefaultTime;
                    clamp.BakingStopTime = TengDa.Common.DefaultTime;
                    clamp.isUploaded = false;
                    clamp.isFinished = false;
                    clamp.techNo = techNo;

                    int clampid = Clamp.Add(clamp, out msg);
                    if (clampid < 1)
                    {
                        Error.Alert("clampid < 1");
                        return;
                    }

                    Current.ovens[i].Floors[j].Clamps.Add(new Clamp(clampid));
                    string clampids = string.Empty;
                    foreach (Clamp c in Current.ovens[i].Floors[j].Clamps)
                    {
                        clampids += c.Id + ",";
                    }
                    Current.ovens[i].Floors[j].ClampIds = clampids.TrimEnd(',');

                    this.BeginInvoke(new MethodInvoker(() =>
                    {
                        ListViewItem li = new ListViewItem();//创建行对象
                        li.Text = (++clampNum).ToString();
                        li.SubItems.Add(code);
                        li.SubItems.Add("");
                        this.lvClampCodes.Items.Add(li);
                    }));

                    clampCodes.Add(code);

                    if (Current.mes.IsAlive)
                    {

                        //电芯数据保存到数据库
                        List<Battery> batteries = new List<Battery>();
                        for (int ii = 0; ii < cells.Count; ii++)
                        {
                            Battery battery = new Battery();
                            battery.Code = cells[ii].sfc_no;
                            battery.ClampId = clampid;
                            battery.Location = cells[ii].seq_no;
                            batteries.Add(battery);
                        }


                        this.BeginInvoke(new MethodInvoker(() =>
                        {
                            ListViewItem li_n = lvClampCodes.Items.Cast<ListViewItem>().First(x => x.SubItems[1].Text == code);
                            if (li_n != null)
                            {
                                li_n.SubItems[2].Text = batteries.Count.ToString();
                            }
                        }));


                        if (!Battery.Add(batteries, out msg))
                        {
                            this.BeginInvoke(new MethodInvoker(() => { this.lbTip.Text = code + " 电芯数据存入数据库失败！"; this.lbTip.ForeColor = Color.Red; }));
                            LogHelper.WriteError(code + " 电芯数据存入数据库失败");
                            return;
                        }
                        this.BeginInvoke(new MethodInvoker(() => { this.lbTip.Text = code + " 电芯数据存入数据库成功！"; this.lbTip.ForeColor = Color.LightGreen; }));
                        LogHelper.WriteInfo(code + " 电芯数据存入数据库成功！");
                        Thread.Sleep(100);

                        //工艺参数保存到数据库
                        if (!TechStandard4DB.Add(techStandards, clampid, out msg))
                        {
                            this.BeginInvoke(new MethodInvoker(() => { this.lbTip.Text = code + " 工艺参数存入数据库失败！"; this.lbTip.ForeColor = Color.Red; }));
                            LogHelper.WriteError(code + " 工艺参数存入数据库失败");
                            return;
                        }
                        this.BeginInvoke(new MethodInvoker(() => { this.lbTip.Text = code + " 工艺参数存入数据库成功！"; this.lbTip.ForeColor = Color.LightGreen; }));
                        LogHelper.WriteInfo(code + " 工艺参数存入数据库成功！");
                    }
                    else if (Current.mes.IsOffline)
                    {

                        Battery battery = new Battery();
                        battery.Code = code + "-" + DateTime.Now.ToString("yyyyMMddHHmmss");
                        battery.ClampId = clampid;
                        battery.Location = "1";

                        if (Battery.Add(battery, out msg) < 1)
                        {
                            this.BeginInvoke(new MethodInvoker(() => { this.lbTip.Text = code + " MES离线模式，生成电芯数据存入数据库失败！"; this.lbTip.ForeColor = Color.Red; }));
                            LogHelper.WriteInfo(code + " MES离线模式，生成电芯数据存入数据库失败！");
                            return;
                        }
                        this.BeginInvoke(new MethodInvoker(() => { this.lbTip.Text = code + " MES离线模式，生成电芯数据存入数据库成功！"; this.lbTip.ForeColor = Color.LightGreen; }));
                        LogHelper.WriteError(code + " MES离线模式，生成电芯数据存入数据库成功！");
                        Thread.Sleep(100);
                    }
                }
                catch (Exception ex)
                {
                    Error.Alert(ex.ToString());
                }
            }
        }
    }
}
