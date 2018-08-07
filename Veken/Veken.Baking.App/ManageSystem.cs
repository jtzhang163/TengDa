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

namespace Veken.Baking.App
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
            LoginInit();
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
            if (string.IsNullOrEmpty(TengDa.WF.Current.user.Number))
            {
                lbUserName.Text = TengDa.WF.Current.user.Name;
            }
            else
            {
                lbUserName.Text = string.Format("{0}[{1}]", TengDa.WF.Current.user.Name, TengDa.WF.Current.user.Number);
            }
            lbUserGroupName.Text = TengDa.WF.Current.user.Group.Name;
        }

        private void LoginInit()
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

        #region 使用委托更新UI

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
            lbTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            for (int i = 0; i < Current.ovens.Count; i++)
            {
                Oven oven = Current.ovens[i];
                oven.IsAlive = oven.IsEnable && oven.Plcs[0].IsAlive;
                if (oven.Plcs[0].IsAlive)
                {
                    if (tbPlcStatus[i].Text.Trim() == "未连接")
                    {
                        tbPlcStatus[i].Text = "连接成功";
                    }

                    this.pbPlcLamp[i].Image = Properties.Resources.Green_Round;

                    if (!string.IsNullOrEmpty(oven.AlarmStr))
                    {
                        this.panelOven01[i].BackColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        this.panelOven01[i].BackColor = System.Drawing.Color.PaleGreen;
                    }

                    switch (oven.triLamp)
                    {
                        case TriLamp.Green: this.pbOvenLamp[i].Image = Properties.Resources.Green_Round; break;
                        case TriLamp.Yellow: this.pbOvenLamp[i].Image = Properties.Resources.Yellow_Round; break;
                        case TriLamp.Red: this.pbOvenLamp[i].Image = Properties.Resources.Red_Round; break;
                        case TriLamp.Unknown: this.pbOvenLamp[i].Image = Properties.Resources.Gray_Round; break;
                    }
                }
                else
                {
                    this.tbPlcStatus[i].Text = "未连接";
                    this.pbPlcLamp[i].Image = Properties.Resources.Red_Round;
                    this.panelOven01[i].BackColor = System.Drawing.Color.LightGray;
                }

                if (!string.IsNullOrEmpty(oven.AlarmStr) && oven.Plcs[0].IsAlive)
                {
                    if (Current.ovens[i].PreAlarmStr != oven.AlarmStr)
                    {
                        this.lbOvenAlarm01[i].Text = oven.AlarmStr.TrimEnd(',') + "...";
                    }
                    else
                    {
                        string alarmStr = this.lbOvenAlarm01[i].Text;
                        this.lbOvenAlarm01[i].Text = alarmStr.Substring(1, alarmStr.Length - 1) + alarmStr.Substring(0, 1);
                    }

                    this.lbOvenAlarm01[i].ForeColor = Color.White;
                }
                else
                {
                    this.lbOvenAlarm01[i].Text = "无报警...";
                    this.lbOvenAlarm01[i].ForeColor = Color.Red;
                }
                oven.PreAlarmStr = oven.AlarmStr;


                for (int j = 0; j < oven.Floors.Count; j++)
                {
                    Floor floor = oven.Floors[j];


                    this.lbFloorTem01[i][j].Text = floor.Temperatures[Current.option.DisplayTemperIndex].ToString("#0.0") + "℃";
                    this.lbFloorVacc01[i][j].Text = floor.Vacuum.ToString("#0") + "Pa";
                    this.lbFloorRunTime01[i][j].Text = string.Format("{0}/{1}", floor.Runmode == RunMode.自动 ? floor.RunMinutes : 0, floor.RunMinutesSet);
                    this.pbRunTime01[i][j].Maximum = floor.RunMinutesSet;
                    this.pbRunTime01[i][j].Value = floor.Runmode == RunMode.自动 ? (floor.RunMinutes > floor.RunMinutesSet ? floor.RunMinutesSet : floor.RunMinutes) : 0;

                    if (floor.Id == Current.option.CurveFloorId)
                    {
                        for (int k = 0; k < Option.TemperaturePointCount; k++)
                        {
                            lbTemperature[k].Text = floor.Temperatures[k].ToString("#0.0") + "℃";
                        }
                    }

                    if (!string.IsNullOrEmpty(floor.AlarmStr) && oven.Plcs[0].IsAlive)
                    {
                        if (floor.PreAlarmStr != floor.AlarmStr)
                        {
                            this.lbFloorAlarm01[i][j].Text = floor.AlarmStr.TrimEnd(',') + "...";
                        }
                        else
                        {
                            string alarmStr = this.lbFloorAlarm01[i][j].Text;
                            this.lbFloorAlarm01[i][j].Text = alarmStr.Substring(1, alarmStr.Length - 1) + alarmStr.Substring(0, 1);
                        }
                        this.lbFloorAlarm01[i][j].ForeColor = Color.White;
                    }
                    else
                    {
                        this.lbFloorAlarm01[i][j].Text = "无报警...";
                        this.lbFloorAlarm01[i][j].ForeColor = Color.Red;
                    }

                    floor.PreAlarmStr = floor.AlarmStr;

                    if (oven.Plcs[0].IsAlive)
                    {
                        if (!string.IsNullOrEmpty(floor.AlarmStr))
                        {
                            this.tlpFloor01[i][j].BackColor = System.Drawing.Color.Crimson;
                        }
                        else
                        {
                            switch (floor.floorStatus)
                            {
                                case FloorStatus.空腔: this.tlpFloor01[i][j].BackColor = System.Drawing.Color.White; break;
                                case FloorStatus.待烘烤: this.tlpFloor01[i][j].BackColor = System.Drawing.Color.Gold; break;
                                case FloorStatus.烘烤中: this.tlpFloor01[i][j].BackColor = System.Drawing.Color.Lime; break;
                                case FloorStatus.待出腔: this.tlpFloor01[i][j].BackColor = System.Drawing.Color.Violet; break;
                                case FloorStatus.未定义: this.tlpFloor01[i][j].BackColor = System.Drawing.SystemColors.Control; break;
                            }
                        }
                    }
                    else
                    {
                        this.tlpFloor01[i][j].BackColor = System.Drawing.SystemColors.Control;
                    }

                    this.lbClampBatteryCount01[i][j].Text = string.Format("料盒【{0}】电池【{1}】", floor.Clamps.Count, floor.Clamps.Sum(b => b.Batteries.Count));
                    this.lbFloorStatus01[i][j].Text = string.Format("当前状态：{0}", floor.floorStatus);

                }
            }


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
                this.pbMesLamp.Image = Properties.Resources.Red_Round;
            }

            if (!Current.scaner.IsEnable) Current.scaner.IsAlive = false;

            if (Current.scaner.IsAlive)
            {
                this.pbScanerLamp.Image = Properties.Resources.Green_Round;
            }
            else
            {
                this.pbScanerLamp.Image = Properties.Resources.Red_Round;
            }

            lbRunStatus.Text = Current.runStstus.ToString();
            switch (Current.runStstus)
            {
                case RunStatus.闲置: lbRunStatus.ForeColor = Color.Gray; break;
                case RunStatus.运行: lbRunStatus.ForeColor = Color.Lime; break;
                case RunStatus.暂停: lbRunStatus.ForeColor = Color.Yellow; break;
                case RunStatus.异常: lbRunStatus.ForeColor = Color.Red; break;
            }
            lbShowFeedingOK.Text = Yield.FeedingOK.ToString();
            lbShowBlankingOK.Text = Yield.BlankingOK.ToString();
        }

        #endregion

        #region 控件数组

        private const int OvenCount = 4;

        private static int[] OvenFloorCount = { 3, 3, 1, 1 };
        // private static int[] OvenFloorClampCount = { 2, 2, 2, 2 };

        private Panel[] panelOven01 = new Panel[OvenCount];
        private Label[] lbOvenName = new Label[OvenCount];
        private PictureBox[] pbOvenLamp = new PictureBox[OvenCount];
        private Label[] lbOvenNameN = new Label[OvenCount];
        private Label[] lbPLC = new Label[OvenCount];
        private CheckBox[] cbOvenIsEnable = new CheckBox[OvenCount];
        private PictureBox[] pbPlcLamp = new PictureBox[OvenCount];
        private TextBox[] tbPlcStatus = new TextBox[OvenCount];
        private Label[] lbOvenAlarm01 = new Label[OvenCount];

        private TableLayoutPanel[][] tlpFloor01 = new TableLayoutPanel[OvenCount][];
        private Label[][] lbFloorName01 = new Label[OvenCount][];
        private Label[][] lbFloorTem01 = new Label[OvenCount][];
        private Label[][] lbFloorVacc01 = new Label[OvenCount][];
        private RadioButton[][] rbFloorCurve01 = new RadioButton[OvenCount][];
        private Label[][] lbFloorRunTime01 = new Label[OvenCount][];
        private ProgressBar[][] pbRunTime01 = new ProgressBar[OvenCount][];
        private CheckBox[] cbTemperIndex = new CheckBox[Option.TemperaturePointCount];
        private Label[] lbTemperature = new Label[Option.TemperaturePointCount];

        private Label[][] lbClampBatteryCount01 = new Label[OvenCount][];
        private Label[][] lbFloorStatus01 = new Label[OvenCount][];

        private Label[][] lbFloorAlarm01 = new Label[OvenCount][];

        private void InitControls()
        {

            lbTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.Text = Current.option.AppName + Version.AssemblyVersion;
            this.cbInOvenCheck.Checked = Current.option.InOvenCheck;
            Current.runStstus = RunStatus.闲置;

            if (!Current.option.IsMesUserEnable)
            {
                lbToMesUserLoginTip.Visible = false;
                llToMesUserLogin.Visible = false;
            }
            for (int i = 0; i < Option.TemperaturePointCount; i++)
            {
                cbTemperIndex[i] = (CheckBox)(this.Controls.Find(string.Format("cbTemperIndex{0}", (i + 1).ToString("D2")), true)[0]);
                lbTemperature[i] = (Label)(this.Controls.Find(string.Format("lbTemperature{0}", (i + 1).ToString("D2")), true)[0]);
                cbTemperIndex[i].ForeColor = Current.option.CurveColors[i];
                lbTemperature[i].ForeColor = Current.option.CurveColors[i];
            }
            for (int i = 0; i < OvenCount; i++)
            {
                lbOvenName[i] = (Label)(this.Controls.Find(string.Format("lbOvenName{0}", (i + 1).ToString("D2")), true)[0]);
                pbOvenLamp[i] = (PictureBox)(this.Controls.Find(string.Format("pbOvenLamp{0}", (i + 1).ToString("D2")), true)[0]);
                lbOvenNameN[i] = (Label)(this.Controls.Find(string.Format("lbOvenNameN{0}", (i + 1).ToString("D2")), true)[0]);
                panelOven01[i] = (Panel)(this.Controls.Find(string.Format("panelOven01{0}", (i + 1).ToString("D2")), true)[0]);
                lbOvenAlarm01[i] = (Label)(this.Controls.Find(string.Format("lbOvenAlarm01{0}", (i + 1).ToString("D2")), true)[0]);

                lbPLC[i] = (Label)(this.Controls.Find(string.Format("lbPLC{0}", (i + 1).ToString("D2")), true)[0]);
                pbPlcLamp[i] = (PictureBox)(this.Controls.Find(string.Format("pbPlcLamp{0}", (i + 1).ToString("D2")), true)[0]);
                tbPlcStatus[i] = (TextBox)(this.Controls.Find(string.Format("tbPlcStatus{0}", (i + 1).ToString("D2")), true)[0]);
                cbOvenIsEnable[i] = (CheckBox)(this.Controls.Find(string.Format("cbOvenIsEnable{0}", (i + 1).ToString("D2")), true)[0]);

                tlpFloor01[i] = new TableLayoutPanel[OvenFloorCount[i]];
                lbFloorName01[i] = new Label[OvenFloorCount[i]];
                lbFloorTem01[i] = new Label[OvenFloorCount[i]];
                lbFloorVacc01[i] = new Label[OvenFloorCount[i]];
                rbFloorCurve01[i] = new RadioButton[OvenFloorCount[i]];
                lbFloorRunTime01[i] = new Label[OvenFloorCount[i]];
                pbRunTime01[i] = new ProgressBar[OvenFloorCount[i]];

                lbClampBatteryCount01[i] = new Label[OvenFloorCount[i]];
                lbFloorStatus01[i] = new Label[OvenFloorCount[i]];

                lbFloorAlarm01[i] = new Label[OvenFloorCount[i]];

                for (int j = 0; j < OvenFloorCount[i]; j++)
                {
                    tlpFloor01[i][j] = (TableLayoutPanel)(this.Controls.Find(string.Format("tlpFloor01{0}{1}", (i + 1).ToString("D2"), (j + 1).ToString("D2")), true)[0]);
                    lbFloorName01[i][j] = (Label)(this.Controls.Find(string.Format("lbFloorName01{0}{1}", (i + 1).ToString("D2"), (j + 1).ToString("D2")), true)[0]);
                    lbFloorTem01[i][j] = (Label)(this.Controls.Find(string.Format("lbFloorTem01{0}{1}", (i + 1).ToString("D2"), (j + 1).ToString("D2")), true)[0]);
                    lbFloorVacc01[i][j] = (Label)(this.Controls.Find(string.Format("lbFloorVacc01{0}{1}", (i + 1).ToString("D2"), (j + 1).ToString("D2")), true)[0]);
                    rbFloorCurve01[i][j] = (RadioButton)(this.Controls.Find(string.Format("rbFloorCurve01{0}{1}", (i + 1).ToString("D2"), (j + 1).ToString("D2")), true)[0]);
                    lbFloorRunTime01[i][j] = (Label)(this.Controls.Find(string.Format("lbFloorRunTime01{0}{1}", (i + 1).ToString("D2"), (j + 1).ToString("D2")), true)[0]);
                    pbRunTime01[i][j] = (ProgressBar)(this.Controls.Find(string.Format("pbRunTime01{0}{1}", (i + 1).ToString("D2"), (j + 1).ToString("D2")), true)[0]);
                    lbClampBatteryCount01[i][j] = (Label)(this.Controls.Find(string.Format("lbClampBatteryCount01{0}{1}", (i + 1).ToString("D2"), (j + 1).ToString("D2")), true)[0]);
                    lbFloorStatus01[i][j] = (Label)(this.Controls.Find(string.Format("lbFloorStatus01{0}{1}", (i + 1).ToString("D2"), (j + 1).ToString("D2")), true)[0]);
                    lbFloorAlarm01[i][j] = (Label)(this.Controls.Find(string.Format("lbFloorAlarm01{0}{1}", (i + 1).ToString("D2"), (j + 1).ToString("D2")), true)[0]);
                }
            }
        }

        private void InitSettingsTreeView()
        {
            tvSettings.Nodes.Clear();
            List<TreeNode> tnOvens = new List<TreeNode>();
            for (int i = 0; i < Current.ovens.Count; i++)
            {
                List<TreeNode> tnFloors = new List<TreeNode>();
                TreeNode tnOvenPlc = new TreeNode("PLC");
                tnFloors.Add(tnOvenPlc);
                for (int j = 0; j < Current.ovens[i].Floors.Count; j++)
                {
                    List<TreeNode> tnClamps = new List<TreeNode>();
                    for (int k = 0; k < Current.ovens[i].Floors[j].Clamps.Count; k++)
                    {
                        List<TreeNode> tnBatteries = new List<TreeNode>();
                        for (int n = 0; n < Current.ovens[i].Floors[j].Clamps[k].Batteries.Count; n++)
                        {
                            TreeNode tnBattery = new TreeNode((n + 1) + ".电池:" + Current.ovens[i].Floors[j].Clamps[k].Batteries[n].Code);
                            tnBatteries.Add(tnBattery);
                        }
                        TreeNode tnClamp = new TreeNode((k + 1) + ".料盒:" + Current.ovens[i].Floors[j].Clamps[k].Code, tnBatteries.ToArray());
                        tnClamps.Add(tnClamp);
                    }

                    TreeNode tnFloor = new TreeNode("腔体:" + Current.ovens[i].Floors[j].Name, tnClamps.ToArray());
                    tnFloors.Add(tnFloor);
                }

                TreeNode tnOven = new TreeNode(Current.ovens[i].Name, tnFloors.ToArray());
                tnOvens.Add(tnOven);
            }
            TreeNode tnTerminal = new TreeNode("烤箱", tnOvens.ToArray());
            TreeNode tnMES = new TreeNode("MES");
            TreeNode tnConfig = new TreeNode("配置");
            TreeNode tnScaner = new TreeNode("扫码枪");
            tvSettings.Nodes.AddRange(new TreeNode[] { tnConfig, tnMES, tnTerminal, tnScaner });
        }

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
                        Current.runStstus = RunStatus.闲置;
                        Tip.Alert("复位成功！");
                        Operation.Add("复位");
                        AddTips("复位");
                    }
                }
                else if (Current.runStstus == RunStatus.运行)
                {
                    Tip.Alert("请先暂停！");
                }
            }
            else if (pictureBox.Name.Contains("Exit"))
            {
                ManageSystem_FormClosing(null, null);//关闭窗体
            }
        }

        private void InitTerminal()
        {
            Current.ovens = Oven.OvenList;
            cbFloors.Items.Add("All");
            cbAlarmFloors.Items.Add("All");
            for (int i = 0; i < Current.ovens.Count; i++)
            {
                lbOvenNameN[i].Text = string.Format("{0}：", Current.ovens[i].Name);
                lbOvenName[i].Text = Current.ovens[i].Name;
                pbOvenLamp[i].Image = Properties.Resources.Gray_Round;
                lbPLC[i].Text = string.Format("{0}:{1}", Current.ovens[i].Plcs[0].IP, Current.ovens[i].Plcs[0].Port);
                cbOvenIsEnable[i].Checked = Current.ovens[i].IsEnable;
                cbAlarmFloors.Items.Add(Current.ovens[i].Name);
                ///查询温度真空时下拉列表数据            
                for (int j = 0; j < Current.ovens[i].Floors.Count; j++)
                {
                    lbFloorName01[i][j].Text = Current.ovens[i].Floors[j].Name;
                    cbFloors.Items.Add(Current.ovens[i].Floors[j].Name);
                    cbAlarmFloors.Items.Add(Current.ovens[i].Floors[j].Name);
                    rbFloorCurve01[i][j].Text = string.Format("{0}", Current.ovens[i].Floors[j].Name);
                    //cbIsCurveEnable01[i][j].ForeColor = Current.ovens[i].Floors[j].CurveColor;
                    if (Current.ovens[i].Floors[j].Id == Current.option.CurveFloorId)
                    {
                        rbFloorCurve01[i][j].Checked = true;
                    }
                }
                cbFloors.SelectedIndex = 0;
                cbAlarmFloors.SelectedIndex = 0;
            }

            if (Current.scaner.Id < 1)
            {
                string msg = string.Empty;
                Scaner scaner = Scaner.GetScaner(out msg);
                if (scaner == null)
                {
                    Error.Alert(msg);
                }
                Current.scaner = scaner;
            }


            lbScanerName.Text = string.Format("{0}：", Current.scaner.Name);
            lbScaner.Text = string.Format("{0}", Current.scaner.PortName);
            cbScanerIsEnable.Checked = Current.scaner.IsEnable;
        }

        private void InitMES()
        {
            Current.mes = new MES(1);
            cbMesIsEnable.Checked = Current.mes.IsEnable;
        }

        private bool PlcConnect()
        {
            string msg = string.Empty;
            for (int i = 0; i < Current.ovens.Count; i++)
            {
                if (Current.ovens[i].IsEnable)
                {
                    if (!Current.ovens[i].Plcs[0].IsPingSuccess)
                    {
                        Error.Alert("无法连接到PLC：" + Current.ovens[i].Plcs[0].IP);
                        return false;
                    }

                    if (!Current.ovens[i].Plcs[0].TcpConnect(out msg))
                    {
                        Error.Alert(msg);
                        return false;
                    }
                    int ii = i;
                    this.BeginInvoke(new MethodInvoker(() => { tbPlcStatus[ii].Text = "连接成功"; }));
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
            for (int i = 0; i < Current.ovens.Count; i++)
            {
                if (Current.ovens[i].IsEnable)
                {
                    if (!Current.ovens[i].Plcs[0].TcpDisConnect(out msg))
                    {
                        Error.Alert(msg);
                        return false;
                    }
                    tbPlcStatus[i].Text = "未连接";
                    this.pbOvenLamp[i].Image = Properties.Resources.Gray_Round;
                }

                Current.ovens[i].PreAlarmStr = string.Empty;
                for (int j = 0; j < Current.ovens[i].Floors.Count; j++)
                {
                    Current.ovens[i].Floors[j].PreAlarmStr = string.Empty;
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
                this.BeginInvoke(new MethodInvoker(() => { this.tbScanerStatus.Text = "未连接"; }));
            }

            return true;
        }

        private bool ScanerConnect()
        {

            if (Current.scaner.IsEnable)
            {
                string msg = string.Empty;
                if (!Current.scaner.Connect(out msg))
                {
                    Error.Alert(msg);
                    return false;
                }
                this.BeginInvoke(new MethodInvoker(() => { this.tbScanerStatus.Text = "连接成功"; }));
            }

            return true;
        }

        private bool ScanerDisConnect()
        {
            string msg = string.Empty;

            if (Current.scaner.IsEnable)
            {
                if (Current.scaner.IsAlive && !Current.scaner.DisConnect(out msg))
                {
                    Error.Alert(msg);
                    return false;
                }
                this.BeginInvoke(new MethodInvoker(() => { this.tbScanerStatus.Text = "未连接"; }));

            }

            return true;
        }

        #endregion

        #region 主运行逻辑

        delegate void UpdateUI1PDelegate(string text);
        //   delegate void UpdateUI2PDelegate(int i, string text);

        System.Timers.Timer[] timerRun = new System.Timers.Timer[OvenCount] { null, null, null, null };// = new System.Timers.Timer();

        private static bool timerlock = false;

        /// <summary>
        /// 启动前检测是否所有启用的设备均已连接成功
        /// </summary>
        /// <returns></returns>
        public bool CheckStart(out string msg)
        {
            for (int i = 0; i < Current.ovens.Count; i++)
            {
                if (cbOvenIsEnable[i].Checked && !Current.ovens[i].Plcs[0].IsAlive)
                {
                    msg = Current.ovens[i].Name + " 启动异常！";
                    return false;
                }
            }

            if (cbScanerIsEnable.Checked && !Current.scaner.IsAlive)
            {
                msg = Current.scaner.Name + " 启动异常！";
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
                    timerRun[i] = new System.Timers.Timer();
                    timerRun[i].Interval = TengDa._Convert.StrToInt(TengDa.WF.Option.GetOption("CheckPlcPeriod"), 1000);
                    timerRun[i].Elapsed += delegate
                    {
                        Thread listen = new Thread(new ParameterizedThreadStart(RunInvokeFunc));
                        listen.IsBackground = true;
                        listen.Start(index);
                    };
                    Thread.Sleep(500);
                    timerRun[i].Start();
                }

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

                this.timerCheckScaner = new System.Timers.Timer();
                this.timerCheckScaner.Interval = Current.option.CheckScanerInterval;
                this.timerCheckScaner.Elapsed += new System.Timers.ElapsedEventHandler(Timer_CheckScaner);
                this.timerCheckScaner.AutoReset = true;
                this.timerCheckScaner.Start();

                this.timerUploadMes = new System.Timers.Timer();
                this.timerUploadMes.Interval = TengDa._Convert.StrToInt(Current.option.UploadMesInterval, 600000);
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
                if (timerRun[i] != null)
                {
                    timerRun[i].Stop();
                    timerRun[i].Close();
                    timerRun[i].Dispose();
                }
            }

            if (timerPaintCurve != null)
            {
                timerPaintCurve.Stop();
                timerPaintCurve.Close();
                timerPaintCurve.Dispose();
            }

            if (timerCheckScaner != null)
            {
                timerCheckScaner.Stop();
                timerCheckScaner.Close();
                timerCheckScaner.Dispose();
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
            for (int i = 0; i < Current.ovens.Count; i++)
            {
                cbOvenIsEnable[i].Enabled = IsEnable;
            }
            cbScanerIsEnable.Enabled = IsEnable;
            cbMesIsEnable.Enabled = IsEnable;
        }

        private void RunInvokeFunc(object obj)
        {

            string msg = string.Empty;

            int i = System.Convert.ToInt32(obj);

            if (timerlock)
            {
                if (Current.ovens[i].IsEnable)
                {
                    this.BeginInvoke(new MethodInvoker(() => { tbPlcStatus[i].Text = "发送指令到PLC"; }));
                    if (Current.ovens[i].GetInfo())
                    {
                        this.BeginInvoke(new MethodInvoker(() => { tbPlcStatus[i].Text = "成功获得腔体信息"; }));
                    }
                    else
                    {
                        this.BeginInvoke(new MethodInvoker(() => { tbPlcStatus[i].Text = "获得腔体信息失败"; }));
                    }
                }
            }

            if (Current.ovens[i].AlreadyGetAllInfo)
            {
                for (int j = 0; j < Current.ovens[i].Floors.Count; j++)
                {
                    Floor floor = Current.ovens[i].Floors[j];
                    if (floor.IsBaking)
                    {
                        floor.floorstatus = FloorStatus.烘烤中;
                    }
                    else if (/*(floor.PreFloorStatus == FloorStatus.待出腔 || floor.PreFloorStatus == FloorStatus.空腔) &&*/ floor.Clamps.Count == 0 && !floor.IsBaking)
                    {
                        floor.floorstatus = FloorStatus.空腔;
                    }
                    else if ((floor.PreFloorStatus == FloorStatus.空腔 || floor.PreFloorStatus == FloorStatus.待烘烤) && floor.Clamps.Count > 0 && !floor.IsBaking)
                    {
                        floor.floorstatus = FloorStatus.待烘烤;
                    }
                    else if ((floor.PreFloorStatus == FloorStatus.烘烤中 || floor.PreFloorStatus == FloorStatus.待出腔) && floor.Clamps.Count > 0 && !floor.IsBaking)
                    {
                        floor.floorstatus = FloorStatus.待出腔;
                    }
                    else
                    {
                        floor.floorstatus = FloorStatus.未定义;
                    }

                    if (floor.PreFloorStatus == FloorStatus.待烘烤 && floor.floorStatus == FloorStatus.烘烤中)
                    {
                        for (int k = 0; k < floor.Clamps.Count; k++)
                        {
                            if (floor.Clamps[k].Id > 0)
                            {
                                floor.Clamps[k].BakingStartTime = DateTime.Now;
                                Clamp.Update(floor.Clamps[k], out msg);
                            }
                        }

                        if (Current.mes.IsAlive)
                        {
                            Thread t = new Thread(new ParameterizedThreadStart(UploadBatteriesInfo));//入炉上传MES，20171207新需求
                            t.Start(floor.Clamps);
                        }
                    }

                    if (floor.PreFloorStatus == FloorStatus.烘烤中 && floor.floorStatus == FloorStatus.待出腔)
                    {
                        Database.NonQuery(string.Format("UPDATE [dbo].[{0}] SET [BakingStopTime] = GETDATE(),[Vacuum]= {1},[Temperature] = {2} WHERE [Id] IN ({3});",
                            Clamp.TableName,
                            floor.Vacuum,
                            floor.Temperatures[Current.option.DisplayTemperIndex],
                            Current.ovens[i].Floors[j].ClampIds), out msg);

                        foreach (Clamp c in Current.ovens[i].Floors[j].Clamps)
                        {
                            c.BakingStopTime = DateTime.Now;
                            c.Vacuum = floor.Vacuum;
                            c.Temperature = floor.Temperatures[Current.option.DisplayTemperIndex];
                        }
                    }

                    if (floor.PreFloorStatus != floor.floorStatus)
                    {
                        floor.floorStatus = floor.floorStatus;
                        string tip = string.Format("{0}:{1}-->{2}", floor.Name, floor.PreFloorStatus, floor.floorStatus);
                        AddTips(tip);
                        Operation.Add(tip);
                    }

                    floor.PreFloorStatus = floor.floorStatus;
                }
            }

        }

        public void RefreshMesStatus(string text)
        {
            this.tbMesStatus.Text = text;
        }

        #endregion

        #region 定时将温度真空存入数据库
        System.Timers.Timer timerRecordTV = null;

        private void Timer_RecordTV(object sender, ElapsedEventArgs e)
        {
            if (timerlock)
            {
                List<TVD> TVDs = new List<TVD>();

                for (int i = 0; i < Current.ovens.Count; i++)
                {
                    if (Current.ovens[i].IsEnable && Current.ovens[i].Plcs[0].IsAlive)
                    {
                        for (int j = 0; j < Current.ovens[i].Floors.Count; j++)
                        {

                            TVD tvd = new TVD();
                            tvd.FloorId = Current.ovens[i].Floors[j].Id;
                            tvd.UserId = TengDa.WF.Current.user.Id;
                            tvd.RunMinutes = Current.ovens[i].Floors[j].RunMinutes;
                            tvd.T = Current.ovens[i].Floors[j].Temperatures;
                            tvd.V1 = Current.ovens[i].Floors[j].Vacuum;
                            TVDs.Add(tvd);

                        }
                    }
                }

                string msg = string.Empty;
                if (!TVD.Add(TVDs, out msg))
                {
                    Error.Alert(msg);
                }
            }
        }
        #endregion

        #region 定时将数据上传MES(设备信息和未上传成功的电芯信息)
        System.Timers.Timer timerUploadMes = null;

        private void Timer_UploadMes(object sender, ElapsedEventArgs e)
        {
            string msg = string.Empty;
            if (timerlock)
            {
                List<MachineInfo> list = new List<MachineInfo>();
                for (int i = 0; i < Current.ovens.Count; i++)
                {
                    for (int j = 0; j < Current.ovens[i].Floors.Count; j++)
                    {
                        MachineInfo info = new MachineInfo();
                        info.FloorId = Current.ovens[i].Floors[j].Id;
                        info.ActivationRate = string.Empty;
                        info.FinalProductsRate = string.Empty;
                        if (Current.ovens[i].IsAlive && Current.ovens[i].Floors[j].IsBaking)
                        {
                            info.machineStatus = MachineStatus.A;
                        }
                        else if (Current.ovens[i].IsAlive && !Current.ovens[i].Floors[j].IsBaking)
                        {
                            info.machineStatus = MachineStatus.B;
                        }
                        else
                        {
                            info.machineStatus = MachineStatus.D;
                        }

                        info.UtilizationRate = string.Empty;
                        info.FailureRate = string.Empty;
                        info.ErrorCode = string.Empty;
                        info.ErrorDescription = string.Empty;
                        list.Add(info);
                    }
                }
                MachineInfo.Add(list, out msg);
            }

            if (timerlock && Current.mes.IsAlive)
            {
                List<MachineInfo> list = MachineInfo.SelectNotUploaded();
                this.BeginInvoke(new UpdateUI1PDelegate(RefreshMesStatus), "开始上传烤箱状态");
                for (int i = 0; i < list.Count; i++)
                {
                    if (MES.UploadMachineInfo(list[i]))
                    {
                        this.BeginInvoke(new UpdateUI1PDelegate(RefreshMesStatus), "烤箱状态上传完成,Id:" + list[i].Id);
                        AddTips("烤箱状态上传MES完成,Id:" + list[i].Id);
                        MachineInfo.UploadFinished(list[i].Id, out msg);
                    }
                    else
                    {
                        this.BeginInvoke(new UpdateUI1PDelegate(RefreshMesStatus), "烤箱状态上传失败,Id:" + list[i].Id);
                        AddTips("烤箱状态上传MES失败,Id:" + list[i].Id);
                        LogHelper.WriteError("烤箱状态上传MES失败,Id:" + list[i].Id);
                    }
                }
                UploadBatteriesInfo(new List<Clamp>());
            }
        }

        public void UploadBatteriesInfo(object obj)
        {
            Thread.Sleep(200);
            List<Clamp> clamps = (List<Clamp>)obj;
            bool isOut = false;
            string msg = string.Empty;
            if (clamps.Count < 1)
            {
                clamps = Clamp.GetList(string.Format("SELECT * FROM [dbo].[{0}] WHERE IsUploaded = 'false' AND IsFinished = 'true'", Clamp.TableName), out msg);
                if (!string.IsNullOrEmpty(msg))
                {
                    Error.Alert(msg);
                    return;
                }
                isOut = true;
            }

            if (clamps.Count < 1)
            {
                return;
            }
            //上传数据

            foreach (Clamp clamp in clamps)
            {
                Floor floor = (from f in Floor.FloorList where f.Id == clamp.FloorId select f).ToList()[0];

                this.BeginInvoke(new UpdateUI1PDelegate(RefreshMesStatus), "开始上传" + clamp.Code);
                AddTips("开始上传MES:" + clamp.Code);


                if (MES.UploadCellInfo(clamp))
                {
                    this.BeginInvoke(new UpdateUI1PDelegate(RefreshMesStatus), "上传完成" + clamp.Code);
                    AddTips("上传MES完成:" + clamp.Code);

                    if (isOut)//如果是出炉，才更新IsUploaded，入炉不需要更新
                    {
                        clamp.IsUploaded = true;
                        if (!Clamp.Update(clamp, out msg))
                        {
                            Error.Alert(msg);
                        }
                    }
                }
                else
                {
                    this.BeginInvoke(new UpdateUI1PDelegate(RefreshMesStatus), "上传失败" + clamp.Code);
                    AddTips("上传MES失败:" + clamp.Code);
                }
                Thread.Sleep(200);
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

            if (!MES.GetUserName(userNumber, out userName, out msg))
            {
                Error.Alert(msg);
                return;
            }

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

            DataTable dt = Database.Query(string.Format("SELECT * FROM [dbo].[{0}.V_Battery] WHERE [入腔时间] BETWEEN '{1}' AND '{2}'", Config.DbTableNamePre, dtPickerStart.Value, dtPickerStop.Value), out msg);
            dgViewBattery.DataSource = dt;
            dgViewBattery.Columns[4].DefaultCellStyle.Format = "yyyy-MM-dd  HH:mm:ss";
            dgViewBattery.Columns[5].DefaultCellStyle.Format = "yyyy-MM-dd  HH:mm:ss";
            dgViewBattery.Columns[6].DefaultCellStyle.Format = "yyyy-MM-dd  HH:mm:ss";
            dgViewBattery.Columns[7].DefaultCellStyle.Format = "yyyy-MM-dd  HH:mm:ss";

            //设置显示列宽度
            dgViewBattery.Columns[1].Width = 80;
            dgViewBattery.Columns[2].Width = 100;
            dgViewBattery.Columns[3].Width = 90;

            dgViewBattery.Columns[4].Width = 130;
            dgViewBattery.Columns[5].Width = 130;
            dgViewBattery.Columns[6].Width = 130;
            dgViewBattery.Columns[7].Width = 130;
            dgViewBattery.Columns[8].Width = 100;

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
            Yield.FeedingOK = 0;
            Yield.FeedingNG = 0;
            Yield.BlankingOK = 0;
            Yield.BlankingNG = 0;
            Operation.Add("清空产量");
            AddTips("清空产量");
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
            Current.scaner.IsEnable = cbScanerIsEnable.Checked;
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

        private void cbMesIsEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (!TengDa.WF.Current.IsRunning) return;
            Current.mes.IsEnable = cbMesIsEnable.Checked;
        }

        private void btnQueryTV_Click(object sender, EventArgs e)
        {
            string msg = string.Empty;

            TimeSpan ts = dtpStart.Value - dtpStop.Value;
            int timeSpan = TengDa._Convert.StrToInt(Current.option.QueryTVTimeSpan, -1);
            if (Math.Abs(ts.Days) > timeSpan)
            {
                Tip.Alert("查询时间范围太大，请将时间范围设置在 " + timeSpan + " 天 之内！");
                return;
            }

            DataTable dt = null;
            if (cbFloors.Text.Trim() == "All")
            {
                dt = Database.Query(string.Format("SELECT * FROM [dbo].[{0}.V_TV] WHERE [记录时间] BETWEEN '{1}' AND '{2}' ", Config.DbTableNamePre, dtpStart.Value, dtpStop.Value), out msg);
            }
            else
            {
                dt = Database.Query(string.Format("SELECT * FROM [dbo].[{0}.V_TV] WHERE [腔体] = '{1}' AND [记录时间] BETWEEN '{2}' AND '{3}' ", Config.DbTableNamePre, cbFloors.Text.Trim(), dtpStart.Value, dtpStop.Value), out msg);
            }

            dgvTV.DataSource = dt;
            dgvTV.Columns[12].DefaultCellStyle.Format = "yyyy-MM-dd  HH:mm:ss";

            //设置显示列宽度
            dgvTV.Columns[0].Width = 52;
            dgvTV.Columns[1].Width = 60;
            dgvTV.Columns[2].Width = 60;
            dgvTV.Columns[3].Width = 60;
            dgvTV.Columns[4].Width = 60;
            dgvTV.Columns[5].Width = 60;
            dgvTV.Columns[6].Width = 60;
            dgvTV.Columns[7].Width = 60;
            dgvTV.Columns[8].Width = 60;
            dgvTV.Columns[9].Width = 65;
            dgvTV.Columns[10].Width = 52;
            dgvTV.Columns[11].Width = 80;
            dgvTV.Columns[12].Width = 130;
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
                dt = Database.Query(string.Format("SELECT * FROM [dbo].[{0}.V_Alarm] WHERE [开始时间] BETWEEN '{1}' AND '{2}' ", Config.DbTableNamePre, dtpAlarmStart.Value, dtpAlarmStop.Value), out msg);
            }
            else
            {
                dt = Database.Query(string.Format("SELECT * FROM [dbo].[{0}.V_Alarm] WHERE [名称] = '{1}' AND [开始时间] BETWEEN '{2}' AND '{3}' ", Config.DbTableNamePre, cbAlarmFloors.Text.Trim(), dtpAlarmStart.Value, dtpAlarmStop.Value), out msg);
            }

            dgvAlarm.DataSource = dt;
            dgvAlarm.Columns[2].DefaultCellStyle.Format = "yyyy-MM-dd  HH:mm:ss";
            dgvAlarm.Columns[3].DefaultCellStyle.Format = "yyyy-MM-dd  HH:mm:ss";

            //设置显示列宽度
            dgvAlarm.Columns[0].Width = 100;
            dgvAlarm.Columns[0].HeaderText = "烤箱/腔体";
            dgvAlarm.Columns[1].Width = 150;
            dgvAlarm.Columns[2].Width = 150;
            dgvAlarm.Columns[3].Width = 150;
            dgvAlarm.Columns[4].Width = 150;
            dgvAlarm.Columns[4].HeaderText = "持续时间(s)";
            dgvAlarm.Columns[5].Width = 150;
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
                    if (e.Node.Level == 1 && e.Node.Parent.Text == "烤箱")
                    {
                        if (TengDa.WF.Current.user.Group.Level < 2)
                        {
                            Tip.Alert("当前用户权限不足！");
                            this.propertyGridSettings.Enabled = false;
                        }
                        this.propertyGridSettings.SelectedObject = (from o in Current.ovens where o.Name == e.Node.Text select o).ToList()[0];
                    }
                    else if (e.Node.Level == 2 && e.Node.Text == "PLC")
                    {
                        Oven oven = (from o in Current.ovens where o.Name == e.Node.Parent.Text select o).ToList()[0];
                        this.propertyGridSettings.SelectedObject = oven.Plcs[0];
                    }
                    else if (e.Node.Level == 2 && e.Node.Text.IndexOf("腔体") > -1)
                    {
                        Oven oven = (from o in Current.ovens where o.Name == e.Node.Parent.Text select o).ToList()[0];
                        Floor floor = (from f in oven.Floors where e.Node.Text.IndexOf(f.Name) > -1 select f).ToList()[0];
                        this.propertyGridSettings.SelectedObject = floor;
                    }
                    else if (e.Node.Level == 3 && e.Node.Text.IndexOf("料盒") > -1)
                    {
                        Oven oven = (from o in Current.ovens where o.Name == e.Node.Parent.Parent.Text select o).ToList()[0];
                        Floor floor = (from f in oven.Floors where e.Node.Parent.Text.IndexOf(f.Name) > -1 select f).ToList()[0];
                        int clampIndex = _Convert.StrToInt(e.Node.Text.Split('.')[0], 1) - 1;
                        Clamp clamp = floor.Clamps[clampIndex];
                        this.propertyGridSettings.SelectedObject = clamp;
                    }
                    else if (e.Node.Level == 4 && e.Node.Text.IndexOf("电池") > -1)
                    {
                        Oven oven = (from o in Current.ovens where o.Name == e.Node.Parent.Parent.Parent.Text select o).ToList()[0];
                        Floor floor = (from f in oven.Floors where e.Node.Parent.Parent.Text.IndexOf(f.Name) > -1 select f).ToList()[0];
                        int clampIndex = _Convert.StrToInt(e.Node.Parent.Text.Split('.')[0], 1) - 1;
                        Clamp clamp = floor.Clamps[clampIndex];
                        int batteryIndex = _Convert.StrToInt(e.Node.Text.Split('.')[0], 1) - 1;
                        Battery battery = clamp.Batteries[batteryIndex];
                        this.propertyGridSettings.SelectedObject = battery;
                    }
                    else if (e.Node.Text == "MES")
                    {
                        if (TengDa.WF.Current.user.Group.Level < 2)
                        {
                            Tip.Alert("当前用户权限不足！");
                            this.propertyGridSettings.Enabled = false;
                        }
                        this.propertyGridSettings.SelectedObject = Current.mes;
                    }
                    else if (e.Node.Text == "配置")
                    {
                        if (TengDa.WF.Current.user.Group.Level < 3)
                        {
                            Tip.Alert("当前用户权限不足！");
                            this.propertyGridSettings.Enabled = false;
                        }
                        this.propertyGridSettings.SelectedObject = Current.option;
                    }
                    else if (e.Node.Text == "扫码枪")
                    {
                        if (TengDa.WF.Current.user.Group.Level < 2)
                        {
                            Tip.Alert("当前用户权限不足！");
                            this.propertyGridSettings.Enabled = false;
                        }
                        this.propertyGridSettings.SelectedObject = Current.scaner;
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

            if (type == typeof(Oven) || type == typeof(Floor) || type == typeof(Clamp) || type == typeof(Battery) || type == typeof(Scaner) || type == typeof(PLC))
            {
                System.Reflection.PropertyInfo propertyInfoId = type.GetProperty("Id"); //获取指定名称的属性
                int Id = (int)propertyInfoId.GetValue(o, null); //获取属性值
                settingsStr = string.Format("将Id为 {0} 的 {1} 的 {2} 由 {3} 修改为 {4} ", Id, type.Name, e.ChangedItem.PropertyDescriptor.DisplayName, e.OldValue, e.ChangedItem.Value);

            }
            else if (type == typeof(MES))
            {
                settingsStr = string.Format("将MES的 {0} 由 {1} 修改为 {2} ", e.ChangedItem.PropertyDescriptor.DisplayName, e.OldValue, e.ChangedItem.Value);
            }
            else if (type == typeof(Option))
            {
                settingsStr = string.Format("将配置项的 {0} 由 {1} 修改为 {2} ", e.ChangedItem.PropertyDescriptor.DisplayName, e.OldValue, e.ChangedItem.Value);
            }

            if (!string.IsNullOrEmpty(settingsStr))
            {
                Operation.Add(settingsStr);
                Tip.Alert("成功" + settingsStr);
            }
        }

        #endregion

        #region 绘制温度曲线

        private int gridOffset = 0;//网格偏移
        private const int gridSize = 20;//网格大小
        private Pen gridColor = new Pen(Color.FromArgb(0x33, 0x33, 0x33));//网格颜色

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

            #region 绘制曲线1

            for (int i = 0; i < Current.ovens.Count; i++)
            {
                if (Current.ovens[i].Plcs[0].IsAlive)
                {
                    for (int j = 0; j < Current.ovens[i].Floors.Count; j++)
                    {
                        if (Current.ovens[i].Floors[j].Id == Current.option.CurveFloorId)
                        {
                            for (int k = 0; k < Option.TemperaturePointCount; k++)
                            {
                                if (Array.IndexOf(Current.ovens[i].Floors[j].CurveIndexs.Split(','), k.ToString()) > -1)
                                {
                                    if (Current.ovens[i].Floors[j].sampledDatas[k].Count <= 1) return; // 一个数据就不绘制了
                                    float A = Current.ovens[i].Floors[j].sampledDatas[k][0];
                                    for (int kk = 1; kk < Current.ovens[i].Floors[j].sampledDatas[k].Count; kk++)
                                    {
                                        float B = Current.ovens[i].Floors[j].sampledDatas[k][kk];
                                        e.Graphics.DrawLine(new Pen(Current.option.CurveColors[k]),
                                            new Point(pCurve.ClientSize.Width - Current.ovens[i].Floors[j].sampledDatas[k].Count + kk - 1, pCurve.ClientSize.Height -
                                                (int)(((double)A / 130) * pCurve.ClientSize.Height)),
                                            new Point(pCurve.ClientSize.Width - Current.ovens[i].Floors[j].sampledDatas[k].Count + kk, pCurve.ClientSize.Height -
                                                (int)(((double)B / 130) * pCurve.ClientSize.Height)));
                                        A = B;
                                    }
                                }
                            }
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
                    if (Current.ovens[i].Plcs[0].IsAlive)
                    {
                        for (int j = 0; j < Current.ovens[i].Floors.Count; j++)
                        {
                            for (int k = 0; k < Option.TemperaturePointCount; k++)
                            {
                                while (Current.ovens[i].Floors[j].sampledDatas[k].Count > 1000)
                                    Current.ovens[i].Floors[j].sampledDatas[k].RemoveAt(0);
                                Current.ovens[i].Floors[j].sampledDatas[k].Add(Current.ovens[i].Floors[j].Temperatures[k]);
                            }
                        }
                    }
                }

                gridOffset = (gridOffset + 1) % gridSize;
                pCurve.Invalidate();
            }
        }

        private void rbFloorCurve01_CheckedChanged(object sender, EventArgs e)
        {
            if (!(sender as RadioButton).Checked)
            {
                return;
            }
            for (int i = 0; i < Current.ovens.Count; i++)
            {
                for (int j = 0; j < Current.ovens[i].Floors.Count; j++)
                {
                    if (rbFloorCurve01[i][j] == sender as RadioButton)
                    {
                        Current.option.CurveFloorId = Current.ovens[i].Floors[j].Id;

                        for (int k = 0; k < Option.TemperaturePointCount; k++)
                        {
                            cbTemperIndex[k].CheckedChanged -= this.cbTemperIndex_CheckedChanged;
                            if (Array.IndexOf(Current.ovens[i].Floors[j].CurveIndexs.Split(','), k.ToString()) > -1)
                            {
                                cbTemperIndex[k].Checked = true;
                            }
                            else
                            {
                                cbTemperIndex[k].Checked = false;
                            }
                            cbTemperIndex[k].CheckedChanged += this.cbTemperIndex_CheckedChanged;
                        }
                    }
                }
            }
        }

        private void cbTemperIndex_CheckedChanged(object sender, EventArgs e)
        {
            if (!TengDa.WF.Current.IsRunning) return;
            for (int i = 0; i < Current.ovens.Count; i++)
            {
                for (int j = 0; j < Current.ovens[i].Floors.Count; j++)
                {
                    if (Current.ovens[i].Floors[j].Id == Current.option.CurveFloorId)
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int k = 0; k < Option.TemperaturePointCount; k++)
                        {
                            if (cbTemperIndex[k].Checked)
                            {
                                sb.Append(k + ",");
                            }
                        }
                        Current.ovens[i].Floors[j].CurveIndexs = sb.ToString().TrimEnd(',');
                    }
                }
            }
        }

        #endregion

        #region 检测扫码枪
        System.Timers.Timer timerCheckScaner = null;
        private void Timer_CheckScaner(object sender, ElapsedEventArgs e)
        {
            if (timerlock)
            {
                if (Current.scaner.IsAlive && Current.scaner.IsGetNewData && !string.IsNullOrEmpty(Current.scaner.ReceiveString))
                {
                    this.BeginInvoke(new MethodInvoker(() => { this.tbScanerStatus.Text = string.Format("获得条码：{0}", Current.scaner.ReceiveString); }));
                    AddTips(string.Format("扫码:{0}", Current.scaner.ReceiveString));

                    if (Regex.IsMatch(Current.scaner.ReceiveString, Current.option.FloorNumberRegexStr))
                    {

                        for (int i = 0; i < Current.ovens.Count; i++)
                        {
                            for (int j = 0; j < Current.ovens[i].Floors.Count; j++)
                            {
                                if (Current.ovens[i].Floors[j].Number == Current.scaner.ReceiveString)
                                {

                                    if (!Current.IsInOvenFormShow && (Current.ovens[i].Floors[j].floorStatus == FloorStatus.空腔 || Current.ovens[i].Floors[j].floorStatus == FloorStatus.待烘烤))
                                    {
                                        srcFloorName = tlpFloor01[i][j].Name;
                                        this.BeginInvoke(new MethodInvoker(() => { tsmInOven_Click(null, null); }));
                                        //线程不是UI主线程的话,操作窗体需要用委托调用的
                                    }
                                    else if (Current.IsInOvenFormShow && (Current.ovens[i].Floors[j].floorStatus == FloorStatus.空腔 || Current.ovens[i].Floors[j].floorStatus == FloorStatus.待烘烤))
                                    {
                                        this.BeginInvoke(new MethodInvoker(() => { inOvenForm.Close(); }));
                                    }
                                    else if (Current.ovens[i].Floors[j].floorStatus == FloorStatus.待出腔)
                                    {
                                        outOven(i, j);
                                    }
                                }
                            }
                        }
                    }
                    else if (Regex.IsMatch(Current.scaner.ReceiveString, Current.option.ClampCodeRegexStr))
                    {
                        if (Current.IsInOvenFormShow)
                        {
                            inOvenForm.BeginInvoke(new MethodInvoker(() => { inOvenForm.SetTbClampCode(Current.scaner.ReceiveString); }));
                        }
                    }
                    Current.scaner.IsGetNewData = false;
                }
            }
        }

        /// <summary>
        /// 右键源控件名称 如：tlpFloor010401
        /// </summary>
        private string srcFloorName = string.Empty;

        private void cmsFloor_Opening(object sender, CancelEventArgs e)
        {
            srcFloorName = (sender as ContextMenuStrip).SourceControl.Name;
        }

        private void tsmInOven_Click(object sender, EventArgs e)
        {
            if (Current.runStstus != RunStatus.运行)
            {
                Tip.Alert("请先启动！");
                return;
            }

            int i = TengDa._Convert.StrToInt(srcFloorName.Substring(10, 2), 0) - 1;
            int j = TengDa._Convert.StrToInt(srcFloorName.Substring(12, 2), 0) - 1;
            inOvenForm = new InOvenForm(i, j, this);
            inOvenForm.ShowDialog();

        }

        InOvenForm inOvenForm = null;

        private void tsmOutOven_Click(object sender, EventArgs e)
        {
            if (Current.runStstus != RunStatus.运行)
            {
                Tip.Alert("请先启动！");
                return;
            }

            int i = TengDa._Convert.StrToInt(srcFloorName.Substring(10, 2), 0) - 1;
            int j = TengDa._Convert.StrToInt(srcFloorName.Substring(12, 2), 0) - 1;
            if (Current.ovens[i].Floors[j].floorStatus == FloorStatus.待出腔)
            {
                outOven(i, j);
            }
        }

        private void outOven(int i, int j)
        {
            string msg = string.Empty;
            for (int k = 0; k < Current.ovens[i].Floors[j].Clamps.Count; k++)
            {
                Yield.BlankingOK += Current.ovens[i].Floors[j].Clamps[k].Batteries.Count;
            }

            Database.NonQuery(string.Format("UPDATE [dbo].[{0}] SET [IsFinished] = 'TRUE', [OutTime] = GETDATE() WHERE [Id] IN ({1})", Clamp.TableName, Current.ovens[i].Floors[j].ClampIds), out msg);

            //清除该腔体绑定料盒/托盘数据
            Current.ovens[i].Floors[j].Clamps.Clear();
            Current.ovens[i].Floors[j].ClampIds = string.Empty;

            if (Current.mes.IsAlive)
            {
                Thread t = new Thread(new ParameterizedThreadStart(UploadBatteriesInfo));
                t.Start(new List<Clamp>());
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

        #region 手动修改腔体状态
        private void tsmiStatusSet_Click(object sender, EventArgs e)
        {
            if (TengDa.WF.Current.user.Id < 1)
            {
                Tip.Alert("请先登录");
                return;
            }
            ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
            FloorStatus fs = (FloorStatus)Enum.Parse(typeof(FloorStatus), tsmi.Text);
            int i = TengDa._Convert.StrToInt(srcFloorName.Substring(10, 2), 0) - 1;
            int j = TengDa._Convert.StrToInt(srcFloorName.Substring(12, 2), 0) - 1;

            if (Current.ovens[i].Floors[j].floorStatus != fs)
            {
                Current.ovens[i].Floors[j].PreFloorStatus = fs;
                Current.ovens[i].Floors[j].floorStatus = fs;
                string tip = string.Format("手动修改 {0} 状态为 {1}", Current.ovens[i].Floors[j].Name, fs);
                AddTips(tip);
                Operation.Add(tip);
            }
        }
        #endregion

        private void cbInOvenCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (!TengDa.WF.Current.IsRunning) return;
            Current.option.InOvenCheck = this.cbInOvenCheck.Checked;
            string flag = Current.option.InOvenCheck ? "N" : "Y";
            VekenDll.Common.Configuration.WriteString(VekenDll.Common.Constant.PATH_CONFIG, "special", flag);
        }
    }
}
