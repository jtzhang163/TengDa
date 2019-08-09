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
using CAMEL.Baking.Control;

namespace CAMEL.Baking.App
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
            cbCount.SelectedIndex = 1;
            lbTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.Text = string.Format("{0} {1}   {2}", Current.option.AppName, Version.AssemblyVersion, new Version().VersionTime.ToString("yyyy/M/d"));
            Current.runStstus = RunStatus.闲置;
            Current.TaskMode = TaskMode.手动任务;

            yieldDisplay.SetYieldType();
            yieldDisplay.SetClearYieldTime(_Convert.StrToDateTime(Current.option.ClearYieldTime, Common.DefaultTime));

            if (!Current.option.IsMesUserEnable)
            {
                lbToMesUserLoginTip.Visible = false;
                llToMesUserLogin.Visible = false;
            }


            #region 烤箱相关控件数组

            for (int i = 0; i < OvenCount; i++)
            {
                ovenUCs[i] = (OvenUC)(this.Controls.Find(string.Format("ovenUC{0}", (i + 1).ToString("D2")), true)[0]);
            }

            #endregion


            for (int i = 0; i < Option.TemperaturePointCount; i++)
            {
                cbTemperIndex[i] = (CheckBox)(this.Controls.Find(string.Format("cbTemperIndex{0}", (i + 1).ToString("D2")), true)[0]);
                cbTemperIndex[i].ForeColor = Current.option.CurveColors[i];
            }

            for (int i = 0; i < Option.VacuumPointCount; i++)
            {
                cbVacuumIndex[i] = (CheckBox)(this.Controls.Find(string.Format("cbVacuumIndex{0}", (i + 1).ToString("D2")), true)[0]);
                cbVacuumIndex[i].ForeColor = Color.Violet;
            }

            if (System.Windows.SystemParameters.PrimaryScreenHeight > 800)
            {
                this.tlpOvenCol1.Margin = new Padding(3, 50, 3, 3);
                this.tlpOvenCol2.Margin = new Padding(3, 3, 3, 50);
            }
        }

        private void InitSettingsTreeView()
        {
            tvSettings.Nodes.Clear();

            TreeNode tnConfig = new TreeNode("配置");

            List<TreeNode> tnOvens = new List<TreeNode>();
            for (int i = 0; i < OvenCount; i++)
            {
                List<TreeNode> tnFloors = new List<TreeNode>();
                TreeNode tnOvenPlc = new TreeNode("PLC");
                tnFloors.Add(tnOvenPlc);
                for (int j = 0; j < Current.ovens[i].Floors.Count; j++)
                {
                    TreeNode tnFloor = new TreeNode(string.Format("{0}:{1}", Current.ovens[i].Floors[j].Id, Current.ovens[i].Floors[j].Name));
                    tnFloors.Add(tnFloor);
                }

                TreeNode tnOven = new TreeNode(string.Format("{0}:{1}", Current.ovens[i].Id, Current.ovens[i].Name), tnFloors.ToArray());
                tnOvens.Add(tnOven);
            }

            TreeNode tnOvenList = new TreeNode("烤箱", tnOvens.ToArray());

            List<TreeNode> tnFeeders = new List<TreeNode>();
            foreach (Feeder feeder in Feeder.FeederList)
            {
                tnFeeders.Add(new TreeNode(string.Format("{0}:{1}", feeder.Id, feeder.Name), new TreeNode[] { new TreeNode("PLC") }));
            }
            TreeNode tnFeederList = new TreeNode("上下料机", tnFeeders.ToArray());

            //List<TreeNode> tnBlankers = new List<TreeNode>();
            //foreach (Blanker blanker in Blanker.BlankerList)
            //{
            //    tnBlankers.Add(new TreeNode(string.Format("{0}:{1}", blanker.Id, blanker.Name), new TreeNode[] { new TreeNode("PLC") }));
            //}
            //TreeNode tnBlankerList = new TreeNode("下料机", tnBlankers.ToArray());

            List<TreeNode> tnScaners = new List<TreeNode>();
            foreach (Scaner scaner in Scaner.ScanerList)
            {
                tnScaners.Add(new TreeNode(string.Format("{0}:{1}", scaner.Id, scaner.Name)));
            }
            TreeNode tnScanerList = new TreeNode("扫码枪", tnScaners.ToArray());

            //TreeNode tnCache = new TreeNode("缓存架");

            //TreeNode tnRotater = new TreeNode("转移台");

            List<TreeNode> tnStations = new List<TreeNode>();

            var stationList = new List<Station>();
            Current.Feeder.Stations.ForEach(s => stationList.Add(s));
            //Current.Blanker.Stations.ForEach(s => stationList.Add(s));
            Current.ovens.ForEach(o => o.Floors.ForEach(f => f.Stations.ForEach(s => stationList.Add(s))));
            //Current.Cacher.Stations.ForEach(s => stationList.Add(s));
            //stationList.Add(Current.Transfer.Station);

            foreach (Station station in stationList)
            {
                List<TreeNode> tnClamps = new List<TreeNode>();
                if (station.Clamp.Id > 0)
                {
                    List<TreeNode> tnBatteries = new List<TreeNode>();
                    var batteries = station.Clamp.Batteries;
                    foreach (var b in batteries)
                    {
                        tnBatteries.Add(new TreeNode(string.Format("{0}:{1}(电芯)", b.Id, b.Code)));
                    }
                    tnClamps.Add(new TreeNode(string.Format("{0}:{1}(夹具)", station.Clamp.Id, station.Clamp.Code), tnBatteries.ToArray()));
                }
                tnStations.Add(new TreeNode(string.Format("{0}:{1}", station.Id, station.Name), tnClamps.ToArray()));
            }
            TreeNode tnStationList = new TreeNode("工位列表", tnStations.ToArray());


            List<TreeNode> tnTasks = new List<TreeNode>();
            foreach (Task task in Task.TaskList)
            {
                tnTasks.Add(new TreeNode(string.Format("{0}:{1}", task.Id, task.Description)));
            }

            TreeNode tnTaskList = new TreeNode("全部任务列表", tnTasks.ToArray());

            List<TreeNode> tnEnabledTasks = new List<TreeNode>();
            foreach (Task task in Task.CanGetPutTaskList)
            {
                tnEnabledTasks.Add(new TreeNode(string.Format("{0}:{1}", task.Id, task.Description)));
            }

            TreeNode tnEnabledTaskList = new TreeNode("有效任务排序", tnEnabledTasks.ToArray());

            List<TreeNode> tnRGVNodes = new List<TreeNode>();

            List<TreeNode> tnRGVClamps = new List<TreeNode>();
            if (Current.RGV.Clamp.Id > 0)
            {
                List<TreeNode> tnBatteries = new List<TreeNode>();
                var batteries = Current.RGV.Clamp.Batteries;
                foreach (var b in batteries)
                {
                    tnBatteries.Add(new TreeNode(string.Format("{0}:{1}(电芯)", b.Id, b.Code)));
                }
                tnRGVClamps.Add(new TreeNode(string.Format("{0}:{1}(夹具)", Current.RGV.Clamp.Id, Current.RGV.Clamp.Code), tnBatteries.ToArray()));
            }

            tnRGVNodes.Add(new TreeNode("PLC"));
            tnRGVNodes.AddRange(tnRGVClamps);
            TreeNode tnRGV = new TreeNode("RGV", tnRGVNodes.ToArray());

            TreeNode tnMES = new TreeNode("MES");

            TreeNode tnCurrentTask = new TreeNode("当前任务");

            tvSettings.Nodes.AddRange(new TreeNode[] { tnCurrentTask, tnConfig, tnOvenList, tnFeederList, tnScanerList, tnStationList, tnTaskList, tnEnabledTaskList, tnRGV, tnMES });
        }

        private void InitTerminal()
        {
            Current.ovens = Oven.OvenList;
            Current.Yields = Yield.YieldList;
            cbStations.Items.Add("All");
            cbAlarmFloors.Items.Add("All");

            for (int i = 0; i < OvenCount; i++)
            {
                this.ovenUCs[i].Init(Current.ovens[i]);

                cbAlarmFloors.Items.Add(Current.ovens[i].Name);

                ///查询温度真空时下拉列表数据            
                for (int j = 0; j < Current.ovens[i].Floors.Count; j++)
                {
                    Current.ovens[i].Floors[j].Stations.ForEach(s => cbStations.Items.Add(s.Name));
                    cbAlarmFloors.Items.Add(Current.ovens[i].Floors[j].Name);
                }
                cbStations.SelectedIndex = 0;
                cbAlarmFloors.SelectedIndex = 0;
            }

            this.feederUC1.Init(Current.Feeder);

            //for (int i = 0; i < 1; i++)
            //{
            //    lbFeederNameN[i].Text = Current.Feeder.Name;
            //    cbFeederIsEnable[i].Checked = Current.Feeder.IsEnable;

            //    for (int j = 0; j < Current.Feeder.Scaners.Count; j++)
            //    {
            //        lbScanerNameN[i][j].Text = Current.Feeder.Scaners[j].Name;
            //        pbScanerLamp[i][j].Image = Properties.Resources.Gray_Round;
            //        cbScanerIsEnable[i][j].Checked = Current.Feeder.Scaners[j].IsEnable;
            //    }
            //}

            //this.blankerUC1.Init(Current.Blanker);

            this.robotUC1.Init(Current.RGV);


            bool isAll = true;
            for (int k = 0; k < Option.TemperaturePointCount; k++)
            {
                if (Array.IndexOf(Current.option.CurveIndexs.Split(','), k.ToString()) > -1)
                {
                    cbTemperIndex[k].Checked = true;
                }
                else
                {
                    cbTemperIndex[k].Checked = false;
                    isAll = false;
                }
            }
            cbTemperAll.Checked = isAll;

            for (int k = 0; k < Option.VacuumPointCount; k++)
            {
                cbVacuumIndex[k].Checked = true;
            }

            //温度曲线显示工位初始化

            Station curveStation = Station.StationList.Where(s => s.Id == Current.option.CurveStationId).FirstOrDefault();
            Floor curveFloor = Floor.FloorList.First(f => f.Stations.Contains(curveStation));
            Oven curveOven = Oven.OvenList.First(o => o.Floors.Contains(curveFloor));

            int ii = Current.ovens.IndexOf(curveOven);
            int jj = curveOven.Floors.IndexOf(curveFloor);
            int kk = curveFloor.Stations.IndexOf(curveStation);

            Current.ovens.ForEach(o =>
            {
                cbCurveSelectedOven.Items.Add(o.Name);
            });
            cbCurveSelectedOven.SelectedIndex = ii;

            curveOven.Floors.ForEach(f =>
            {
                cbCurveSelectedFloor.Items.Add(f.Name);
            });
            cbCurveSelectedFloor.SelectedIndex = jj;

            curveFloor.Stations.ForEach(s =>
            {
                cbCurveSelectedStation.Items.Add(s.Name);
            });
            cbCurveSelectedStation.SelectedIndex = kk;


            //水分手动上传选择炉层初始化

            Floor sampleFloor = Floor.FloorList.Where(f => f.Id == Current.option.SampleFloorId).FirstOrDefault();
            Oven sampleOven = Oven.OvenList.First(o => o.Floors.Contains(sampleFloor));

            int iii = Current.ovens.IndexOf(sampleOven);
            int jjj = sampleOven.Floors.IndexOf(sampleFloor);

            //cbBatteryScaner.Items.Add(Current.BatteryScaner.Name);
            cbClampScaner.Items.Add(Current.ClampScaner.Name);

            //cbBatteryScaner.SelectedIndex = 0;
            cbClampScaner.SelectedIndex = 0;


            this.machinesStatusUC1.Init();
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

            if (Current.ovens.Count(o => o.IsAlive && !o.AlreadyGetAllInfo) > 0)
            {
                TengDa.WF.Current.IsTerminalInitFinished = false;
            }
            else
            {
                TengDa.WF.Current.IsTerminalInitFinished = true;
            }

            #region 烤箱

            for (int i = 0; i < OvenCount; i++)
            {
                Oven oven = Current.ovens[i];

                this.ovenUCs[i].UpdateUI();

                if (oven.Plc.IsAlive) { if (this.machinesStatusUC1.GetStatusInfo(oven) == "未连接") { this.machinesStatusUC1.SetStatusInfo(oven, "连接成功"); } }
                else { this.machinesStatusUC1.SetStatusInfo(oven, "未连接"); }

                for (int j = 0; j < OvenFloorCount; j++)
                {
                    oven.Floors[j].Stations.ForEach(s =>
                    {
                        if (s.Id == Current.option.CurveStationId)
                        {
                            for (int k = 0; k < Option.TemperaturePointCount; k++)
                            {
                                //cbTemperIndex[k].Text = string.Format("{0}:{1}℃", Current.option.TemperNames[k], s.Temperatures[k].ToString("#0.0").PadLeft(5));
                            }
                            for (int k = 0; k < Option.VacuumPointCount; k++)
                            {
                                cbVacuumIndex[k].Text = string.Format("{0}:{1}Pa", "真空度", s.GetFloor().Vacuum.ToString());
                            }
                        }
                    });

                    var floor = oven.Floors[j];

                    if (floor.IsAlive && floor.Stations.Count(s => s.Id == Current.Task.FromStationId || s.Id == Current.Task.ToStationId) > 0)
                    {
                        this.ovenUCs[i].Invalidate(j);
                    }

                    if (floor.PreIsAlive != floor.IsAlive)
                    {
                        this.ovenUCs[i].Invalidate(j);
                    }

                }

            }

            #endregion

            #region 上料机、扫码枪

            this.feederUC1.Update(Current.Feeder);

            if (Current.Feeder.Plc.IsAlive) { if (this.machinesStatusUC1.GetStatusInfo(Current.Feeder) == "未连接") { this.machinesStatusUC1.SetStatusInfo(Current.Feeder, "连接成功"); } }
            else { this.machinesStatusUC1.SetStatusInfo(Current.Feeder, "未连接"); }

            //    for (int j = 0; j < Current.Feeder.Scaners.Count; j++)
            //    {
            //        Scaner scaner = Current.Feeder.Scaners[j];
            //        if (!scaner.IsEnable)
            //            scaner.IsAlive = false;
            //        if (scaner.IsAlive)
            //        {
            //            if (tbScanerStatus[i][j].Text.Trim() == "未连接")
            //            {
            //                tbScanerStatus[i][j].Text = "连接成功";
            //            }

            //            this.pbScanerLamp[i][j].Image = Properties.Resources.Green_Round;
            //        }
            //        else
            //        {
            //            this.tbScanerStatus[i][j].Text = "未连接";
            //            this.pbScanerLamp[i][j].Image = Properties.Resources.Gray_Round;
            //        }
            //    }



            //    tlpFeeders[i].Invalidate();


            #endregion


            #region MES

            if (Current.runStstus != RunStatus.闲置 && Current.mes.IsAlive)
            {
                if (this.machinesStatusUC1.GetStatusInfo(Current.mes) == "未连接")
                {
                    this.machinesStatusUC1.SetStatusInfo(Current.mes, "连接成功");
                }
                this.machinesStatusUC1.SetLampColor(Current.mes, Color.Green);
            }
            else
            {
                this.machinesStatusUC1.SetStatusInfo(Current.mes, "未连接");
                this.machinesStatusUC1.SetLampColor(Current.mes, Color.Gray);
            }

            #endregion

            #region RGV

            this.robotUC1.Update(Current.RGV);

            if (Current.RGV.Plc.IsAlive) { if (this.machinesStatusUC1.GetStatusInfo(Current.RGV) == "未连接") { this.machinesStatusUC1.SetStatusInfo(Current.RGV, "连接成功"); } }
            else { this.machinesStatusUC1.SetStatusInfo(Current.RGV, "未连接"); }

            //this.panelRGV.Padding = new Padding(Current.RGV.Position + 3, 3, 0, 3);

            #endregion

            #region 当前时间，运行状态、产量、任务信息

            lbTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            lbRunStatus.Text = Current.runStstus.ToString();
            lbAuto.Text = Current.TaskMode == TaskMode.自动任务 ? "切换手动" : "切换自动";

            switch (Current.runStstus)
            {
                case RunStatus.闲置: lbRunStatus.ForeColor = Color.Gray; break;
                case RunStatus.运行: lbRunStatus.ForeColor = Color.Lime; break;
                case RunStatus.暂停: lbRunStatus.ForeColor = Color.Yellow; break;
                case RunStatus.异常: lbRunStatus.ForeColor = Color.Red; break;
            }

            if (!TengDa.WF.Current.IsTerminalInitFinished && Current.runStstus == RunStatus.运行)
            {
                lbTaskMode.Text = "初始化烤箱信息...";
                lbTaskMode.ForeColor = Color.Red;
            }
            else
            {
                lbTaskMode.Text = Current.TaskMode.ToString();
                lbTaskMode.ForeColor = Current.TaskMode == TaskMode.自动任务 ? Color.Lime : Color.Gray;
            }

            yieldDisplay.YieldUpdate();

            //产量自动清零
            if (!Yield.IsCurrentShift(_Convert.StrToDateTime(Current.option.ClearYieldTime, Common.DefaultTime)) && TengDa.WF.Current.user.Id > 0)
            {
                Yield.Clear();
                Current.option.ClearYieldTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                yieldDisplay.SetClearYieldTime(DateTime.Now);
            }

            // this.lbTaskStatus.Text = Current.Task.Status.ToString();
            this.taskInfo1.UpdateInfo();
            #endregion

        }

        #endregion

        #region 控件数组

        private const int OvenCount = 25;
        private const int OvenFloorCount = 5;

        private OvenUC[] ovenUCs = new OvenUC[OvenCount];

        private CheckBox[] cbTemperIndex = new CheckBox[Option.TemperaturePointCount];
        private CheckBox[] cbVacuumIndex = new CheckBox[Option.VacuumPointCount];

        #endregion

        #region 启动逻辑

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
                        Current.TaskMode = TaskMode.手动任务;
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
            else if (pictureBox.Name.Contains("Auto"))
            {
                if (Current.runStstus == RunStatus.闲置)
                {
                    Tip.Alert("请先启动！");
                    return;
                }

                if (Current.runStstus == RunStatus.运行)
                {
                    Tip.Alert("请先停止！");
                    return;
                }

                if (!TengDa.WF.Current.IsTerminalInitFinished)
                {
                    Tip.Alert("烤箱信息初始化尚未完成，请稍后！");
                    return;
                }

                if (Current.TaskMode == TaskMode.手动任务 && Current.Task.Status != TaskStatus.完成)
                {   
                    Tip.Alert("当前手动任务尚未完成，无法切换为自动任务！若要强制切换，请先点击任务复位。");
                    return;
                }


                if (Current.TaskMode == TaskMode.手动任务)
                {
                    for (int i = 0; i < OvenCount; i++)
                    {
                        for (int j = 0; j < OvenFloorCount; j++)
                        {
                            if (Current.ovens[i].Floors[j].IsAlive && Current.ovens[i].Floors[j].DoorStatus == DoorStatus.打开)
                            {
                                Tip.Alert(Current.ovens[i].Floors[j].Name + "门尚未关闭，请手动关闭后再切换自动");
                                return;
                            }
                        }
                    }
                    if (Current.RGV.IsAlive)
                    {
                        //if (!Current.RGV.IsExecuting)
                        //{
                        //    Tip.Alert("RGV尚未成功启动，不能切换自动");
                        //    return;
                        //}

                        if (Current.RGV.ClampStatus != ClampStatus.无夹具)
                        {
                            Tip.Alert("RGV负载有夹具，不能切换自动");
                            return;
                        }
                    }
                }

                   

                if (Current.TaskMode == TaskMode.自动任务 && Current.Task.Status != TaskStatus.完成)
                {
                 
                    Tip.Alert("当前任务完成后会切换至手动任务。若要立即切换，请点击任务复位");
                    CurrentTask.ToSwitchManuTaskMode = true;
                    return;
                }

                //Station dangerousStation = null;
                //int dangerousDoorCount = 0;
                //Current.feeders.ForEach(f => f.Stations.ForEach(s =>
                //{
                //    if (s.Status == StationStatus.可取 && s.DoorStatus != DoorStatus.关闭)
                //    {
                //        dangerousStation = s;
                //        dangerousDoorCount++;
                //    }
                //}));

                //if (Current.TaskMode == TaskMode.手动任务 && dangerousDoorCount > 0)
                //{
                //    Tip.Alert(string.Format("请先关闭{0}的门！", dangerousStation.Name));
                //    return;
                //}

                TaskReset();
                Current.TaskMode = Current.TaskMode == TaskMode.自动任务 ? TaskMode.手动任务 : TaskMode.自动任务;
                Operation.Add("切换为 " + Current.TaskMode);
                lbAuto.Text = Current.TaskMode == TaskMode.自动任务 ? "切换手动" : "切换自动";
                Tip.Alert(string.Format("成功切换为{0}！", Current.TaskMode));
                CurrentTask.ToSwitchManuTaskMode = false;
            }
            else if (pictureBox.Name.Contains("TaskFuWei"))
            {
                //if (Current.Task.Status == TaskStatus.完成)
                //{
                //    Tip.Alert("尚未生成任务！");
                //    return;
                //}

                if (Current.runStstus == RunStatus.运行)
                {
                    Tip.Alert("请先停止！");
                    return;
                }
                TaskReset();
                Operation.Add("任务复位成功！");
                Tip.Alert("任务复位成功！");
            }
        }

        /// <summary>
        /// 复位成功
        /// </summary>
        private void TaskReset()
        {
            Current.Task.NextFromStationId = -1;
            Current.Task.NextToStationId = -1;
            Current.Task.FromStationId = -1;
            Current.Task.ToStationId = -1;
            Current.Task.TaskId = -1;
            Current.Task.StartTime = TengDa.Common.DefaultTime;
            Current.Task.FromClampStatus = ClampStatus.未知;
            Current.Task.Status = TaskStatus.完成;
            Current.RGV.IsMoving = false;
        }

        private bool PlcConnect()
        {
            string msg = string.Empty;

            if (Current.Feeder.IsEnable)
            {
                if (!Current.Feeder.Plc.IsPingSuccess)
                {
                    Error.Alert(string.Format("无法连接到{0}, IP:{1}", Current.Feeder.Plc.Name, Current.Feeder.Plc.IP));
                    return false;
                }

                if (!Current.Feeder.Plc.TcpConnect(out msg))
                {
                    Error.Alert(string.Format("{0}:打开连接失败，原因：{1}", Current.Feeder.Name, msg));
                    return false;
                }
                this.machinesStatusUC1.SetStatusInfo(Current.Feeder, "连接成功");
                this.machinesStatusUC1.SetLampColor(Current.Feeder, Color.Green);
            }
            

            for (int i = 0; i < OvenCount; i++)
            {
                if (Current.ovens[i].IsEnable)
                {
                    if (!Current.ovens[i].Plc.IsPingSuccess)
                    {
                        Error.Alert(string.Format("无法连接到{0}, IP:{1}", Current.ovens[i].Plc.Name, Current.ovens[i].Plc.IP));
                        return false;
                    }

                    if (!Current.ovens[i].Plc.TcpConnect(out msg))
                    {
                        Error.Alert(string.Format("{0}:打开连接失败，原因：{1}", Current.ovens[i].Name, msg));
                        return false;
                    }
                    this.machinesStatusUC1.SetStatusInfo(Current.ovens[i], "连接成功");
                    this.machinesStatusUC1.SetLampColor(Current.ovens[i], Color.Green);
                }
            }


            //if (Current.Blanker.IsEnable)
            //{
            //    if (!Current.Blanker.Plc.IsPingSuccess)
            //    {
            //        Error.Alert(string.Format("无法连接到{0}, IP:{1}", Current.Blanker.Plc.Name, Current.Blanker.Plc.IP));
            //        return false;
            //    }

            //    if (!Current.Blanker.Plc.TcpConnect(out msg))
            //    {
            //        Error.Alert(string.Format("{0}:打开连接失败，原因：{1}", Current.Blanker.Name, msg));
            //        return false;
            //    }
            //    this.machinesStatusUC1.SetStatusInfo(Current.Blanker, "连接成功");
            //    this.machinesStatusUC1.SetLampColor(Current.Blanker, Color.Green);
            //}
            

            if (Current.RGV.IsEnable)
            {
                if (!Current.RGV.Plc.IsPingSuccess)
                {
                    Error.Alert(string.Format("无法连接到{0}, IP:{1}", Current.RGV.Plc.Name, Current.RGV.Plc.IP));
                    return false;
                }

                if (!Current.RGV.Plc.TcpConnect(out msg))
                {
                    Error.Alert(string.Format("打开RGV连接失败，原因：{0}", msg));
                    return false;
                }

                Current.RGV.Plc.StartListenReceiveData();

                this.machinesStatusUC1.SetStatusInfo(Current.RGV, "连接成功");
                this.machinesStatusUC1.SetLampColor(Current.RGV, Color.Green);
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

            if (Current.Feeder.IsEnable)
            {
                if (!Current.Feeder.Plc.TcpDisConnect(out msg))
                {
                    Error.Alert(msg);
                    return false;
                }
                this.machinesStatusUC1.SetStatusInfo(Current.Feeder, "未连接");
                this.machinesStatusUC1.SetLampColor(Current.Feeder, Color.Gray);
            }

            Current.Feeder.PreAlarmStr = string.Empty;
            

            for (int i = 0; i < OvenCount; i++)
            {
                if (Current.ovens[i].IsEnable)
                {
                    if (!Current.ovens[i].Plc.TcpDisConnect(out msg))
                    {
                        Error.Alert(msg);
                        return false;
                    }
                    this.machinesStatusUC1.SetStatusInfo(Current.ovens[i], "未连接");
                    this.machinesStatusUC1.SetLampColor(Current.ovens[i], Color.Gray);
                }

                //防止长时间未连接导致烤箱信息与实际不符
                Current.ovens[i].AlreadyGetAllInfo = false;
                Current.ovens[i].getInfoNum = 0;

                Current.ovens[i].PreAlarmStr = string.Empty;
                for (int j = 0; j < Current.ovens[i].Floors.Count; j++)
                {
                    Current.ovens[i].Floors[j].DoorStatus = DoorStatus.未知;
                    Current.ovens[i].Floors[j].PreAlarmStr = string.Empty;
                }
            }



            //if (Current.Blanker.IsEnable)
            //{
            //    if (!Current.Blanker.Plc.TcpDisConnect(out msg))
            //    {
            //        Error.Alert(msg);
            //        return false;
            //    }
            //    this.machinesStatusUC1.SetStatusInfo(Current.Blanker, "未连接");
            //    this.machinesStatusUC1.SetLampColor(Current.Blanker, Color.Gray);
            //}

            //Current.Blanker.PreAlarmStr = string.Empty;
            

            if (Current.RGV.IsEnable)
            {
                //if (Current.RGV.IsStartting && !Current.RGV.IsPausing)
                //{
                //    Current.RGV.Pause(out msg);
                //}

                if (!Current.RGV.Plc.TcpDisConnect(out msg))
                {
                    Error.Alert(msg);
                    return false;
                }
                this.machinesStatusUC1.SetStatusInfo(Current.RGV, "未连接");
                this.machinesStatusUC1.SetLampColor(Current.RGV, Color.Gray);
            }

            //Current.Transfer.IsAlive = false;
            //Current.Cacher.IsAlive = false;

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

            //if (Current.BatteryScaner.IsEnable)
            //{
            //    if (!Current.BatteryScaner.IsPingSuccess)
            //    {

            //        Error.Alert(string.Format("无法连接到{0}：{1}", Current.BatteryScaner.Name, Current.BatteryScaner.IP));
            //        return false;
            //    }

            //    if (!Current.BatteryScaner.TcpConnect(out msg))
            //    {
            //        Error.Alert(string.Format("{0}:打开连接失败，原因：{1}", Current.BatteryScaner.Name, msg));
            //        return false;
            //    }
            //    this.machinesStatusUC1.SetStatusInfo(Current.BatteryScaner, "连接成功");
            //    this.machinesStatusUC1.SetLampColor(Current.BatteryScaner, Color.Green);
            //}

            if (Current.ClampScaner.IsEnable)
            {
                if (!Current.ClampScaner.IsPingSuccess)
                {

                    Error.Alert(string.Format("无法连接到{0}：{1}", Current.ClampScaner.Name, Current.ClampScaner.IP));
                    return false;
                }

                if (!Current.ClampScaner.TcpConnect(out msg))
                {
                    Error.Alert(string.Format("{0}:打开连接失败，原因：{1}", Current.ClampScaner.Name, msg));
                    return false;
                }

                this.machinesStatusUC1.SetStatusInfo(Current.ClampScaner, "连接成功");
                this.machinesStatusUC1.SetLampColor(Current.ClampScaner, Color.Green);
            }

            return true;
        }

        private bool ScanerDisConnect()
        {
            string msg = string.Empty;

            //if (Current.BatteryScaner.IsEnable)
            //{
            //    if (Current.BatteryScaner.IsAlive)
            //    {
            //        Current.BatteryScaner.StopClampScan();
            //    }

            //    if (!Current.BatteryScaner.TcpDisConnect(out msg))
            //    {
            //        Error.Alert(msg);
            //        return false;
            //    }

            //    //this.machinesStatusUC1.SetStatusInfo(Current.BatteryScaner, "未连接");
            //    //this.machinesStatusUC1.SetLampColor(Current.BatteryScaner, Color.Gray);
            //}

            if (Current.ClampScaner.IsEnable)
            {
                if (Current.ClampScaner.IsAlive)
                {
                    Current.ClampScaner.StopClampScan();
                }
                    
                if (!Current.ClampScaner.TcpDisConnect(out msg))
                {
                    Error.Alert(msg);
                    return false;
                }

                this.machinesStatusUC1.SetStatusInfo(Current.ClampScaner, "未连接");
                this.machinesStatusUC1.SetLampColor(Current.ClampScaner, Color.Gray);
            }

            
            return true;
        }

        #endregion

        #region 主运行逻辑

        delegate void UpdateUI1PDelegate(string text);

        System.Timers.Timer[] timerOvenRuns = new System.Timers.Timer[OvenCount];

        System.Timers.Timer[] timerFeederRuns = new System.Timers.Timer[1] { null };

        System.Timers.Timer[] timerBlankerRuns = new System.Timers.Timer[1] { null };

        System.Timers.Timer timerRGVRun = null;

        System.Timers.Timer timerRun = null;

        private static bool timerlock = false;

        /// <summary>
        /// 启动前检测是否所有启用的设备均已连接成功
        /// </summary>
        /// <returns></returns>
        public bool CheckStart(out string msg)
        {
            for (int i = 0; i < OvenCount; i++)
            {
                if (Current.ovens[i].IsEnable && !Current.ovens[i].Plc.IsAlive)
                {
                    msg = Current.ovens[i].Name + " 启动异常！";
                    return false;
                }
            }


            if (Current.Feeder.IsEnable && !Current.Feeder.Plc.IsAlive)
            {
                msg = Current.Feeder.Name + " 启动异常！";
                return false;
            }


            if (Current.ClampScaner.IsEnable && !Current.ClampScaner.IsAlive)
            {
                msg = Current.ClampScaner.Name + " 启动异常！";
                return false;
            }

            //if (Current.BatteryScaner.IsEnable && !Current.BatteryScaner.IsAlive)
            //{
            //    msg = Current.BatteryScaner.Name + " 启动异常！";
            //    return false;
            //}


            //if (Current.Blanker.IsEnable && !Current.Blanker.Plc.IsAlive)
            //{
            //    msg = Current.Blanker.Name + " 启动异常！";
            //    return false;
            //}
            

            if (Current.RGV.IsEnable && !Current.RGV.Plc.IsAlive)
            {
                msg = Current.RGV.Name + " 启动异常！";
                return false;
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
                for (int i = 0; i < OvenCount; i++)
                {
                    int index = i;//如果直接用i, 则完成循环后 i一直 = OvenCount
                    timerOvenRuns[i] = new System.Timers.Timer();
                    timerOvenRuns[i].Interval = TengDa._Convert.StrToInt(TengDa.WF.Option.GetOption("CheckPlcPeriod"), 1000);
                    timerOvenRuns[i].Elapsed += delegate
                    {
                        Thread listen = new Thread(new ParameterizedThreadStart(OvenRunInvokeFunc));
                        listen.IsBackground = true;
                        listen.Start(index);
                    };
                    Thread.Sleep(200);
                    timerOvenRuns[i].Start();
                }

                for (int i = 0; i < 1; i++)
                {
                    int index = i;//如果直接用i, 则完成循环后 i一直 = OvenCount
                    timerFeederRuns[i] = new System.Timers.Timer();
                    timerFeederRuns[i].Interval = TengDa._Convert.StrToInt(TengDa.WF.Option.GetOption("CheckPlcPeriod"), 1000) / 3;
                    timerFeederRuns[i].Elapsed += delegate
                    {
                        Thread listen = new Thread(new ParameterizedThreadStart(FeederRunInvokeFunc));
                        listen.IsBackground = true;
                        listen.Start(index);
                    };
                    Thread.Sleep(200);
                    timerFeederRuns[i].Start();
                }


                timerBlankerRuns[0] = new System.Timers.Timer();
                timerBlankerRuns[0].Interval = TengDa._Convert.StrToInt(TengDa.WF.Option.GetOption("CheckPlcPeriod"), 1000);
                timerBlankerRuns[0].Elapsed += delegate
                {
                    Thread listen = new Thread(new ParameterizedThreadStart(BlankerRunInvokeFunc));
                    listen.IsBackground = true;
                    listen.Start(0);
                };
                Thread.Sleep(200);
                timerBlankerRuns[0].Start();
                

                timerRGVRun = new System.Timers.Timer();
                timerRGVRun.Interval = TengDa._Convert.StrToInt(TengDa.WF.Option.GetOption("CheckPlcPeriod"), 1000);
                timerRGVRun.Elapsed += delegate
                {
                    Thread listen = new Thread(new ThreadStart(RGVRunInvokeFunc));
                    listen.IsBackground = true;
                    listen.Start();
                };
                Thread.Sleep(200);
                timerRGVRun.Start();

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

                this.timerTask = new System.Timers.Timer();
                this.timerTask.Interval = Current.option.TaskInterval;
                this.timerTask.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Task);
                this.timerTask.AutoReset = true;
                this.timerTask.Start();

                this.timerRecordTV = new System.Timers.Timer();
                this.timerRecordTV.Interval = TengDa._Convert.StrToInt(Current.option.RecordTVInterval, 60000);
                this.timerRecordTV.Elapsed += new System.Timers.ElapsedEventHandler(Timer_RecordTV);
                this.timerRecordTV.AutoReset = true;
                this.timerRecordTV.Start();

                this.timerPaintCurve = new System.Timers.Timer();
                this.timerPaintCurve.Interval = Current.option.PaintCurveInterval;
                this.timerPaintCurve.Elapsed += new System.Timers.ElapsedEventHandler(Timer_PaintCurve);
                this.timerPaintCurve.AutoReset = true;
                this.timerPaintCurve.Start();


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
            for (int i = 0; i < OvenCount; i++)
            {
                if (timerOvenRuns[i] != null)
                {
                    timerOvenRuns[i].Stop();
                    timerOvenRuns[i].Close();
                    timerOvenRuns[i].Dispose();
                }
            }

            for (int i = 0; i < 1; i++)
            {
                if (timerFeederRuns[i] != null)
                {
                    timerFeederRuns[i].Stop();
                    timerFeederRuns[i].Close();
                    timerFeederRuns[i].Dispose();
                }
            }

            if (timerBlankerRuns[0] != null)
            {
                timerBlankerRuns[0].Stop();
                timerBlankerRuns[0].Close();
                timerBlankerRuns[0].Dispose();
            }
            

            if (timerRGVRun != null)
            {
                timerRGVRun.Stop();
                timerRGVRun.Close();
                timerRGVRun.Dispose();
            }

            if (timerPaintCurve != null)
            {
                timerPaintCurve.Stop();
                timerPaintCurve.Close();
                timerPaintCurve.Dispose();
            }

            if (timerTask != null)
            {
                timerTask.Stop();
                timerTask.Close();
                timerTask.Dispose();
            }

            if (timerRecordTV != null)
            {
                timerRecordTV.Stop();
                timerRecordTV.Close();
                timerRecordTV.Dispose();
            }

            if (timerUploadMes != null)
            {
                timerUploadMes.Stop();
                timerUploadMes.Close();
                timerUploadMes.Dispose();
            }
        }

        private void OvenRunInvokeFunc(object obj)
        {

            string msg = string.Empty;

            int i = System.Convert.ToInt32(obj);

            if (timerlock && Current.ovens[i].IsEnable)
            {

                this.BeginInvoke(new MethodInvoker(() => { this.machinesStatusUC1.SetStatusInfo(Current.ovens[i], "发送指令"); }));
                if (Current.ovens[i].GetInfo())
                {
                    this.BeginInvoke(new MethodInvoker(() => { this.machinesStatusUC1.SetStatusInfo(Current.ovens[i], "获取信息成功"); }));
                }
                else
                {
                    this.BeginInvoke(new MethodInvoker(() => { this.machinesStatusUC1.SetStatusInfo(Current.ovens[i], "获取信息失败"); }));
                }
                

                if (Current.ovens[i].AlreadyGetAllInfo)
                {
                    for (int j = 0; j < Current.ovens[i].Floors.Count; j++)
                    {
                        Floor floor = Current.ovens[i].Floors[j];
                        for (int k = 0; k < Current.ovens[i].Floors[j].Stations.Count; k++)
                        {
                            Station station = Current.ovens[i].Floors[j].Stations[k];
                            if (floor.IsBaking)
                            {
                                station.FloorStatus = FloorStatus.烘烤;
                            }
                            else if (station.ClampStatus == ClampStatus.无夹具)
                            {
                                station.FloorStatus = FloorStatus.无盘;
                            }
                            else if (station.ClampStatus == ClampStatus.空夹具)
                            {
                                station.FloorStatus = FloorStatus.空盘;
                            }
                            else if (station.ClampStatus == ClampStatus.满夹具 && (station.PreFloorStatus == FloorStatus.无盘 || station.PreFloorStatus == FloorStatus.待烤 || station.PreFloorStatus == FloorStatus.未知))
                            {
                                station.FloorStatus = FloorStatus.待烤;
                            }
                            else if (station.ClampStatus == ClampStatus.满夹具 && (station.PreFloorStatus == FloorStatus.烘烤 || station.PreFloorStatus == FloorStatus.待出 || station.PreFloorStatus == FloorStatus.未知))
                            {
                                station.FloorStatus = FloorStatus.待出;
                            }
                            else
                            {
                                station.FloorStatus = FloorStatus.未知;
                            }

                            if (station.PreFloorStatus != station.FloorStatus)
                            {
                                string tip = string.Format("{0}:{1}-->{2}", station.Name, station.PreFloorStatus, station.FloorStatus);
                                AddTips(tip);
                                StationLog.Add(new List<StationLog>() { new StationLog() { StationId = station.Id, Message = tip } }, out msg);

                                //烘烤完成直接泄真空                         
                                if (station.FloorStatus == FloorStatus.待出 && floor.IsVacuum && floor.RunRemainMinutes <= 1)
                                {
                                    Current.ovens[i].UploadVacuum(j);
                                }
                                this.ovenUCs[i].Invalidate(j);
                              
                            }
                            else if (station.ClampStatus == ClampStatus.异常)
                            {
                                this.ovenUCs[i].Invalidate(j);                            
                            }

                            station.PreFloorStatus = station.FloorStatus;

                            switch (station.FloorStatus)
                            {
                                case FloorStatus.无盘:
                                    station.Status = StationStatus.可放;
                                    break;
                                case FloorStatus.空盘:
                                    station.Status = StationStatus.可取;
                                    break;
                                case FloorStatus.待出:
                                    if (floor.RunRemainMinutes <= 0 || floor.RunMinutes <= 0)
                                    {
                                        station.Status = StationStatus.可取;
                                    }
                                    else
                                    {
                                        station.Status = StationStatus.工作中;
                                    }                                   
                                    break;
                                case FloorStatus.待烤:
                                case FloorStatus.烘烤:
                                    station.Status = StationStatus.工作中;
                                    break;
                                default:
                                    station.Status = StationStatus.不可用;
                                    break;
                            }

                        }

                        //自动状态下网控强制打开
                        if (Current.TaskMode == TaskMode.自动任务 && !floor.IsNetControlOpen && floor.IsEnable)
                        {
                            Current.ovens[i].OpenNetControl(j);
                        }
                    }
                }
            }
        }

        private void FeederRunInvokeFunc(object obj)
        {

            string msg = string.Empty;

            int i = System.Convert.ToInt32(obj);

            if (timerlock && Current.Feeder.IsEnable)
            {
                this.BeginInvoke(new MethodInvoker(() => { this.machinesStatusUC1.SetStatusInfo(Current.Feeder, "发送指令"); }));
                if (Current.Feeder.GetInfo())
                {
                    this.BeginInvoke(new MethodInvoker(() => { this.machinesStatusUC1.SetStatusInfo(Current.Feeder, "获取信息成功"); }));
                }
                else
                {
                    this.BeginInvoke(new MethodInvoker(() => { this.machinesStatusUC1.SetStatusInfo(Current.Feeder, "获取信息失败"); }));
                }

                if (Current.Feeder.AlreadyGetAllInfo)
                {
                    #region 夹具扫码逻辑
                    if (Current.ClampScaner.IsEnable && Current.ClampScaner.CanScan)
                    {
                        Current.Feeder.Stations.ForEach(s =>
                        {
                            if (s.IsClampScanReady)
                            {
                                string code = string.Empty;
                                ScanResult result = Current.ClampScaner.StartClampScan(out code, out msg);
                                if (result == ScanResult.OK)
                                {
                                    //this.BeginInvoke(new MethodInvoker(() => { this.tbScanerStatus[i][2].Text = "+" + code; }));                           
                                    s.Clamp.Code = code;
                                    s.Clamp.ScanTime = DateTime.Now;

                                    if (!Current.Feeder.SetScanClampResult(ScanResult.OK, out msg))
                                    {
                                        Error.Alert(msg);
                                    }
                                }
                                else if (result == ScanResult.NG || result == ScanResult.Timeout)
                                {
                                    //this.BeginInvoke(new MethodInvoker(() => { this.tbScanerStatus[i][2].Text = "扫码NG"; }));
                                    if (!Current.Feeder.SetScanClampResult(ScanResult.NG, out msg))
                                    {
                                        Error.Alert(msg);
                                    }
                                }
                                else if (!string.IsNullOrEmpty(msg))
                                {
                                    Error.Alert(msg);
                                }

                            }
                        });
                    }
                    #endregion

                    #region 绘制夹具中电池个数图示
                    for (int j = 0; j < 3; j++)
                    {
                        if (Current.Feeder.Stations[j].Id == Current.Feeder.CurrentPutStationId)
                        {
                            if (Current.Feeder.PreCurrentBatteryCount != Current.Feeder.CurrentBatteryCount)
                            {
                               // this.feederUC1.Invalidate(j);
                                //  this.tlpFeederStationClamp[i][j].Invalidate();
                            }
                        }

                        if (Current.Feeder.Stations[j].PreIsAlive != Current.Feeder.Stations[j].IsAlive)
                        {
                            //this.feederUC1.Invalidate(j);
                            // this.tlpFeederStationClamp[i][j].Invalidate();
                        }
                        Current.Feeder.Stations[j].PreIsAlive = Current.Feeder.Stations[j].IsAlive;
                    }
                    Current.Feeder.PreCurrentBatteryCount = Current.Feeder.CurrentBatteryCount;
                    #endregion
                }
            }
        }

        private void BlankerRunInvokeFunc(object obj)
        {

            string msg = string.Empty;

            int i = System.Convert.ToInt32(obj);


            //if (timerlock && Current.Blanker.IsEnable)
            //{
            //    this.BeginInvoke(new MethodInvoker(() => { this.machinesStatusUC1.SetStatusInfo(Current.Blanker, "发送指令"); }));
            //    if (Current.Blanker.GetInfo())
            //    {
            //        this.BeginInvoke(new MethodInvoker(() => { this.machinesStatusUC1.SetStatusInfo(Current.Blanker, "获取信息成功"); }));
            //    }
            //    else
            //    {
            //        this.BeginInvoke(new MethodInvoker(() => { this.machinesStatusUC1.SetStatusInfo(Current.Blanker, "获取信息失败"); }));
            //    }
            //}
        }

        private void RGVRunInvokeFunc()
        {
            string msg = string.Empty;
            if (timerlock && Current.RGV.IsEnable)
            {
                this.BeginInvoke(new MethodInvoker(() => { this.machinesStatusUC1.SetStatusInfo(Current.RGV, "发送指令"); }));
                if (Current.RGV.GetInfo())
                {
                    this.BeginInvoke(new MethodInvoker(() => { this.machinesStatusUC1.SetStatusInfo(Current.RGV, "获取信息成功"); }));
                }
                else
                {
                    this.BeginInvoke(new MethodInvoker(() => { this.machinesStatusUC1.SetStatusInfo(Current.RGV, "获取信息失败"); }));
                }

                if (Current.RGV.AlreadyGetAllInfo)
                {


                }
            }
        }
        /// <summary>
        /// 总体运行逻辑
        /// </summary>
        private void RunInvokeFunc()
        {
            if (!TengDa.WF.Current.IsTerminalInitFinished)
            {
                return;
            }

            if (timerlock && Math.Abs((DateTime.Now - Current.ChangeModeTime).Seconds) > 3)
            {
                if (Current.TaskMode == TaskMode.自动任务)
                {
                    //待烤层关门启动
                    for (int i = 0; i < OvenCount; i++)
                    {
                        for (int j = 0; j < Current.ovens[i].Floors.Count; j++)
                        {
                            Floor floor = Current.ovens[i].Floors[j];

                            //无任务默认关门
                            if (floor.DoorStatus == DoorStatus.打开 && floor.Stations.Count(s => s.Id == Current.Task.FromStationId) == 0
                                && floor.Stations.Count(s => s.Id == Current.Task.ToStationId) == 0
                                && floor.Stations[0].IsAlive && floor.Stations[1].IsAlive)
                            {
                                Current.ovens[i].CloseDoor(j);
                            }

                            ////从某一炉子取完盘后，立即关门，无需等到整个任务结束
                            //if (floor.DoorStatus == DoorStatus.打开 && floor.Stations.Count(s => s.Id == Current.Task.FromStationId) > 0 && floor.Stations[0].IsAlive && floor.Stations[1].IsAlive
                            //    && Current.RGV.IsAlive && (Current.Task.Status == TaskStatus.取完 || Current.Task.Status == TaskStatus.正放))
                            //{
                            //    Current.ovens[i].CloseDoor(j);
                            //}

                            //开始烘烤
                            if (floor.Stations.Count(s => s.FloorStatus == FloorStatus.待烤) == floor.Stations.Count)
                            {
                                if (floor.Stations[0].IsAlive && floor.Stations[1].IsAlive && floor.DoorStatus == DoorStatus.关闭)
                                {
                                    // Current.ovens[i].ClearRunTime(j);
                                    Current.ovens[i].StartBaking(j);
                                }
                            }

                            ////水分NG回炉的重新开始烘烤
                            //if (floor.Stations.Count(s => s.FloorStatus == FloorStatus.待烤 && s.SampleStatus == SampleStatus.测试NG) == 1 && floor.Stations.Count(s => s.FloorStatus == FloorStatus.待出 && s.SampleStatus == SampleStatus.测试NG) == 1)
                            //{
                            //    if (floor.Stations[0].IsAlive && floor.Stations[1].IsAlive && floor.DoorStatus == DoorStatus.关闭)
                            //    {
                            //        //if (floor.NgTimes < 2)
                            //        //{
                            //        //Current.ovens[i].ClearRunTime(j);
                            //        Current.ovens[i].StartBaking(j);
                            //        //}
                            //        //else
                            //        //{
                            //        //    //水分测试两次NG，禁用炉腔
                            //        //    floor.IsEnable = false;
                            //        //}
                            //    }
                            //}
                        }
                    }

                    //测水分出炉前泄真空
                    var floors1 = Floor.FloorList.Where(f => f.IsAlive && f.IsVacuum && !f.Stations[0].IsOpenDoorIntervene && f.Stations.Count(s => s.FloorStatus == FloorStatus.待出) == 2).OrderBy(f => f.Stations[0].GetPutTime);
                    if (floors1.Count() > 0)
                    {
                        var floor = floors1.First();
                        if (floor.RunRemainMinutes <= 0)
                        {
                            var oven = floor.GetOven();
                            var jj = oven.Floors.IndexOf(floor);
                            oven.UploadVacuum(jj);
                        }
                    }

                    //测试OK出炉前泄真空
                    var floors2 = Floor.FloorList.Where(f => f.IsAlive && f.IsVacuum && !f.Stations[0].IsOpenDoorIntervene && f.Stations.Count(s => s.FloorStatus == FloorStatus.待出) > 0 && f.Stations.Count(s => s.SampleStatus == SampleStatus.水分OK) > 0).OrderBy(f => f.Stations[0].GetPutTime);
                    if (floors2.Count() > 0)
                    {
                        var floor = floors2.First();
                        if (floor.RunRemainMinutes <= 0)
                        {
                            var oven = floor.GetOven();
                            var jj = oven.Floors.IndexOf(floor);
                            oven.UploadVacuum(jj);
                        }
                    }


                    //List<ClampOri> ClampOris = new List<ClampOri> { ClampOri.A, ClampOri.B };
                    //foreach (ClampOri clampOri in ClampOris)
                    //{
                    //List<Station> baseStations = Station.StationList.Where(s =>
                    //        s.ClampOri == clampOri
                    //        && s.IsAlive
                    //        && s.GetPutType == GetPutType.烤箱
                    //        && s.FloorStatus == FloorStatus.待出).ToList();

                    //if (baseStations.Count < 1 || baseStations.Where(s => s.DoorStatus == DoorStatus.打开).ToList().Count > 0)
                    //{
                    //    continue;
                    //}

                    //Station oStation = baseStations.Where(s => !s.IsOpenDoorIntervene).OrderBy(s => s.Priority).OrderBy(s => s.GetPutTime).OrderBy(s => s.OutPriority).FirstOrDefault();
                    //if (oStation == null) continue;

                    //Floor oFloor = oStation.GetFloor();
                    //Oven oOven = oStation.GetOven();
                    //int jj = oOven.Floors.IndexOf(oFloor);
                    //if (oFloor.IsVacuum)
                    //{
                    //    oOven.UploadVacuum(jj);
                    //}

                    //if (oFloor.DoorStatus == DoorStatus.关闭 && !oFloor.IsVacuum)
                    //{
                    //    oOven.OpenDoor(jj);
                    //}
                    //}

                    ////下料台取消干涉

                    //for (int j = 0; j < Current.Blanker.Stations.Count; j++)
                    //{
                    //    Station station = Current.Blanker.Stations[j];

                    //    ////无任务默认关门
                    //    //if (station.DoorStatus == DoorStatus.打开 && station.Id != Current.Task.FromStationId && station.Id != Current.Task.ToStationId && station.IsAlive)
                    //    //{
                    //    //    station.CloseDoor();
                    //    //}

                    //    ////从某一炉子取完盘后，立即关门，无需等到整个任务结束
                    //    //if (station.DoorStatus == DoorStatus.打开 && station.Id == Current.Task.FromStationId && station.IsAlive
                    //    //    && (Current.Task.Status == TaskStatus.取完 || Current.Task.Status == TaskStatus.正放))
                    //    //{
                    //    //    station.CloseDoor();
                    //    //}
                    //}
                    
                }

                //无论手动还是自动任务模式都会执行
                for (int i = 0; i < OvenCount; i++)
                {
                    for (int j = 0; j < Current.ovens[i].Floors.Count; j++)
                    {
                        Floor floor = Current.ovens[i].Floors[j];

                        if (floor.Stations.Count(s => s.FloorStatus == FloorStatus.烘烤) == 2)
                        {
                            floor.Stations.ForEach(s =>
                            {
                                s.SampleStatus = SampleStatus.待结果;
                            });
                        }

                        if (floor.Stations.Count(s => s.FloorStatus == FloorStatus.待出 && s.SampleInfo == SampleInfo.无样品) == 2)
                        {
                            floor.Stations.ForEach(s =>
                            {
                                s.SampleStatus = SampleStatus.水分OK;
                            });
                        }
                    }
                }
            }
        }


        #endregion

        #region RGV搬运主逻辑

        System.Timers.Timer timerTask = null;
        private void Timer_Task(object sender, ElapsedEventArgs e)
        {
            if (timerlock)
            {
                Task.Run();
            }
        }
        #endregion

        #region 定时将温度真空存入数据库
        System.Timers.Timer timerRecordTV = null;
        private void Timer_RecordTV(object sender, ElapsedEventArgs e)
        {
            if (timerlock)
            {
                TVD.Add();
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
                UploadBatteryInfo(new List<Clamp>());
            }
        }

        public void UploadBatteryInfo(object obj)
        {
            try
            {
                Thread.Sleep(200);
                List<Clamp> clamps = (List<Clamp>)obj;
                string msg = string.Empty;
                if (clamps.Count < 1)
                {
                    clamps = Clamp.GetList(string.Format("SELECT TOP 3 * FROM [dbo].[{0}] WHERE [IsUploaded] = 'false' AND [IsFinished] = 'true' ORDER BY [Id] DESC", Clamp.TableName), out msg);
                    if (!string.IsNullOrEmpty(msg))
                    {
                        Error.Alert(msg);
                        return;
                    }
                }

                if (clamps.Count < 1)
                {
                    return;
                }

                this.BeginInvoke(new MethodInvoker(() =>
                {
                    this.machinesStatusUC1.SetStatusInfo(Current.mes, "上传烘烤数据ID:" + clamps[0].Id);
                }));
                MES.UploadBatteryInfo(clamps);
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
                DataTable dt = Database.Query(string.Format("SELECT * FROM [dbo].[{0}.V_Battery] WHERE [扫码时间] BETWEEN '{1}' AND '{2}'", Config.DbTableNamePre, startTime, stopTime), out msg);
                if (dt == null)
                {
                    Error.Alert(msg);
                    return;
                }

                this.BeginInvoke(new MethodInvoker(() =>
                {
                    dgViewBattery.DataSource = dt;

                    dgViewBattery.Columns[1].DefaultCellStyle.Format = "yyyy-MM-dd  HH:mm:ss";
                    dgViewBattery.Columns[4].DefaultCellStyle.Format = "yyyy-MM-dd  HH:mm:ss";
                    dgViewBattery.Columns[5].DefaultCellStyle.Format = "yyyy-MM-dd  HH:mm:ss";
                    dgViewBattery.Columns[6].DefaultCellStyle.Format = "yyyy-MM-dd  HH:mm:ss";
                    dgViewBattery.Columns[7].DefaultCellStyle.Format = "yyyy-MM-dd  HH:mm:ss";

                    ////设置显示列宽度
                    dgViewBattery.Columns[0].Width = 160;
                    dgViewBattery.Columns[1].Width = 130;
                    dgViewBattery.Columns[2].Width = 80;

                    dgViewBattery.Columns[4].Width = 130;
                    dgViewBattery.Columns[5].Width = 130;
                    dgViewBattery.Columns[6].Width = 130;
                    dgViewBattery.Columns[7].Width = 130;
                    //dgViewBattery.Columns[8].Width = 100;

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
            yieldDisplay.SetClearYieldTime(DateTime.Now);
        }

        private void btnOperQuery_Click(object sender, EventArgs e)
        {
            var startTime = dtPickerOperStart.Value;
            var stopTime = dtPickerOperStop.Value;

            TimeSpan ts = startTime - stopTime;
            int timeSpan = TengDa._Convert.StrToInt(Current.option.QueryTVTimeSpan, -1);
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
                    dgViewOper.Columns[3].Width = 400;
                    dgViewOper.Columns[4].Width = 130;
                }));
            });
            t.Start();
        }

        private void btnQueryTV_Click(object sender, EventArgs e)
        {
            var startTime = dtpStart.Value;
            var stopTime = dtpStop.Value;
            var count = cbCount.Text.Trim();
            var selectStation = cbStations.Text.Trim();

            Thread t = new Thread(() =>
            {
                string msg = string.Empty;
                DataTable dt = null;
                if (selectStation == "All")
                {
                    dt = Database.Query(string.Format("SELECT TOP {3} * FROM [dbo].[{0}.V_TV] WHERE [记录时间] BETWEEN '{1}' AND '{2}' ORDER BY [记录时间]", Config.DbTableNamePre, startTime, stopTime, count), out msg);
                }
                else
                {
                    dt = Database.Query(string.Format("SELECT TOP {4} * FROM [dbo].[{0}.V_TV] WHERE [烤箱工位] = '{1}' AND [记录时间] BETWEEN '{2}' AND '{3}' ORDER BY [记录时间]", Config.DbTableNamePre, selectStation, startTime, stopTime, count), out msg);
                }

                if (dt == null)
                {
                    Error.Alert(msg);
                    return;
                }
                this.BeginInvoke(new MethodInvoker(() =>
                {
                    dgvTV.DataSource = dt;
                    //设置显示列宽度
                    dgvTV.Columns[0].Width = 110;
                    for (int i = 1; i <= Option.TemperaturePointCount; i++)
                    {
                        dgvTV.Columns[i].Width = 65;
                    }
                    dgvTV.Columns[Option.TemperaturePointCount + 1].Width = 70;
                    dgvTV.Columns[Option.TemperaturePointCount + 2].Width = 100;
                    dgvTV.Columns[Option.TemperaturePointCount + 3].Width = 100;
                    dgvTV.Columns[Option.TemperaturePointCount + 4].Width = 140;
                    dgvTV.Columns[Option.TemperaturePointCount + 4].DefaultCellStyle.Format = "yyyy-MM-dd  HH:mm:ss";
                    tbNumTV.Text = dt.Rows.Count.ToString();
                }));
            });
            t.Start();
        }

        private void btnExportTV_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)this.dgvTV.DataSource;
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
            kk.FileName = "Temperature and Vacuum" + dtpStart.Value.ToString("yyyyMMddHHmmss") + "-" + dtpStop.Value.ToString("yyyyMMddHHmmss");

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
                    dgvAlarm.Columns[0].Width = 100;
                    dgvAlarm.Columns[0].HeaderText = "烤箱/腔体";
                    dgvAlarm.Columns[1].Width = 100;
                    dgvAlarm.Columns[2].Width = 100;
                    dgvAlarm.Columns[3].Width = 150;
                    dgvAlarm.Columns[4].Width = 150;
                    dgvAlarm.Columns[5].Width = 150;
                    dgvAlarm.Columns[6].Width = 150;
                    dgvAlarm.Columns[6].HeaderText = "持续时间(s)";
                    dgvAlarm.Columns[7].Width = 150;
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

        private void btnQueryTaskLog_Click(object sender, EventArgs e)
        {
            var startTime = dtpTaskStart.Value;
            var stopTime = dtpTaskStop.Value;

            TimeSpan ts = startTime - stopTime;
            int timeSpan = TengDa._Convert.StrToInt(Current.option.QueryTVTimeSpan, -1);
            if (Math.Abs(ts.TotalDays) > timeSpan)
            {
                Tip.Alert("查询时间范围太大，请将时间范围设置在 " + timeSpan + " 天 之内！");
                return;
            }

            Thread t = new Thread(() =>
            {

                string msg = string.Empty;

                DataTable dt = Database.Query(string.Format("SELECT * FROM [dbo].[{0}.V_TaskLog] WHERE [任务生成时间] BETWEEN '{1}' AND '{2}' ", Config.DbTableNamePre, startTime, stopTime), out msg);

                if (dt == null)
                {
                    Error.Alert(msg);
                    return;
                }

                this.BeginInvoke(new MethodInvoker(() =>
                {
                    dgvTaskLog.DataSource = dt;
                    //设置显示列宽度

                    dgvTaskLog.Columns[1].Width = 130;
                    dgvTaskLog.Columns[1].DefaultCellStyle.Format = "yyyy-MM-dd  HH:mm:ss";
                    dgvTaskLog.Columns[4].Width = 130;
                    dgvTaskLog.Columns[4].DefaultCellStyle.Format = "yyyy-MM-dd  HH:mm:ss";
                    tbTaskCount.Text = dt.Rows.Count.ToString();
                }));
            });
            t.Start();
        }

        private void btnExportTaskLog_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)this.dgvTaskLog.DataSource;
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
            kk.FileName = "RGV搬运任务 " + dtpStart.Value.ToString("yyyyMMddHHmmss") + "-" + dtpStop.Value.ToString("yyyyMMddHHmmss");

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
                    else if (e.Node.Level == 0 && e.Node.Text == "当前任务")
                    {
                        this.propertyGridSettings.SelectedObject = Current.Task;
                    }
                    else if (e.Node.Level == 1 && e.Node.Parent.Text == "烤箱")
                    {
                        this.propertyGridSettings.SelectedObject = Current.ovens.First(o => o.Id.ToString() == e.Node.Text.Split(':')[0]);
                    }
                    else if (e.Node.Level == 2 && e.Node.Parent.Parent.Text == "烤箱" && e.Node.Text == "PLC")
                    {
                        this.propertyGridSettings.SelectedObject = Current.ovens.First(o => o.Id.ToString() == e.Node.Parent.Text.Split(':')[0]).Plc;
                    }
                    else if (e.Node.Level == 2 && e.Node.Parent.Parent.Text == "烤箱" && e.Node.Text.IndexOf("烤箱") > -1)
                    {
                        Oven oven = Current.ovens.First(o => o.Id.ToString() == e.Node.Parent.Text.Split(':')[0]);
                        this.propertyGridSettings.SelectedObject = oven.Floors.First(f => f.Id.ToString() == e.Node.Text.Split(':')[0]);
                    }
                    else if (e.Node.Level == 0 && e.Node.Text == "MES")
                    {
                        this.propertyGridSettings.SelectedObject = Current.mes;
                    }
                    else if (e.Node.Level == 0 && e.Node.Text == "RGV")
                    {
                        this.propertyGridSettings.SelectedObject = Current.RGV;
                    }
                    //else if (e.Node.Level == 0 && e.Node.Text == "缓存架")
                    //{
                    //    this.propertyGridSettings.SelectedObject = Current.Cacher;
                    //}
                    //else if (e.Node.Level == 0 && e.Node.Text == "转移台")
                    //{
                    //    this.propertyGridSettings.SelectedObject = Current.Transfer;
                    //}
                    else if (e.Node.Level == 1 && e.Node.Parent.Text == "工位列表")
                    {
                        this.propertyGridSettings.SelectedObject = Station.StationList.First(s => s.Id.ToString() == e.Node.Text.Split(':')[0]);
                    }
                    else if (e.Node.Level == 1 && e.Node.Parent.Text == "扫码枪")
                    {
                        this.propertyGridSettings.SelectedObject = Scaner.ScanerList.First(s => s.Id.ToString() == e.Node.Text.Split(':')[0]);
                    }
                    else if (e.Node.Level == 1 && e.Node.Parent.Text == "全部任务列表")
                    {
                        if (TengDa.WF.Current.user.Group.Level < 3)
                        {
                            this.propertyGridSettings.Enabled = false;
                            Tip.Alert("当前用户权限不足！");
                        }
                        this.propertyGridSettings.SelectedObject = Task.TaskList.First(t => t.Id.ToString() == e.Node.Text.Split(':')[0]);
                    }
                    else if (e.Node.Level == 1 && e.Node.Parent.Text == "有效任务排序")
                    {
                        if (TengDa.WF.Current.user.Group.Level < 3)
                        {
                            this.propertyGridSettings.Enabled = false;
                            Tip.Alert("当前用户权限不足！");
                        }
                        this.propertyGridSettings.SelectedObject = Task.TaskList.First(t => t.Id.ToString() == e.Node.Text.Split(':')[0]);
                    }
                    else if (e.Node.Level == 1 && e.Node.Parent.Text == "上下料机")
                    {
                        this.propertyGridSettings.SelectedObject = Current.Feeder;
                        //this.propertyGridSettings.SelectedObject = Feeder.FeederList.First(f => f.Id.ToString() == e.Node.Text.Split(':')[0]);
                    }
                    else if (e.Node.Level == 2 && e.Node.Parent.Parent.Text == "上下料机" && e.Node.Text == "PLC")
                    {
                        this.propertyGridSettings.SelectedObject = Feeder.FeederList.First(f => f.Id.ToString() == e.Node.Parent.Text.Split(':')[0]).Plc;
                    }
                    //else if (e.Node.Level == 1 && e.Node.Parent.Text == "下料机")
                    //{
                    //    this.propertyGridSettings.SelectedObject = Blanker.BlankerList.First(b => b.Id.ToString() == e.Node.Text.Split(':')[0]);
                    //}
                    //else if (e.Node.Level == 2 && e.Node.Parent.Parent.Text == "下料机" && e.Node.Text == "PLC")
                    //{
                    //    this.propertyGridSettings.SelectedObject = Blanker.BlankerList.First(b => b.Id.ToString() == e.Node.Parent.Text.Split(':')[0]).Plc;
                    //}
                    else if (e.Node.Level == 1 && e.Node.Parent.Text == "RGV" && e.Node.Text == "PLC")
                    {
                        this.propertyGridSettings.SelectedObject = Current.RGV.Plc;
                    }
                    else if (e.Node.Level == 1 && e.Node.Parent.Text == "RGV" && e.Node.Text.IndexOf("夹具") > -1)
                    {
                        this.propertyGridSettings.SelectedObject = Current.RGV.Clamp;
                    }
                    else if (e.Node.Level == 2 && e.Node.Text.IndexOf("夹具") > -1)
                    {
                        int stationId = _Convert.StrToInt(e.Node.Parent.Text.Split(':')[0], -1);
                        Clamp clamp = Station.StationList.FirstOrDefault(s => s.Id == stationId).Clamp;
                        if (clamp != null && clamp.Id == _Convert.StrToInt(e.Node.Text.Split(':')[0], -1))
                        {
                            this.propertyGridSettings.SelectedObject = clamp;
                        }
                    }
                    else if (e.Node.Level == 3 && e.Node.Text.IndexOf("电芯") > -1)
                    {
                        int stationId = _Convert.StrToInt(e.Node.Parent.Parent.Text.Split(':')[0], -1);
                        Clamp clamp = Station.StationList.FirstOrDefault(s => s.Id == stationId).Clamp;
                        if (clamp.Id == _Convert.StrToInt(e.Node.Parent.Text.Split(':')[0], -1))
                        {
                            Battery battery = clamp.Batteries.FirstOrDefault(b => b.Id == _Convert.StrToInt(e.Node.Text.Split(':')[0], -1));
                            if (battery != null)
                            {
                                this.propertyGridSettings.SelectedObject = battery;
                            }
                        }
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

            if (type == typeof(Oven) || type == typeof(Floor) || type == typeof(Clamp) || type == typeof(Battery) || type == typeof(Scaner)
                || type == typeof(Feeder) || type == typeof(Blanker) || type == typeof(PLC) || type == typeof(Station) || type == typeof(Task))
            {
                System.Reflection.PropertyInfo propertyInfoId = type.GetProperty("Id"); //获取指定名称的属性
                int Id = (int)propertyInfoId.GetValue(o, null); //获取属性值
                settingsStr = string.Format("将Id为 {0} 的 {1} 的 {2} 由 {3} 修改为 {4} ", Id, type.Name, e.ChangedItem.PropertyDescriptor.DisplayName, e.OldValue, e.ChangedItem.Value);
            }
            else if (type == typeof(RGV))
            {
                settingsStr = string.Format("将{3}的 {0} 由 {1} 修改为 {2} ", e.ChangedItem.PropertyDescriptor.DisplayName, e.OldValue, e.ChangedItem.Value, Current.RGV.Name);
            }
            //else if (type == typeof(Transfer))
            //{
            //    settingsStr = string.Format("将{3}的 {0} 由 {1} 修改为 {2} ", e.ChangedItem.PropertyDescriptor.DisplayName, e.OldValue, e.ChangedItem.Value, Current.Transfer.Name);
            //}
            //else if (type == typeof(Cacher))
            //{
            //    settingsStr = string.Format("将{3}的 {0} 由 {1} 修改为 {2} ", e.ChangedItem.PropertyDescriptor.DisplayName, e.OldValue, e.ChangedItem.Value, Current.Cacher.Name);
            //}
            else if (type == typeof(MES))
            {
                settingsStr = string.Format("将{3}的 {0} 由 {1} 修改为 {2} ", e.ChangedItem.PropertyDescriptor.DisplayName, e.OldValue, e.ChangedItem.Value, Current.mes.Name);
            }
            else if (type == typeof(Option))
            {
                settingsStr = string.Format("将配置项的 {0} 由 {1} 修改为 {2} ", e.ChangedItem.PropertyDescriptor.DisplayName, e.OldValue, e.ChangedItem.Value);
            }
            else if (type == typeof(CurrentTask))
            {
                settingsStr = string.Format("将当前任务的 {0} 由 {1} 修改为 {2} ", e.ChangedItem.PropertyDescriptor.DisplayName, e.OldValue, e.ChangedItem.Value);
            }

            if (!string.IsNullOrEmpty(settingsStr))
            {
                Operation.Add(settingsStr);
                Tip.Alert("成功" + settingsStr);
            }
        }

        private void tsmOvenOpenDoor_Click(object sender, EventArgs e)
        {

        }

        private void tsmOvenCloseDoor_Click(object sender, EventArgs e)
        {

        }

        private void tsmStartBaking_Click(object sender, EventArgs e)
        {

        }

        private void tsmStopBaking_Click(object sender, EventArgs e)
        {

        }

        private void tlpBlanker_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            //tlpBlanker1
            //int i = _Convert.StrToInt((sender as TableLayoutPanel).Name.Substring(10, 1), 0) - 1;

            //Graphics g = e.Graphics;
            //Rectangle r = e.CellBounds;
            //Brush brush = Brushes.White;

            //for (int j = 0; j < Current.Blanker.Stations.Count; j++)
            //{
            //    if (e.Column == 2 - j - 1)
            //    {
            //        if (!Current.Blanker.Stations[j].IsAlive)
            //        {
            //            brush = Brushes.LightGray;
            //        }
            //    }
            //}
            //g.FillRectangle(brush, r);
        }

        private void tlpFeeder_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            //tlpFeeder1
            //int i = _Convert.StrToInt((sender as TableLayoutPanel).Name.Substring(9, 1), 0) - 1;

            //Graphics g = e.Graphics;
            //Rectangle r = e.CellBounds;
            //Brush brush = Brushes.White;

            //for (int j = 0; j < FeederStationCount; j++)
            //{
            //    if (e.Column == FeederStationCount - j - 1 && i == 1 || e.Column == j && i == 0)
            //    {
            //        if (!Current.Feeder.Stations[j].IsAlive)
            //        {
            //            brush = Brushes.LightGray;
            //        }
            //    }
            //}
            //g.FillRectangle(brush, r);
        }


        private string srcBlankerName = string.Empty;
        private void cmsBlanker_Opening(object sender, CancelEventArgs e)
        {
            //tlpBlanker1
            srcBlankerName = (sender as ContextMenuStrip).SourceControl.Name;
            int i = TengDa._Convert.StrToInt(srcBlankerName.Substring(10, 1), 0) - 1;
        }

        //private void TsmCancelRasterInductive_Click(object sender, EventArgs e)
        //{
        //    if (Current.runStstus != RunStatus.运行)
        //    {
        //        Tip.Alert("请先启动！");
        //        return;
        //    }

        //    int i = TengDa._Convert.StrToInt(srcBlankerName.Substring(10, 1), 0) - 1;
        //    Current.Blanker.CancelRasterInductive();
        //}
        #endregion

        #region 绘制温度曲线

        private int gridOffset = 0;//网格偏移
        private const int gridSize = 4;//网格大小

        private int gridOffsetLarge = 0;//网格偏移
        private const int gridSizeLarge = 40;//网格大小

        private Pen gridColor = new Pen(Color.FromArgb(0x33, 0x33, 0x33));//网格颜色

        private Pen gridColorLarge = new Pen(Color.FromArgb(0x44, 0x44, 0x44));//大网格颜色

        private void pCurve_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.Black, e.Graphics.ClipBounds);
            // e.Graphics.FillRectangle(Brushes.White, e.Graphics.ClipBounds);
            #region 绘制网格
            for (int i = pCurve.ClientSize.Width - gridOffset; i >= 0; i -= gridSize)
                e.Graphics.DrawLine(gridColor, i, 0, i, pCurve.ClientSize.Height);
            for (int i = pCurve.ClientSize.Height; i >= 0; i -= gridSize)
                e.Graphics.DrawLine(gridColor, 0, i, pCurve.ClientSize.Width, i);
            #endregion

            #region 绘制大网格
            for (int i = pCurve.ClientSize.Width - gridOffsetLarge; i >= 0; i -= gridSizeLarge)
                e.Graphics.DrawLine(gridColorLarge, i, 0, i, pCurve.ClientSize.Height);
            for (int i = pCurve.ClientSize.Height; i >= 0; i -= gridSizeLarge)
                e.Graphics.DrawLine(gridColorLarge, 0, i, pCurve.ClientSize.Width, i);
            #endregion

            #region 绘制曲线

            Station station = Station.StationList.Where(s => s.Id == Current.option.CurveStationId).FirstOrDefault();
            if (station == null) return;

            Floor floor = Floor.FloorList.First(f => f.Stations.Contains(station));
            Oven oven = Oven.OvenList.First(o => o.Floors.Contains(floor));
            int jj = oven.Floors.IndexOf(floor);

            if (oven.IsAlive)
            {
                for (int k = 0; k < Option.TemperaturePointCount; k++)
                {
                    if (Array.IndexOf(Current.option.CurveIndexs.Split(','), k.ToString()) > -1)
                    {
                        if (station.sampledDatas[k].Count <= 1) return; // 一个数据就不绘制了
                        float A = station.sampledDatas[k][0];//station.sampledDatas[k][0] - 20
                        for (int kk = 1; kk < station.sampledDatas[k].Count; kk++)
                        {
                            float B = station.sampledDatas[k][kk];//station.sampledDatas[k][kk] - 20
                            e.Graphics.DrawLine(new Pen(Current.option.CurveColors[k]),
                                new Point(pCurve.ClientSize.Width - station.sampledDatas[k].Count + kk - 1, pCurve.ClientSize.Height -
                                    (int)(((double)A / 100) * pCurve.ClientSize.Height)),
                                new Point(pCurve.ClientSize.Width - station.sampledDatas[k].Count + kk, pCurve.ClientSize.Height -
                                    (int)(((double)B / 100) * pCurve.ClientSize.Height)));
                            A = B;
                        }
                    }
                }

                for (int k = 0; k < Option.VacuumPointCount; k++)
                {
                    if (cbVacuumIndex[k].Checked)
                    {
                        if (station.sampledDatas[k + Option.TemperaturePointCount].Count <= 1) return; // 一个数据就不绘制了
                        float A = station.sampledDatas[k + Option.TemperaturePointCount][0];//station.sampledDatas[k][0] - 20
                        for (int kk = 1; kk < station.sampledDatas[k + Option.TemperaturePointCount].Count; kk++)
                        {
                            float B = station.sampledDatas[k + Option.TemperaturePointCount][kk];//station.sampledDatas[k][kk] - 20
                            e.Graphics.DrawLine(new Pen(Color.Violet),
                                new Point(pCurve.ClientSize.Width - station.sampledDatas[k + Option.TemperaturePointCount].Count + kk - 1, pCurve.ClientSize.Height -
                                    (int)(((double)A / 100) * pCurve.ClientSize.Height)),
                                new Point(pCurve.ClientSize.Width - station.sampledDatas[k + Option.TemperaturePointCount].Count + kk, pCurve.ClientSize.Height -
                                    (int)(((double)B / 100) * pCurve.ClientSize.Height)));
                            A = B;
                        }
                    }
                }
            }

            #endregion
        }

        System.Timers.Timer timerPaintCurve = null;

        private void Timer_PaintCurve(object sender, ElapsedEventArgs e)
        {
            if (timerlock)
            {
                for (int i = 0; i < Current.ovens.Count; i++)
                {
                    if (Current.ovens[i].IsAlive && Current.ovens[i].AlreadyGetAllInfo)
                    {
                        for (int j = 0; j < Current.ovens[i].Floors.Count; j++)
                        {
                            for (int k = 0; k < Current.ovens[i].Floors[j].Stations.Count; k++)
                            {
                                for (int m = 0; m < Option.TemperaturePointCount + 1; m++)
                                {
                                    while (Current.ovens[i].Floors[j].Stations[k].sampledDatas[m].Count > 1500)
                                        Current.ovens[i].Floors[j].Stations[k].sampledDatas[m].RemoveAt(0);
                                    if (m == Option.TemperaturePointCount)
                                    {
                                        Current.ovens[i].Floors[j].Stations[k].sampledDatas[Option.TemperaturePointCount].Add((float)(Math.Log10(Current.ovens[i].Floors[j].Stations[k].GetFloor().Vacuum4Show) * 10 + 20));
                                    }
                                    else
                                    {
                                        //Current.ovens[i].Floors[j].Stations[k].sampledDatas[m].Add(Current.ovens[i].Floors[j].Stations[k].Temperatures[m]);
                                    }
                                }
                            }
                        }
                    }
                }

                gridOffset = (gridOffset + 1) % gridSize;
                pCurve.Invalidate();
            }
        }

        private void cbTemperIndex_CheckedChanged(object sender, EventArgs e)
        {
            if (!TengDa.WF.Current.IsRunning) return;

            bool isAll = true;
            StringBuilder sb = new StringBuilder();
            for (int k = 0; k < Option.TemperaturePointCount; k++)
            {
                if (cbTemperIndex[k].Checked)
                {
                    sb.Append(k + ",");
                }
                else
                {
                    isAll = false;
                }
            }
            cbTemperAll.CheckedChanged -= cbTemperAll_CheckedChanged;
            cbTemperAll.Checked = isAll;
            cbTemperAll.CheckedChanged += cbTemperAll_CheckedChanged;
            Current.option.CurveIndexs = sb.ToString().TrimEnd(',');
        }

        private void cbTemperAll_CheckedChanged(object sender, EventArgs e)
        {
            if (!TengDa.WF.Current.IsRunning) return;
            for (int k = 0; k < Option.TemperaturePointCount; k++)
            {
                cbTemperIndex[k].CheckedChanged -= cbTemperIndex_CheckedChanged;
                cbTemperIndex[k].Checked = (sender as CheckBox).Checked;
                cbTemperIndex[k].CheckedChanged += cbTemperIndex_CheckedChanged;
            }
            cbTemperIndex_CheckedChanged(null, e);
        }

        private void cbCurveSelected_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!TengDa.WF.Current.IsRunning) return;
            string cbSelectName = (sender as ComboBox).Name;
            int ii = 0, jj = 0, kk = 0;

            if (cbSelectName.Contains("Oven"))
            {
                ii = cbCurveSelectedOven.SelectedIndex;

                cbCurveSelectedFloor.Items.Clear();
                Current.ovens[ii].Floors.ForEach(f =>
                {
                    cbCurveSelectedFloor.Items.Add(f.Name);
                });
                jj = 0;
                cbCurveSelectedFloor.SelectedIndex = jj;

                cbCurveSelectedStation.Items.Clear();
                Current.ovens[ii].Floors[jj].Stations.ForEach(s =>
                {
                    cbCurveSelectedStation.Items.Add(s.Name);
                });
                kk = 0;
                cbCurveSelectedStation.SelectedIndex = kk;
            }
            else if (cbSelectName.Contains("Floor"))
            {
                ii = cbCurveSelectedOven.SelectedIndex;
                jj = cbCurveSelectedFloor.SelectedIndex;

                cbCurveSelectedStation.Items.Clear();
                Current.ovens[ii].Floors[jj].Stations.ForEach(s =>
                {
                    cbCurveSelectedStation.Items.Add(s.Name);
                });
                kk = 0;
                cbCurveSelectedStation.SelectedIndex = kk;
            }
            else if (cbSelectName.Contains("Station"))
            {
                ii = cbCurveSelectedOven.SelectedIndex;
                jj = cbCurveSelectedFloor.SelectedIndex;
                kk = cbCurveSelectedStation.SelectedIndex;
            }

            Current.option.CurveStationId = Current.ovens[ii].Floors[jj].Stations[kk].Id;
        }

        #endregion

        #region 右键
        /// <summary>
        /// 右键源控件名称 如：tlpFloor0401
        /// </summary>

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

        #region 手动调试夹具扫码枪

        private void btnClampScanStart_Click(object sender, EventArgs e)
        {
            string code = string.Empty;
            string msg = string.Empty;
            ScanResult scanResult = Current.ClampScaner.StartClampScan(out code, out msg);
            if (scanResult == ScanResult.OK)
            {
                Tip.Alert(code);
            }
            else if (scanResult == ScanResult.NG)
            {
                Tip.Alert("扫码返回NG！");
            }
            else
            {
                Error.Alert(msg);
            }
        }

        private void btnClampScanOkBackToFeeder_Click(object sender, EventArgs e)
        {
            string msg = string.Empty;
            if (!Current.Feeder.SetScanClampResult(ScanResult.OK, out msg))
            {
                Error.Alert(msg);
            }
            else
            {
                Tip.Alert("OK");
            }
        }

        private void btnClampScanNgBackToFeeder_Click(object sender, EventArgs e)
        {
            string msg = string.Empty;
            if (!Current.Feeder.SetScanClampResult(ScanResult.NG, out msg))
            {
                Error.Alert(msg);
            }
            else
            {
                Tip.Alert("OK");
            }
        }

        #endregion

        private void btnDebug_Click(object sende, EventArgs e)
        {
            Current.RGV.Plc.GetInfoNoWrite(out string msg);
            Console.WriteLine(msg);
        }


        /// <summary>
        /// 键盘按下空格键急停RGV
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ManageSystem_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString() == " ")
            {
                if (Current.RGV.IsAlive)
                {
                    if (Current.RGV.Stop(out string msg))
                    {
                        Tip.Alert("按下空格键急停大RGV成功！");
                    }
                }
            }
        }

    }
}
