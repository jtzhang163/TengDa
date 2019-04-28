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

namespace Soundon.Dispatcher.App
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
            InitMES();
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

            #region 上料机相关控件数组
            for (int i = 0; i < FeederCount; i++)
            {
                int ii = i + 1;

                tlpFeeders[i] = (TableLayoutPanel)(this.Controls.Find(string.Format("tlpFeeder{0}", ii.ToString()), true)[0]);
                lbFeederName[i] = (Label)(this.Controls.Find(string.Format("lbFeederName{0}", ii.ToString("D2")), true)[0]);
                lbFeederNameN[i] = (Label)(this.Controls.Find(string.Format("lbFeederNameN{0}", ii.ToString("D2")), true)[0]);
                cbFeederIsEnable[i] = (CheckBox)(this.Controls.Find(string.Format("cbFeederIsEnable{0}", ii.ToString("D2")), true)[0]);
                pbFeederLamp[i] = (PictureBox)(this.Controls.Find(string.Format("pbFeederLamp{0}", ii.ToString("D2")), true)[0]);
                tbFeederStatus[i] = (TextBox)(this.Controls.Find(string.Format("tbFeederStatus{0}", ii.ToString("D2")), true)[0]);
                pbFeederTriLamp[i] = (PictureBox)(this.Controls.Find(string.Format("pbFeederTriLamp{0}", ii.ToString("D2")), true)[0]);

                lbFeederStationName[i] = new Label[FeederStationCount];
                lbFeederClampCode[i] = new Label[FeederStationCount];
                tlpFeederStationClamp[i] = new TableLayoutPanel[FeederStationCount];

                for (int j = 0; j < FeederStationCount; j++)
                {
                    int jj = j + 1;
                    lbFeederStationName[i][j] = (Label)(this.Controls.Find(string.Format("lbFeederStationName{0}{1}", ii.ToString("D2"), (j + 1).ToString("D2")), true)[0]);
                    lbFeederClampCode[i][j] = (Label)(this.Controls.Find(string.Format("lbFeederClampCode{0}{1}", ii.ToString("D2"), (j + 1).ToString("D2")), true)[0]);
                    tlpFeederStationClamp[i][j] = (TableLayoutPanel)(this.Controls.Find(string.Format("tlpFeederStationClamp{0}{1}", ii.ToString("D2"), (j + 1).ToString("D2")), true)[0]);
                }

                lbScanerNameN[i] = new Label[FeederScanerCount];
                cbScanerIsEnable[i] = new CheckBox[FeederScanerCount];
                pbScanerLamp[i] = new PictureBox[FeederScanerCount];
                tbScanerStatus[i] = new TextBox[FeederScanerCount];

                for (int j = 0; j < FeederScanerCount; j++)
                {
                    lbScanerNameN[i][j] = (Label)(this.Controls.Find(string.Format("lbScanerNameN{0}{1}", ii.ToString("D2"), (j + 1).ToString("D2")), true)[0]);
                    cbScanerIsEnable[i][j] = (CheckBox)(this.Controls.Find(string.Format("cbScanerIsEnable{0}{1}", ii.ToString("D2"), (j + 1).ToString("D2")), true)[0]);
                    pbScanerLamp[i][j] = (PictureBox)(this.Controls.Find(string.Format("pbScanerLamp{0}{1}", ii.ToString("D2"), (j + 1).ToString("D2")), true)[0]);
                    tbScanerStatus[i][j] = (TextBox)(this.Controls.Find(string.Format("tbScanerStatus{0}{1}", ii.ToString("D2"), (j + 1).ToString("D2")), true)[0]);
                }
            }
            #endregion

            #region 烤箱相关控件数组

            for (int i = 0; i < OvenCount; i++)
            {
                int ii = i + 1;
                lbOvenName[i] = (Label)(this.Controls.Find(string.Format("lbOvenName{0}", ii.ToString("D2")), true)[0]);
                lbOvenNameN[i] = (Label)(this.Controls.Find(string.Format("lbOvenNameN{0}", ii.ToString("D2")), true)[0]);
                cbOvenIsEnable[i] = (CheckBox)(this.Controls.Find(string.Format("cbOvenIsEnable{0}", ii.ToString("D2")), true)[0]);
                pbOvenLamp[i] = (PictureBox)(this.Controls.Find(string.Format("pbOvenLamp{0}", ii.ToString("D2")), true)[0]);
                tbOvenStatus[i] = (TextBox)(this.Controls.Find(string.Format("tbOvenStatus{0}", ii.ToString("D2")), true)[0]);
                pbOvenTriLamp[i] = (PictureBox)(this.Controls.Find(string.Format("pbOvenTriLamp{0}", ii.ToString("D2")), true)[0]);

                tlpFloor[i] = new TableLayoutPanel[OvenFloorCount];
                pbRunTime[i] = new ProgressBar[OvenFloorCount];
                lbFloorInfoTop[i] = new Label[OvenFloorCount];
                lbFloorStatus[i] = new Label[OvenFloorCount];

                for (int j = 0; j < OvenFloorCount; j++)
                {
                    tlpFloor[i][j] = (TableLayoutPanel)(this.Controls.Find(string.Format("tlpFloor{0}{1}", ii.ToString("D2"), (j + 1).ToString("D2")), true)[0]);
                    pbRunTime[i][j] = (ProgressBar)(this.Controls.Find(string.Format("pbRunTime{0}{1}", ii.ToString("D2"), (j + 1).ToString("D2")), true)[0]);
                    lbFloorInfoTop[i][j] = (Label)(this.Controls.Find(string.Format("lbFloorInfoTop{0}{1}", ii.ToString("D2"), (j + 1).ToString("D2")), true)[0]);
                    lbFloorStatus[i][j] = (Label)(this.Controls.Find(string.Format("lbFloorStatus{0}{1}", ii.ToString("D2"), (j + 1).ToString("D2")), true)[0]);
                }
            }

            #endregion

            #region 下料机相关控件数组
            for (int i = 0; i < BlankerCount; i++)
            {
                int ii = i + 1;

                tlpBlankers[i] = (TableLayoutPanel)(this.Controls.Find(string.Format("tlpBlanker{0}", ii.ToString()), true)[0]);
                lbBlankerName[i] = (Label)(this.Controls.Find(string.Format("lbBlankerName{0}", ii.ToString("D2")), true)[0]);
                lbBlankerNameN[i] = (Label)(this.Controls.Find(string.Format("lbBlankerNameN{0}", ii.ToString("D2")), true)[0]);
                cbBlankerIsEnable[i] = (CheckBox)(this.Controls.Find(string.Format("cbBlankerIsEnable{0}", ii.ToString("D2")), true)[0]);
                pbBlankerLamp[i] = (PictureBox)(this.Controls.Find(string.Format("pbBlankerLamp{0}", ii.ToString("D2")), true)[0]);
                tbBlankerStatus[i] = (TextBox)(this.Controls.Find(string.Format("tbBlankerStatus{0}", ii.ToString("D2")), true)[0]);
                pbBlankerTriLamp[i] = (PictureBox)(this.Controls.Find(string.Format("pbBlankerTriLamp{0}", ii.ToString("D2")), true)[0]);

                lbBlankerStationName[i] = new Label[BlankerStationCount];
                lbBlankerFromStationName[i] = new Label[BlankerStationCount];
                lbBlankerClampCode[i] = new Label[BlankerStationCount];
                tlpBlankerStationClamp[i] = new TableLayoutPanel[BlankerStationCount];

                for (int j = 0; j < BlankerStationCount; j++)
                {
                    int jj = j + 1;
                    lbBlankerStationName[i][j] = (Label)(this.Controls.Find(string.Format("lbBlankerStationName{0}{1}", ii.ToString("D2"), jj.ToString("D2")), true)[0]);
                    lbBlankerFromStationName[i][j] = (Label)(this.Controls.Find(string.Format("lbBlankerFromStationName{0}{1}", ii.ToString("D2"), (j + 1).ToString("D2")), true)[0]);
                    lbBlankerClampCode[i][j] = (Label)(this.Controls.Find(string.Format("lbBlankerClampCode{0}{1}", ii.ToString("D2"), jj.ToString("D2")), true)[0]);
                    tlpBlankerStationClamp[i][j] = (TableLayoutPanel)(this.Controls.Find(string.Format("tlpBlankerStationClamp{0}{1}", ii.ToString("D2"), jj.ToString("D2")), true)[0]);
                }
            }
            #endregion

            #region 缓存位相关控件数组
            for (int i = 0; i < CacheStationCount; i++)
            {
                lbCacheClampCode[i] = (Label)(this.Controls.Find(string.Format("lbCacheClampCode{0}", (i + 1).ToString("D2")), true)[0]);
                tlpCacheClamp[i] = (TableLayoutPanel)(this.Controls.Find(string.Format("tlpCacheClamp{0}", (i + 1).ToString("D2")), true)[0]);
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
            TreeNode tnFeederList = new TreeNode("上料机", tnFeeders.ToArray());

            List<TreeNode> tnBlankers = new List<TreeNode>();
            foreach (Blanker blanker in Blanker.BlankerList)
            {
                tnBlankers.Add(new TreeNode(string.Format("{0}:{1}", blanker.Id, blanker.Name), new TreeNode[] { new TreeNode("PLC") }));
            }
            TreeNode tnBlankerList = new TreeNode("下料机", tnBlankers.ToArray());

            List<TreeNode> tnScaners = new List<TreeNode>();
            foreach (Scaner scaner in Scaner.ScanerList)
            {
                tnScaners.Add(new TreeNode(string.Format("{0}:{1}", scaner.Id, scaner.Name)));
            }
            TreeNode tnScanerList = new TreeNode("扫码枪", tnScaners.ToArray());

            TreeNode tnCache = new TreeNode("缓存架");

            TreeNode tnRotater = new TreeNode("转移台");

            List<TreeNode> tnStations = new List<TreeNode>();

            var stationList = new List<Station>();
            Current.feeders.ForEach(f => f.Stations.ForEach(s => stationList.Add(s)));
            Current.ovens.ForEach(o => o.Floors.ForEach(f => f.Stations.ForEach(s => stationList.Add(s))));
            Current.blankers.ForEach(b => b.Stations.ForEach(s => stationList.Add(s)));
            Current.Cache.Stations.ForEach(s => stationList.Add(s));
            stationList.Add(Current.Transfer.Station);

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

            List<TreeNode> tnRobotNodes = new List<TreeNode>();

            List<TreeNode> tnRobotClamps = new List<TreeNode>();
            if (Current.Robot.Clamp.Id > 0)
            {
                List<TreeNode> tnBatteries = new List<TreeNode>();
                var batteries = Current.Robot.Clamp.Batteries;
                foreach (var b in batteries)
                {
                    tnBatteries.Add(new TreeNode(string.Format("{0}:{1}(电芯)", b.Id, b.Code)));
                }
                tnRobotClamps.Add(new TreeNode(string.Format("{0}:{1}(夹具)", Current.Robot.Clamp.Id, Current.Robot.Clamp.Code), tnBatteries.ToArray()));
            }

            tnRobotNodes.Add(new TreeNode("PLC"));
            tnRobotNodes.AddRange(tnRobotClamps);
            TreeNode tnRobot = new TreeNode("机器人", tnRobotNodes.ToArray());

            TreeNode tnMES = new TreeNode("MES");

            TreeNode tnCurrentTask = new TreeNode("当前任务");

            tvSettings.Nodes.AddRange(new TreeNode[] { tnCurrentTask, tnConfig, tnOvenList, tnFeederList, tnBlankerList, tnScanerList, tnCache, tnRotater, tnStationList, tnTaskList, tnEnabledTaskList, tnRobot, tnMES });
        }

        private void InitTerminal()
        {
            Current.ovens = Oven.OvenList;
            Current.feeders = Feeder.FeederList;
            Current.blankers = Blanker.BlankerList;
            Current.Yields = Yield.YieldList;
            cbStations.Items.Add("All");
            cbAlarmFloors.Items.Add("All");

            for (int i = 0; i < OvenCount; i++)
            {
                lbOvenName[i].Text = Current.ovens[i].Name;
                lbOvenNameN[i].Text = Current.ovens[i].Name;
                pbOvenLamp[i].Image = Properties.Resources.Gray_Round;
                cbOvenIsEnable[i].Checked = Current.ovens[i].IsEnable;
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

            for (int i = 0; i < Current.feeders.Count; i++)
            {
                lbFeederName[i].Text = Current.feeders[i].Name;
                lbFeederNameN[i].Text = Current.feeders[i].Name;
                pbFeederLamp[i].Image = Properties.Resources.Gray_Round;
                cbFeederIsEnable[i].Checked = Current.feeders[i].IsEnable;

                for (int j = 0; j < FeederStationCount; j++)
                {
                    //  lbFeederStationName[i][j].Text = Current.feeders[i].Stations[j].Name.Replace("上料","");
                    lbFeederStationName[i][j].Text = Current.feeders[i].Stations[j].Name.Substring(3);
                }

                for (int j = 0; j < Current.feeders[i].Scaners.Count; j++)
                {
                    lbScanerNameN[i][j].Text = Current.feeders[i].Scaners[j].Name;
                    pbScanerLamp[i][j].Image = Properties.Resources.Gray_Round;
                    cbScanerIsEnable[i][j].Checked = Current.feeders[i].Scaners[j].IsEnable;
                }
            }

            for (int i = 0; i < BlankerCount; i++)
            {
                lbBlankerName[i].Text = Current.blankers[i].Name;
                lbBlankerNameN[i].Text = Current.blankers[i].Name;
                pbBlankerLamp[i].Image = Properties.Resources.Gray_Round;
                cbBlankerIsEnable[i].Checked = Current.blankers[i].IsEnable;

                for (int j = 0; j < Current.blankers[i].Stations.Count; j++)
                {
                    // lbBlankerStationName[i][j].Text = Current.blankers[i].Stations[j].Name.Replace("下料", "");
                    lbBlankerStationName[i][j].Text = Current.blankers[i].Stations[j].Name.Substring(3);
                }
            }


            lbRobotName.Text = Current.Robot.Name;
            lbRobotNameN.Text = Current.Robot.Name;

            lbRobotClampCode.Text = Current.Robot.Clamp.Code;

            lbRobotClampCode.BackColor = Current.Robot.ClampStatus == ClampStatus.异常 ? Color.Red : Color.Transparent;

            cbRobotIsEnable.Checked = Current.Robot.IsEnable;

            lbTransferName.Text = Current.Transfer.Name;
            lbTransferClampCode.Text = Current.Transfer.Station.Clamp.Code;

            lbCacheName.Text = Current.Cache.Name;
            for (int i = 0; i < Current.Cache.Stations.Count; i++)
            {
                lbCacheClampCode[i].Text = Current.Cache.Stations[i].Clamp.Code;
            }

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

            Current.ovens.ForEach(o =>
            {
                cbSampleSelectedOven.Items.Add(o.Name);
            });
            cbSampleSelectedOven.SelectedIndex = iii;

            sampleOven.Floors.ForEach(f =>
            {
                cbSampleSelectedFloor.Items.Add(f.Name);
            });
            cbSampleSelectedFloor.SelectedIndex = jjj;

            Current.feeders.ForEach(f =>
            {
                f.BatteryScaners.ForEach(s => cbBatteryScaner.Items.Add(s.Name));
                cbClampScaner.Items.Add(f.ClampScaner.Name);
            });
            cbBatteryScaner.SelectedIndex = 0;
            cbClampScaner.SelectedIndex = 0;
        }

        private void InitMES()
        {
            //Current.mes = new MES(1);
            lbMesNameN.Text = Current.mes.Name;
            cbMesIsEnable.Checked = Current.mes.IsEnable;
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
                oven.IsAlive = oven.IsEnable && oven.Plc.IsAlive;
                oven.Floors.ForEach(f => f.IsAlive = f.IsEnable && (oven.IsAlive || oven.PreIsAlive));
                oven.Floors.ForEach(f => f.Stations.ForEach(s => s.IsAlive = s.IsEnable && f.IsAlive));
                if (oven.Plc.IsAlive) { if (tbOvenStatus[i].Text.Trim() == "未连接") { tbOvenStatus[i].Text = "连接成功"; } }
                else { this.tbOvenStatus[i].Text = "未连接"; }

                this.pbOvenLamp[i].Image = oven.Plc.IsAlive ? Properties.Resources.Green_Round : Properties.Resources.Gray_Round;

                switch (oven.TriLamp)
                {
                    case TriLamp.Green: this.pbOvenTriLamp[i].Image = Properties.Resources.Green_Round; break;
                    case TriLamp.Yellow: this.pbOvenTriLamp[i].Image = Properties.Resources.Yellow_Round; break;
                    case TriLamp.Red: this.pbOvenTriLamp[i].Image = Properties.Resources.Red_Round; break;
                    case TriLamp.Unknown: this.pbOvenTriLamp[i].Image = Properties.Resources.Gray_Round; break;
                }

                if (!string.IsNullOrEmpty(oven.AlarmStr) && oven.Plc.IsAlive)
                {
                    if (oven.PreAlarmStr != oven.AlarmStr)
                    {
                        this.lbOvenName[i].Text = oven.AlarmStr.TrimEnd(',') + "...";
                    }
                    else
                    {
                        string alarmStr = this.lbOvenName[i].Text;
                        this.lbOvenName[i].Text = alarmStr.Substring(1, alarmStr.Length - 1) + alarmStr.Substring(0, 1);
                    }

                    this.lbOvenName[i].ForeColor = Color.White;
                    this.lbOvenName[i].BackColor = Color.Red;
                }
                else
                {
                    this.lbOvenName[i].Text = Current.ovens[i].Name;
                    this.lbOvenName[i].ForeColor = SystemColors.WindowText;
                    this.lbOvenName[i].BackColor = Color.Transparent;
                }
                oven.PreAlarmStr = oven.AlarmStr;


                for (int j = 0; j < OvenFloorCount; j++)
                {
                    Floor floor = oven.Floors[j];
                    this.pbRunTime[i][j].Maximum = floor.RunMinutesSet;
                    this.pbRunTime[i][j].Value = floor.IsAlive ? (floor.RunMinutesSet > floor.RunMinutes ? floor.RunMinutes : floor.RunMinutesSet) : 0;

                    floor.Stations.ForEach(s =>
                    {
                        if (s.Id == Current.option.CurveStationId)
                        {
                            for (int k = 0; k < Option.TemperaturePointCount; k++)
                            {
                                cbTemperIndex[k].Text = string.Format("{0}:{1}℃", Current.option.TemperNames[k], s.Temperatures[k].ToString("#0.0").PadLeft(5));
                            }
                            for (int k = 0; k < Option.VacuumPointCount; k++)
                            {
                                cbVacuumIndex[k].Text = string.Format("{0}:{1}Pa", "真空度", s.GetFloor().Vacuum.ToString());
                            }
                        }
                    });

                    if (!string.IsNullOrEmpty(floor.AlarmStr) && floor.IsAlive)
                    {
                        if (floor.PreAlarmStr != floor.AlarmStr)
                        {
                            this.lbFloorInfoTop[i][j].Text = floor.AlarmStr.TrimEnd(',') + "...";
                        }
                        else
                        {
                            string alarmStr = this.lbFloorInfoTop[i][j].Text;
                            this.lbFloorInfoTop[i][j].Text = alarmStr.Substring(1, alarmStr.Length - 1) + alarmStr.Substring(0, 1);
                        }
                        this.lbFloorInfoTop[i][j].ForeColor = Color.White;
                        this.lbFloorInfoTop[i][j].BackColor = Color.Red;
                    }
                    else
                    {
                        var centerStr = floor.IsEnable ? floor.Vacuum.ToString("#0").PadLeft(6) + "Pa" : "炉层禁用";

                        if (floor.IsBaking)
                        {
                            this.lbFloorInfoTop[i][j].Text = string.Format("{0}℃ {1} {2}℃",
                                 floor.Stations[0].Temperatures[Current.option.DisplayTemperIndex].ToString("#0.0").PadLeft(4),
                                 centerStr,
                                 floor.Stations[1].Temperatures[Current.option.DisplayTemperIndex].ToString("#0.0").PadLeft(4));
                        }
                        else
                        {
                            Station s1, s2;

                            s1 = oven.ClampOri == ClampOri.B ? floor.Stations[0] : floor.Stations[1];
                            s2 = oven.ClampOri == ClampOri.B ? floor.Stations[1] : floor.Stations[0];

                            string str1 = "", str2 = "";

                            if (floor.Stations.Count(s => s.FloorStatus == FloorStatus.待出 && s.SampleStatus == SampleStatus.待结果) == 1 && floor.Stations.Count(s => s.FloorStatus == FloorStatus.待烤 && s.SampleStatus == SampleStatus.待测试) == 1)
                            {
                                str1 = "正测";
                                str2 = "正测";
                            }
                            else if (floor.Stations.Count(s => s.FloorStatus == FloorStatus.待出 && s.SampleStatus == SampleStatus.待结果) == 1 && floor.Stations.Count(s => s.FloorStatus == FloorStatus.待出 && s.SampleStatus == SampleStatus.待测试) == 1)
                            {
                                str1 = "待测";
                                str2 = "待测";
                            }
                            else
                            {
                                str1 = s1.FloorStatus == FloorStatus.待出 && s1.SampleStatus == SampleStatus.测试OK || s1.FloorStatus == FloorStatus.待烤 && s1.SampleStatus == SampleStatus.测试NG ? s1.SampleStatus.ToString() : s1.FloorStatus.ToString();
                                str2 = s2.FloorStatus == FloorStatus.待出 && s2.SampleStatus == SampleStatus.测试OK || s2.FloorStatus == FloorStatus.待烤 && s2.SampleStatus == SampleStatus.测试NG ? s2.SampleStatus.ToString() : s2.FloorStatus.ToString();
                            }

                            this.lbFloorInfoTop[i][j].Text = string.Format("{0} {3}{1} {4}{2}",
                                 str1,
                                 centerStr,
                                 str2,
                                 s1.HasSampleFlag ? "★ " : "",
                                 s2.HasSampleFlag ? "★ " : "");
                        }

                        this.lbFloorInfoTop[i][j].ForeColor = Color.Red;
                        this.lbFloorInfoTop[i][j].BackColor = Color.Transparent;
                    }



                    floor.PreAlarmStr = floor.AlarmStr;

                    if (oven.Plc.IsAlive)
                    {
                        if (!string.IsNullOrEmpty(floor.AlarmStr))
                        {
                            this.tlpFloor[i][j].BackColor = System.Drawing.Color.Crimson;
                        }
                        else
                        {

                        }
                    }
                    else if(!oven.Plc.PreIsAlive)
                    {
                        this.tlpFloor[i][j].BackColor = Color.LightGray;
                    }

                    var index = oven.Floors.IndexOf(floor);

                    if (!IsDisplayOvenCode)
                    {
                        lbFloorStatus[i][j].Text =
                            string.Format("{0} {1} {2}/{3} {4}",
                            oven.ClampOri == ClampOri.B ? floor.Stations[0].RobotGetCode : floor.Stations[1].RobotGetCode,
                            floor.DoorStatus,
                            floor.RunMinutes.ToString().PadLeft(3),
                            floor.RunMinutesSet.ToString().PadLeft(3),
                            oven.ClampOri == ClampOri.A ? floor.Stations[0].RobotGetCode : floor.Stations[1].RobotGetCode
                            );
                    }
                    else
                    {
                        lbFloorStatus[i][j].Text =
                            string.Format("{0} {1}",
                            (oven.ClampOri == ClampOri.B ? floor.Stations[0].Clamp.Code : floor.Stations[1].Clamp.Code).PadRight(8),
                            (oven.ClampOri == ClampOri.A ? floor.Stations[0].Clamp.Code : floor.Stations[1].Clamp.Code).PadLeft(8)
                            );
                    }


                    switch (floor.DoorStatus)
                    {
                        case DoorStatus.打开:
                            lbFloorStatus[i][j].ForeColor = Color.White;
                            lbFloorStatus[i][j].BackColor = SystemColors.WindowText;
                            break;
                        case DoorStatus.异常:
                            lbFloorStatus[i][j].ForeColor = Color.White;
                            lbFloorStatus[i][j].BackColor = Color.Red;
                            break;
                        default:
                            lbFloorStatus[i][j].ForeColor = SystemColors.WindowText;
                            lbFloorStatus[i][j].BackColor = Color.Transparent;
                            break;
                    }

                    if (floor.IsAlive && floor.Stations.Count(s => s.Id == Current.Task.FromStationId || s.Id == Current.Task.ToStationId) > 0)
                    {
                        this.tlpFloor[i][j].Invalidate();
                    }

                    if (floor.PreIsAlive != floor.IsAlive)
                    {
                        this.tlpFloor[i][j].Invalidate();
                    }

                }

                oven.PreIsAlive = oven.IsAlive;
                oven.Floors.ForEach(f => f.PreIsAlive = f.IsAlive);
                oven.Floors.ForEach(f => f.Stations.ForEach(s => s.PreIsAlive = s.IsAlive));

            }

            #endregion

            #region 上料机


            for (int i = 0; i < Current.feeders.Count; i++)
            {
                Feeder feeder = Current.feeders[i];
                feeder.IsAlive = feeder.IsEnable && feeder.Plc.IsAlive;
                feeder.Stations.ForEach(s => s.IsAlive = s.IsEnable && feeder.IsAlive);

                if (feeder.Plc.IsAlive) { if (tbFeederStatus[i].Text.Trim() == "未连接") { tbFeederStatus[i].Text = "连接成功"; } }
                else { this.tbFeederStatus[i].Text = "未连接"; }

                //两次离线再变灰（避免一直闪烁）
                this.tlpFeeders[i].BackColor = feeder.Plc.IsAlive || feeder.Plc.PreIsAlive ? Color.White : Color.LightGray;

                feeder.Plc.PreIsAlive = feeder.Plc.IsAlive;
                this.pbFeederLamp[i].Image = feeder.Plc.IsAlive ? Properties.Resources.Green_Round : Properties.Resources.Gray_Round;

                switch (feeder.TriLamp)
                {
                    case TriLamp.Green: this.pbFeederTriLamp[i].Image = Properties.Resources.Green_Round; break;
                    case TriLamp.Yellow: this.pbFeederTriLamp[i].Image = Properties.Resources.Yellow_Round; break;
                    case TriLamp.Red: this.pbFeederTriLamp[i].Image = Properties.Resources.Red_Round; break;
                    case TriLamp.Unknown: this.pbFeederTriLamp[i].Image = Properties.Resources.Gray_Round; break;
                }

                if (!string.IsNullOrEmpty(feeder.AlarmStr) && feeder.Plc.IsAlive)
                {
                    if (feeder.PreAlarmStr != feeder.AlarmStr)
                    {
                        this.lbFeederName[i].Text = feeder.AlarmStr.TrimEnd(',') + "...";
                    }
                    else
                    {
                        string alarmStr = this.lbFeederName[i].Text;
                        this.lbFeederName[i].Text = alarmStr.Substring(1, alarmStr.Length - 1) + alarmStr.Substring(0, 1);
                    }

                    this.lbFeederName[i].ForeColor = Color.White;
                    this.lbFeederName[i].BackColor = Color.Red;
                }
                else
                {
                    this.lbFeederName[i].Text = feeder.Name;
                    this.lbFeederName[i].ForeColor = SystemColors.WindowText;
                    this.lbFeederName[i].BackColor = Color.Transparent;
                }
                feeder.PreAlarmStr = feeder.AlarmStr;

                for (int j = 0; j < Current.feeders[i].Scaners.Count; j++)
                {
                    Scaner scaner = Current.feeders[i].Scaners[j];
                    if (!scaner.IsEnable)
                        scaner.IsAlive = false;
                    if (scaner.IsAlive)
                    {
                        if (tbScanerStatus[i][j].Text.Trim() == "未连接")
                        {
                            tbScanerStatus[i][j].Text = "连接成功";
                        }

                        this.pbScanerLamp[i][j].Image = Properties.Resources.Green_Round;
                    }
                    else
                    {
                        this.tbScanerStatus[i][j].Text = "未连接";
                        this.pbScanerLamp[i][j].Image = Properties.Resources.Gray_Round;
                    }
                }


                for (int j = 0; j < FeederStationCount; j++)
                {
                    Station station = Current.feeders[i].Stations[j];

                    lbFeederClampCode[i][j].Text = station.Clamp.Code;

                    lbFeederClampCode[i][j].BackColor = station.HasSampleFlag ? Color.Blue : Color.White;
                    lbFeederClampCode[i][j].ForeColor = station.HasSampleFlag ? Color.White : Color.Blue;

                    bool canChangeVisible = DateTime.Now.Second % 3 == 1;

                    if (Current.feeders[i].IsAlive && canChangeVisible && station.Id == Current.Task.FromStationId && (Current.Task.Status == TaskStatus.就绪 || Current.Task.Status == TaskStatus.可取 || Current.Task.Status == TaskStatus.正取))
                    {
                        tlpFeederStationClamp[i][j].Visible = false;
                    }
                    else if (Current.feeders[i].IsAlive && canChangeVisible && station.Id == Current.Task.ToStationId)
                    {
                        tlpFeederStationClamp[i][j].Visible = true;
                        tlpFeederStationClamp[i][j].BackColor = Current.Task.FromClampStatus == ClampStatus.空夹具 ? Color.Cyan : Color.LimeGreen;
                    }
                    else
                    {
                        tlpFeederStationClamp[i][j].Visible = station.ClampStatus != ClampStatus.无夹具;

                        if (!station.IsAlive)
                        {
                            tlpFeederStationClamp[i][j].BackColor = SystemColors.Control;
                        }
                        else
                        {
                            switch (station.ClampStatus)
                            {
                                case ClampStatus.满夹具: tlpFeederStationClamp[i][j].BackColor = Color.LimeGreen; break;
                                case ClampStatus.空夹具: tlpFeederStationClamp[i][j].BackColor = Color.Cyan; break;
                                case ClampStatus.异常: tlpFeederStationClamp[i][j].BackColor = Color.Red; break;
                                default: tlpFeederStationClamp[i][j].BackColor = SystemColors.Control; break;
                            }
                        }
                    }
                }
                tlpFeeders[i].Invalidate();
            }

            #endregion

            #region 下料机

            for (int i = 0; i < BlankerCount; i++)
            {
                Blanker blanker = Current.blankers[i];
                blanker.IsAlive = blanker.IsEnable && blanker.Plc.IsAlive;
                blanker.Stations.ForEach(s => s.IsAlive = s.IsEnable && blanker.IsAlive);

                if (blanker.Plc.IsAlive) { if (tbBlankerStatus[i].Text.Trim() == "未连接") { tbBlankerStatus[i].Text = "连接成功"; } }
                else { this.tbBlankerStatus[i].Text = "未连接"; }


                this.tlpBlankers[i].BackColor = blanker.Plc.IsAlive ? Color.White : Color.LightGray;
                this.pbBlankerLamp[i].Image = blanker.Plc.IsAlive ? Properties.Resources.Green_Round : Properties.Resources.Gray_Round;

                switch (blanker.TriLamp)
                {
                    case TriLamp.Green: this.pbBlankerTriLamp[i].Image = Properties.Resources.Green_Round; break;
                    case TriLamp.Yellow: this.pbBlankerTriLamp[i].Image = Properties.Resources.Yellow_Round; break;
                    case TriLamp.Red: this.pbBlankerTriLamp[i].Image = Properties.Resources.Red_Round; break;
                    case TriLamp.Unknown: this.pbBlankerTriLamp[i].Image = Properties.Resources.Gray_Round; break;
                }

                if (!string.IsNullOrEmpty(blanker.AlarmStr) && blanker.Plc.IsAlive)
                {
                    if (blanker.PreAlarmStr != blanker.AlarmStr)
                    {
                        this.lbBlankerName[i].Text = blanker.AlarmStr.TrimEnd(',') + "...";
                    }
                    else
                    {
                        string alarmStr = this.lbBlankerName[i].Text;
                        this.lbBlankerName[i].Text = alarmStr.Substring(1, alarmStr.Length - 1) + alarmStr.Substring(0, 1);
                    }

                    this.lbBlankerName[i].ForeColor = Color.White;
                    this.lbBlankerName[i].BackColor = Color.Red;
                }
                else
                {
                    this.lbBlankerName[i].Text = blanker.Name;
                    this.lbBlankerName[i].ForeColor = SystemColors.WindowText;
                    this.lbBlankerName[i].BackColor = Color.Transparent;
                }
                blanker.PreAlarmStr = blanker.AlarmStr;

                for (int j = 0; j < Current.blankers[i].Stations.Count; j++)
                {
                    Station station = Current.blankers[i].Stations[j];

                    lbBlankerStationName[i][j].BackColor = station.DoorStatus == DoorStatus.关闭 ? Color.Black : Color.Transparent;
                    lbBlankerStationName[i][j].ForeColor = station.DoorStatus == DoorStatus.关闭 ? Color.White : Color.Black;

                    lbBlankerClampCode[i][j].Text = station.Clamp.Code;

                    lbBlankerClampCode[i][j].BackColor = station.HasSampleFlag ? Color.Blue : Color.White;
                    lbBlankerClampCode[i][j].ForeColor = station.HasSampleFlag ? Color.White : Color.Blue;

                    bool canChangeVisible = DateTime.Now.Second % 3 == 1;

                    if (Current.blankers[i].IsAlive && canChangeVisible && station.Id == Current.Task.FromStationId && (Current.Task.Status == TaskStatus.就绪 || Current.Task.Status == TaskStatus.可取 || Current.Task.Status == TaskStatus.正取))
                    {
                        tlpBlankerStationClamp[i][j].Visible = false;
                    }
                    else if (Current.blankers[i].IsAlive && canChangeVisible && station.Id == Current.Task.ToStationId)
                    {
                        tlpBlankerStationClamp[i][j].Visible = true;
                        tlpBlankerStationClamp[i][j].BackColor = Current.Task.FromClampStatus == ClampStatus.空夹具 ? Color.Cyan : Color.LimeGreen;
                    }
                    else
                    {

                        tlpBlankerStationClamp[i][j].Visible = Current.blankers[i].Stations[j].ClampStatus != ClampStatus.无夹具;

                        if (!station.IsAlive)
                        {
                            tlpBlankerStationClamp[i][j].BackColor = SystemColors.Control;
                        }
                        else
                        {
                            switch (Current.blankers[i].Stations[j].ClampStatus)
                            {
                                case ClampStatus.满夹具: tlpBlankerStationClamp[i][j].BackColor = Color.LimeGreen; break;
                                case ClampStatus.空夹具: tlpBlankerStationClamp[i][j].BackColor = Color.Cyan; break;
                                case ClampStatus.异常: tlpBlankerStationClamp[i][j].BackColor = Color.Red; break;
                                default: tlpBlankerStationClamp[i][j].BackColor = SystemColors.Control; break;
                            }
                        }
                    }

                    if (Current.blankers[i].Stations[j].ClampStatus != ClampStatus.无夹具 && Current.blankers[i].Stations[j].FromStationId > 0)
                    {
                        var sta = Station.StationList.FirstOrDefault(s => s.Id == Current.blankers[i].Stations[j].FromStationId);
                        if (sta.GetPutType == GetPutType.烤箱)
                        {
                            lbBlankerFromStationName[i][j].Text = sta.Name.Substring(0, 7);
                        }
                    }
                    else
                    {
                        lbBlankerFromStationName[i][j].Text = string.Empty;
                    }

                }
                tlpBlankers[i].Invalidate();
            }

            #endregion

            #region 缓存架

            Current.Cache.IsAlive = Current.Cache.IsEnable && Current.Cache.Plc.IsAlive;

            Current.Cache.Stations.ForEach(s => s.IsAlive = s.IsEnable && Current.Cache.IsAlive);

            this.tlpCache.BackColor = Current.Cache.IsAlive ? Color.White : Color.LightGray;

            for (int j = 0; j < Current.Cache.Stations.Count; j++)
            {
                lbCacheClampCode[j].Text = Current.Cache.Stations[j].Clamp.Code;

                Station station = Current.Cache.Stations[j];
                bool canChangeVisible = DateTime.Now.Second % 3 == 1;

                if (Current.Cache.IsAlive && canChangeVisible && station.Id == Current.Task.FromStationId && (Current.Task.Status == TaskStatus.就绪 || Current.Task.Status == TaskStatus.可取 || Current.Task.Status == TaskStatus.正取))
                {
                    lbCacheClampCode[j].Visible = false;
                }
                else if (Current.Cache.IsAlive && canChangeVisible && station.Id == Current.Task.ToStationId)
                {
                    lbCacheClampCode[j].Visible = true;
                    lbCacheClampCode[j].BackColor = Current.Task.FromClampStatus == ClampStatus.空夹具 ? Color.Cyan : Color.LimeGreen;
                }
                else
                {
                    if (!station.IsAlive)
                    {
                        lbCacheClampCode[j].Visible = true;
                        lbCacheClampCode[j].BackColor = Color.LightGray;
                    }
                    else
                    {
                        lbCacheClampCode[j].Visible = station.ClampStatus != ClampStatus.无夹具;
                        switch (station.ClampStatus)
                        {
                            case ClampStatus.满夹具: lbCacheClampCode[j].BackColor = Color.LimeGreen; break;
                            case ClampStatus.空夹具: lbCacheClampCode[j].BackColor = Color.Cyan; break;
                            case ClampStatus.异常: lbCacheClampCode[j].BackColor = Color.Red; break;
                            default: lbCacheClampCode[j].BackColor = SystemColors.Control; break;
                        }
                    }
                }

            }

            #endregion

            #region 转移台

            Current.Transfer.IsAlive = Current.Transfer.IsEnable && Current.Transfer.Plc.IsAlive;

            Current.Transfer.Station.IsAlive = Current.Transfer.IsAlive && Current.Transfer.Station.IsEnable;

            this.tlpTransfer.BackColor = Current.Transfer.IsAlive ? Color.White : Color.LightGray;

          //  var sampleStatusFlag = Current.Transfer.Station.SampleStatus == SampleStatus.待测试 ? "" : Current.Transfer.Station.SampleStatus == SampleStatus.测试OK ? "✔ " : "✘ ";
            lbTransferClampCode.Text = Current.Transfer.Station.Clamp.Code;

            bool canChangeVisibleRotater = DateTime.Now.Second % 3 == 1;

            if (Current.Transfer.IsAlive && canChangeVisibleRotater && Current.Transfer.Station.Id == Current.Task.FromStationId && (Current.Task.Status == TaskStatus.就绪 || Current.Task.Status == TaskStatus.可取 || Current.Task.Status == TaskStatus.正取))
            {
                lbTransferClampCode.Visible = false;
            }
            else if (Current.Transfer.IsAlive && canChangeVisibleRotater && Current.Transfer.Station.Id == Current.Task.ToStationId)
            {
                lbTransferClampCode.Visible = true;
                lbTransferClampCode.BackColor = Current.Task.FromClampStatus == ClampStatus.空夹具 ? Color.Cyan : Color.LimeGreen;
            }
            else
            {

                if (!Current.Transfer.Station.IsAlive)
                {
                    lbTransferClampCode.Visible = true;
                    lbTransferClampCode.BackColor = Color.LightGray;
                }
                else
                {
                    lbTransferClampCode.Visible = Current.Transfer.Station.ClampStatus != ClampStatus.无夹具;
                    switch (Current.Transfer.Station.ClampStatus)
                    {
                        case ClampStatus.满夹具: lbTransferClampCode.BackColor = Color.LimeGreen; break;
                        case ClampStatus.空夹具: lbTransferClampCode.BackColor = Color.Cyan; break;
                        case ClampStatus.异常: lbTransferClampCode.BackColor = Color.Red; break;
                        default: lbTransferClampCode.BackColor = SystemColors.Control; break;
                    }
                }
            }

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

            #region MES

            if (Current.runStstus != RunStatus.闲置 && Current.mes.IsAlive)
            {
                if (tbMesStatus.Text.Trim() == "未连接")
                {
                    tbMesStatus.Text = "连接成功";
                }
                this.pbMesLamp.Image = Properties.Resources.Green_Round;
            }
            else
            {
                this.tbMesStatus.Text = "未连接";
                this.pbMesLamp.Image = Properties.Resources.Gray_Round;
            }
            #endregion

            #region 机器人

            Current.Robot.IsAlive = Current.Robot.IsEnable && Current.Robot.Plc.IsAlive;

            if (Current.Robot.IsAlive)
            {

                switch (Current.Robot.ClampStatus)
                {
                    case ClampStatus.满夹具: this.panelRobot.BackColor = Color.LimeGreen; break;
                    case ClampStatus.空夹具: this.panelRobot.BackColor = Color.Cyan; break;
                    case ClampStatus.无夹具: this.panelRobot.BackColor = Color.White; break;
                    default: this.panelRobot.BackColor = SystemColors.Control; break;
                }

                if (tbRobotStatus.Text.Trim() == "未连接")
                {
                    tbRobotStatus.Text = "连接成功";
                }

                this.pbRobotLamp.Image = Properties.Resources.Green_Round;
            }
            else
            {
                this.panelRobot.BackColor = Color.LightGray;
                this.tbRobotStatus.Text = "未连接";
                this.pbRobotLamp.Image = Properties.Resources.Gray_Round;
            }

            if (Current.Robot.PrePosition != Current.Robot.Position && Current.Robot.Position > 0)
            {
                this.tlpTrack.Controls.Remove(this.panelRobot);
                int col = Current.Robot.Position - 1;
                this.tlpTrack.Controls.Add(this.panelRobot, col, 0);
            }

            Current.Robot.PrePosition = Current.Robot.Position;

            if (Current.Robot.IsAlive)
            {
                if (Current.Robot.IsPausing)
                {
                    this.lbRobotInfo.Text = "暂停中";
                    this.lbRobotInfo.ForeColor = Color.Red;

                }
                else if (Current.Robot.IsMoving && TengDa.WF.Current.IsTerminalInitFinished)
                {
                    this.lbRobotInfo.Text = Current.Robot.MovingDirection == MovingDirection.前进 ? string.Format("{0}移动", Current.Robot.MovingDirSign) : string.Format("移动{0}", Current.Robot.MovingDirSign);
                    this.lbRobotInfo.ForeColor = Color.Blue;
                }
                else if (Current.Robot.IsGettingOrPutting)
                {
                    this.lbRobotInfo.Text = Current.Task.Status == TaskStatus.取完 || Current.Task.Status == TaskStatus.可取 || Current.Task.Status == TaskStatus.正取 ? "取盘中" : "放盘中";
                    this.lbRobotInfo.ForeColor = Color.Blue;
                }
                else if (!Current.Robot.IsExecuting)
                {
                    this.lbRobotInfo.Text = "未就绪";
                    this.lbRobotInfo.ForeColor = Color.Red;
                }
                else
                {
                    this.lbRobotInfo.Text = "闲置";
                    this.lbRobotInfo.ForeColor = SystemColors.WindowText;
                }
            }
            else
            {
                this.lbRobotInfo.Text = "未知";
                this.lbRobotInfo.ForeColor = SystemColors.WindowText;
            }

            lbRobotClampCode.Text = Current.Robot.Clamp.Code;


            if (!string.IsNullOrEmpty(Current.Robot.AlarmStr) && Current.Robot.IsAlive)
            {
                if (Current.Robot.PreAlarmStr != Current.Robot.AlarmStr)
                {
                    this.lbRobotName.Text = Current.Robot.AlarmStr.TrimEnd(',') + "...";
                }
                else
                {
                    string alarmStr = this.lbRobotName.Text;
                    this.lbRobotName.Text = alarmStr.Substring(1, alarmStr.Length - 1) + alarmStr.Substring(0, 1);
                }

                this.lbRobotName.ForeColor = Color.White;
                this.lbRobotName.BackColor = Color.Red;
            }
            else
            {
                this.lbRobotName.Text = Current.Robot.Name;
                this.lbRobotName.ForeColor = SystemColors.WindowText;
                this.lbRobotName.BackColor = Color.Transparent;
            }
            Current.Robot.PreAlarmStr = Current.Robot.AlarmStr;

            //机器人急停按钮显示逻辑
            tlpEmergencyStop.Visible = Current.Robot.IsAlive && TengDa.WF.Current.IsRunning && TengDa.WF.Current.user.Id > 0;

            panelRobot.Margin = new Padding(Current.Robot.Position + 3, 3, 0, 3);

            #endregion
        }

        #endregion

        #region 控件数组

        private const int OvenCount = 10;
        private const int OvenFloorCount = 3;

        private Label[] lbOvenName = new Label[OvenCount];
        private Label[] lbOvenNameN = new Label[OvenCount];

        private CheckBox[] cbOvenIsEnable = new CheckBox[OvenCount];
        private PictureBox[] pbOvenLamp = new PictureBox[OvenCount];
        private TextBox[] tbOvenStatus = new TextBox[OvenCount];
        private PictureBox[] pbOvenTriLamp = new PictureBox[OvenCount];

        private TableLayoutPanel[][] tlpFloor = new TableLayoutPanel[OvenCount][];
        private ProgressBar[][] pbRunTime = new ProgressBar[OvenCount][];
        private Label[][] lbFloorInfoTop = new Label[OvenCount][];
        private Label[][] lbFloorStatus = new Label[OvenCount][];

        private const int FeederCount = 2;
        private const int FeederStationCount = 3;

        private TableLayoutPanel[] tlpFeeders = new TableLayoutPanel[FeederCount];
        private Label[] lbFeederName = new Label[FeederCount];
        private Label[] lbFeederNameN = new Label[FeederCount];
        private CheckBox[] cbFeederIsEnable = new CheckBox[FeederCount];
        private PictureBox[] pbFeederLamp = new PictureBox[FeederCount];
        private TextBox[] tbFeederStatus = new TextBox[FeederCount];
        private PictureBox[] pbFeederTriLamp = new PictureBox[FeederCount];

        private Label[][] lbFeederStationName = new Label[FeederCount][];
        private Label[][] lbFeederClampCode = new Label[FeederCount][];
        private TableLayoutPanel[][] tlpFeederStationClamp = new TableLayoutPanel[FeederCount][];

        private const int FeederScanerCount = 3;

        private Label[][] lbScanerNameN = new Label[FeederCount][];
        private CheckBox[][] cbScanerIsEnable = new CheckBox[FeederCount][];
        private PictureBox[][] pbScanerLamp = new PictureBox[FeederCount][];
        private TextBox[][] tbScanerStatus = new TextBox[FeederCount][];

        private const int BlankerCount = 2;
        private const int BlankerStationCount = 2;

        private TableLayoutPanel[] tlpBlankers = new TableLayoutPanel[BlankerCount];
        private Label[] lbBlankerName = new Label[BlankerCount];
        private Label[] lbBlankerNameN = new Label[BlankerCount];
        private CheckBox[] cbBlankerIsEnable = new CheckBox[BlankerCount];
        private PictureBox[] pbBlankerLamp = new PictureBox[BlankerCount];
        private TextBox[] tbBlankerStatus = new TextBox[BlankerCount];
        private PictureBox[] pbBlankerTriLamp = new PictureBox[BlankerCount];

        private Label[][] lbBlankerStationName = new Label[BlankerCount][];
        private Label[][] lbBlankerFromStationName = new Label[BlankerCount][];
        private Label[][] lbBlankerClampCode = new Label[BlankerCount][];
        private TableLayoutPanel[][] tlpBlankerStationClamp = new TableLayoutPanel[BlankerCount][];

        private const int CacheStationCount = 3;

        private Label[] lbCacheClampCode = new Label[CacheStationCount];
        private TableLayoutPanel[] tlpCacheClamp = new TableLayoutPanel[CacheStationCount];

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
                            SetCheckBoxEnable(false);//禁止操作启用复选框
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
                        SetCheckBoxEnable(true);
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
                    if (Current.Robot.IsAlive)
                    {
                        if (!Current.Robot.IsExecuting)
                        {
                            Tip.Alert("机器人尚未成功启动，不能切换自动");
                            return;
                        }

                        if (Current.Robot.ClampStatus != ClampStatus.无夹具)
                        {
                            Tip.Alert("机器人负载有夹具，不能切换自动");
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
                lbAuto.Text = Current.TaskMode == TaskMode.自动任务 ? "切换手动" : "切换自动";
                Tip.Alert(string.Format("成功切换为{0}！", Current.TaskMode));
                CurrentTask.ToSwitchManuTaskMode = false;
            }
            else if (pictureBox.Name.Contains("TaskFuWei"))
            {
                if (Current.Task.Status == TaskStatus.完成)
                {
                    Tip.Alert("尚未生成任务！");
                    return;
                }

                if (Current.runStstus == RunStatus.运行)
                {
                    Tip.Alert("请先停止！");
                    return;
                }
                TaskReset();
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
        }

        private bool PlcConnect()
        {
            string msg = string.Empty;

            for (int i = 0; i < Current.feeders.Count; i++)
            {
                if (Current.feeders[i].IsEnable)
                {
                    if (!Current.feeders[i].Plc.IsPingSuccess)
                    {
                        Error.Alert(string.Format("无法连接到{0}, IP:{1}", Current.feeders[i].Plc.Name, Current.feeders[i].Plc.IP));
                        return false;
                    }

                    if (!Current.feeders[i].Plc.TcpConnect(out msg))
                    {
                        Error.Alert(string.Format("{0}:打开连接失败，原因：{1}", Current.feeders[i].Name, msg));
                        return false;
                    }
                    int ii = i;
                    this.BeginInvoke(new MethodInvoker(() => { tbFeederStatus[ii].Text = "连接成功"; }));
                }
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
                    int ii = i;
                    this.BeginInvoke(new MethodInvoker(() => { tbOvenStatus[ii].Text = "连接成功"; }));
                }
            }

            for (int i = 0; i < Current.blankers.Count; i++)
            {
                if (Current.blankers[i].IsEnable)
                {
                    if (!Current.blankers[i].Plc.IsPingSuccess)
                    {
                        Error.Alert(string.Format("无法连接到{0}, IP:{1}", Current.blankers[i].Plc.Name, Current.blankers[i].Plc.IP));
                        return false;
                    }

                    if (!Current.blankers[i].Plc.TcpConnect(out msg))
                    {
                        Error.Alert(string.Format("{0}:打开连接失败，原因：{1}", Current.blankers[i].Name, msg));
                        return false;
                    }
                    int ii = i;
                    this.BeginInvoke(new MethodInvoker(() => { tbBlankerStatus[ii].Text = "连接成功"; }));
                }
            }

            if (Current.Robot.IsEnable)
            {
                if (!Current.Robot.Plc.IsPingSuccess)
                {
                    Error.Alert(string.Format("无法连接到{0}, IP:{1}", Current.Robot.Plc.Name, Current.Robot.Plc.IP));
                    return false;
                }

                if (!Current.Robot.Plc.TcpConnect(out msg))
                {
                    Error.Alert(string.Format("打开机器人连接失败，原因：{0}", msg));
                    return false;
                }

                this.BeginInvoke(new MethodInvoker(() => { tbRobotStatus.Text = "连接成功"; }));
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

            for (int i = 0; i < Current.feeders.Count; i++)
            {
                if (Current.feeders[i].IsEnable)
                {
                    if (!Current.feeders[i].Plc.TcpDisConnect(out msg))
                    {
                        Error.Alert(msg);
                        return false;
                    }
                    tbFeederStatus[i].Text = "未连接";
                    this.pbFeederLamp[i].Image = Properties.Resources.Gray_Round;
                }

                Current.feeders[i].PreAlarmStr = string.Empty;
            }

            for (int i = 0; i < OvenCount; i++)
            {
                if (Current.ovens[i].IsEnable)
                {
                    if (!Current.ovens[i].Plc.TcpDisConnect(out msg))
                    {
                        Error.Alert(msg);
                        return false;
                    }
                    tbOvenStatus[i].Text = "未连接";
                    this.pbOvenLamp[i].Image = Properties.Resources.Gray_Round;
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


            for (int i = 0; i < BlankerCount; i++)
            {
                if (Current.blankers[i].IsEnable)
                {
                    if (!Current.blankers[i].Plc.TcpDisConnect(out msg))
                    {
                        Error.Alert(msg);
                        return false;
                    }
                    tbBlankerStatus[i].Text = "未连接";
                    this.pbBlankerLamp[i].Image = Properties.Resources.Gray_Round;
                }

                Current.blankers[i].PreAlarmStr = string.Empty;
            }

            if (Current.Robot.IsEnable)
            {
                //if (Current.Robot.IsStartting && !Current.Robot.IsPausing)
                //{
                //    Current.Robot.Pause(out msg);
                //}

                if (!Current.Robot.Plc.TcpDisConnect(out msg))
                {
                    Error.Alert(msg);
                    return false;
                }
                tbRobotStatus.Text = "未连接";
                this.pbRobotLamp.Image = Properties.Resources.Gray_Round;
            }

            Current.Transfer.IsAlive = false;
            Current.Cache.IsAlive = false;

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
                    this.BeginInvoke(new UpdateUI1PDelegate(RefreshMesStatus), "连接成功");
                    if (Current.mes.IsOffline)
                    {
                        Current.mes.IsOffline = false;
                    }
                }
            }
            return true;
        }

        private bool MesDisConnect()
        {
            string msg = string.Empty;

            if (Current.mes.IsEnable)
            {
                this.BeginInvoke(new MethodInvoker(() => { this.tbMesStatus.Text = "未连接"; }));
            }

            return true;
        }

        private bool ScanerConnect()
        {
            string msg = string.Empty;

            for (int i = 0; i < Current.feeders.Count; i++)
            {

                for (int j = 0; j < Current.feeders[i].Scaners.Count; j++)
                {

                    if (Current.feeders[i].Scaners[j].IsEnable)
                    {
                        if (!Current.feeders[i].Scaners[j].IsPingSuccess)
                        {

                            Error.Alert(string.Format("无法连接到{0}：{1}", Current.feeders[i].Scaners[j].Name, Current.feeders[i].Scaners[j].IP));
                            return false;
                        }

                        if (!Current.feeders[i].Scaners[j].TcpConnect(out msg))
                        {
                            Error.Alert(string.Format("{0}:打开连接失败，原因：{1}", Current.feeders[i].Scaners[j].Name, msg));
                            return false;
                        }
                        int ii = i;
                        int jj = j;
                        this.BeginInvoke(new MethodInvoker(() => { tbScanerStatus[ii][jj].Text = "连接成功"; }));
                    }
                }
            }

            return true;
        }

        private bool ScanerDisConnect()
        {
            string msg = string.Empty;
            for (int i = 0; i < Current.feeders.Count; i++)
            {
                for (int j = 0; j < Current.feeders[i].BatteryScaners.Count; j++)
                {
                    if (Current.feeders[i].BatteryScaners[j].IsEnable)
                    {
                        if (Current.feeders[i].BatteryScaners[j].IsAlive)
                        {
                            Current.feeders[i].BatteryScaners[j].StopBatteryScan();
                        }                   
                        if (!Current.feeders[i].BatteryScaners[j].TcpDisConnect(out msg))
                        {
                            Error.Alert(msg);
                            return false;
                        }
                        tbScanerStatus[i][j].Text = "未连接";
                        this.pbScanerLamp[i][j].Image = Properties.Resources.Gray_Round;
                    }
                }


                if (Current.feeders[i].ClampScaner.IsEnable)
                {
                    if (Current.feeders[i].ClampScaner.IsAlive)
                    {
                        Current.feeders[i].ClampScaner.StopClampScan();
                    }
                    
                    if (!Current.feeders[i].ClampScaner.TcpDisConnect(out msg))
                    {
                        Error.Alert(msg);
                        return false;
                    }
                    tbScanerStatus[i][2].Text = "未连接";
                    this.pbScanerLamp[i][2].Image = Properties.Resources.Gray_Round;
                }

            }
            return true;
        }

        #endregion

        #region 主运行逻辑

        delegate void UpdateUI1PDelegate(string text);

        System.Timers.Timer[] timerOvenRuns = new System.Timers.Timer[OvenCount] { null, null, null, null, null, null, null, null, null, null };

        System.Timers.Timer[] timerFeederRuns = new System.Timers.Timer[FeederCount] { null, null };

        System.Timers.Timer[] timerBlankerRuns = new System.Timers.Timer[BlankerCount] { null, null };

        System.Timers.Timer timerRobotRun = null;

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
                if (cbOvenIsEnable[i].Checked && !Current.ovens[i].Plc.IsAlive)
                {
                    msg = Current.ovens[i].Name + " 启动异常！";
                    return false;
                }
            }

            for (int i = 0; i < Current.feeders.Count; i++)
            {
                if (cbFeederIsEnable[i].Checked && !Current.feeders[i].Plc.IsAlive)
                {
                    msg = Current.feeders[i].Name + " 启动异常！";
                    return false;
                }

                for (int j = 0; j < Current.feeders[i].Scaners.Count; j++)
                {
                    if (cbScanerIsEnable[i][j].Checked && !Current.feeders[i].Scaners[j].IsAlive)
                    {
                        msg = Current.feeders[i].Scaners[j].Name + " 启动异常！";
                        return false;
                    }
                }
            }

            for (int i = 0; i < Current.blankers.Count; i++)
            {
                if (cbBlankerIsEnable[i].Checked && !Current.blankers[i].Plc.IsAlive)
                {
                    msg = Current.blankers[i].Name + " 启动异常！";
                    return false;
                }
            }

            if (cbRobotIsEnable.Checked && !Current.Robot.Plc.IsAlive)
            {
                msg = Current.Robot.Name + " 启动异常！";
                return false;
            }

            if (cbMesIsEnable.Checked && !Current.mes.IsPingSuccess)
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

                for (int i = 0; i < FeederCount; i++)
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

                for (int i = 0; i < BlankerCount; i++)
                {
                    int index = i;//如果直接用i, 则完成循环后 i一直 = OvenCount
                    timerBlankerRuns[i] = new System.Timers.Timer();
                    timerBlankerRuns[i].Interval = TengDa._Convert.StrToInt(TengDa.WF.Option.GetOption("CheckPlcPeriod"), 1000);
                    timerBlankerRuns[i].Elapsed += delegate
                    {
                        Thread listen = new Thread(new ParameterizedThreadStart(BlankerRunInvokeFunc));
                        listen.IsBackground = true;
                        listen.Start(index);
                    };
                    Thread.Sleep(200);
                    timerBlankerRuns[i].Start();
                }

                timerRobotRun = new System.Timers.Timer();
                timerRobotRun.Interval = TengDa._Convert.StrToInt(TengDa.WF.Option.GetOption("CheckPlcPeriod"), 1000);
                timerRobotRun.Elapsed += delegate
                {
                    Thread listen = new Thread(new ThreadStart(RobotRunInvokeFunc));
                    listen.IsBackground = true;
                    listen.Start();
                };
                Thread.Sleep(200);
                timerRobotRun.Start();

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

            SetCheckBoxEnable(false);//运行后禁止操作启用复选框

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

            for (int i = 0; i < FeederCount; i++)
            {
                if (timerFeederRuns[i] != null)
                {
                    timerFeederRuns[i].Stop();
                    timerFeederRuns[i].Close();
                    timerFeederRuns[i].Dispose();
                }
            }

            for (int i = 0; i < BlankerCount; i++)
            {
                if (timerBlankerRuns[i] != null)
                {
                    timerBlankerRuns[i].Stop();
                    timerBlankerRuns[i].Close();
                    timerBlankerRuns[i].Dispose();
                }
            }

            if (timerRobotRun != null)
            {
                timerRobotRun.Stop();
                timerRobotRun.Close();
                timerRobotRun.Dispose();
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

        private void SetCheckBoxEnable(bool IsEnable)
        {
            for (int i = 0; i < Current.feeders.Count; i++)
            {
                cbFeederIsEnable[i].Enabled = IsEnable;
                for (int j = 0; j < Current.feeders[i].Scaners.Count; j++)
                {
                    cbScanerIsEnable[i][j].Enabled = IsEnable;
                }
            }
            for (int i = 0; i < OvenCount; i++)
            {
                cbOvenIsEnable[i].Enabled = IsEnable;
            }
            for (int i = 0; i < Current.blankers.Count; i++)
            {
                cbBlankerIsEnable[i].Enabled = IsEnable;
            }
            cbScanerIsEnable0103.Enabled = IsEnable;
            cbMesIsEnable.Enabled = IsEnable;
            cbRobotIsEnable.Enabled = IsEnable;
        }

        private void OvenRunInvokeFunc(object obj)
        {

            string msg = string.Empty;

            int i = System.Convert.ToInt32(obj);

            if (timerlock && Current.ovens[i].IsEnable)
            {
                this.BeginInvoke(new MethodInvoker(() => { tbOvenStatus[i].Text = "发送指令—>" + Current.ovens[i].Name + "PLC"; }));
                if (Current.ovens[i].GetInfo())
                {
                    this.BeginInvoke(new MethodInvoker(() => { tbOvenStatus[i].Text = "成功获得" + Current.ovens[i].Name + "信息"; }));
                }
                else
                {
                    this.BeginInvoke(new MethodInvoker(() => { tbOvenStatus[i].Text = "获取" + Current.ovens[i].Name + "信息失败"; }));
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

                                //烘烤完成直接破真空                         
                                if (station.FloorStatus == FloorStatus.待出 && floor.IsVacuum && floor.RunRemainMinutes <= 1)
                                {
                                    Current.ovens[i].UploadVacuum(j);

                                }
                                this.tlpFloor[i][j].Invalidate();
                            }
                            else if (station.ClampStatus == ClampStatus.异常)
                            {
                                this.tlpFloor[i][j].Invalidate();
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
                                    if (floor.RunRemainMinutes <= 0)
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
                    }
                }
            }
        }

        private void FeederRunInvokeFunc(object obj)
        {

            string msg = string.Empty;

            int i = System.Convert.ToInt32(obj);

            if (Current.feeders[i].IsEnable)
            {
                this.BeginInvoke(new MethodInvoker(() => { tbFeederStatus[i].Text = "发送指令—>" + Current.feeders[i].Name + "PLC"; }));
                if (Current.feeders[i].GetInfo())
                {
                    this.BeginInvoke(new MethodInvoker(() => { tbFeederStatus[i].Text = "成功获得" + Current.feeders[i].Name + "信息"; }));
                }
                else
                {
                    this.BeginInvoke(new MethodInvoker(() => { tbFeederStatus[i].Text = "获取" + Current.feeders[i].Name + "信息失败"; }));
                }

                if (Current.feeders[i].AlreadyGetAllInfo)
                {
                    #region 夹具扫码逻辑
                    if (Current.feeders[i].ClampScaner.IsEnable && Current.feeders[i].ClampScaner.CanScan)
                    {
                        Current.feeders[i].Stations.ForEach(s =>
                        {
                            if (s.IsClampScanReady)
                            {
                                string code = string.Empty;
                                ScanResult result = Current.feeders[i].ClampScaner.StartClampScan(out code, out msg);
                                if (result == ScanResult.OK)
                                {
                                    this.BeginInvoke(new MethodInvoker(() => { this.tbScanerStatus[i][2].Text = "+" + code; }));                           
                                    s.Clamp.Code = code;
                                    s.Clamp.ScanTime = DateTime.Now;

                                    if (!Current.feeders[i].SetScanClampResult(ScanResult.OK, out msg))
                                    {
                                        Error.Alert(msg);
                                    }
                                }
                                else if (result == ScanResult.NG || result == ScanResult.Timeout)
                                {
                                    this.BeginInvoke(new MethodInvoker(() => { this.tbScanerStatus[i][2].Text = "扫码NG"; }));
                                    if (!Current.feeders[i].SetScanClampResult(ScanResult.NG, out msg))
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

                    #region 电池扫码逻辑

                    var batteryScanResults = new ScanResult[2];

                    var isScanFlag = false;//是否执行了扫码

                    for (int j = 0;j < Current.feeders[i].BatteryScaners.Count; j++)
                    {
                        int ii = i;
                        int jj = j;
                        if (Current.feeders[ii].BatteryScaners[jj].IsEnable && Current.feeders[ii].BatteryScaners[jj].CanScan)
                        {

                            string code = string.Empty;
                            ScanResult result = Current.feeders[ii].BatteryScaners[j].StartBatteryScan(out code, out msg);

                            isScanFlag = true;

                            if (result == ScanResult.OK)
                            {
                                this.BeginInvoke(new MethodInvoker(() =>
                                {
                                    this.tbScanerStatus[ii][jj].Text = "+" + code;
                                    this.tbScanerStatus[ii][jj].ForeColor = SystemColors.Control;
                                    this.tbScanerStatus[ii][jj].BackColor = Color.Green;
                                }));

                                Thread t = new Thread(() => {
                                    Thread.Sleep(1000);
                                    this.BeginInvoke(new MethodInvoker(() =>
                                    {
                                        this.tbScanerStatus[ii][jj].ForeColor = Color.Green;
                                        this.tbScanerStatus[ii][jj].BackColor = SystemColors.Control;
                                    }));
                                });
                                t.Start();

                                int id = Battery.Add(new Battery(code, Current.feeders[ii].Id, Current.feeders[ii].CurrentPutClampId), out msg);
                                if (id < 1)
                                {
                                    Error.Alert(msg);
                                }

                                batteryScanResults[j] = ScanResult.OK;

                            }
                            else if (result == ScanResult.NG || result == ScanResult.Timeout)
                            {
                                //再扫一次
                                result = Current.feeders[ii].BatteryScaners[jj].StartBatteryScan(out code, out msg);
                                if (result == ScanResult.OK)
                                {
                                    this.BeginInvoke(new MethodInvoker(() => 
                                    {
                                        this.tbScanerStatus[ii][jj].Text = "+" + code;
                                        this.tbScanerStatus[ii][jj].ForeColor = SystemColors.Control;
                                        this.tbScanerStatus[ii][jj].BackColor = Color.Green;
                                    }));

                                    Thread t = new Thread(() => {
                                        Thread.Sleep(1000);
                                        this.BeginInvoke(new MethodInvoker(() =>
                                        {
                                            this.tbScanerStatus[ii][jj].ForeColor = Color.Green;
                                            this.tbScanerStatus[ii][jj].BackColor = SystemColors.Control;
                                        }));
                                    });
                                    t.Start();

                                    int id = Battery.Add(new Battery(code, Current.feeders[ii].Id, Current.feeders[ii].CurrentPutClampId), out msg);
                                    if (id < 1)
                                    {
                                        Error.Alert(msg);
                                    }

                                    batteryScanResults[jj] = ScanResult.OK;
                                }
                                else if (result == ScanResult.NG || result == ScanResult.Timeout)
                                {
                                    this.BeginInvoke(new MethodInvoker(() => { this.tbScanerStatus[ii][jj].Text = "扫码NG"; }));

                                    batteryScanResults[jj] = ScanResult.NG;
                                }
                            }
                            else
                            {
                                Error.Alert(msg);
                            }

                            Current.feeders[ii].BatteryScaners[j].CanScan = false;
                        }
                    }

                    if (isScanFlag)
                    {
                        if (!Current.feeders[i].SetScanBatteryResult(batteryScanResults, out msg))
                        {
                            Error.Alert(msg);
                        }
                    }

                    #endregion

                    #region 绘制夹具中电池个数图示
                    for (int j = 0; j < FeederStationCount; j++)
                    {
                        if (Current.feeders[i].Stations[j].Id == Current.feeders[i].CurrentPutStationId)
                        {
                            if (Current.feeders[i].PreCurrentBatteryCount != Current.feeders[i].CurrentBatteryCount)
                            {
                                this.tlpFeederStationClamp[i][j].Invalidate();
                            }
                        }

                        if (Current.feeders[i].Stations[j].PreIsAlive != Current.feeders[i].Stations[j].IsAlive)
                        {
                            this.tlpFeederStationClamp[i][j].Invalidate();
                        }
                        Current.feeders[i].Stations[j].PreIsAlive = Current.feeders[i].Stations[j].IsAlive;
                    }
                    Current.feeders[i].PreCurrentBatteryCount = Current.feeders[i].CurrentBatteryCount;
                    #endregion
                }
            }
        }

        private void BlankerRunInvokeFunc(object obj)
        {

            string msg = string.Empty;

            int i = System.Convert.ToInt32(obj);


            if (timerlock && Current.blankers[i].IsEnable)
            {
                this.BeginInvoke(new MethodInvoker(() => { tbBlankerStatus[i].Text = "发送指令—>" + Current.blankers[i].Name + "PLC"; }));
                if (Current.blankers[i].GetInfo())
                {
                    this.BeginInvoke(new MethodInvoker(() => { tbBlankerStatus[i].Text = "成功获得" + Current.blankers[i].Name + "信息"; }));
                }
                else
                {
                    this.BeginInvoke(new MethodInvoker(() => { tbBlankerStatus[i].Text = "获取" + Current.blankers[i].Name + "信息失败"; }));
                }

                if (Current.blankers[i].AlreadyGetAllInfo && Current.blankers[i].IsAlive && Current.Robot.IsAlive)
                {
                    //人员进入安全光栅感应区
                    if (Current.blankers[i].IsRasterInductive && !Current.Robot.IsPausing && Current.Robot.Position <= Current.option.RobotStopPosition4RasterInductive)
                    {
                        if (Current.Robot.Stop(out msg))
                        {
                            Error.Alert(string.Format("人员进入 {0} 安全光栅感应区域，已远程发送急停信号给 {1}", Current.blankers[i].Name, Current.Robot.Name));
                        }
                        else
                        {
                            Error.Alert(string.Format("人员进入 {0} 安全光栅感应区域，远程发送急停信号给 {1} 失败！", Current.blankers[i].Name, Current.Robot.Name));
                        }
                    }

                    //另一台下料机
                    int ii = BlankerCount - i - 1;
                    bool otherBlankerIsRasterInductive = Current.blankers[ii].IsRasterInductive && Current.blankers[ii].IsAlive;

                    //安全光栅感应报警结束
                    if (!Current.blankers[i].IsRasterInductive && !otherBlankerIsRasterInductive && Current.Robot.IsPausing && Current.Robot.Position <= Current.option.RobotStopPosition4RasterInductive)
                    {
                        if (Current.Robot.Restart(out msg))
                        {
                            Tip.Alert(string.Format("{0} 安全光栅感应报警结束，已远程发送继续运动信号给 {1}", Current.blankers[i].Name, Current.Robot.Name));
                        }
                        else
                        {
                            Error.Alert(string.Format("{0} 安全光栅感应报警结束，远程发送继续运动信号给 {1} 失败！", Current.blankers[i].Name, Current.Robot.Name));
                        }
                    }

                }
            }
        }

        private void RobotRunInvokeFunc()
        {
            string msg = string.Empty;
            if (timerlock && Current.Robot.IsEnable)
            {
                this.BeginInvoke(new MethodInvoker(() => { tbRobotStatus.Text = "发送指令—>" + Current.Robot.Name + "PLC"; }));
                if (Current.Robot.GetInfo())
                {
                    this.BeginInvoke(new MethodInvoker(() => { tbRobotStatus.Text = "成功获得" + Current.Robot.Name + "信息"; }));
                }
                else
                {
                    this.BeginInvoke(new MethodInvoker(() => { tbRobotStatus.Text = "获取" + Current.Robot.Name + "信息失败"; }));
                }

                if (Current.Robot.AlreadyGetAllInfo)
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
                            if (floor.DoorStatus == DoorStatus.打开 && floor.Stations.Count(s => s.Id == Current.Task.FromStationId) < 1
                                && floor.Stations.Count(s => s.Id == Current.Task.ToStationId) < 1
                                && floor.Stations[0].IsAlive && floor.Stations[1].IsAlive)
                            {
                                Current.ovens[i].CloseDoor(j);
                            }

                            //从某一炉子取完盘后，立即关门，无需等到整个任务结束
                            if (floor.DoorStatus == DoorStatus.打开 && floor.Stations.Count(s => s.Id == Current.Task.FromStationId) > 0 && floor.Stations[0].IsAlive && floor.Stations[1].IsAlive
                                && Current.Robot.IsAlive && (Current.Task.Status == TaskStatus.取完 || Current.Task.Status == TaskStatus.正放))
                            {
                                Current.ovens[i].CloseDoor(j);
                            }

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

                    //测水分出炉前破真空
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

                    //测试OK出炉前破真空
                    var floors2 = Floor.FloorList.Where(f => f.IsAlive && f.IsVacuum && !f.Stations[0].IsOpenDoorIntervene && f.Stations.Count(s => s.FloorStatus == FloorStatus.待出) > 0 && f.Stations.Count(s => s.SampleStatus == SampleStatus.测试OK) > 0).OrderBy(f => f.Stations[0].GetPutTime);
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

                    //下料台取消干涉
                    for (int i = 0; i < BlankerCount; i++)
                    {
                        for (int j = 0; j < Current.blankers[i].Stations.Count; j++)
                        {
                            Station station = Current.blankers[i].Stations[j];

                            //无任务默认关门
                            if (station.DoorStatus == DoorStatus.打开 && station.Id != Current.Task.FromStationId && station.Id != Current.Task.ToStationId && station.IsAlive)
                            {
                                station.CloseDoor();
                            }

                            //从某一炉子取完盘后，立即关门，无需等到整个任务结束
                            if (station.DoorStatus == DoorStatus.打开 && station.Id == Current.Task.FromStationId && station.IsAlive
                                && (Current.Task.Status == TaskStatus.取完 || Current.Task.Status == TaskStatus.正放))
                            {
                                station.CloseDoor();
                            }
                        }
                    }
                }

                //无论手动还是自动任务模式都会执行
                for (int i = 0; i < OvenCount; i++)
                {
                    for (int j = 0; j < Current.ovens[i].Floors.Count; j++)
                    {
                        Floor floor = Current.ovens[i].Floors[j];

                        if (floor.Stations.Count(s => s.FloorStatus == FloorStatus.烘烤 && s.SampleInfo == SampleInfo.有样品) == 2 || floor.Stations.Count(s => s.FloorStatus == FloorStatus.烘烤 && s.SampleInfo == SampleInfo.无样品) == 2)
                        {
                            floor.Stations[0].SampleInfo = SampleInfo.有样品;
                            floor.Stations[1].SampleInfo = SampleInfo.无样品;
                            floor.Stations[0].SampleStatus = SampleStatus.待测试;
                            floor.Stations[1].SampleStatus = SampleStatus.待结果;
                        }

                        if (floor.Stations.Count(s => s.FloorStatus == FloorStatus.烘烤) == 2 && floor.Stations.Count(s => s.SampleInfo == SampleInfo.有样品) == 1 && floor.Stations.Count(s => s.SampleInfo == SampleInfo.无样品) == 1)
                        {
                            floor.Stations.ForEach(s =>
                            {
                                s.SampleStatus = s.SampleInfo == SampleInfo.有样品 ? SampleStatus.待测试 : SampleStatus.待结果;
                            });
                        }

                        if (floor.Stations.Count(s => s.FloorStatus == FloorStatus.无盘) == 2 || floor.Stations.Count(s => s.FloorStatus == FloorStatus.空盘) == 1 && floor.Stations.Count(s => s.FloorStatus == FloorStatus.无盘) == 1)
                        {
                            floor.Stations.ForEach(s => s.SampleStatus = SampleStatus.未知);
                        }

                        floor.Stations.ForEach(s => { if (s.FloorStatus == FloorStatus.空盘) { s.SampleStatus = SampleStatus.未知; } });
                    }
                }
            }
        }

        public void RefreshMesStatus(string text)
        {
            this.tbMesStatus.Text = text;
        }

        #endregion

        #region 机器人搬运主逻辑

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
                UploadMesTVD();

                UploadInOvenInfo(new List<Clamp>());

                UploadOutOvenInfo(new List<Clamp>());
            }
        }

        public async void UploadInOvenInfo(object obj)
        {
            try
            {
                Thread.Sleep(200);
                List<Clamp> clamps = (List<Clamp>)obj;
                string msg = string.Empty;
                if (clamps.Count < 1)
                {
                    clamps = Clamp.GetList(string.Format("SELECT TOP 10 * FROM [dbo].[{0}] WHERE [IsInUploaded] = 'false' AND [IsInFinished] = 'true' ORDER BY [ScanTime] DESC", Clamp.TableName), out msg);
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

                this.BeginInvoke(new MethodInvoker(() => { tbMesStatus.Text = "上传入烤箱数据..."; }));
                await MES.UploadInOvenAsync(clamps);
                this.BeginInvoke(new MethodInvoker(() => { tbMesStatus.Text = "入烤箱数据上传完成..."; }));
            }
            catch (Exception ex)
            {
                Error.Alert(ex.Message);
            }
        }

        public async void UploadOutOvenInfo(object obj)
        {
            try
            {
                Thread.Sleep(200);
                List<Clamp> clamps = (List<Clamp>)obj;
                string msg = string.Empty;
                if (clamps.Count < 1)
                {
                    clamps = Clamp.GetList(string.Format("SELECT TOP 10 * FROM [dbo].[{0}] WHERE [IsOutUploaded] = 'false' AND [IsOutFinished] = 'true' ORDER BY [ScanTime] DESC", Clamp.TableName), out msg);
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

                this.BeginInvoke(new MethodInvoker(() => { tbMesStatus.Text = "上传出烤箱数据..."; }));
                await MES.UploadOutOvenAsync(clamps);
                this.BeginInvoke(new MethodInvoker(() => { tbMesStatus.Text = "出烤箱数据上传完成..."; }));
            }
            catch (Exception ex)
            {
                Error.Alert(ex.Message);
            }
        }

        /// <summary>
        /// 定时上传真空温度数据
        /// </summary>
        public async void UploadMesTVD()
        {
            try
            {
                var uploadTVDs = new List<UploadTVD>();
                for (int i = 0; i < OvenCount; i++)
                {
                    for (int j = 0; j < OvenFloorCount; j++)
                    {
                        for (int k = 0; k < Current.ovens[i].Floors[j].Stations.Count; k++)
                        {
                            var station = Current.ovens[i].Floors[j].Stations[k];
                            if (station.IsAlive && station.FloorStatus == FloorStatus.烘烤 && station.ClampId > 0)
                            {
                                uploadTVDs.Add(new UploadTVD()
                                {
                                    ClampId = station.ClampId,
                                    DeviceStatus = 0,
                                    CollectorTime = DateTime.Now,
                                    IsUploaded = false,
                                    ParameterFlag = 0,
                                    T = Current.ovens[i].Floors[j].Temperatures,
                                    V1 = Current.ovens[i].Floors[j].Vacuum
                                });
                            }
                        }
                    }
                }

                if (uploadTVDs.Count > 0 && Current.mes.IsAlive)
                {
                    this.BeginInvoke(new MethodInvoker(() => { tbMesStatus.Text = "上传实时温度真空数据..."; }));
                    await MES.UploadTvdInfoAsync(uploadTVDs);
                    Thread.Sleep(100);
                    this.BeginInvoke(new MethodInvoker(() => { tbMesStatus.Text = "实时温度真空数据上传完成"; }));
                }

                //保存进数据库
                var msg = string.Empty;
                if (!UploadTVD.Add(uploadTVDs, out msg))
                {
                    LogHelper.WriteError(msg);
                }

                if (Current.mes.IsAlive)
                {
                    //检测上传失败的
                    uploadTVDs.Clear();
                    uploadTVDs = UploadTVD.GetList("SELECT TOP 100 * FROM [" + UploadTVD.TableName + "] WHERE IsUploaded = 'False' ORDER BY [CollectorTime] DESC", out msg);

                    if (uploadTVDs.Count > 0)
                    {
                        this.BeginInvoke(new MethodInvoker(() => { tbMesStatus.Text = "上传历史温度真空数据..."; }));
                        await MES.UploadTvdInfoAsync(uploadTVDs);
                        Thread.Sleep(100);
                        this.BeginInvoke(new MethodInvoker(() => { tbMesStatus.Text = "历史温度真空数据上传完成"; }));
                    }
                }

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
            string msg = string.Empty;

            TimeSpan ts = dtPickerStart.Value - dtPickerStop.Value;
            int queryBatteryTimeSpan = TengDa._Convert.StrToInt(Current.option.QueryBatteryTimeSpan, -1);
            if (Math.Abs(ts.Days) > TengDa._Convert.StrToInt(Current.option.QueryBatteryTimeSpan, -1))
            {
                Tip.Alert("查询时间范围太大，请将时间范围设置在 " + queryBatteryTimeSpan + " 天 之内！");
                return;
            }

            DataTable dt = Database.Query(string.Format("SELECT * FROM [dbo].[{0}.V_Battery] WHERE [扫码时间] BETWEEN '{1}' AND '{2}'", Config.DbTableNamePre, dtPickerStart.Value, dtPickerStop.Value), out msg);
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
            string msg = string.Empty;

            DataTable dt = Database.Query(string.Format("SELECT * FROM [dbo].[{0}.V_Operation] WHERE [操作时间] BETWEEN '{1}' AND '{2}'", Config.DbTableNamePre, dtPickerOperStart.Value, dtPickerOperStop.Value), out msg);
            dgViewOper.DataSource = dt;

            dgViewOper.Columns[4].DefaultCellStyle.Format = "yyyy-MM-dd  HH:mm:ss";

            //设置显示列宽度
            dgViewOper.Columns[3].Width = 400;
            dgViewOper.Columns[4].Width = 130;
        }

        private void cbScanerIsEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (!TengDa.WF.Current.IsRunning) return;
            for (int i = 0; i < Current.feeders.Count; i++)
            {
                for (int j = 0; j < Current.feeders[i].Scaners.Count; j++)
                {
                    if ((CheckBox)sender == cbScanerIsEnable[i][j])
                    {
                        Current.feeders[i].Scaners[j].IsEnable = cbScanerIsEnable[i][j].Checked;
                        return;
                    }
                }
            }
        }

        private void cbOvenIsEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (!TengDa.WF.Current.IsRunning) return;
            for (int i = 0; i < OvenCount; i++)
            {
                if ((CheckBox)sender == cbOvenIsEnable[i])
                {
                    Current.ovens[i].IsEnable = cbOvenIsEnable[i].Checked;
                    return;
                }
            }
        }

        private void cbFeederIsEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (!TengDa.WF.Current.IsRunning) return;
            for (int i = 0; i < FeederCount; i++)
            {
                if ((CheckBox)sender == cbFeederIsEnable[i])
                {
                    Current.feeders[i].IsEnable = cbFeederIsEnable[i].Checked;
                    return;
                }
            }
        }

        private void cbBlankerIsEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (!TengDa.WF.Current.IsRunning) return;
            for (int i = 0; i < BlankerCount; i++)
            {
                if ((CheckBox)sender == cbBlankerIsEnable[i])
                {
                    Current.blankers[i].IsEnable = cbBlankerIsEnable[i].Checked;
                    return;
                }
            }
        }

        private void cbRobotIsEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (!TengDa.WF.Current.IsRunning) return;
            Current.Robot.IsEnable = cbRobotIsEnable.Checked;
        }

        private void cbMesIsEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (!TengDa.WF.Current.IsRunning) return;
            Current.mes.IsEnable = cbMesIsEnable.Checked;
        }

        private void btnQueryTV_Click(object sender, EventArgs e)
        {
            string msg = string.Empty;

            DataTable dt = null;
            if (cbStations.Text.Trim() == "All")
            {
                dt = Database.Query(string.Format("SELECT TOP {3} * FROM [dbo].[{0}.V_TV] WHERE [记录时间] BETWEEN '{1}' AND '{2}' ", Config.DbTableNamePre, dtpStart.Value, dtpStop.Value, cbCount.Text), out msg);
            }
            else
            {
                dt = Database.Query(string.Format("SELECT TOP {4} * FROM [dbo].[{0}.V_TV] WHERE [烤箱工位] = '{1}' AND [记录时间] BETWEEN '{2}' AND '{3}' ", Config.DbTableNamePre, cbStations.Text.Trim(), dtpStart.Value, dtpStop.Value, cbCount.Text), out msg);
            }

            if (dt == null)
            {
                Error.Alert(msg);
                return;
            }

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
            string msg = string.Empty;

            TimeSpan ts = dtpAlarmStart.Value - dtpAlarmStop.Value;
            int timeSpan = TengDa._Convert.StrToInt(Current.option.QueryAlarmTimeSpan, -1);
            if (Math.Abs(ts.Days) > timeSpan)
            {
                Tip.Alert("查询时间范围太大，请将时间范围设置在 " + timeSpan + " 天 之内！");
                return;
            }

            DataTable dt = null;
            if (cbAlarmFloors.Text.Trim() == "All")
            {
                dt = Database.Query(string.Format("SELECT * FROM [dbo].[{0}.V_Alarm] WHERE [开始时间] BETWEEN '{1}' AND '{2}' ORDER BY [开始时间]", Config.DbTableNamePre, dtpAlarmStart.Value, dtpAlarmStop.Value), out msg);
            }
            else
            {
                dt = Database.Query(string.Format("SELECT * FROM [dbo].[{0}.V_Alarm] WHERE [名称] = '{1}' AND [开始时间] BETWEEN '{2}' AND '{3}' ORDER BY [开始时间]", Config.DbTableNamePre, cbAlarmFloors.Text.Trim(), dtpAlarmStart.Value, dtpAlarmStop.Value), out msg);
            }

            if (dt == null)
            {
                Error.Alert(msg);
                return;
            }

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
            string msg = string.Empty;

            TimeSpan ts = dtpStart.Value - dtpStop.Value;
            int timeSpan = TengDa._Convert.StrToInt(Current.option.QueryTVTimeSpan, -1) * 10;
            if (Math.Abs(ts.Days) > timeSpan)
            {
                Tip.Alert("查询时间范围太大，请将时间范围设置在 " + timeSpan + " 天 之内！");
                return;
            }

            DataTable dt = Database.Query(string.Format("SELECT * FROM [dbo].[{0}.V_TaskLog] WHERE [任务生成时间] BETWEEN '{1}' AND '{2}' ", Config.DbTableNamePre, dtpTaskStart.Value, dtpTaskStop.Value), out msg);

            if (dt == null)
            {
                Error.Alert(msg);
                return;
            }

            dgvTaskLog.DataSource = dt;
            //设置显示列宽度

            dgvTaskLog.Columns[1].Width = 130;
            dgvTaskLog.Columns[1].DefaultCellStyle.Format = "yyyy-MM-dd  HH:mm:ss";
            dgvTaskLog.Columns[4].Width = 130;
            dgvTaskLog.Columns[4].DefaultCellStyle.Format = "yyyy-MM-dd  HH:mm:ss";
            tbTaskCount.Text = dt.Rows.Count.ToString();
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
            kk.FileName = "机器人搬运任务 " + dtpStart.Value.ToString("yyyyMMddHHmmss") + "-" + dtpStop.Value.ToString("yyyyMMddHHmmss");

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
                        this.propertyGridSettings.SelectedObject = Oven.OvenList.First(o => o.Id.ToString() == e.Node.Text.Split(':')[0]);
                    }
                    else if (e.Node.Level == 2 && e.Node.Parent.Parent.Text == "烤箱" && e.Node.Text == "PLC")
                    {
                        this.propertyGridSettings.SelectedObject = Oven.OvenList.First(o => o.Id.ToString() == e.Node.Parent.Text.Split(':')[0]).Plc;
                    }
                    else if (e.Node.Level == 2 && e.Node.Parent.Parent.Text == "烤箱" && e.Node.Text.IndexOf("烤箱") > -1)
                    {
                        Oven oven = Oven.OvenList.First(o => o.Id.ToString() == e.Node.Parent.Text.Split(':')[0]);
                        this.propertyGridSettings.SelectedObject = oven.Floors.First(f => f.Id.ToString() == e.Node.Text.Split(':')[0]);
                    }
                    else if (e.Node.Level == 0 && e.Node.Text == "MES")
                    {
                        this.propertyGridSettings.SelectedObject = Current.mes;
                    }
                    else if (e.Node.Level == 0 && e.Node.Text == "机器人")
                    {
                        this.propertyGridSettings.SelectedObject = Current.Robot;
                    }
                    else if (e.Node.Level == 0 && e.Node.Text == "缓存架")
                    {
                        this.propertyGridSettings.SelectedObject = Current.Cache;
                    }
                    else if (e.Node.Level == 0 && e.Node.Text == "转移台")
                    {
                        this.propertyGridSettings.SelectedObject = Current.Transfer;
                    }
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
                    else if (e.Node.Level == 1 && e.Node.Parent.Text == "上料机")
                    {
                        this.propertyGridSettings.SelectedObject = Feeder.FeederList.First(f => f.Id.ToString() == e.Node.Text.Split(':')[0]);
                    }
                    else if (e.Node.Level == 2 && e.Node.Parent.Parent.Text == "上料机" && e.Node.Text == "PLC")
                    {
                        this.propertyGridSettings.SelectedObject = Feeder.FeederList.First(f => f.Id.ToString() == e.Node.Parent.Text.Split(':')[0]).Plc;
                    }
                    else if (e.Node.Level == 1 && e.Node.Parent.Text == "下料机")
                    {
                        this.propertyGridSettings.SelectedObject = Blanker.BlankerList.First(b => b.Id.ToString() == e.Node.Text.Split(':')[0]);
                    }
                    else if (e.Node.Level == 2 && e.Node.Parent.Parent.Text == "下料机" && e.Node.Text == "PLC")
                    {
                        this.propertyGridSettings.SelectedObject = Blanker.BlankerList.First(b => b.Id.ToString() == e.Node.Parent.Text.Split(':')[0]).Plc;
                    }
                    else if (e.Node.Level == 1 && e.Node.Parent.Text == "机器人" && e.Node.Text == "PLC")
                    {
                        this.propertyGridSettings.SelectedObject = Current.Robot.Plc;
                    }
                    else if (e.Node.Level == 1 && e.Node.Parent.Text == "机器人" && e.Node.Text.IndexOf("夹具") > -1)
                    {
                        this.propertyGridSettings.SelectedObject = Current.Robot.Clamp;
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

                //用来写入参数值
                if (type == typeof(Floor))
                {
                    var floor = (Floor)o;
                    var oven = floor.GetOven();
                    var j = oven.Floors.IndexOf(floor);
                    var propertyName = ((PropertyGrid)s).SelectedGridItem.PropertyDescriptor.Name;

                    string command = "";

                    if (propertyName == "PreheatTimeSet")
                    {
                        command = string.Format("%01#WDD{0:D5}{0:D5}{1}**", int.Parse(Current.option.PreheatTimeSetAddrs.Split(',')[j]), PanasonicPLC.ToRevertHexString((int)e.ChangedItem.Value));
                    }
                    else if (propertyName == "BakingTimeSet")
                    {
                        command = string.Format("%01#WDD{0:D5}{0:D5}{1}**", int.Parse(Current.option.BakingTimeSetAddrs.Split(',')[j]), PanasonicPLC.ToRevertHexString((int)e.ChangedItem.Value));
                    }
                    else if (propertyName == "BreathingCycleSet")
                    {
                        command = string.Format("%01#WDD{0:D5}{0:D5}{1}**", int.Parse(Current.option.BreathingCycleSetAddrs.Split(',')[j]), PanasonicPLC.ToRevertHexString((int)e.ChangedItem.Value));
                    }

                    if (!string.IsNullOrEmpty(command))
                    {
                        oven.ControlCommands.Add(command);
                    }

                }

            }
            else if (type == typeof(Robot))
            {
                settingsStr = string.Format("将{3}的 {0} 由 {1} 修改为 {2} ", e.ChangedItem.PropertyDescriptor.DisplayName, e.OldValue, e.ChangedItem.Value, Current.Robot.Name);
            }
            else if (type == typeof(Transfer))
            {
                settingsStr = string.Format("将{3}的 {0} 由 {1} 修改为 {2} ", e.ChangedItem.PropertyDescriptor.DisplayName, e.OldValue, e.ChangedItem.Value, Current.Transfer.Name);
            }
            else if (type == typeof(Cache))
            {
                settingsStr = string.Format("将{3}的 {0} 由 {1} 修改为 {2} ", e.ChangedItem.PropertyDescriptor.DisplayName, e.OldValue, e.ChangedItem.Value, Current.Cache.Name);
            }
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
            if (Current.runStstus != RunStatus.运行)
            {
                Tip.Alert("请先启动！");
                return;
            }

            int i = TengDa._Convert.StrToInt(srcFloorName.Substring(8, 2), 0) - 1;
            int j = TengDa._Convert.StrToInt(srcFloorName.Substring(10, 2), 0) - 1;
            Current.ovens[i].Floors[j].AddLog("手动开门");
            Current.ovens[i].OpenDoor(j);
        }

        private void tsmOvenCloseDoor_Click(object sender, EventArgs e)
        {
            if (Current.runStstus != RunStatus.运行)
            {
                Tip.Alert("请先启动！");
                return;
            }

            int i = TengDa._Convert.StrToInt(srcFloorName.Substring(8, 2), 0) - 1;
            int j = TengDa._Convert.StrToInt(srcFloorName.Substring(10, 2), 0) - 1;

            if (Current.ovens[i].Floors[j].Stations.Count(s => s.Id == Current.Task.FromStationId) > 0 && (Current.Task.Status == TaskStatus.就绪 || Current.Task.Status == TaskStatus.可取 || Current.Task.Status == TaskStatus.正取))
            {
                Tip.Alert(Current.Task.FromStationName + "正在取盘，无法关门！");
                return;
            }

            if (Current.ovens[i].Floors[j].Stations.Count(s => s.Id == Current.Task.ToStationId) > 0 && (Current.Task.Status == TaskStatus.可放 || Current.Task.Status == TaskStatus.正放))
            {
                Tip.Alert(Current.Task.FromStationName + "正在放盘，无法关门！");
                return;
            }

            Current.ovens[i].Floors[j].AddLog("手动关门");
            Current.ovens[i].CloseDoor(j);
        }

        private void tlpFeederStationClamp_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            return;
            /*
            //tlpFeederStationClamp0102
            int i = _Convert.StrToInt((sender as TableLayoutPanel).Name.Substring(21, 2), 0) - 1;
            int j = _Convert.StrToInt((sender as TableLayoutPanel).Name.Substring(23, 2), 0) - 1;

            Graphics g = e.Graphics;
            Rectangle r = e.CellBounds;
            Brush brush = Brushes.Cyan;
            if (!(Current.feeders[i].Stations[j].IsAlive || Current.feeders[i].Plc.PreIsAlive))//|| Current.feeders[i].Plc.PreIsAlive防闪烁
            {
                brush = Brushes.WhiteSmoke;
            }
            else if (Current.feeders[i].Stations[j].ClampStatus == ClampStatus.满夹具)
            {
                brush = Brushes.LimeGreen;
            }
            else if (Current.feeders[i].Stations[j].ClampStatus == ClampStatus.空夹具)
            {
                brush = Brushes.Cyan;
                if (Current.feeders[i].Stations[j].Id == Current.feeders[i].CurrentPutStationId)
                {
                    for (int x = 0; x < 2; x++)
                    {
                        for (int y = 0; y < 25; y++)
                        {
                            if (e.Row == y && e.Column == x)
                            {

                                if ((24 - y) * 2 + x < Current.feeders[i].CurrentBatteryCount)
                                {
                                    brush = Brushes.LimeGreen;
                                }

                                //g.FillRectangle(brush, r);
                            }
                        }
                    }
                }
            }
            g.FillRectangle(brush, r);
            //tableLayoutPanel1.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).SetValue(tableLayoutPanel1, true, null); 
            */
        }

        private void tsmStartBaking_Click(object sender, EventArgs e)
        {
            if (Current.runStstus != RunStatus.运行)
            {
                Tip.Alert("请先启动！");
                return;
            }

            int i = TengDa._Convert.StrToInt(srcFloorName.Substring(8, 2), 0) - 1;
            int j = TengDa._Convert.StrToInt(srcFloorName.Substring(10, 2), 0) - 1;
            Current.ovens[i].Floors[j].AddLog("手动启动运行");
            Current.ovens[i].StartBaking(j);
        }

        private void tsmStopBaking_Click(object sender, EventArgs e)
        {
            if (Current.runStstus != RunStatus.运行)
            {
                Tip.Alert("请先启动！");
                return;
            }

            int i = TengDa._Convert.StrToInt(srcFloorName.Substring(8, 2), 0) - 1;
            int j = TengDa._Convert.StrToInt(srcFloorName.Substring(10, 2), 0) - 1;
            Current.ovens[i].Floors[j].AddLog("手动停止运行");
            Current.ovens[i].StopBaking(j);
        }

        private void tlpFloor_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            //tlpFloor0501
            int i = _Convert.StrToInt((sender as TableLayoutPanel).Name.Substring(8, 2), 0) - 1;
            int j = _Convert.StrToInt((sender as TableLayoutPanel).Name.Substring(10, 2), 0) - 1;

            Graphics g = e.Graphics;
            Rectangle r = e.CellBounds;
            Brush brush = Brushes.White;

            for (int k = 0; k < Current.ovens[i].Floors[j].Stations.Count; k++)
            {
                if (e.Column == (Current.ovens[i].ClampOri == ClampOri.B ? k * 2 : Current.ovens[i].Floors[j].Stations.Count - k * 2))
                {

                    Station station = Current.ovens[i].Floors[j].Stations[k];

                    bool canChangeColor = DateTime.Now.Second % 3 == 1;

                    if (canChangeColor && station.Id == Current.Task.FromStationId && (Current.Task.Status == TaskStatus.就绪 || Current.Task.Status == TaskStatus.可取 || Current.Task.Status == TaskStatus.正取))
                    {
                        brush = Brushes.White;
                    }
                    else if (canChangeColor && station.ClampStatus == ClampStatus.异常)
                    {
                        brush = Brushes.Red;
                    }
                    else if (canChangeColor && station.Id == Current.Task.ToStationId)
                    {
                        brush = Current.Task.FromClampStatus == ClampStatus.空夹具 ? Brushes.Cyan : Brushes.Yellow;
                    }
                    else
                    {
                        if (!station.IsAlive)
                        {
                            brush = Brushes.LightGray;
                        }
                        else
                        {
                            var floor = station.GetFloor();
                            if (floor.Stations.Count(s => s.FloorStatus == FloorStatus.待出 && s.SampleStatus == SampleStatus.待结果) == 1 && floor.Stations.Count(s => s.FloorStatus == FloorStatus.待烤 && s.SampleStatus == SampleStatus.待测试) == 1)
                            {
                                //正测
                                brush = Brushes.DeepSkyBlue;
                            }
                            else if (floor.Stations.Count(s => s.FloorStatus == FloorStatus.待出 && s.SampleStatus == SampleStatus.待结果) == 1 && floor.Stations.Count(s => s.FloorStatus == FloorStatus.待出 && s.SampleStatus == SampleStatus.待测试) == 1)
                            {
                                brush = station.SampleStatus == SampleStatus.待测试 ? Brushes.LimeGreen : Brushes.DeepSkyBlue;
                            }
                            else if (station.FloorStatus == FloorStatus.待出 && station.SampleStatus == SampleStatus.待结果 && station.GetLabStation().FloorStatus == FloorStatus.无盘)
                            {
                                brush = Brushes.DeepSkyBlue;
                            }
                            else
                            {
                                switch (station.FloorStatus)
                                {
                                    case FloorStatus.无盘: brush = Brushes.White; break;
                                    case FloorStatus.空盘: brush = Brushes.Cyan; break;
                                    case FloorStatus.待烤: brush = Brushes.Yellow; break;
                                    //case FloorStatus.烘烤: brush = Brushes.Lime; break;
                                    case FloorStatus.烘烤: brush = Brushes.Pink; break;
                                    case FloorStatus.待出: brush = Brushes.LimeGreen; break;
                                    default: brush = Brushes.WhiteSmoke; break;
                                }
                            }
                        }
                    }

                }
            }
            g.FillRectangle(brush, r);
        }

        private void tlpBlanker_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            //tlpBlanker1
            int i = _Convert.StrToInt((sender as TableLayoutPanel).Name.Substring(10, 1), 0) - 1;

            Graphics g = e.Graphics;
            Rectangle r = e.CellBounds;
            Brush brush = Brushes.White;

            for (int j = 0; j < Current.blankers[i].Stations.Count; j++)
            {
                if (e.Column == 2 - j - 1)
                {
                    if (!Current.blankers[i].Stations[j].IsAlive)
                    {
                        brush = Brushes.LightGray;
                    }
                }
            }
            g.FillRectangle(brush, r);
        }

        private void tlpFeeder_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            //tlpFeeder1
            int i = _Convert.StrToInt((sender as TableLayoutPanel).Name.Substring(9, 1), 0) - 1;

            Graphics g = e.Graphics;
            Rectangle r = e.CellBounds;
            Brush brush = Brushes.White;

            for (int j = 0; j < FeederStationCount; j++)
            {
                if (e.Column == FeederStationCount - j - 1 && i == 1 || e.Column == j && i == 0)
                {
                    if (!Current.feeders[i].Stations[j].IsAlive)
                    {
                        brush = Brushes.LightGray;
                    }
                }
            }
            g.FillRectangle(brush, r);
        }

        private void tsmUploadVacuum_Click(object sender, EventArgs e)
        {
            if (Current.runStstus != RunStatus.运行)
            {
                Tip.Alert("请先启动！");
                return;
            }

            int i = TengDa._Convert.StrToInt(srcFloorName.Substring(8, 2), 0) - 1;
            int j = TengDa._Convert.StrToInt(srcFloorName.Substring(10, 2), 0) - 1;

            Current.ovens[i].Floors[j].AddLog("手动破真空");
            Current.ovens[i].UploadVacuum(j);

        }

        private void tsmLoadVacuum_Click(object sender, EventArgs e)
        {
            if (Current.runStstus != RunStatus.运行)
            {
                Tip.Alert("请先启动！");
                return;
            }

            int i = TengDa._Convert.StrToInt(srcFloorName.Substring(8, 2), 0) - 1;
            int j = TengDa._Convert.StrToInt(srcFloorName.Substring(10, 2), 0) - 1;

            Current.ovens[i].Floors[j].AddLog("手动抽真空");
            Current.ovens[i].LoadVacuum(j);

        }

        private void tsmCancelLoadVacuum_Click(object sender, EventArgs e)
        {
            if (Current.runStstus != RunStatus.运行)
            {
                Tip.Alert("请先启动！");
                return;
            }

            int i = TengDa._Convert.StrToInt(srcFloorName.Substring(8, 2), 0) - 1;
            int j = TengDa._Convert.StrToInt(srcFloorName.Substring(10, 2), 0) - 1;

            Current.ovens[i].Floors[j].AddLog("手动取消破真空");
            Current.ovens[i].CancelLoadVacuum(j);
        }

        private void tsmCancelUploadVacuum_Click(object sender, EventArgs e)
        {
            if (Current.runStstus != RunStatus.运行)
            {
                Tip.Alert("请先启动！");
                return;
            }

            int i = TengDa._Convert.StrToInt(srcFloorName.Substring(8, 2), 0) - 1;
            int j = TengDa._Convert.StrToInt(srcFloorName.Substring(10, 2), 0) - 1;

            Current.ovens[i].Floors[j].AddLog("手动取消抽真空");
            Current.ovens[i].CancelUploadVacuum(j);
        }

        private void tsmClearRunTime_Click(object sender, EventArgs e)
        {
            if (Current.runStstus != RunStatus.运行)
            {
                Tip.Alert("请先启动！");
                return;
            }

            int i = TengDa._Convert.StrToInt(srcFloorName.Substring(8, 2), 0) - 1;
            int j = TengDa._Convert.StrToInt(srcFloorName.Substring(10, 2), 0) - 1;
            Current.ovens[i].Floors[j].AddLog("手动运行时间清零");
            Current.ovens[i].ClearRunTime(j);
        }

        private void tsmOpenNetControl_Click(object sender, EventArgs e)
        {
            if (Current.runStstus != RunStatus.运行)
            {
                Tip.Alert("请先启动！");
                return;
            }

            int i = TengDa._Convert.StrToInt(srcFloorName.Substring(8, 2), 0) - 1;
            int j = TengDa._Convert.StrToInt(srcFloorName.Substring(10, 2), 0) - 1;
            Current.ovens[i].Floors[j].AddLog("手动打开网控");
            Current.ovens[i].OpenNetControl(j);
        }

        private void tsmAlarmReset_Click(object sender, EventArgs e)
        {
            if (Current.runStstus != RunStatus.运行)
            {
                Tip.Alert("请先启动！");
                return;
            }

            int i = TengDa._Convert.StrToInt(srcFloorName.Substring(8, 2), 0) - 1;
            int j = TengDa._Convert.StrToInt(srcFloorName.Substring(10, 2), 0) - 1;

            Current.ovens[i].Floors[j].AddLog("手动报警复位");
            Current.ovens[i].AlarmReset(j);
        }

        private string srcBlankerName = string.Empty;
        private void cmsBlanker_Opening(object sender, CancelEventArgs e)
        {
            //tlpBlanker1
            srcBlankerName = (sender as ContextMenuStrip).SourceControl.Name;
            int i = TengDa._Convert.StrToInt(srcBlankerName.Substring(10, 1), 0) - 1;
            this.tsmCancelRasterInductive.Enabled = Current.blankers[i].IsAlive;
        }

        private void TsmCancelRasterInductive_Click(object sender, EventArgs e)
        {
            if (Current.runStstus != RunStatus.运行)
            {
                Tip.Alert("请先启动！");
                return;
            }

            int i = TengDa._Convert.StrToInt(srcBlankerName.Substring(10, 1), 0) - 1;
            Current.blankers[i].CancelRasterInductive();
        }
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
                                        Current.ovens[i].Floors[j].Stations[k].sampledDatas[Option.TemperaturePointCount].Add((float)(Math.Log10(Current.ovens[i].Floors[j].Stations[k].GetFloor().Vacuum) * 10 + 20));
                                    }
                                    else
                                    {
                                        Current.ovens[i].Floors[j].Stations[k].sampledDatas[m].Add(Current.ovens[i].Floors[j].Stations[k].Temperatures[m]);
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
        private string srcFloorName = string.Empty;

        private void cmsFloor_Opening(object sender, CancelEventArgs e)
        {
            srcFloorName = (sender as ContextMenuStrip).SourceControl.Name;

            int i = TengDa._Convert.StrToInt(srcFloorName.Substring(8, 2), 0) - 1;
            int j = TengDa._Convert.StrToInt(srcFloorName.Substring(10, 2), 0) - 1;

            this.tsmOvenOpenDoor.Enabled =
                Current.ovens[i].Floors[j].IsNetControlOpen
                && Current.ovens[i].IsAlive
                && Current.ovens[i].Floors[j].DoorStatus != DoorStatus.打开
                && !Current.ovens[i].Floors[j].IsBaking
                && !Current.ovens[i].Floors[j].IsVacuum
                && !Current.ovens[i].Floors[j].Stations[0].IsOpenDoorIntervene;
            this.tsmOvenCloseDoor.Enabled =
                Current.ovens[i].Floors[j].IsNetControlOpen
                && Current.ovens[i].IsAlive
                && Current.ovens[i].Floors[j].DoorStatus != DoorStatus.关闭;
          //  this.tsmOpenNetControl.Enabled = Current.ovens[i].Floors[j].IsAlive && !Current.ovens[i].Floors[j].IsNetControlOpen;
            this.tsmLoadVacuum.Enabled =
                Current.ovens[i].Floors[j].IsNetControlOpen
                && Current.ovens[i].IsAlive
                 && Current.ovens[i].Floors[j].DoorStatus == DoorStatus.关闭
                && !Current.ovens[i].Floors[j].VacuumIsLoading
                && !Current.ovens[i].Floors[j].IsVacuum;
            this.tsmCancelLoadVacuum.Enabled =
                Current.ovens[i].Floors[j].IsNetControlOpen
                && Current.ovens[i].IsAlive
              && Current.ovens[i].Floors[j].VacuumIsLoading;
            this.tsmUploadVacuum.Enabled =
                Current.ovens[i].Floors[j].IsNetControlOpen
                && Current.ovens[i].IsAlive
                && !Current.ovens[i].Floors[j].IsBaking
              && !Current.ovens[i].Floors[j].VacuumIsUploading
              && Current.ovens[i].Floors[j].IsVacuum;
            this.tsmCancelUploadVacuum.Enabled =
                Current.ovens[i].Floors[j].IsNetControlOpen
                && Current.ovens[i].IsAlive
                && Current.ovens[i].Floors[j].VacuumIsUploading;
            this.tsmClearRunTime.Enabled = false;
                //Current.ovens[i].Floors[j].IsNetControlOpen
                //&& Current.ovens[i].IsAlive;
            this.tsmStartBaking.Enabled =
                Current.ovens[i].Floors[j].IsNetControlOpen
                && Current.ovens[i].IsAlive
                && !Current.ovens[i].Floors[j].IsBaking
                && Current.ovens[i].Floors[j].DoorStatus == DoorStatus.关闭;
            this.tsmStopBaking.Enabled =
                Current.ovens[i].Floors[j].IsNetControlOpen
                && Current.ovens[i].IsAlive
                && Current.ovens[i].Floors[j].IsBaking;
            this.tsmAlarmReset.Enabled = Current.ovens[i].IsAlive;
            this.tsmWatContentResult.Enabled = Current.ovens[i].Floors[j].Stations.Count(s => s.FloorStatus == FloorStatus.待出) > 0;
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

        #region 手动调试夹具扫码枪

        private void btnClampScanStart_Click(object sender, EventArgs e)
        {
            string code = string.Empty;
            string msg = string.Empty;
            ScanResult scanResult = Current.feeders[cbClampScaner.SelectedIndex].ClampScaner.StartClampScan(out code, out msg);
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
            if (!Current.feeders[cbClampScaner.SelectedIndex].SetScanClampResult(ScanResult.OK, out msg))
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
            if (!Current.feeders[cbClampScaner.SelectedIndex].SetScanClampResult(ScanResult.NG, out msg))
            {
                Error.Alert(msg);
            }
            else
            {
                Tip.Alert("OK");
            }
        }

        #endregion

        #region 手动调试电池扫码枪

        private void btnBatteryScanStart_Click(object sender, EventArgs e)
        {
            int i = cbBatteryScaner.SelectedIndex / 2;
            int j = cbBatteryScaner.SelectedIndex % 2;

            string code = string.Empty;
            string msg = string.Empty;
            ScanResult scanResult = Current.feeders[i].BatteryScaners[j].StartBatteryScan(out code, out msg);
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

        private void btnBatteryScanOkBackToFeeder_Click(object sender, EventArgs e)
        {
            int i = cbBatteryScaner.SelectedIndex / 2;
            string msg = string.Empty;
            if (!Current.feeders[i].SetScanBatteryResult(new ScanResult[2] { ScanResult.OK, ScanResult.OK }, out msg))
            {
                Error.Alert(msg);
            }
            else
            {
                Tip.Alert("OK");
            }
        }

        private void btnBatteryScanNgBackToFeeder_Click(object sender, EventArgs e)
        {
            int i = cbBatteryScaner.SelectedIndex / 2;
            string msg = string.Empty;
            if (!Current.feeders[i].SetScanBatteryResult(new ScanResult[2] { ScanResult.NG, ScanResult.NG }, out msg))
            {
                Error.Alert(msg);
            }
            else
            {
                Tip.Alert("OK");
            }
        }

        #endregion

        #region 水含量手动输入

        private void cbSampleSelected_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!TengDa.WF.Current.IsRunning) return;
            string cbSelectName = (sender as ComboBox).Name;
            int ii = 0, jj = 0;

            if (cbSelectName.Contains("Oven"))
            {
                ii = cbSampleSelectedOven.SelectedIndex;

                cbSampleSelectedFloor.Items.Clear();
                Current.ovens[ii].Floors.ForEach(f =>
                {
                    cbSampleSelectedFloor.Items.Add(f.Name);
                });
                jj = 0;
                cbSampleSelectedFloor.SelectedIndex = jj;
            }
            else if (cbSelectName.Contains("Floor"))
            {
                ii = cbSampleSelectedOven.SelectedIndex;
                jj = cbSampleSelectedFloor.SelectedIndex;
            }
            Current.option.SampleFloorId = Current.ovens[ii].Floors[jj].Id;
        }

        private void btnWaterContVerify_Click(object sender, EventArgs e)
        {
            int i = cbSampleSelectedOven.SelectedIndex;
            int j = cbSampleSelectedFloor.SelectedIndex;

            //if (Current.ovens[i].Floors[j].Stations.Count(s => s.FloorStatus == FloorStatus.烘烤) > 0)
            //{
            //    Tip.Alert(Current.ovens[i].Floors[j].Name + "正在烘烤，不能输入水含量数据！");
            //    return;
            //}

            float waterContent = TengDa._Convert.StrToFloat(tbWaterContent.Text.Trim(), 0);
            if (waterContent > 0)
            {
                //    Current.ovens[i].Floors[j].Stations.ForEach(s => s.Clamp.WaterContent = waterContent);
                //    Current.blankers.ForEach(b => b.Stations.ForEach(bs =>
                //    {
                //        Current.ovens[i].Floors[j].Stations.ForEach(fs =>
                //{
                //          if (bs.FromStationId == fs.Id && bs.ClampId > 0)
                //          {
                //              bs.Clamp.WaterContent = waterContent;
                //          }
                //      });
                //    }));
                Clamp.SetWaterCont(Current.ovens[i].Floors[j], waterContent);
                Tip.Alert("水含量数据输入成功！");
                if (timerlock && Current.mes.IsAlive)
                {
                    //UploadBatteriesInfo(new List<Clamp>());
                }
            }
            else
            {
                Error.Alert("输入水含量数据格式错误！");
            }
        }

        #endregion

        #region 机器人手动操作

        private void cmsRobot_Opening(object sender, CancelEventArgs e)
        {
            this.tsmRobotPause.Enabled = Current.Robot.IsAlive && !Current.Robot.IsPausing;
            this.tsmRobotRestart.Enabled = Current.Robot.IsAlive && Current.Robot.IsPausing;
            this.tsmManuGetStation.Enabled = Current.TaskMode == TaskMode.手动任务 && Current.Robot.IsAlive && Current.Robot.ClampStatus == ClampStatus.无夹具 && (Current.Task.Status == TaskStatus.正放 || Current.Robot.IsStarting) && Current.Task.Status != TaskStatus.可取 && Current.Task.Status != TaskStatus.正取;
            this.tsmManuPutStation.Enabled = Current.TaskMode == TaskMode.手动任务 && Current.Robot.IsAlive && Current.Robot.ClampStatus != ClampStatus.无夹具 && (Current.Task.Status == TaskStatus.正取 || Current.Robot.IsStarting) && Current.Task.Status != TaskStatus.可放 && Current.Task.Status != TaskStatus.正放;
        }

        private void tsmRobotStart_Click(object sender, EventArgs e)
        {
            var msg = string.Empty;
            if (Current.Robot.Start(out msg))
            {
                Tip.Alert(Current.Robot.Name + "启动成功！");
            }
            else
            {
                Error.Alert("启动失败！原因：" + msg);
            }
        }

        private void tsmRobotPause_Click(object sender, EventArgs e)
        {
            var msg = string.Empty;
            if (Current.Robot.Pause(out msg))
            {
                Tip.Alert(Current.Robot.Name + "暂停运行成功！");
            }
            else
            {
                Error.Alert(msg);
            }
        }

        private void tsmRobotRestart_Click(object sender, EventArgs e)
        {
            var msg = string.Empty;
            if (Current.Robot.Restart(out msg))
            {
                Tip.Alert(Current.Robot.Name + "继续运行成功！");
            }
            else
            {
                Error.Alert(msg);
            }
        }

        private void pbEmergencyStop_Click(object sender, EventArgs e)
        {
            var msg = string.Empty;
            if (Current.Robot.Stop(out msg))
            {
                Tip.Alert(Current.Robot.Name + "急停成功！");
            }
            else
            {
                Error.Alert(msg);
            }
        }

        private void tsmManuStation_DropDownOpening(object sender, EventArgs e)
        {
            string ManuFlag = string.Empty;
            bool isGet = false, isPut = false; 

            if ((sender as ToolStripItem).Name.Contains("Get"))
            {
                isGet = true;
                ManuFlag = "Get";
            }
            else if ((sender as ToolStripItem).Name.Contains("Put"))
            {
                isPut = true;
                ManuFlag = "Put";
            }

            List<ToolStripItem> tsiStations = new List<ToolStripItem>();

            for (int i = 0; i < Current.feeders.Count; i++)
            {
                ToolStripMenuItem tsmiFeederStation = new ToolStripMenuItem();
                List<ToolStripItem> tsiFeederStations = new List<ToolStripItem>();
                tsmiFeederStation.Text = Current.feeders[i].Name;

                var isEnabled = false;

                Current.feeders[i].Stations.ForEach(s =>
                {
                    ToolStripMenuItem tsiStation = new ToolStripMenuItem();
                    tsiStation.Name = string.Format("tsmManu_{0}_{1}", ManuFlag, s.Name);
                    tsiStation.Text = s.Name;
                    tsiStation.Click += new System.EventHandler(this.tsmManuStation_Click);

                    tsiStation.Enabled = GetTsiEnabled(ManuFlag, s);
                    tsiStation.ForeColor = GetForeColor(ManuFlag, s);

                    if (tsiStation.Enabled) isEnabled = true;

                    tsmiFeederStation.DropDownItems.Add(tsiStation);
                });

                tsmiFeederStation.Enabled = isEnabled;
                tsiStations.Add(tsmiFeederStation);
            }

            for (int i = 0; i < OvenCount; i++)
            {
                ToolStripMenuItem tsmiOvenStation = new ToolStripMenuItem();
                List<ToolStripItem> tsiOvenStations = new List<ToolStripItem>();
                tsmiOvenStation.Text = Current.ovens[i].Name;

                var isEnabled = false;

                Current.ovens[i].Floors.ForEach(f => f.Stations.ForEach(s =>
                {
                    ToolStripMenuItem tsiStation = new ToolStripMenuItem();
                    tsiStation.Name = string.Format("tsmManu_{0}_{1}", ManuFlag, s.Name);
                    tsiStation.Text = s.Name;
                    tsiStation.Click += new System.EventHandler(this.tsmManuStation_Click);

                    tsiStation.Enabled = GetTsiEnabled(ManuFlag, s);
                    tsiStation.ForeColor = GetForeColor(ManuFlag, s);

                    if (tsiStation.Enabled) isEnabled = true;

                    tsmiOvenStation.DropDownItems.Add(tsiStation);
                }));

                tsmiOvenStation.Enabled = isEnabled;
                tsiStations.Add(tsmiOvenStation);
            }

            for (int i = 0; i < Current.blankers.Count; i++)
            {
                ToolStripMenuItem tsmiBlankerStation = new ToolStripMenuItem();
                List<ToolStripItem> tsiBlankerStations = new List<ToolStripItem>();
                tsmiBlankerStation.Text = Current.blankers[i].Name;

                var isEnabled = false;

                Current.blankers[i].Stations.ForEach(s =>
                {
                    ToolStripMenuItem tsiStation = new ToolStripMenuItem();
                    tsiStation.Name = string.Format("tsmManu_{0}_{1}", ManuFlag, s.Name);
                    tsiStation.Text = s.Name;
                    tsiStation.Click += new System.EventHandler(this.tsmManuStation_Click);
                    tsiStation.Enabled = GetTsiEnabled(ManuFlag, s);
                    tsiStation.ForeColor = GetForeColor(ManuFlag, s);

                    if (tsiStation.Enabled) isEnabled = true;

                    tsmiBlankerStation.DropDownItems.Add(tsiStation);
                });

                tsmiBlankerStation.Enabled = isEnabled;
                tsiStations.Add(tsmiBlankerStation);
            }

            ToolStripMenuItem tsmiCacheStation = new ToolStripMenuItem();
            List<ToolStripItem> tsiCacheStations = new List<ToolStripItem>();
            tsmiCacheStation.Text = Current.Cache.Name;

            var isCacheEnabled = false;

            Current.Cache.Stations.ForEach(s =>
            {
                ToolStripMenuItem tsiStation = new ToolStripMenuItem();
                tsiStation.Name = string.Format("tsmManu_{0}_{1}", ManuFlag, s.Name);
                tsiStation.Text = s.Name;
                tsiStation.Click += new System.EventHandler(this.tsmManuStation_Click);

                tsiStation.Enabled = GetTsiEnabled(ManuFlag, s);
                tsiStation.ForeColor = GetForeColor(ManuFlag, s);

                if (tsiStation.Enabled) isCacheEnabled = true;
                tsmiCacheStation.DropDownItems.Add(tsiStation);
            });

            tsmiCacheStation.Enabled = isCacheEnabled;
            tsiStations.Add(tsmiCacheStation);

            ToolStripMenuItem tsmiRotaterStation = new ToolStripMenuItem();
            tsmiRotaterStation.Text = Current.Transfer.Station.Name;
            tsmiRotaterStation.Name = string.Format("tsmManu_{0}_{1}", ManuFlag, Current.Transfer.Station.Name);
            tsmiRotaterStation.Click += new System.EventHandler(this.tsmManuStation_Click);

            tsmiRotaterStation.Enabled = GetTsiEnabled(ManuFlag, Current.Transfer.Station);
            tsmiRotaterStation.ForeColor = GetForeColor(ManuFlag, Current.Transfer.Station);

            tsiStations.Add(tsmiRotaterStation);

            if (isGet)
            {
                this.tsmManuGetStation.DropDownItems.Clear();
                this.tsmManuGetStation.DropDownItems.AddRange(tsiStations.ToArray());
            }
            else if (isPut)
            {
                this.tsmManuPutStation.DropDownItems.Clear();
                this.tsmManuPutStation.DropDownItems.AddRange(tsiStations.ToArray());
            }

        }

        /// <summary>
        /// 手动时取放盘的按钮变灰防呆
        /// </summary>
        /// <param name="manuFlag"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool GetTsiEnabled(string manuFlag, Station s)
        {
            var result = true;
            if (manuFlag == "Get")
            {
                result = s.IsAlive && s.DoorStatus == DoorStatus.打开 && s.ClampStatus != ClampStatus.无夹具;
            }
            else if (manuFlag == "Put")
            {
                result = s.IsAlive && s.DoorStatus == DoorStatus.打开 && s.ClampStatus == ClampStatus.无夹具;
            }
            return result;
        }

        private Color GetForeColor(string manuFlag, Station s)
        {
            var result = Color.Red;
            if (manuFlag == "Get" && s.IsAlive && s.DoorStatus == DoorStatus.打开 && s.Status == StationStatus.可取)
            {
                result = Color.Black;
            }
            else if (manuFlag == "Put" && s.IsAlive && s.DoorStatus == DoorStatus.打开 && s.Status == StationStatus.可放)
            {
                result = Color.Black;
            }
            return result;
        }

        private void tsmManuStation_Click(object sender, EventArgs e)
        {

            try
            {
                if (Current.TaskMode == TaskMode.自动任务)
                {
                    Tip.Alert("请切换至手动任务模式！");
                    return;
                }

                if (!TengDa.WF.Current.IsTerminalInitFinished)
                {
                    Tip.Alert("烤箱信息初始化尚未完成，请稍后！");
                    return;
                }

                bool isGet = false, isPut = false; // (sender as ToolStripItem).Name.Contains("Get");

                if ((sender as ToolStripItem).Name.Contains("Get"))
                {
                    isGet = true;
                }
                else if ((sender as ToolStripItem).Name.Contains("Put"))
                {
                    isPut = true;
                }

                Station station = Station.StationList.First(s => s.Name == (sender as ToolStripItem).Name.Split('_')[2]);

                if (isGet)
                {
                    if (station.ClampStatus == ClampStatus.无夹具)
                    {
                        Tip.Alert("该位置无夹具，不能取盘！");
                        return;
                    }

                    if (Current.Robot.ClampStatus != ClampStatus.无夹具)
                    {
                        Tip.Alert(Current.Robot.Name + "上有夹具，不能取盘！");
                        return;
                    }

                    if (station.DoorStatus != DoorStatus.打开)
                    {
                        Tip.Alert(station.Name + "门未打开，不能取盘！");
                        return;
                    }

                    Current.Task.StartTime = DateTime.Now;
                    Current.Task.TaskId = -1;
                    Current.Task.NextFromStationId = station.Id;
                }
                else if (isPut)
                {

                    //if (Current.Task.Status == TaskStatus.就绪 || Current.Task.Status == TaskStatus.可取 || Current.Task.Status == TaskStatus.正取)
                    //{
                    //    Tip.Alert("取盘任务尚未完成！");
                    //    return;
                    //}

                    if (station.ClampStatus != ClampStatus.无夹具)
                    {
                        Tip.Alert("该位置有夹具，不能放盘！");
                        return;
                    }

                    if (Current.Task.FromStationId > 0 && station.ClampOri != Current.Task.FromStation.ClampOri)
                    {
                        Tip.Alert("夹具方向相反，不能放盘！");
                        return;
                    }

                    if (Current.Task.FromClampStatus == ClampStatus.满夹具 && station.GetPutType == GetPutType.上料机)
                    {
                        Tip.Alert(station.Name + "不允许放满夹具！");
                        return;
                    }

                    if (Current.Robot.ClampStatus == ClampStatus.无夹具)
                    {
                        Tip.Alert(Current.Robot.Name + "上无夹具，不能放盘！");
                        return;
                    }

                    if (station.DoorStatus != DoorStatus.打开)
                    {
                        Tip.Alert(station.Name + "门未打开，不能放盘！");
                        return;
                    }

                    if (Current.Task.StartTime == TengDa.Common.DefaultTime)
                    {
                        Current.Task.StartTime = DateTime.Now;
                    }

                    Current.Task.TaskId = -1;
                    Current.Task.NextToStationId = station.Id;

                    if (Current.Task.Status != TaskStatus.正取 && Current.Task.Status != TaskStatus.取完)
                    {
                        Current.Task.Status = TaskStatus.取完;
                    }

                    if (Current.Task.FromClampStatus != ClampStatus.空夹具 && Current.Task.FromClampStatus != ClampStatus.满夹具)
                    {
                        Current.Task.FromClampStatus = Current.Robot.ClampStatus;
                    }

                }

            }
            catch (Exception ex)
            {
                Error.Alert(ex);
            }

        }


        #endregion

        private void tsmWatContentTestOK_Click(object sender, EventArgs e)
        {
            if (Current.runStstus != RunStatus.运行)
            {
                Tip.Alert("请先启动！");
                return;
            }

            int i = TengDa._Convert.StrToInt(srcFloorName.Substring(8, 2), 0) - 1;
            int j = TengDa._Convert.StrToInt(srcFloorName.Substring(10, 2), 0) - 1;

            var floorName = Current.ovens[i].Floors[j].Name;

            DialogResult dr = MessageBox.Show("确定"+ floorName + "水分测试结果OK？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            if (dr == DialogResult.OK)
            {
                Current.ovens[i].Floors[j].Stations.ForEach(s =>
                {
                    if (s.ClampStatus == ClampStatus.满夹具)
                    {
                        s.SampleInfo = SampleInfo.无样品;
                        s.SampleStatus = SampleStatus.测试OK;
                        s.FloorStatus = FloorStatus.待出;
                    }
                    else
                    {
                        s.SampleStatus = SampleStatus.未知;
                    }
                });
                //var testStation = Current.ovens[i].Floors[j].Stations.FirstOrDefault(s => s.ClampStatus == ClampStatus.无夹具 && s.SampleStatus == SampleStatus.待测试);
                //if (testStation != null)
                //{
                //    testStation.GetFloor().Stations.ForEach(s => {
                //        if (s.ClampStatus == ClampStatus.满夹具)
                //        {
                //            s.SampleStatus = SampleStatus.测试OK;
                //            s.FloorStatus = FloorStatus.待出;
                //        }
                //        else
                //        {
                //            s.SampleStatus = SampleStatus.未知;
                //        }
                //    });
                //    //  testStation.SampleStatus = SampleStatus.未知;
                //    //  testStation.GetLabStation().SampleStatus = SampleStatus.测试OK;

                //    //var blankerStation = Station.StationList.FirstOrDefault(s => s.GetPutType == GetPutType.下料机 && s.SampleStatus == SampleStatus.待测试 && s.FromStationId == testStation.Id);
                //    //if (blankerStation != null)
                //    //{
                //    //    blankerStation.SampleStatus = SampleStatus.测试OK;
                //    //}
                //}
            }
        }

        private void tsmWatContentTestNG_Click(object sender, EventArgs e)
        {
            if (Current.runStstus != RunStatus.运行)
            {
                Tip.Alert("请先启动！");
                return;
            }

            int i = TengDa._Convert.StrToInt(srcFloorName.Substring(8, 2), 0) - 1;
            int j = TengDa._Convert.StrToInt(srcFloorName.Substring(10, 2), 0) - 1;

            var floorName = Current.ovens[i].Floors[j].Name;

            DialogResult dr = MessageBox.Show("确定" + floorName + "水分测试结果NG？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            if (dr == DialogResult.OK)
            {
                Current.ovens[i].Floors[j].Stations.ForEach(s =>
                {
                    if (s.ClampStatus == ClampStatus.满夹具)
                    {
                        s.SampleStatus = SampleStatus.测试NG;
                        s.FloorStatus = FloorStatus.待烤;
                    }
                });
                //var testStation = Current.ovens[i].Floors[j].Stations.FirstOrDefault(s => s.ClampStatus == ClampStatus.无夹具 && s.SampleStatus == SampleStatus.待测试);
                //if (testStation != null)
                //{
                //    testStation.GetFloor().Stations.ForEach(s => {
                //            if (s.ClampStatus == ClampStatus.满夹具)
                //            {
                //                s.SampleStatus = SampleStatus.测试NG;
                //                s.FloorStatus = FloorStatus.待烤;
                //            }
                //        });

                //    //var blankerStation = Station.StationList.FirstOrDefault(s => s.GetPutType == GetPutType.下料机 && s.SampleStatus == SampleStatus.待测试 && s.FromStationId == testStation.Id);
                //    //if (blankerStation != null)
                //    //{
                //    //    blankerStation.SampleStatus = SampleStatus.测试NG;
                //    //}
                //}
            }
        }

        private void btnDebug_Click(object sende, EventArgs e)
        {
            string msg = string.Empty;
            for (int i = 4600; i < 5000; i++)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("INSERT INTO [dbo].[Soundon.Dispatcher.Alarm] ([WordAdd], [BitAdd1], [BitAdd2], [AlarmStr], [FloorNum]) VALUES ");

                for (int j = 0; j < 16; j++)
                {
                    var wordAdd = string.Format("D{0:D4}.{1:D2}", i, j);
                    sb.Append(string.Format("('{0}', '{1}', '{2}', '{3}',  {4}),", wordAdd, "", "", "", -2));
                }
                Database.NonQuery(sb.ToString().TrimEnd(','), out msg);
                Thread.Sleep(10);
            }
        }

        private bool IsDisplayOvenCode = false;

        private void cbDisplayOvenCode_CheckedChanged(object sender, EventArgs e)
        {
            if (TengDa.WF.Current.IsRunning)
            {
                IsDisplayOvenCode = cbDisplayOvenCode.Checked;
            }
        }

    }
}
