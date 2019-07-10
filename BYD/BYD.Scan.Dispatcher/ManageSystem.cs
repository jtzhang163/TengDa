using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using TengDa;
using System.Threading;
using System.IO.Ports;
using System.Text.RegularExpressions;
using TengDa.Encrypt;
using System.IO;
using System.Timers;
using TengDa.WF;
using System.Net;
using System.Net.Sockets;
using BYD.Scan.Controls;

namespace BYD.Scan.Dispatcher
{
    /// <summary>
    /// 管理界面
    /// </summary>
    public partial class ManageSystem : Form
    {
        #region 初始化
        public ManageSystem()
        {
            InitializeComponent();
            this.MaximizedBounds = Screen.PrimaryScreen.WorkingArea;
            if (TengDa.WF.Current.IsRun)
            {
                DialogResult dr = MessageBox.Show("当前程序已经在运行，请勿重复启动！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                if (dr == DialogResult.OK)
                {
                    System.Environment.Exit(0);
                    return;
                }
            }
            Init();
        }

        private void Init()
        {
            InitControls();
            DisplayUserInfo();
            InitLogin();
            InitTerminal();
            InitSettingsTreeView();
            TengDa.WF.Current.IsRunning = true;
            Operation.Add("打开软件");
            Thread.Sleep(500);
            AddTips("打开软件", isUiThread: true);
            StartRefreshUI();
        }

        private void DisplayUserInfo()
        {
            userDisplay.DisplayUserInfo();
        }

        private void InitLogin()
        {

            PictureBox pb = new PictureBox(); pb.Name = "Login";
            pbUser_Click(pb, EventArgs.Empty);//先到登录界面

            if (Current.option.RememberMe)
            {
                this.cbRemPwd.Checked = true;

                if (Current.option.RememberUserId > 0)
                {
                    User user = new User(Current.option.RememberUserId);
                    tbUserName.Text = user.Name;
                    tbPassword.Text = Base64.DecodeBase64(user.Password);
                }
            }
            else
            {
                this.cbRemPwd.Checked = false;
            }


            if (Current.option.MesRememberMe)
            {
                this.cbMesRem.Checked = true;
                if (Current.option.MesRememberUserId > 0)
                {
                    User user = new User(Current.option.MesRememberUserId);
                    tbMesUserNumber.Text = user.Number;
                }
            }
            else
            {
                this.cbMesRem.Checked = false;
            }
        }

        private void InitControls()
        {
            lbTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.Text = string.Format("{0} {1}   {2}", Current.option.AppName, Version.AssemblyVersion, new Version().VersionTime.ToString("yyyy/M/d"));
            Current.runStstus = RunStatus.闲置;
            //Current.TaskMode = TaskMode.手动任务;

            //yieldDisplay.SetYieldType();
            //yieldDisplay.SetClearYieldTime(_Convert.StrToDateTime(Current.option.ClearYieldTime, Common.DefaultTime));

            if (!Current.option.IsMesUserEnable)
            {
                lbToMesUserLoginTip.Visible = false;
                llToMesUserLogin.Visible = false;
            }


            #region 烤箱相关控件数组

            //for (int i = 0; i < OvenCount; i++)
            //{
            //    ovenUCs[i] = (OvenUC)(this.Controls.Find(string.Format("ovenUC{0}", (i + 1).ToString("D2")), true)[0]);
            //}

            #endregion
        }

        private void InitSettingsTreeView()
        {
            tvSettings.Nodes.Clear();

            TreeNode tnConfig = new TreeNode("配置");

            List<TreeNode> tnLines = new List<TreeNode>();
            for (int i = 0; i < Option.LineCount; i++)
            {
                List<TreeNode> tnChildLines = new List<TreeNode>();
                for (int j = 0; j < Current.Lines[i].ChildLines.Count; j++)
                {
                    TreeNode tnChildLine = new TreeNode(string.Format("{1}[ID:{0}]", Current.Lines[i].ChildLines[j].Id, Current.Lines[i].ChildLines[j].Name));
                    tnChildLines.Add(tnChildLine);
                }

                TreeNode tnLine = new TreeNode(string.Format("{1}[ID:{0}]", Current.Lines[i].Id, Current.Lines[i].Name), tnChildLines.ToArray());
                tnLines.Add(tnLine);
            }
            TreeNode tnLineList = new TreeNode("线别", tnLines.ToArray());

            List<TreeNode> tnTouchscreens = new List<TreeNode>();
            for (int i = 0; i < Touchscreen.TouchscreenList.Count; i++)
            {
                TreeNode tnTouchscreen = new TreeNode(string.Format("{1}[ID:{0}]", Touchscreen.TouchscreenList[i].Id, Touchscreen.TouchscreenList[i].Name));
                tnTouchscreens.Add(tnTouchscreen);
            }
            TreeNode tnTouchscreenList = new TreeNode("触摸屏", tnTouchscreens.ToArray());

            List<TreeNode> tnScaners = new List<TreeNode>();
            for (int i = 0; i < Scaner.ScanerList.Count; i++)
            {
                TreeNode tnScaner = new TreeNode(string.Format("{1}[ID:{0}]", Scaner.ScanerList[i].Id, Scaner.ScanerList[i].Name));
                tnScaners.Add(tnScaner);
            }
            TreeNode tnScanerList = new TreeNode("扫码枪", tnScaners.ToArray());

            TreeNode tnMES = new TreeNode("MES");

            tvSettings.Nodes.AddRange(new TreeNode[] { tnConfig, tnLineList, tnTouchscreenList, tnScanerList, tnMES });
        }

        private void InitTerminal()
        {
            Console.WriteLine(Current.Lines[0].ChildLines[1].AutoScaner.Name);

            Current.Yields = Yield.YieldList;

            cbAlarmFloors.Items.Add("All");

            for (int i = 0; i < Option.LineCount; i++)
            {
                //this.ovenUCs[i].Init(Current.ovens[i]);

                //cbAlarmFloors.Items.Add(Current.ovens[i].Name);

                /////查询温度真空时下拉列表数据            
                //for (int j = 0; j < Current.ovens[i].Floors.Count; j++)
                //{
                //    Current.ovens[i].Floors[j].Stations.ForEach(s => cbStations.Items.Add(s.Name));
                //    cbAlarmFloors.Items.Add(Current.ovens[i].Floors[j].Name);
                //}
                //cbStations.SelectedIndex = 0;
                //cbAlarmFloors.SelectedIndex = 0;
            }

            this.machinesStatusUC1.Init();
            this.globalViewUC1.Init();

            this.scanerDebugUC1.Init();
            this.touchscreenDebugUC1.Init();
        }

        private void ManageSystem_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Current.runStstus == RunStatus.运行)
            {
                Tip.Alert("请先暂停");
                if (e == null)
                {
                    return;
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else if (MessageBox.Show("确定要退出程序？", "退出程序确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.OK)
            {
                Operation.Add("关闭软件");

                AlarmLog.StopAll();//正在进行的报警强制停止

                TimersDispose();

                if (timerRefreshUI != null)
                {
                    timerRefreshUI.Stop();
                    timerRefreshUI.Close();
                    timerRefreshUI.Dispose();
                }
                //断开与其他设备的连接
                PlcDisConnect();
                ScanerDisConnect();

                //释放当前对象，避免关闭程序出现异常
                lvUsers.Dispose();
                this.Dispose();

                //彻底退出程序
                System.Environment.Exit(0);
            }
            else
            {
                if (e == null)
                {
                    return;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
        #endregion

        #region 定时更新UI&设备在线确认

        delegate void RefreshUIDelegate();
        System.Timers.Timer timerRefreshUI = new System.Timers.Timer();

        private void StartRefreshUI()
        {
            timerRefreshUI.Interval = 1000;
            timerRefreshUI.Elapsed += delegate
            {
                Thread listen = new Thread(new ThreadStart(RefreshUiReceive));
                listen.IsBackground = true;
                listen.Start();
            };
            timerRefreshUI.Start();
        }

        private void RefreshUiReceive()
        {
            if (TengDa.WF.Current.IsRunning)
            {
                this.BeginInvoke(new RefreshUIDelegate(RefreshUI));
            }
        }

        private void RefreshUI()
        {

            //if (Current.ovens.Count(o => o.IsAlive && !o.AlreadyGetAllInfo) > 0)
            //{
            //    TengDa.WF.Current.IsTerminalInitFinished = false;
            //}
            //else
            //{
            //    TengDa.WF.Current.IsTerminalInitFinished = true;
            //}

            this.globalViewUC1.UpdateUI();

            #region 烤箱

            for (int i = 0; i < Option.LineCount; i++)
            {

                //Oven oven = Current.ovens[i];

                //this.ovenUCs[i].UpdateUI();

                ////if (oven.Plc.IsAlive) { if (this.machinesStatusUC1.GetStatusInfo(oven) == "未连接") { this.machinesStatusUC1.SetStatusInfo(oven, "连接成功"); } }
                ////else { this.machinesStatusUC1.SetStatusInfo(oven, "未连接"); }

                //for (int j = 0; j < OvenFloorCount; j++)
                //{
                //    oven.Floors[j].Stations.ForEach(s =>
                //    {
                //        if (s.Id == Current.option.CurveStationId)
                //        {
                //            for (int k = 0; k < Option.TemperaturePointCount; k++)
                //            {
                //                cbTemperIndex[k].Text = string.Format("{0}:{1}℃", Current.option.TemperNames[k], s.Temperatures[k].ToString("#0.0").PadLeft(5));
                //            }
                //            for (int k = 0; k < Option.VacuumPointCount; k++)
                //            {
                //                cbVacuumIndex[k].Text = string.Format("{0}:{1}Pa", "真空度", s.GetFloor().Vacuum.ToString());
                //            }
                //        }
                //    });

                //    var floor = oven.Floors[j];

                //    if (floor.IsAlive && floor.Stations.Count(s => s.Id == Current.Task.FromStationId || s.Id == Current.Task.ToStationId) > 0)
                //    {
                //        this.ovenUCs[i].Invalidate(j);
                //    }

                //    if (floor.PreIsAlive != floor.IsAlive)
                //    {
                //        this.ovenUCs[i].Invalidate(j);
                //    }

                //}

            }

            #endregion

            #region MES

            //if (Current.runStstus != RunStatus.闲置 && Current.mes.IsAlive)
            //{
            //    if (this.machinesStatusUC1.GetStatusInfo(Current.mes) == "未连接")
            //    {
            //        this.machinesStatusUC1.SetStatusInfo(Current.mes, "连接成功");
            //    }
            //    this.machinesStatusUC1.SetLampColor(Current.mes, Color.Green);
            //}
            //else
            //{
            //    this.machinesStatusUC1.SetStatusInfo(Current.mes, "未连接");
            //    this.machinesStatusUC1.SetLampColor(Current.mes, Color.Gray);
            //}

            #endregion

            #region 机器人

           // this.robotUC1.Update(Current.Robot);

            //if (Current.Robot.Plc.IsAlive) { if (this.machinesStatusUC1.GetStatusInfo(Current.Robot) == "未连接") { this.machinesStatusUC1.SetStatusInfo(Current.Robot, "连接成功"); } }
            //else { this.machinesStatusUC1.SetStatusInfo(Current.Robot, "未连接"); }

            //this.panelRobot.Padding = new Padding(Current.Robot.Position + 3, 3, 0, 3);

            #endregion

            #region 当前时间，运行状态、产量、任务信息

            lbTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            lbRunStatus.Text = Current.runStstus.ToString();
            //lbAuto.Text = Current.TaskMode == TaskMode.自动任务 ? "切换手动" : "切换自动";

            switch (Current.runStstus)
            {
                case RunStatus.闲置: lbRunStatus.ForeColor = Color.Gray; break;
                case RunStatus.运行: lbRunStatus.ForeColor = Color.Lime; break;
                case RunStatus.暂停: lbRunStatus.ForeColor = Color.Yellow; break;
                case RunStatus.异常: lbRunStatus.ForeColor = Color.Red; break;
            }

            //if (!TengDa.WF.Current.IsTerminalInitFinished && Current.runStstus == RunStatus.运行)
            //{
            //    lbTaskMode.Text = "初始化烤箱信息...";
            //    lbTaskMode.ForeColor = Color.Red;
            //}
            //else
            //{
            //    lbTaskMode.Text = Current.TaskMode.ToString();
            //    lbTaskMode.ForeColor = Current.TaskMode == TaskMode.自动任务 ? Color.Lime : Color.Gray;
            //}

            //yieldDisplay.YieldUpdate();

            //产量自动清零
            if (!Yield.IsCurrentShift(_Convert.StrToDateTime(Current.option.ClearYieldTime, Common.DefaultTime)) && TengDa.WF.Current.user.Id > 0)
            {
                Yield.Clear();
                Current.option.ClearYieldTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //yieldDisplay.SetClearYieldTime(DateTime.Now);
            }

            // this.lbTaskStatus.Text = Current.Task.Status.ToString();
            //this.taskInfo1.UpdateInfo();
            #endregion

        }

        #endregion

        #region 控件数组

        #endregion

        #region 启动逻辑

        DateTime StartTime = DateTime.Now;

        private void pictureBox_Click(object sender, EventArgs e)
        {

            string msg = string.Empty;

            PictureBox pictureBox = sender as PictureBox;

            if (TengDa.WF.Current.user.Id < 1 && !pictureBox.Name.Contains("Exit"))
            {
                Tip.Alert("请先登录系统！");
                return;
            }

            if (pictureBox.Name.Contains("Start"))//启动
            {
                if (Current.runStstus != RunStatus.运行)
                {
                    if (isFirstStart)
                    {
                        if (PlcConnect() && ScanerConnect() && MesConnect())
                        {

                            StartRun();
                            Current.ChangeModeTime = DateTime.Now;
                            Current.runStstus = RunStatus.运行;
                            Tip.Alert("成功启动！");
                            Operation.Add("启动运行");
                            AddTips("启动运行");
                        }
                        else if (!CheckStart(out msg))
                        {
                            Current.runStstus = RunStatus.异常;
                            this.machinesStatusUC1.SetCheckBoxEnabled(false);//禁止操作启用复选框
                            Operation.Add("启动出现异常，" + msg);
                            AddTips("启动出现异常");
                        }

                    }
                    else if (Current.runStstus == RunStatus.暂停)
                    {

                        timerlock = true;
                        Current.runStstus = RunStatus.运行;
                        Tip.Alert("成功启动！");
                        Operation.Add("启动运行");
                        AddTips("启动运行");
                    }
                }
                else
                {
                    Tip.Alert("系统已经在运行，请勿重复启动！");
                }
            }
            else if (pictureBox.Name.Contains("Stop"))
            {
                if (Current.runStstus == RunStatus.运行)
                {
                    timerlock = false;
                    Current.runStstus = RunStatus.暂停;
                    Thread.Sleep(1000);
                    Operation.Add("暂停运行");
                    Tip.Alert("运行已暂停！");
                    AddTips("暂停运行");
                }
            }
            else if (pictureBox.Name.Contains("Reset"))
            {
                if (Current.runStstus == RunStatus.暂停 || Current.runStstus == RunStatus.异常)
                {
                    TimersDispose();
                    if (PlcDisConnect() && ScanerDisConnect() && MesDisConnect())
                    {
                        this.machinesStatusUC1.SetCheckBoxEnabled(true);
                        isFirstStart = true;
                        //Current.TaskMode = TaskMode.手动任务;
                        Current.runStstus = RunStatus.闲置;
                        Tip.Alert("复位成功！");
                        Operation.Add("复位");
                        AddTips("复位");
                    }
                }
                else if (Current.runStstus == RunStatus.运行)
                {
                    Tip.Alert("请先停止！");
                }
            }
        }


        private bool PlcConnect()
        {
            string msg = string.Empty;

            for (int i = 0; i < Option.LineCount; i++)
            {
                if (Current.Lines[i].Touchscreen.IsEnable)
                {
                    if (!Current.Lines[i].Touchscreen.IsPingSuccess)
                    {
                        Error.Alert(string.Format("无法连接到{0}, IP:{1}", Current.Lines[i].Touchscreen.Name, Current.Lines[i].Touchscreen.IP));
                        return false;
                    }

                    if (!Current.Lines[i].Touchscreen.TcpConnect(out msg))
                    {
                        Error.Alert(string.Format("{0}:打开连接失败，原因：{1}", Current.Lines[i].Name, msg));
                        return false;
                    }
                    this.machinesStatusUC1.SetStatusInfo(Current.Lines[i].Touchscreen, "连接成功");
                    this.machinesStatusUC1.SetLampColor(Current.Lines[i].Touchscreen, Color.Green);
                }
            }

            return true;
        }
        /// <summary>
        /// 断开连接
        /// </summary>
        /// <returns></returns>
        private bool PlcDisConnect()
        {
            string msg = string.Empty;

            for (int i = 0; i < Option.LineCount; i++)
            {
                if (Current.Lines[i].Touchscreen.IsEnable)
                {
                    if (!Current.Lines[i].Touchscreen.TcpDisConnect(out msg))
                    {
                        Error.Alert(msg);
                        return false;
                    }
                    this.machinesStatusUC1.SetStatusInfo(Current.Lines[i].Touchscreen, "未连接");
                    this.machinesStatusUC1.SetLampColor(Current.Lines[i].Touchscreen, Color.Gray);
                }
            }

            return true;
        }

        private bool MesConnect()
        {
            string msg = string.Empty;

            if (Current.mes.IsEnable)
            {
                if (!Current.mes.IsPingSuccess)
                {
                    Error.Alert("无法连接到MES服务器：" + Current.mes.Host);
                    return false;
                }
                else
                {
                    if (Current.mes.IsOffline)
                    {
                        Current.mes.IsOffline = false;
                    }
                }
                this.machinesStatusUC1.SetStatusInfo(Current.mes, "连接成功");
                this.machinesStatusUC1.SetLampColor(Current.mes, Color.Green);
            }
            return true;
        }

        private bool MesDisConnect()
        {
            string msg = string.Empty;

            if (Current.mes.IsEnable)
            {
                this.machinesStatusUC1.SetStatusInfo(Current.mes, "未连接");
                this.machinesStatusUC1.SetLampColor(Current.mes, Color.Gray);
            }

            return true;
        }

        private bool ScanerConnect()
        {
            string msg = string.Empty;
            for (int i = 0; i < Option.LineCount; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        var scaner = Current.Lines[i].ChildLines[j].Scaners[k];
                        if (scaner.IsEnable)
                        {
                            if (!scaner.IsPingSuccess)
                            {

                                Error.Alert(string.Format("无法连接到{0}：{1}", scaner.Name, scaner.IP));
                                return false;
                            }

                            if (!scaner.TcpConnect(out msg))
                            {
                                Error.Alert(string.Format("{0}:打开连接失败，原因：{1}", scaner.Name, msg));
                                return false;
                            }
                            this.machinesStatusUC1.SetStatusInfo(scaner, "连接成功");
                            this.machinesStatusUC1.SetLampColor(scaner, Color.Green);
                            scaner.StartListenReceiveData();
                        }
                    }
                }
            }
            return true;
        }

        private bool ScanerDisConnect()
        {
            string msg = string.Empty;
            for (int i = 0; i < Option.LineCount; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        var scaner = Current.Lines[i].ChildLines[j].Scaners[k];
                        if (scaner.IsEnable)
                        {
                            if (scaner.IsAlive)
                            {
                                scaner.StopScan();
                            }

                            if (!scaner.TcpDisConnect(out msg))
                            {
                                Error.Alert(msg);
                                return false;
                            }

                            this.machinesStatusUC1.SetStatusInfo(scaner, "未连接");
                            this.machinesStatusUC1.SetLampColor(scaner, Color.Gray);
                        }
                    }
                }
            }

            return true;
        }

        #endregion

        #region 主运行逻辑

        delegate void UpdateUI1PDelegate(string text);

        System.Timers.Timer[] timerTouchscreenRuns = new System.Timers.Timer[Option.LineCount];

        System.Timers.Timer timerRun = null;

        private static bool timerlock = false;

        /// <summary>
        /// 启动前检测是否所有启用的设备均已连接成功
        /// </summary>
        /// <returns></returns>
        public bool CheckStart(out string msg)
        {
            for (int i = 0; i < Option.LineCount; i++)
            {
                if (Current.Lines[i].Touchscreen.IsEnable && !Current.Lines[i].Touchscreen.IsAlive)
                {
                    msg = Current.Lines[i].Touchscreen.Name + " 启动异常！";
                    return false;
                }

                for (int j = 0; j < Option.ChildLineCount; j++)
                {
                    for (int k = 0; k < Option.ChildLineScanerCount; k++)
                    {
                        if (Current.Lines[i].ChildLines[j].Scaners[k].IsEnable && !Current.Lines[i].ChildLines[j].Scaners[k].IsAlive)
                        {
                            msg = Current.Lines[i].ChildLines[j].Scaners[k].Name + " 启动异常！";
                            return false;
                        }
                    }
                }
            }

            if (Current.mes.IsEnable && !Current.mes.IsPingSuccess)
            {
                msg = Current.mes.Name + " 启动异常！";
                return false;
            }
            msg = string.Empty;
            return true;
        }

        public void StartRun()
        {

            if (isFirstStart)
            {
                for (int i = 0; i < Option.LineCount; i++)
                {
                    int index = i;//如果直接用i, 则完成循环后 i一直 = OvenCount
                    timerTouchscreenRuns[i] = new System.Timers.Timer();
                    timerTouchscreenRuns[i].Interval = TengDa._Convert.StrToInt("3000", 1000);
                    timerTouchscreenRuns[i].Elapsed += delegate
                    {
                        Thread listen = new Thread(new ParameterizedThreadStart(TouchscreenRunInvokeFunc));
                        listen.IsBackground = true;
                        listen.Start(index);
                    };
                    Thread.Sleep(200);
                    timerTouchscreenRuns[i].Start();
                }

                timerRun = new System.Timers.Timer();
                timerRun.Interval = TengDa._Convert.StrToInt(TengDa.WF.Option.GetOption("CheckPlcPeriod"), 1000);
                timerRun.Elapsed += delegate
                {
                    Thread listen = new Thread(new ThreadStart(RunInvokeFunc));
                    listen.IsBackground = true;
                    listen.Start();
                };
                Thread.Sleep(200);
                timerRun.Start();

                this.timerUploadMes = new System.Timers.Timer();
                this.timerUploadMes.Interval = TengDa._Convert.StrToInt(Current.option.UploadMesInterval, 60) * 1000;
                this.timerUploadMes.Elapsed += new System.Timers.ElapsedEventHandler(Timer_UploadMes);
                this.timerUploadMes.AutoReset = true;
                this.timerUploadMes.Start();
            }

            this.machinesStatusUC1.SetCheckBoxEnabled(false);//运行后禁止操作启用复选框

            isFirstStart = false;
            timerlock = true;
        }

        private static bool isFirstStart = true;

        private void TimersDispose()
        {
            for (int i = 0; i < Option.LineCount; i++)
            {
                if (timerTouchscreenRuns[i] != null)
                {
                    timerTouchscreenRuns[i].Stop();
                    timerTouchscreenRuns[i].Close();
                    timerTouchscreenRuns[i].Dispose();
                }
            }

            if (timerUploadMes != null)
            {
                timerUploadMes.Stop();
                timerUploadMes.Close();
                timerUploadMes.Dispose();
            }
        }

        private void TouchscreenRunInvokeFunc(object obj)
        {

            string msg = string.Empty;

            int i = System.Convert.ToInt32(obj);

            if (timerlock && Current.Lines[i].Touchscreen.IsEnable && (DateTime.Now - Current.ChangeModeTime).TotalSeconds > 10)
            {

                this.BeginInvoke(new MethodInvoker(() => { this.machinesStatusUC1.SetStatusInfo(Current.Lines[i].Touchscreen, "发送指令"); }));
                if (Current.Lines[i].GetInfo())
                {
                    this.BeginInvoke(new MethodInvoker(() => { this.machinesStatusUC1.SetStatusInfo(Current.Lines[i].Touchscreen, "获取信息成功"); }));
                }
                else
                {
                    this.BeginInvoke(new MethodInvoker(() => { this.machinesStatusUC1.SetStatusInfo(Current.Lines[i].Touchscreen, "获取信息失败"); }));
                    return;
                }


                for (int j = 0; j < 2; j++)
                {
                    #region 自动扫码逻辑
                    if (Current.Lines[i].ChildLines[j].AutoScaner.IsEnable && Current.Lines[i].ChildLines[j].AutoScaner.CanScan)
                    {
                        int ii = i;
                        int jj = j;

                        ScanResult scanResult = ScanResult.Unknown;
                        string code = "";

                        ScanResult result1 = Current.Lines[ii].ChildLines[jj].AutoScaner.StartScan(out code, out msg);

                        if (result1 == ScanResult.OK)
                        {
                            this.BeginInvoke(new MethodInvoker(() =>
                            {
                                this.machinesStatusUC1.SetStatusInfo(Current.Lines[ii].ChildLines[jj].AutoScaner, "+" + code);
                                this.machinesStatusUC1.SetForeColor(Current.Lines[ii].ChildLines[jj].AutoScaner, SystemColors.Control);
                                this.machinesStatusUC1.SetBackColor(Current.Lines[ii].ChildLines[jj].AutoScaner, Color.Green);
                            }));

                            Thread t = new Thread(() =>
                            {
                                Thread.Sleep(1000);
                                this.BeginInvoke(new MethodInvoker(() =>
                                {
                                    this.machinesStatusUC1.SetForeColor(Current.Lines[ii].ChildLines[jj].AutoScaner, Color.Green);
                                    this.machinesStatusUC1.SetBackColor(Current.Lines[ii].ChildLines[jj].AutoScaner, SystemColors.Control);
                                }));
                            });
                            t.Start();

                            scanResult = ScanResult.OK;
                        }
                        else
                        {
                            //再扫一次
                            ScanResult result2 = Current.Lines[ii].ChildLines[jj].AutoScaner.StartScan(out code, out msg);
                            if (result2 == ScanResult.OK)
                            {
                                this.BeginInvoke(new MethodInvoker(() =>
                                {
                                    this.machinesStatusUC1.SetStatusInfo(Current.Lines[ii].ChildLines[jj].AutoScaner, "+" + code);
                                    this.machinesStatusUC1.SetForeColor(Current.Lines[ii].ChildLines[jj].AutoScaner, SystemColors.Control);
                                    this.machinesStatusUC1.SetBackColor(Current.Lines[ii].ChildLines[jj].AutoScaner, Color.Green);
                                }));

                                Thread t = new Thread(() =>
                                {
                                    Thread.Sleep(1000);
                                    this.BeginInvoke(new MethodInvoker(() =>
                                    {
                                        this.machinesStatusUC1.SetForeColor(Current.Lines[ii].ChildLines[jj].AutoScaner, Color.Green);
                                        this.machinesStatusUC1.SetBackColor(Current.Lines[ii].ChildLines[jj].AutoScaner, SystemColors.Control);
                                    }));
                                });
                                t.Start();

                                scanResult = ScanResult.OK;
                            }
                            else
                            {
                                this.BeginInvoke(new MethodInvoker(() =>
                                {
                                    this.machinesStatusUC1.SetStatusInfo(Current.Lines[ii].ChildLines[jj].AutoScaner, "扫码NG");
                                    this.machinesStatusUC1.SetForeColor(Current.Lines[ii].ChildLines[jj].AutoScaner, Color.Red);
                                }));

                                scanResult = ScanResult.NG;
                            }
                        }


                        if (Current.Lines[ii].WriteScanFinishInfo(j, out msg))
                        {
                            LogHelper.WriteInfo("扫码结束信号成功写入 " + Current.Lines[ii].Touchscreen.Name);
                        }
                        else
                        {
                            Error.Alert("扫码结束信号写入 " + Current.Lines[ii].Touchscreen.Name + "出错，msg：" + msg);
                        }


                        if (Current.Lines[ii].ChildLines[j].AutoScaner.ScanFinish(scanResult, code, out ScanResult finalScanResult))
                        {
                            if (Current.Lines[ii].WriteScanResultInfo(j, finalScanResult, out msg))
                            {
                                LogHelper.WriteInfo("两个扫码完成后结果成功写入 " + Current.Lines[ii].Touchscreen.Name);
                            }
                            else
                            {
                                Error.Alert("两个扫码完成后结果写入 " + Current.Lines[ii].Touchscreen.Name + "出错，msg：" + msg);
                            }
                        }

                    }
                    #endregion

                }
            }
        }

        private object obj = new object();

        /// <summary>
        /// 总体运行逻辑
        /// </summary>
        private void RunInvokeFunc()
        {

            lock (obj)
            {
                if (timerlock && Math.Abs((DateTime.Now - Current.ChangeModeTime).TotalSeconds) > 3)
                {
                    for (int i = 0; i < Option.LineCount; i++)
                    {
                        Line line = Current.Lines[i];
                        if (line.Touchscreen.IsEnable && line.Touchscreen.IsAlive)
                        {
                            for (int j = 0; j < 2; j++)
                            {
                                Scaner scaner = line.ChildLines[j].ManuScaner;
                                if (scaner.IsEnable && scaner.IsAlive)
                                {
                                    if (scaner.Manu(out ScanResult finalScanResult))
                                    {
                                        if (line.WriteScanResultInfoManu(j, finalScanResult, out string msg))
                                        {
                                            LogHelper.WriteInfo("【手动】两个扫码完成后结果成功写入 " + line.Touchscreen.Name);
                                        }
                                        else
                                        {
                                            Error.Alert("【手动】两个扫码完成后结果写入 " + line.Touchscreen.Name + "出错，msg：" + msg);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        #endregion

        #region 定时将数据上传MES(设备信息和未上传成功的电芯信息)
        System.Timers.Timer timerUploadMes = null;

        private void Timer_UploadMes(object sender, ElapsedEventArgs e)
        {
            string msg = string.Empty;
            if (timerlock && Current.mes.IsEnable)
            {
                UploadMachineStatus();
                UploadBatteryInfo();
            }
        }

        public void UploadBatteryInfo()
        {
            try
            {
                Thread.Sleep(200);


                this.BeginInvoke(new MethodInvoker(() =>
                {
                    this.machinesStatusUC1.SetStatusInfo(Current.mes, "上传烘烤数据ID:");
                }));

                MES.UploadBatteryInfo();
            }
            catch (Exception ex)
            {
                Error.Alert(ex.Message);
            }
        }

        /// <summary>
        /// 定时上传真空温度数据
        /// </summary>
        public void UploadMachineStatus()
        {
            try
            {
                this.BeginInvoke(new MethodInvoker(() =>
                {
                    this.machinesStatusUC1.SetStatusInfo(Current.mes, "上传设备数据...");
                }));
                MES.UploadMachineStatus();
            }
            catch (Exception ex)
            {
                Error.Alert(ex.Message);
            }
        }

        #endregion

        #region 用户登录、注销、注册、管理

        private void HideAllUserGroupControl()
        {
            gbLogin.Visible = false;
            panel3333.Visible = false;
            gbReg.Visible = false;
            gbMana.Visible = false;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (TengDa.WF.Current.user.Id > 0)
            {
                Tip.Alert("系统已登录，请勿重复登录！");
                return;
            }
            string msg = string.Empty;
            User user = null;
            if (User.Login(tbUserName.Text.Trim(), tbPassword.Text.Trim(), out user, out msg))
            {
                TengDa.WF.Current.user = user;
                Current.option.RememberUserId = TengDa.WF.Current.user.Id;
                DisplayUserInfo();
                Tip.Alert("您已成功登录！");
                this.tabMain.SelectedIndex = 1;//转到操作选项卡
                tabContent.SelectedIndex = 1;//转到主界面选项卡
                Operation.Add("登录系统");
                AddTips(TengDa.WF.Current.user.Name + "登录");
            }
            else
            {
                if (tbUserName.Text == string.Empty)
                {
                    msg = "用户名为空！";
                }

                else if (tbPassword.Text == string.Empty)
                {
                    msg = "密码为空！";
                }

                Tip.Alert(msg);
            }
        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            if (tbUserName_reg.Text.Trim().Length < 1)
            {
                Error.Alert("请输入用户名！");
                return;
            }

            if (this.tbPassword_reg.Text.Trim().Length < 1)
            {
                Error.Alert("请输入密码！");
                return;
            }

            if (this.tbConfirmPassword_reg.Text.Trim().Length < 1)
            {
                Error.Alert("请输入确认密码！");
                return;
            }

            if (this.tbPassword_reg.Text.Trim() != this.tbConfirmPassword_reg.Text.Trim())
            {
                Error.Alert("两次输入的密码不匹配！");
                return;
            }

            string msg = string.Empty;

            User user = new User();
            user.Name = tbUserName_reg.Text.Trim();
            user.Password = tbPassword_reg.Text.Trim();
            user.Number = "";
            user.Group = new UserGroup("操作员");
            user.IsEnable = false;

            if (User.Add(user, out msg))
            {
                Operation.Add("注册用户" + user.Name);

                AddTips("注册用户:" + user.Name);

                Tip.Alert("您已成功注册，请经审核后登录！");

                //tabContent.SelectedIndex = 0;

                //Current.option.RememberMe = false;
                //cbRemPwd.Checked = false;
                //tbUserName.Text = tbUserName_reg.Text.Trim();
                //tbPassword.Text = tbPassword_reg.Text.Trim();
            }
            else
            {
                Tip.Alert(msg);
            }
        }

        private void cbRemPwd_CheckedChanged(object sender, EventArgs e)
        {
            if (!TengDa.WF.Current.IsRunning) return;
            Current.option.RememberMe = this.cbRemPwd.Checked;
        }

        private void pbUser_Click(object sender, EventArgs e)
        {

            PictureBox pictureBox = sender as PictureBox;
            if (pictureBox.Name.Contains("Login"))//登录
            {
                if (TengDa.WF.Current.user.Id > 0)
                {
                    Tip.Alert("系统已经登录！");
                    return;
                }
                HideAllUserGroupControl();
                if (Current.option.IsMesUserEnable)
                {
                    panel3333.Visible = true;
                }
                else
                {
                    gbLogin.Visible = true;
                }
                tabContent.SelectedIndex = 0;
            }
            else if (pictureBox.Name.Contains("Logout"))//注销
            {
                if (TengDa.WF.Current.user.Id < 1)
                {
                    Tip.Alert("尚未登录！");
                    return;
                }
                Operation.Add("注销登录");

                AddTips(TengDa.WF.Current.user.Name + "注销登录");

                TengDa.WF.Current.user = new User();
                DisplayUserInfo();
                Tip.Alert("注销成功！");
                PictureBox pb = new PictureBox(); pb.Name = "Login";
                pbUser_Click(pb, EventArgs.Empty);
            }
            else if (pictureBox.Name.Contains("Reg"))//注册
            {
                HideAllUserGroupControl();
                gbReg.Visible = true;
                tabContent.SelectedIndex = 0;
            }
            else if (pictureBox.Name.Contains("Mana"))//管理
            {
                if (TengDa.WF.Current.user.Id < 1)
                {
                    Tip.Alert("请先登录！");
                    return;
                }

                HideAllUserGroupControl();
                gbMana.Visible = true;
                tabContent.SelectedIndex = 0;
                lbSelectedUserName.Text = "";

                string msg = string.Empty;

                List<User> users = User.GetList("SELECT * FROM [dbo].[TengDa.Users]", out msg);
                lvUsers.Columns.Clear();
                lvUsers.Columns.Add("用户");
                lvUsers.Columns.Add("工号");
                lvUsers.Columns.Add("所属组");
                lvUsers.Columns.Add("等级");
                lvUsers.Columns.Add("注册时间");
                lvUsers.Columns.Add("上次登录时间");
                lvUsers.Columns.Add("登录次数");
                lvUsers.Columns.Add("审核状态");

                lvUsers.Columns[0].Width = 80;
                lvUsers.Columns[1].Width = 80;
                lvUsers.Columns[2].Width = 80;
                lvUsers.Columns[3].Width = 50;
                lvUsers.Columns[4].Width = 130;
                lvUsers.Columns[5].Width = 130;
                lvUsers.Columns[6].Width = 80;
                lvUsers.Columns[7].Width = 80;
                lvUsers.Items.Clear();
                //2、绑定到ListView上去
                foreach (User u in users)
                {
                    ListViewItem li = new ListViewItem();//创建行对象
                    li.Text = u.Name; //设置第一列显示数据
                                      //绑定剩余列的数据
                    li.SubItems.Add(u.Number);
                    li.SubItems.Add(u.Group.Name);
                    li.SubItems.Add(u.Group.Level.ToString());
                    li.SubItems.Add(u.RegisterTime.ToString());
                    li.SubItems.Add(u.LastLoginTime.ToString());
                    li.SubItems.Add(u.LoginTimes.ToString());
                    li.SubItems.Add(u.IsEnable ? "已审核" : "未审核");

                    //一定记得，行数据创建完毕后添加到列表中
                    lvUsers.Items.Add(li);
                }
            }
        }

        private void llReg_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PictureBox pb = new PictureBox(); pb.Name = "pbReg";
            pbUser_Click(pb, EventArgs.Empty);
        }

        private void llLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PictureBox pb = new PictureBox(); pb.Name = "pbLogin";
            pbUser_Click(pb, EventArgs.Empty);
        }

        private void lvUsers_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            lbSelectedUserName.Text = "";
            cbbUserGroupName.Items.Clear();
            if (lvUsers.SelectedItems.Count > 0)
            {
                string userName = lvUsers.SelectedItems[0].Text.Trim();
                User user = new User(userName);
                lbSelectedUserName.Text = user.Name;
                foreach (UserGroup userGroup in UserGroup.UserGroups)
                {
                    if (userGroup.Name != "超级管理员")
                    {
                        cbbUserGroupName.Items.Add(userGroup.Name);
                    }
                }
                cbbUserGroupName.Text = user.Group.Name;
            }
        }

        private void btnDelUser_Click(object sender, EventArgs e)
        {
            User user = new User(lbSelectedUserName.Text.Trim());

            if (user.Name == TengDa.WF.Current.user.Name)
            {
                Tip.Alert("不能删除当前账号");
                return;
            }

            if (user.Group.Level >= TengDa.WF.Current.user.Group.Level)
            {
                Tip.Alert("您无权限删除该账号");
                return;
            }

            string msg = string.Empty;
            if (User.Delete(user, out msg))
            {
                Operation.Add("删除用户：" + user.Name);
                AddTips("删除用户：" + user.Name);
                Tip.Alert("成功删除用户：" + user.Name);
                PictureBox pb = new PictureBox(); pb.Name = "Mana";
                pbUser_Click(pb, EventArgs.Empty);//再次加载用户列表
            }
            else
            {
                Error.Alert(msg);
            }
        }

        private void btnChangeUserLevel_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cbbUserGroupName.Text))
            {
                User user = new User(lbSelectedUserName.Text.Trim());
                if (cbbUserGroupName.Text.Trim() == user.Group.Name)
                {
                    return;
                }

                if (user.Group.Level >= TengDa.WF.Current.user.Group.Level)
                {
                    Tip.Alert("您无权限修改当前用户等级！");
                    return;
                }

                UserGroup userGroup = new UserGroup(cbbUserGroupName.Text);

                if (userGroup.Level > TengDa.WF.Current.user.Group.Level)
                {
                    Tip.Alert("无法设置该账号的等级高于当前用户等级！");
                    return;
                }

                user.Group = userGroup;
                string msg = string.Empty;
                if (!User.Update(user, out msg))
                {
                    Error.Alert(msg);
                    return;
                }

                Operation.Add("修改用户 " + user.Name + " 为 " + user.Group.Name);
                AddTips("修改" + user.Name + "为" + user.Group.Name);
                Tip.Alert("修改用户等级成功！");

                PictureBox pb = new PictureBox(); pb.Name = "Mana";
                pbUser_Click(pb, EventArgs.Empty);//再次加载用户列表
            }
        }

        private void tbLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)//回车登录
            {
                btnLogin_Click(sender, e);
            }
        }

        private void tbRegister_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)//回车登录
            {
                this.btnReg_Click(sender, e);
            }
        }

        private void btnUserVerify_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cbbUserGroupName.Text))
            {
                User user = new User(lbSelectedUserName.Text.Trim());
                if (user.IsEnable)
                {
                    return;
                }

                if (user.Group.Level >= TengDa.WF.Current.user.Group.Level)
                {
                    Tip.Alert("您无权限审核该用户！");
                    return;
                }

                string msg = string.Empty;
                if (!User.Verify(user, out msg))
                {
                    Error.Alert(msg);
                    return;
                }

                Operation.Add(user.Name + "通过审核");
                AddTips(user.Name + "通过审核");
                Tip.Alert(user.Name + "成功通过审核！");


                PictureBox pb = new PictureBox(); pb.Name = "Mana";
                pbUser_Click(pb, EventArgs.Empty);//再次加载用户列表
            }
        }

        private void btnMesLogin_Click(object sender, EventArgs e)
        {
            string userNumber = tbMesUserNumber.Text.Trim();
            if (string.IsNullOrEmpty(userNumber))
            {
                return;
            }

            if (TengDa.WF.Current.user.Id > 0)
            {
                Tip.Alert("系统已登录，请勿重复登录！");
                return;
            }

            MesLogin();
            //Thread t = new Thread(new ThreadStart(MesLogin));
            //t.Start();
        }

        private void MesLogin()
        {

            string msg = string.Empty;
            string userNumber = tbMesUserNumber.Text.Trim();
            string userName = string.Empty;

            //if (!MES.GetUserName(userNumber, out userName, out msg))
            //{
            //    Error.Alert(msg);
            //    return;
            //}

            User user = new User();
            user.Name = userName;
            user.Password = "";
            user.Number = userNumber;
            user.Group = new UserGroup("操作员");
            user.IsEnable = true;
            User.Add(user, out msg);

            if (User.MesLogin(userNumber, out user, out msg))
            {
                TengDa.WF.Current.user = user;
                Current.option.MesRememberUserId = TengDa.WF.Current.user.Id;
                DisplayUserInfo();
                Tip.Alert("您已成功登录！");
                this.tabMain.SelectedIndex = 1;//转到操作选项卡
                tabContent.SelectedIndex = 1;//转到主界面选项卡
                Operation.Add("登录系统");
                AddTips(TengDa.WF.Current.user.Name + "登录");
            }
            else
            {
                Tip.Alert(msg);
            }
        }

        private void cbMesRem_CheckedChanged(object sender, EventArgs e)
        {
            if (!TengDa.WF.Current.IsRunning) return;
            Current.option.MesRememberMe = this.cbMesRem.Checked;
        }

        private void tbMesUserName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)//回车登录
            {
                btnMesLogin_Click(sender, e);
            }
        }

        private void llToUserLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            HideAllUserGroupControl();
            gbLogin.Visible = true;
        }

        private void llToMesUserLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            HideAllUserGroupControl();
            panel3333.Visible = true;
        }

        #endregion

        #region 事件

        private void btnQuery_Click(object sender, EventArgs e)
        {
            var startTime = dtPickerStart.Value;
            var stopTime = dtPickerStop.Value;

            TimeSpan ts = startTime - stopTime;
            int timeSpan = TengDa._Convert.StrToInt(Current.option.QueryBatteryTimeSpan, -1);
            if (Math.Abs(ts.TotalDays) > timeSpan)
            {
                Tip.Alert("查询时间范围太大，请将时间范围设置在 " + timeSpan + " 天 之内！");
                return;
            }

            Thread t = new Thread(() =>
            {
                string msg = string.Empty;
                DataTable dt = Database.Query(string.Format("SELECT * FROM [dbo].[{0}.V_Battery] WHERE [扫码时间] BETWEEN '{1}' AND '{2}' ORDER BY [扫码时间]", Config.DbTableNamePre, startTime, stopTime), out msg);
                if (dt == null)
                {
                    Error.Alert(msg);
                    return;
                }

                this.BeginInvoke(new MethodInvoker(() =>
                {
                    dgViewBattery.DataSource = dt;

                    dgViewBattery.Columns[2].DefaultCellStyle.Format = "yyyy-MM-dd  HH:mm:ss";
                    //dgViewBattery.Columns[4].DefaultCellStyle.Format = "yyyy-MM-dd  HH:mm:ss";
                    //dgViewBattery.Columns[5].DefaultCellStyle.Format = "yyyy-MM-dd  HH:mm:ss";
                    //dgViewBattery.Columns[6].DefaultCellStyle.Format = "yyyy-MM-dd  HH:mm:ss";
                    //dgViewBattery.Columns[7].DefaultCellStyle.Format = "yyyy-MM-dd  HH:mm:ss";

                    //////设置显示列宽度
                    dgViewBattery.Columns[0].Width = 160;
                    //dgViewBattery.Columns[1].Width = 130;
                    dgViewBattery.Columns[2].Width = 130;

                    //dgViewBattery.Columns[4].Width = 130;
                    //dgViewBattery.Columns[5].Width = 130;
                    //dgViewBattery.Columns[6].Width = 130;
                    //dgViewBattery.Columns[7].Width = 130;
                    ////dgViewBattery.Columns[8].Width = 100;

                    tbBatteryCount.Text = dt.Rows.Count.ToString();
                }));
            });
            t.Start();

        }

        private void btnExport_Click(object sender, EventArgs e)
        {

            DataTable dt = (DataTable)this.dgViewBattery.DataSource;
            if (dt == null || dt.Rows.Count < 1)
            {
                Tip.Alert("尚无数据！");
                return;
            }

            SaveFileDialog kk = new SaveFileDialog();
            kk.Title = "保存EXECL文件";
            kk.Filter = "EXECL文件(*.xlsx) |*.xlsx |所有文件(*.*) |*.*";
            kk.FilterIndex = 1;

            //默认文件名
            kk.FileName = "Barcode" + dtPickerStart.Value.ToString("yyyyMMddHHmmss") + "-" + dtPickerStart.Value.ToString("yyyyMMddHHmmss");

            if (kk.ShowDialog() == DialogResult.OK)
            {
                string FileName = kk.FileName;
                if (File.Exists(FileName)) File.Delete(FileName);

                try
                {
                    using (ExcelHelper excelHelper = new ExcelHelper(FileName))
                    {
                        int count = excelHelper.DataTableToExcel(dt, "Sheet1", true);
                        if (count > 0) MessageBox.Show("导出成功！");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导出Excel出现异常: \r\n" + ex.Message);
                }
            }
        }

        private void btnYieldClear_Click(object sender, EventArgs e)
        {
            if (TengDa.WF.Current.user.Id < 1)
            {
                Tip.Alert("请先登录！");
                return;
            }
            Yield.Clear();
            Operation.Add("清空产量");
            AddTips("清空产量");
            Current.option.ClearYieldTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //yieldDisplay.SetClearYieldTime(DateTime.Now);
        }

        private void btnOperQuery_Click(object sender, EventArgs e)
        {
            var startTime = dtPickerOperStart.Value;
            var stopTime = dtPickerOperStop.Value;

            TimeSpan ts = startTime - stopTime;
            int timeSpan = TengDa._Convert.StrToInt("30", -1);
            if (Math.Abs(ts.TotalDays) > timeSpan)
            {
                Tip.Alert("查询时间范围太大，请将时间范围设置在 " + timeSpan + " 天 之内！");
                return;
            }

            Thread t = new Thread(() =>
            {
                string msg = string.Empty;
                DataTable dt = Database.Query(string.Format("SELECT * FROM [dbo].[{0}.V_Operation] WHERE [操作时间] BETWEEN '{1}' AND '{2}'", Config.DbTableNamePre, startTime, stopTime), out msg);
                if (dt == null)
                {
                    Error.Alert(msg);
                    return;
                }
                this.BeginInvoke(new MethodInvoker(() =>
                {
                    dgViewOper.DataSource = dt;

                    dgViewOper.Columns[4].DefaultCellStyle.Format = "yyyy-MM-dd  HH:mm:ss";

                    //设置显示列宽度
                    dgViewOper.Columns[3].Width = 320;
                    dgViewOper.Columns[4].Width = 130;
                }));
            });
            t.Start();
        }

        private void btnAlarmQuery_Click(object sender, EventArgs e)
        {
            var startTime = dtpAlarmStart.Value;
            var stopTime = dtpAlarmStop.Value;
            var selectFloor = cbAlarmFloors.Text.Trim();

            TimeSpan ts = stopTime - startTime;
            int timeSpan = TengDa._Convert.StrToInt(Current.option.QueryAlarmTimeSpan, -1);
            if (Math.Abs(ts.TotalDays) > timeSpan)
            {
                Tip.Alert("查询时间范围太大，请将时间范围设置在 " + timeSpan + " 天 之内！");
                return;
            }

            Thread t = new Thread(() =>
            {
                string msg = string.Empty;
                DataTable dt = null;
                if (selectFloor == "All")
                {
                    dt = Database.Query(string.Format("SELECT * FROM [dbo].[{0}.V_Alarm] WHERE [开始时间] BETWEEN '{1}' AND '{2}' ORDER BY [开始时间]", Config.DbTableNamePre, startTime, stopTime), out msg);
                }
                else
                {
                    dt = Database.Query(string.Format("SELECT * FROM [dbo].[{0}.V_Alarm] WHERE [名称] = '{1}' AND [开始时间] BETWEEN '{2}' AND '{3}' ORDER BY [开始时间]", Config.DbTableNamePre, selectFloor, startTime, stopTime), out msg);
                }

                if (dt == null)
                {
                    Error.Alert(msg);
                    return;
                }

                this.BeginInvoke(new MethodInvoker(() =>
                {
                    dgvAlarm.DataSource = dt;
                    dgvAlarm.Columns[2].DefaultCellStyle.Format = "yyyy-MM-dd  HH:mm:ss";
                    dgvAlarm.Columns[3].DefaultCellStyle.Format = "yyyy-MM-dd  HH:mm:ss";

                    //设置显示列宽度
                    //dgvAlarm.Columns[0].Width = 100;
                    //dgvAlarm.Columns[0].HeaderText = "烤箱/腔体";
                    //dgvAlarm.Columns[1].Width = 100;
                    //dgvAlarm.Columns[2].Width = 100;
                    //dgvAlarm.Columns[3].Width = 150;
                    //dgvAlarm.Columns[4].Width = 150;
                    //dgvAlarm.Columns[5].Width = 150;
                    //dgvAlarm.Columns[6].Width = 150;
                    //dgvAlarm.Columns[6].HeaderText = "持续时间(s)";
                    //dgvAlarm.Columns[7].Width = 150;
                    tbNumAlarm.Text = dt.Rows.Count.ToString();
                }));
            });
            t.Start();
        }

        private void btnAlarmExport_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)this.dgvAlarm.DataSource;
            if (dt == null || dt.Rows.Count < 1)
            {
                Tip.Alert("尚无数据！");
                return;
            }

            SaveFileDialog kk = new SaveFileDialog();
            kk.Title = "保存EXECL文件";
            kk.Filter = "EXECL文件(*.xlsx) |*.xlsx |所有文件(*.*) |*.*";
            kk.FilterIndex = 1;

            //默认文件名
            kk.FileName = "Alarm Log " + dtpAlarmStart.Value.ToString("yyyyMMddHHmmss") + "-" + dtpAlarmStop.Value.ToString("yyyyMMddHHmmss");

            if (kk.ShowDialog() == DialogResult.OK)
            {
                string FileName = kk.FileName;
                if (File.Exists(FileName)) File.Delete(FileName);

                try
                {
                    using (ExcelHelper excelHelper = new ExcelHelper(FileName))
                    {
                        int count = excelHelper.DataTableToExcel(dt, "Sheet1", true);
                        if (count > 0) MessageBox.Show("导出成功！");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("导出Excel出现异常: \r\n" + ex.Message);
                }
            }
        }

        private void tabContent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabContent.SelectedTab.Text == "设置")
            {
                InitSettingsTreeView();
            }
        }

        private void tvSettings_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse)
            {
                try
                {
                    if (TengDa.WF.Current.user.Id < 1)
                    {
                        Tip.Alert("请先登录！");
                        return;
                    }

                    this.propertyGridSettings.Enabled = true;
                    if (TengDa.WF.Current.user.Group.Level < 2)
                    {
                        this.propertyGridSettings.Enabled = false;
                        Tip.Alert("当前用户权限不足！");
                    }

                    if (e.Node.Level == 0 && e.Node.Text == "配置")
                    {
                        if (TengDa.WF.Current.user.Group.Level < 3)
                        {
                            this.propertyGridSettings.Enabled = false;
                            Tip.Alert("当前用户权限不足！");
                        }
                        this.propertyGridSettings.SelectedObject = Current.option;
                    }
                    else if (e.Node.Level == 1 && e.Node.Parent.Text == "线别")
                    {
                        this.propertyGridSettings.SelectedObject = Current.Lines.First(o => o.Id.ToString() == e.Node.Text.Split(':', ']')[1]);
                    }
                    else if (e.Node.Level == 2 && e.Node.Parent.Parent.Text == "线别")
                    {
                        var line = Current.Lines.First(o => o.Id.ToString() == e.Node.Parent.Text.Split(':', ']')[1]);
                        this.propertyGridSettings.SelectedObject = line.ChildLines.First(o => o.Id.ToString() == e.Node.Text.Split(':', ']')[1]);
                    }
                    else if (e.Node.Level == 1 && e.Node.Parent.Text == "扫码枪")
                    {
                        this.propertyGridSettings.SelectedObject = Scaner.ScanerList.First(o => o.Id.ToString() == e.Node.Text.Split(':', ']')[1]);
                    }
                    else if (e.Node.Level == 1 && e.Node.Parent.Text == "触摸屏")
                    {
                        this.propertyGridSettings.SelectedObject = Touchscreen.TouchscreenList.First(o => o.Id.ToString() == e.Node.Text.Split(':', ']')[1]);
                    }
                    if (e.Node.Level == 0 && e.Node.Text == "MES")
                    {
                        this.propertyGridSettings.SelectedObject = Current.mes;
                    }
                }
                catch (Exception ex)
                {
                    Error.Alert(ex);
                }
            }
        }

        private void propertyGridSettings_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            object o = this.propertyGridSettings.SelectedObject;
            Type type = o.GetType();

            string settingsStr = string.Empty;

            if (type == typeof(Line) || type == typeof(Scaner) || type == typeof(Touchscreen))
            {
                System.Reflection.PropertyInfo propertyInfoId = type.GetProperty("Id"); //获取指定名称的属性
                int Id = (int)propertyInfoId.GetValue(o, null); //获取属性值
                settingsStr = string.Format("将Id为 {0} 的 {1} 的 {2} 由 {3} 修改为 {4} ", Id, type.Name, e.ChangedItem.PropertyDescriptor.DisplayName, e.OldValue, e.ChangedItem.Value);
            }
            else if (type == typeof(Option))
            {
                settingsStr = string.Format("将配置项的 {0} 由 {1} 修改为 {2} ", e.ChangedItem.PropertyDescriptor.DisplayName, e.OldValue, e.ChangedItem.Value);
            }
            else if (type == typeof(MES))
            {
                settingsStr = string.Format("将MES的 {0} 由 {1} 修改为 {2} ", e.ChangedItem.PropertyDescriptor.DisplayName, e.OldValue, e.ChangedItem.Value);
            }

            if (!string.IsNullOrEmpty(settingsStr))
            {
                Operation.Add(settingsStr);
                Tip.Alert("成功" + settingsStr);
            }
        }
        #endregion

        #region 提示信息

        public void AddTips(string tip)
        {
            AddTips(tip, false);
        }

        public void AddTips(string tip, bool isUiThread)
        {
            if (isUiThread)
            {
                tbTips.AppendText(string.Format("[{0}]{1}\r\n", DateTime.Now.ToString("HH:mm:ss"), tip));
                tbTips.Focus();
                tbTips.Select(tbTips.TextLength, 0);
                tbTips.ScrollToCaret();
            }
            else
            {
                this.BeginInvoke(new MethodInvoker(() =>
                {
                    tbTips.AppendText(string.Format("[{0}]{1}\r\n", DateTime.Now.ToString("HH:mm:ss"), tip));
                    tbTips.Focus();
                    tbTips.Select(tbTips.TextLength, 0);
                    tbTips.ScrollToCaret();
                }));
            }
            LogHelper.WriteInfo(tip);
        }

        #endregion

        private void btnDebug_Click(object sende, EventArgs e)
        {
            Current.Lines[1].ChildLines[1].AutoScaner.StartScan(out string code, out string msg);
            Tip.Alert(code);
        }
    }
}
