using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TengDa;
using System.Threading;
using TengDa.Encrypt;
using System.IO;
using System.Timers;
using TengDa.WF;

namespace Tafel.ScanSystem.App
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
            InitMESAbout();
            InitSettingsTreeView();
            Operation.Add("打开软件");
            Thread.Sleep(500);
            AddTips("打开软件", isUiThread: true);
            StartRefreshUI();
            BatteryAndClampIdInit();
            TengDa.WF.Current.IsRunning = true;
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
                    tbMesUserPassword.Text = TengDa.Encrypt.Base64.DecodeBase64(user.Password);
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

                //AlarmLog.StopAll();//正在进行的报警强制停止

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

            if (Current.feeder.Plcs[0].IsAlive)
            {
                if (tbPlcStatus.Text.Trim() == "未连接")
                {
                    tbPlcStatus.Text = "连接成功";
                }

                this.pbPlcLamp.Image = Properties.Resources.Green_Round;
            }
            else
            {
                this.tbPlcStatus.Text = "未连接";
                this.pbPlcLamp.Image = Properties.Resources.Red_Round;
            }

            if (!string.IsNullOrEmpty(Current.feeder.AlarmStr) && Current.feeder.Plcs[0].IsAlive)
            {
                if (Current.feeder.PreAlarmStr != Current.feeder.AlarmStr)
                {

                }
                else
                {

                }
            }
            else
            {

            }
            Current.feeder.PreAlarmStr = Current.feeder.AlarmStr;


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

            for (int i = 0; i < Current.feeder.Stations.Count; i++)
            {
                if (Current.feeder.Stations[i].IsEnable)
                {
                    tlpStations[i].BackColor = Color.PaleGreen;
                }
                else
                {
                    tlpStations[i].BackColor = Color.LightGray;
                }
            }
        }

        #endregion

        #region 控件数组

        private TextBox[,,] BatterySites = new TextBox[Current.ClampCount, Current.ClampRowCount, Current.ClampColCount];

        private TextBox[] tbClampCodes = new TextBox[Current.ClampCount];

        private Label[] lbStationNames = new Label[Current.ClampCount];

        private TableLayoutPanel[] tlpStations = new TableLayoutPanel[Current.ClampCount];

        private void InitControls()
        {

            lbTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.Text = Current.option.AppName + Version.AssemblyVersion;
            Current.runStstus = RunStatus.闲置;

            if (!Current.option.IsMesUserEnable)
            {
                lbToMesUserLoginTip.Visible = false;
                llToMesUserLogin.Visible = false;
            }

            DataTable dt = BatteryMatrix.GetDataTable();

            for (int i = 0; i < Current.ClampCount; i++)
            {
                for (int j = 0; j < Current.ClampRowCount; j++)
                {
                    for (int k = 0; k < Current.ClampColCount; k++)
                    {
                        BatterySites[i, j, k] = (TextBox)(this.Controls.Find(string.Format("bc{0}{1}{2}", (i + 1).ToString("D2"), (j + 1).ToString("D2"), (k + 1).ToString("D2")), true)[0]);
                        BatterySites[i, j, k].Text = dt.Rows[i * Current.ClampRowCount + j]["Col" + (k + 1)].ToString();//将数据库中的保存的电池矩阵显示到界面
                    }
                }

                lbStationNames[i] = (Label)(this.Controls.Find(string.Format("lbStationName{0}", (i + 1).ToString()), true)[0]);
                lbStationNames[i].Text = Current.feeder.Stations[i].Name;

                tbClampCodes[i] = (TextBox)(this.Controls.Find(string.Format("tbClampCode{0}", (i + 1).ToString()), true)[0]);
                tbClampCodes[i].Text = Current.option.TbClampCodes.Split(',')[i];

                tlpStations[i] = (TableLayoutPanel)(this.Controls.Find(string.Format("tlpStation{0}", (i + 1).ToString()), true)[0]);
            }
        }

        private void InitSettingsTreeView()
        {
            tvSettings.Nodes.Clear();
            List<TreeNode> tnFeederChildren = new List<TreeNode>();

            for (int j = 0; j < Current.feeder.Plcs.Count; j++)
            {
                TreeNode feederPlc = new TreeNode(Current.feeder.Plcs[j].Name);
                tnFeederChildren.Add(feederPlc);
            }

            for (int j = 0; j < Current.feeder.Stations.Count; j++)
            {
                Station station = Current.feeder.Stations[j];
                Clamp clamp = station.Clamp;

                List<TreeNode> tnStationClamps = new List<TreeNode>();
                if(clamp.Id > 0)
                {
                    List<TreeNode> tnClampBatteries = new List<TreeNode>();
                    for (int k = 0; k < clamp.Batteries.Count; k++)
                    {
                        tnClampBatteries.Add(new TreeNode((k + 1) + ".电芯：" + clamp.Batteries[k].Code));
                    }
                    tnStationClamps.Add(new TreeNode("夹具" + clamp.Code, tnClampBatteries.ToArray()));
                }

                TreeNode feederStation = new TreeNode((j + 1) + ".工位:" + station.Name, tnStationClamps.ToArray());
                tnFeederChildren.Add(feederStation);
            }

            TreeNode tnFeeder = new TreeNode("上料机", tnFeederChildren.ToArray());
            TreeNode tnMES = new TreeNode("MES");
            TreeNode tnConfig = new TreeNode("配置");
            TreeNode tnScaner = new TreeNode("扫码枪");
            tvSettings.Nodes.AddRange(new TreeNode[] { tnConfig, tnMES, tnFeeder, tnScaner });
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

        private void InitMESAbout()
        {
            string msg = string.Empty;

            string ip = MES.IPAddr.ToString();
            this.lbIPAddr.Text = ip;

            string lbProcessText = string.Empty;
            string lbStationText = string.Empty;
            MES.GetInfo(out lbProcessText, out lbStationText, out msg);
            this.lbProcess.Text = lbProcessText;
            this.lbStation.Text = lbStationText;

            this.tbOrderNo.Text = Current.option.CurrentOrderNo;
            if (!string.IsNullOrEmpty(this.tbOrderNo.Text))
            {
                this.tbOrderNo.ReadOnly = true;
                this.tbOrderNo.BackColor = SystemColors.Control;
            }
            this.tbMaterialOrderNo.Text = Current.option.CurrentMaterialOrderNo;
            if (!string.IsNullOrEmpty(this.tbMaterialOrderNo.Text))
            {
                this.tbMaterialOrderNo.ReadOnly = true;
                this.tbMaterialOrderNo.BackColor = SystemColors.Control;
            }

        }

        private void InitTerminal()
        {
            cbAlarmFloors.Items.Add("All");

            lbFeederName.Text = Current.feeder.Name + ":";
            this.pbPlcLamp.Image = Properties.Resources.Gray_Round;
            lbPLC.Text = string.Format("{0}:{1}", Current.feeder.Plcs[0].IP, Current.feeder.Plcs[0].Port);
            cbFeederIsEnable.Checked = Current.feeder.IsEnable;
            cbAlarmFloors.Items.Add(Current.feeder.Name);
            cbAlarmFloors.SelectedIndex = 0;
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

            if (Current.feeder.IsEnable)
            {
                if (!Current.feeder.Plcs[0].IsPingSuccess)
                {
                    Error.Alert("无法连接到PLC：" + Current.feeder.Plcs[0].IP);
                    return false;
                }

                if (!Current.feeder.Plcs[0].TcpConnect(out msg))
                {
                    Error.Alert(msg);
                    return false;
                }

                this.BeginInvoke(new MethodInvoker(() => { tbPlcStatus.Text = "连接成功"; }));
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

            if (Current.feeder.IsEnable)
            {
                if (!Current.feeder.Plcs[0].TcpDisConnect(out msg))
                {
                    Error.Alert(msg);
                    return false;
                }
                tbPlcStatus.Text = "未连接";
                this.pbPlcLamp.Image = Properties.Resources.Gray_Round;
            }

            Current.feeder.PreAlarmStr = string.Empty;
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
                }
            }
            return true;
        }

        private bool MesDisConnect()
        {
            string msg = string.Empty;

            if (Current.mes.IsEnable)
            {
                this.BeginInvoke(new MethodInvoker(()=> { this.tbScanerStatus.Text = "未连接"; }));
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
                this.BeginInvoke(new MethodInvoker(()=> { this.tbScanerStatus.Text = "未连接"; }));
                
            }

            return true;
        }

        #endregion

        #region 主运行逻辑

        private void RunInvokeFunc()
        {

            string msg = string.Empty;

            if (timerlock)
            {
                if (Current.feeder.IsEnable)
                {

                    int i = PLC.GetJawStationIndex();
                    if (i > -1 && i < Current.ClampCount && string.IsNullOrEmpty(this.tbClampCodes[i].Text.Trim()))
                    {
                        //timerlock = false;
                        //Current.runStstus = RunStatus.异常;
                        Tip.Alert(string.Format("请输入{0}夹具的条码！", Current.feeder.Stations[i].Name));
                        return;
                    }

                    this.BeginInvoke(new MethodInvoker(() => { tbPlcStatus.Text = "发送指令到PLC"; }));
                    if (Current.feeder.GetInfo())
                    {
                        Thread.Sleep(300);
                        if (Current.feeder.IsReady)
                        {
                            this.BeginInvoke(new UpdateUI1PDelegate(RefreshPlcStatus), "电池到位");
                            if (Current.scaner != null && Current.scaner.IsAlive)
                            {

                                if (!Current.feeder.PreIsReady)
                                {
                                    Current.feeder.PreIsReady = Current.feeder.IsReady;
                                    string code = string.Empty;

                                    this.BeginInvoke(new UpdateUI1PDelegate(RefreshScanerStatus), "触发扫码信号发送至扫码枪");
                                    ScanResult scanResult = Current.scaner.Scan(out code, out msg);

                                    switch (scanResult)
                                    {
                                        case ScanResult.OK:

                                            this.BeginInvoke(new UpdateUI1PDelegate(RefreshScanerStatus), "成功扫码：" + code);
                                            AddTips("成功扫码：" + code);

                                            Current.feeder.CurrentScanNgCount = 0;//NG计数清零

                                            if (!MES.CheckSfc(code, out msg))
                                            {
                                                timerlock = false;
                                                Current.runStstus = RunStatus.异常;

                                                Error.Alert(msg);
                                                if (Current.feeder.SetScanResult(ScanResult.NG))
                                                {
                                                    this.BeginInvoke(new UpdateUI1PDelegate(RefreshPlcStatus), code + " MES检测NG-->PLC完成");
                                                    AddTips(code + " MES检测NG-->PLC完成");
                                                }
                                                else
                                                {
                                                    this.BeginInvoke(new UpdateUI1PDelegate(RefreshPlcStatus), code + " MES检测NG-->PLC失败");
                                                    AddTips(code + " MES检测NG-->PLC失败");
                                                }
                                                break;
                                            }
                                            else
                                            {
                                                AddTips(code +" MES检测OK");
                                            }

                                            this.BeginInvoke(new UpdateUI1PDelegate(DisplayAndSaveBatteryCode), code);

                                            if (Current.feeder.SetScanResult(ScanResult.OK))
                                            {
                                                this.BeginInvoke(new UpdateUI1PDelegate(RefreshPlcStatus), "扫码OK-->PLC完成");
                                                AddTips("扫码OK-->PLC完成");
                                            }
                                            else
                                            {
                                                this.BeginInvoke(new UpdateUI1PDelegate(RefreshPlcStatus), "扫码OK-->PLC失败");
                                                AddTips("扫码OK-->PLC失败");
                                            }

                                            UploadBatteriesInfo(new List<Battery>());

                                            break;
                                        case ScanResult.NG:

                                            Current.feeder.CurrentScanNgCount++;
                                            this.BeginInvoke(new UpdateUI1PDelegate(RefreshScanerStatus), "扫码NG");
                                            AddTips("扫码NG,次数：" + Current.feeder.CurrentScanNgCount);

                                            if (Current.feeder.CurrentScanNgCount < Current.feeder.ScanNgCount)
                                            {
                                                break;
                                            }

                                            timerlock = false;
                                            Current.runStstus = RunStatus.异常;
                                            Yield.FeedingNG++;
                                            Tip.Alert("扫码NG");
                                            Operation.Add("扫码NG");

                                            if (Current.feeder.SetScanResult(ScanResult.NG))
                                            {
                                                this.BeginInvoke(new UpdateUI1PDelegate(RefreshPlcStatus), "扫码NG-->PLC完成");
                                                AddTips("扫码NG-->PLC完成");
                                            }
                                            else
                                            {
                                                this.BeginInvoke(new UpdateUI1PDelegate(RefreshPlcStatus), "扫码NG-->PLC失败");
                                                AddTips("扫码NG-->PLC失败");
                                            }

                                            break;
                                        case ScanResult.Error:
                                            Error.Alert(msg);
                                            this.BeginInvoke(new UpdateUI1PDelegate(RefreshScanerStatus), "读码失败");
                                            AddTips("读码失败");
                                            break;
                                        case ScanResult.Unknown:
                                            Tip.Alert(msg);
                                            this.BeginInvoke(new UpdateUI1PDelegate(RefreshScanerStatus), "读码出现未知错误");
                                            AddTips("读码出现未知错误");
                                            break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            this.BeginInvoke(new UpdateUI1PDelegate(RefreshPlcStatus), "电池未到位");
                           
                        }
                    }
                    else
                    {
                        this.BeginInvoke(new MethodInvoker(() => { tbPlcStatus.Text = "获得信息失败"; }));
                    }

                    Current.feeder.PreIsReady = Current.feeder.IsReady;
                }
            }
        }

        delegate void UpdateUI1PDelegate(string text);

        private static bool timerlock = false;

        public bool CheckStart(out string msg)
        {

            if (cbFeederIsEnable.Checked && !Current.feeder.Plcs[0].IsAlive)
            {
                msg = Current.feeder.Name + " 启动异常！";
                return false;
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
        System.Timers.Timer timerRun = null;
        public void StartRun()
        {

            if (isFirstStart)
            {

                timerRun = new System.Timers.Timer();
                timerRun.Interval = TengDa._Convert.StrToInt(TengDa.WF.Option.GetOption("CheckPlcPeriod"), 1000);
                timerRun.Elapsed += delegate
                {
                    Thread listen = new Thread(new ThreadStart(RunInvokeFunc));
                    listen.IsBackground = true;
                    listen.Start();
                };
                Thread.Sleep(500);
                timerRun.Start();

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

            if (timerRun != null)
            {
                timerRun.Stop();
                timerRun.Close();
                timerRun.Dispose();
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
            cbFeederIsEnable.Enabled = IsEnable;
            cbScanerIsEnable.Enabled = IsEnable;
            cbMesIsEnable.Enabled = IsEnable;
        }

        public void RefreshMesStatus(string text)
        {
            this.tbMesStatus.Text = text;
        }

        #endregion

        #region 定时将数据上传MES(设备信息和未上传成功的电芯信息)
        System.Timers.Timer timerUploadMes = null;

        private void Timer_UploadMes(object sender, ElapsedEventArgs e)
        {
            string msg = string.Empty;

            if (timerlock && Current.mes.IsAlive)
            {
                MES.UploadMachineInfo("S");
                UploadBatteriesInfo(new List<Battery>());
            }
        }

        public void UploadBatteriesInfo(object obj)
        {
            List<Battery> batteries = (List<Battery>)obj;
            string msg = string.Empty;
            if (batteries.Count < 1)
            {
                batteries = Battery.GetList(string.Format("SELECT * FROM [dbo].[{0}] WHERE IsUploaded = 'false' AND IsFinished = 'true'", Battery.TableName), out msg);
                if (!string.IsNullOrEmpty(msg))
                {
                    Error.Alert(msg);
                    return;
                }
            }

            if (batteries.Count < 1)
            {
                return;
            }
            //上传数据

            foreach (Battery battery in batteries)
            {

                this.BeginInvoke(new UpdateUI1PDelegate(RefreshMesStatus), battery.Code + "上传MES...");
                AddTips(battery.Code + "上传MES...");

                string trayCode = new Clamp(battery.ClampId).Code;

               // LogHelper.WriteInfo("battery.ClampId: " + battery.ClampId + "  trayCode: " + trayCode);

                if (MES.UploadBattery(battery.Code, trayCode))
                {
                    this.BeginInvoke(new UpdateUI1PDelegate(RefreshMesStatus), battery.Code + "上传完成.");
                    AddTips(battery.Code + "上传完成.");

                    battery.IsUploaded = true;
                    Battery.Update(battery, out msg);
                }
                else
                {
                    this.BeginInvoke(new UpdateUI1PDelegate(RefreshMesStatus), battery.Code + "上传失败.");
                    AddTips(battery.Code + "上传失败.");
                }
                Thread.Sleep(200);
            }
        }
        #endregion

        #region 用户登录、注销、注册、管理

        private void HideAllUserGroupControl()
        {
            gbLogin.Visible = false;
            gbMesLogin.Visible = false;
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
                    gbMesLogin.Visible = true;
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
            string password = tbMesUserPassword.Text.Trim();
            string userName = string.Empty;

            if (!MES.GetUserName(userNumber, password, out userName, out msg))
            {
                Error.Alert(msg);
                return;
            }

            User user = new User();
            user.Name = userName;
            user.Password = password;
            user.Number = userNumber;
            user.Group = new UserGroup("操作员");
            user.IsEnable = true;
            User.Add(user, out msg);

            if (User.MesLogin2(userNumber, password, out user, out msg))
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
            gbMesLogin.Visible = true;
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
            dgViewBattery.Columns[4].DefaultCellStyle.Format = "yyyy-MM-dd  HH:mm:ss";
            dgViewBattery.Columns[5].DefaultCellStyle.Format = "yyyy-MM-dd  HH:mm:ss";
            //dgViewBattery.Columns[6].DefaultCellStyle.Format = "yyyy-MM-dd  HH:mm:ss";
            //dgViewBattery.Columns[7].DefaultCellStyle.Format = "yyyy-MM-dd  HH:mm:ss";

            //设置显示列宽度
            dgViewBattery.Columns[1].Width = 80;
            dgViewBattery.Columns[2].Width = 100;
            dgViewBattery.Columns[3].Width = 90;

            dgViewBattery.Columns[4].Width = 130;
            dgViewBattery.Columns[5].Width = 130;
            dgViewBattery.Columns[6].Width = 130;
            //dgViewBattery.Columns[7].Width = 130;
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
            if ((CheckBox)sender == cbFeederIsEnable)
            {
                Current.feeder.IsEnable = cbFeederIsEnable.Checked;
                return;
            }

        }

        private void cbMesIsEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (!TengDa.WF.Current.IsRunning) return;
            Current.mes.IsEnable = cbMesIsEnable.Checked;
        }

        private void tabContent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabContent.SelectedTab.Text == "设置")
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
                    if (e.Node.Text == "上料机")
                    {
                        if (TengDa.WF.Current.user.Group.Level < 2)
                        {
                            Tip.Alert("当前用户权限不足！");
                            this.propertyGridSettings.Enabled = false;
                        }
                        this.propertyGridSettings.SelectedObject = Current.feeder;
                    }
                    else if (e.Node.Text.IndexOf("PLC") > -1)
                    {
                        this.propertyGridSettings.SelectedObject = Current.feeder.Plcs.First(p => p.Name == e.Node.Text);
                    }
                    else if (e.Node.Text.IndexOf("工位") > -1)
                    {
                        this.propertyGridSettings.SelectedObject = Current.feeder.Stations.First(s => e.Node.Text.IndexOf(s.Name) > -1);
                    }
                    else if (e.Node.Text.IndexOf("夹具") > -1)
                    {
                        this.propertyGridSettings.SelectedObject = Current.feeder.Stations.First(s => e.Node.Parent.Text.IndexOf(s.Name) > -1).Clamp;
                    }
                    else if (e.Node.Text.IndexOf("电芯") > -1)
                    {
                        int stationId = Current.feeder.Stations.First(s => e.Node.Parent.Parent.Text.IndexOf(s.Name) > -1).Id;
                        List<Battery> batteries = Current.feeder.Stations.First(s => e.Node.Parent.Parent.Text.IndexOf(s.Name) > -1).Clamp.Batteries;
                        this.propertyGridSettings.SelectedObject = batteries.First(b => e.Node.Text.IndexOf(b.Code) > -1);
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
                catch(Exception ex)
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


            if (type == typeof(Feeder) || type == typeof(Scaner) || type == typeof(PLC) || type == typeof(Battery) || type == typeof(Clamp)|| type == typeof(Station))
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

        private void tbOrderNo_DoubleClick(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            tb.ReadOnly = !tb.ReadOnly;
            if (tb.ReadOnly)
            {
                if (tb.Name == "tbOrderNo")
                {
                    Current.option.CurrentOrderNo = tb.Text.Trim();
                }
                else if (tb.Name == "tbMaterialOrderNo")
                {
                    Current.option.CurrentMaterialOrderNo = tb.Text.Trim();
                }

                tb.BackColor = SystemColors.Control;
            }
            else
            {
                tb.BackColor = SystemColors.Window;
            }
        }

        private void tbOrderNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)//回车
            {
                TextBox tb = sender as TextBox;
                if (tb.Name == "tbOrderNo")
                {
                    Current.option.CurrentOrderNo = tb.Text.Trim();
                }
                else if (tb.Name == "tbMaterialOrderNo")
                {
                    Current.option.CurrentMaterialOrderNo = tb.Text.Trim();
                }

                tb.ReadOnly = true;
                tb.BackColor = SystemColors.Control;
            }
        }

        private void tbClampCode_TextChanged(object sender, EventArgs e)
        {
            if (TengDa.WF.Current.IsRunning)
            {
                Current.option.TbClampCodes = this.tbClampCode1.Text.Trim() + "," + this.tbClampCode2.Text.Trim();
            }
        }

        private void bc_DoubleClick(object sender, EventArgs e)
        {
            string textBoxName = (sender as TextBox).Name;
            int i = _Convert.StrToInt(textBoxName.Substring(2, 2), -1) - 1;
            int j = _Convert.StrToInt(textBoxName.Substring(4, 2), -1) - 1;
            int k = _Convert.StrToInt(textBoxName.Substring(6, 2), -1) - 1;
            string siteName = string.Format("{0}第{1}行第{2}列", Current.feeder.Stations[i].Name, j + 1, k + 1);
            DialogResult dr = MessageBox.Show("确定要设置" + siteName + "为当前放电池位置？", "更改夹具入电池位置", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                Current.feeder.Plcs[0].SetJawLocation(i, j, k);
                Operation.Add("设置" + siteName + "为当前放电池位置");
            }
        }

        private void btnInit_Click(object sender, EventArgs e)
        {

            DialogResult dr = MessageBox.Show("确定要设置为初始状态？", "设置初始状态", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                ClearAllBatterySites();
                Current.feeder.Plcs[0].SetJawLocation(0, 0, 0);
                Operation.Add("设置为初始状态");
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
                this.BeginInvoke(new MethodInvoker(() => {
                    tbTips.AppendText(string.Format("[{0}]{1}\r\n", DateTime.Now.ToString("HH:mm:ss"), tip));
                    tbTips.Focus();
                    tbTips.Select(tbTips.TextLength, 0);
                    tbTips.ScrollToCaret();
                }));
            }

            LogHelper.WriteInfo(tip);
        }
        #endregion

        #region 其他逻辑
 
        public void DisplayAndSaveBatteryCode(string code)
        {
            string msg = string.Empty;
            int i, j, k;
            if (PLC.GetJawLocation(out i, out j, out k))
            {
                if (i < Current.ClampCount && j < Current.ClampRowCount && k < Current.ClampColCount)
                {

                    if ((j == 0 && k == 0) || Current.clampId < 1)
                    {

                        Clamp clamp = new Clamp(this.tbClampCodes[i].Text.Trim(), i + 1, (i + 1).ToString());

                        int clampId = Clamp.Add(clamp, out msg);
                        if (clampId > 0)
                        {
                            Current.clampId = clampId;
                            Current.feeder.Stations[i].ClampId = clampId;
                        }
                        else
                        {
                            Error.Alert(msg);
                        }
                    }

                    BatterySites[i, j, k].Text = code;
                    BatterySites[i, j, k].BackColor = Color.Blue;
                    BatterySites[i, j, k].ForeColor = Color.White;

                    //int ii = i, jj = j, kk = k;

                    Thread t = new Thread(new ParameterizedThreadStart(SetBatterySitesColor));
                    t.Start(string.Format("{0},{1},{2}", i, j, k));

                    BatteryMatrix.Update(i, j, k, code);
                    Battery battery = new Battery(code, Current.clampId, string.Format("{0},{1}", j + 1, k + 1));

                    
                    int batteryId = Battery.Add(battery, out msg);
                    if (batteryId > 0)
                    {
                        Current.batteryId = batteryId;
                    }
                    else
                    {
                        Error.Alert(msg);
                    }

                    //设置绑盘时间
                    if (j == Current.ClampRowCount - 1 && k == Current.ClampColCount - 1)
                    {
                        Clamp clamp = new Clamp(Current.clampId);
                        clamp.BindTime = DateTime.Now;
                        if (!Clamp.Update(clamp, out msg))
                        {
                            Error.Alert(msg);
                        }
                        else
                        {
                            Yield.BlankingOK += clamp.Batteries.Count;
                        }
                    }

                    SetNextJawLocation(i, j, k);

                    if (i == Current.ClampCount - 1 && j == Current.ClampRowCount - 1 && k == Current.ClampColCount - 1)
                    {
                        Thread.Sleep(1000);
                        ClearAllBatterySites();
                    }
                }
                else
                {
                    Error.Alert("数据库中JawLocation配置项数据超出范围！");
                }
            }
        }

        private void SetBatterySitesColor(Object obj)
        {
            string[] s = obj.ToString().Split(',');
            int i = TengDa._Convert.StrToInt(s[0], 0);
            int j = TengDa._Convert.StrToInt(s[1], 0);
            int k = TengDa._Convert.StrToInt(s[2], 0);

            Thread.Sleep(3000);

            BatterySites[i, j, k].BackColor = Color.White;
            BatterySites[i, j, k].ForeColor = Color.Blue;
        }

        public void SetNextJawLocation(int i, int j, int k)
        {
            int tmp = i * Current.ClampRowCount * Current.ClampColCount + j * Current.ClampColCount + k;
            tmp++;

            k = tmp % Current.ClampColCount;
            j = (tmp - k) / Current.ClampColCount % Current.ClampRowCount;
            i = (tmp - k - j * Current.ClampColCount) / (Current.ClampRowCount * Current.ClampColCount) % Current.ClampCount;
            Current.feeder.Plcs[0].SetJawLocation(i, j, k);
        }

        public void RefreshPlcStatus(string text)
        {
            tbPlcStatus.Text = text;
        }

        public void RefreshScanerStatus(string text)
        {
            this.tbScanerStatus.Text = text;
        }

        private void ClearAllBatterySites()
        {
            for (int i = 0; i < Current.ClampCount; i++)
            {
                for (int j = 0; j < Current.ClampRowCount; j++)
                {
                    for (int k = 0; k < Current.ClampColCount; k++)
                    {
                        BatterySites[i, j, k].Text = "";
                    }
                }
                this.tbClampCodes[i].Text = "";
                Current.feeder.Stations[i].ClampId = -1;
            }
            BatteryMatrix.DbClearAll();
        }

        private void BatteryAndClampIdInit()
        {
            Battery.GetLastId();
            Clamp.GetLastId();
        }
        #endregion

    }
}
