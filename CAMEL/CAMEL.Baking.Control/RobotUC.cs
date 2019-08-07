using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TengDa;
using TengDa.WF;

namespace CAMEL.Baking.Control
{
    public partial class RobotUC : UserControl
    {
        public RobotUC()
        {
            InitializeComponent();
        }

        public void Init(Robot robot)
        {
            this.lbName.Text = robot.Name;
            this.lbInfo.Text = "闲置";
            this.lbClampCode.Text = robot.Clamp.Code;
            this.lbClampCode.BackColor = robot.ClampStatus == ClampStatus.异常 ? Color.Red : Color.Transparent;
        }

        public void Update(Robot robot)
        {

            robot.IsAlive = robot.IsEnable && robot.Plc.IsAlive;

            if (robot.IsAlive)
            {
                switch (robot.ClampStatus)
                {
                    case ClampStatus.满夹具: this.panelRobot.BackColor = Color.LimeGreen; break;
                    case ClampStatus.空夹具: this.panelRobot.BackColor = Color.Cyan; break;
                    case ClampStatus.无夹具: this.panelRobot.BackColor = Color.White; break;
                    default: this.panelRobot.BackColor = SystemColors.Control; break;
                }
            }
            else
            {
                this.panelRobot.BackColor = Color.LightGray;
            }

            robot.PrePosition = robot.Position;

            if (robot.IsAlive)
            {
                if (robot.IsPausing)
                {
                    this.lbInfo.Text = "暂停中";
                    this.lbInfo.ForeColor = Color.Red;

                }
                else if (robot.IsMoving && TengDa.WF.Current.IsTerminalInitFinished)
                {
                    this.lbInfo.Text = Current.Robot.MovingDirection == MovingDirection.前进 ? string.Format("{0}移动中", robot.MovingDirSign) : string.Format("移动中{0}", robot.MovingDirSign);
                    this.lbInfo.ForeColor = Color.Blue;
                }
                else if (robot.IsMoving)
                {
                    this.lbInfo.Text = "取放中";
                    //this.lbInfo.Text = Current.Task.Status == TaskStatus.取完 || Current.Task.Status == TaskStatus.可取 || Current.Task.Status == TaskStatus.正取 ? "取盘中" : "放盘中";
                    this.lbInfo.ForeColor = Color.Blue;
                }
                //else if (!Current.Robot.IsExecuting)
                //{
                //    this.lbInfo.Text = "未就绪";
                //    this.lbInfo.ForeColor = Color.Red;
                //}
                else
                {
                    this.lbInfo.Text = "闲置";
                    this.lbInfo.ForeColor = SystemColors.WindowText;
                }
            }
            else
            {
                this.lbInfo.Text = "未知";
                this.lbInfo.ForeColor = SystemColors.WindowText;
            }

            this.lbClampCode.Text = robot.Clamp.Code;

            if (!string.IsNullOrEmpty(robot.AlarmStr) && robot.IsAlive)
            {
                if (robot.PreAlarmStr != robot.AlarmStr)
                {
                    this.lbName.Text = robot.AlarmStr.TrimEnd(',') + "...";
                }
                else
                {
                    string alarmStr = this.lbName.Text;
                    this.lbName.Text = alarmStr.Substring(1, alarmStr.Length - 1) + alarmStr.Substring(0, 1);
                }

                this.lbName.ForeColor = Color.White;
                this.lbName.BackColor = Color.Red;
            }
            else
            {
                this.lbName.Text = Current.Robot.Name;
                this.lbName.ForeColor = SystemColors.WindowText;
                this.lbName.BackColor = Color.Transparent;
            }
            robot.PreAlarmStr = robot.AlarmStr;
        }

        private void CmsRobot_Opening(object sender, CancelEventArgs e)
        {
            this.tsmManuGetStation.Enabled = Current.TaskMode == TaskMode.手动任务 && Current.Robot.IsAlive && Current.Task.NextFromStationId < 1 && Current.Task.NextToStationId < 1 && Current.Task.Status == TaskStatus.完成;
            this.tsmManuPutStation.Enabled = Current.TaskMode == TaskMode.手动任务 && Current.Robot.IsAlive && Current.Task.NextFromStationId > 0 && Current.Task.NextToStationId < 1 && Current.Task.Status == TaskStatus.完成;
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


            ToolStripMenuItem tsmiFeederStation = new ToolStripMenuItem();
            List<ToolStripItem> tsiFeederStations = new List<ToolStripItem>();
            tsmiFeederStation.Text = Current.Feeder.Name;

            var isFeederEnabled = false;

            Current.Feeder.Stations.ForEach(s =>
            {
                ToolStripMenuItem tsiStation = new ToolStripMenuItem();
                tsiStation.Name = string.Format("tsmManu_{0}_{1}", ManuFlag, s.Name);
                tsiStation.Text = s.Name;
                tsiStation.Click += new System.EventHandler(this.tsmManuStation_Click);

                tsiStation.Enabled = GetTsiEnabled(ManuFlag, s);
                tsiStation.ForeColor = GetForeColor(ManuFlag, s);

                if (tsiStation.Enabled) isFeederEnabled = true;

                tsmiFeederStation.DropDownItems.Add(tsiStation);
            });

            tsmiFeederStation.Enabled = isFeederEnabled;
            tsiStations.Add(tsmiFeederStation);


            for (int i = 0; i < Current.ovens.Count; i++)
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


            ToolStripMenuItem tsmiBlankerStation = new ToolStripMenuItem();
            List<ToolStripItem> tsiBlankerStations = new List<ToolStripItem>();
            tsmiBlankerStation.Text = Current.Blanker.Name;

            var isBlankerEnabled = false;

            Current.Blanker.Stations.ForEach(s =>
            {
                ToolStripMenuItem tsiStation = new ToolStripMenuItem();
                tsiStation.Name = string.Format("tsmManu_{0}_{1}", ManuFlag, s.Name);
                tsiStation.Text = s.Name;
                tsiStation.Click += new System.EventHandler(this.tsmManuStation_Click);
                tsiStation.Enabled = GetTsiEnabled(ManuFlag, s);
                tsiStation.ForeColor = GetForeColor(ManuFlag, s);

                if (tsiStation.Enabled) isBlankerEnabled = true;

                tsmiBlankerStation.DropDownItems.Add(tsiStation);
            });

            tsmiBlankerStation.Enabled = isBlankerEnabled;
            tsiStations.Add(tsmiBlankerStation);


            ToolStripMenuItem tsmiCacheStation = new ToolStripMenuItem();
            List<ToolStripItem> tsiCacheStations = new List<ToolStripItem>();
            tsmiCacheStation.Text = Current.Cacher.Name;

            var isCacheEnabled = false;

            Current.Cacher.Stations.ForEach(s =>
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

                    //if (Current.Robot.ClampStatus != ClampStatus.无夹具)
                    //{
                    //    Tip.Alert(Current.Robot.Name + "上有夹具，不能取盘！");
                    //    return;
                    //}

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

                    //if (Current.Task.FromClampStatus == ClampStatus.满夹具 && station.GetPutType == GetPutType.上料机)
                    //{
                    //    Tip.Alert(station.Name + "不允许放满夹具！");
                    //    return;
                    //}

                    //if (Current.Robot.ClampStatus == ClampStatus.无夹具)
                    //{
                    //    Tip.Alert(Current.Robot.Name + "上无夹具，不能放盘！");
                    //    return;
                    //}

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

                    //if (Current.Task.Status != TaskStatus.正取 && Current.Task.Status != TaskStatus.取完)
                    //{
                    //    Current.Task.Status = TaskStatus.取完;
                    //}

                    //if (Current.Task.FromClampStatus != ClampStatus.空夹具 && Current.Task.FromClampStatus != ClampStatus.满夹具)
                    //{
                    //    Current.Task.FromClampStatus = Current.Robot.ClampStatus;
                    //}

                }

            }
            catch (Exception ex)
            {
                Error.Alert(ex);
            }

        }
    }
}
