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
    public partial class FloorUC : UserControl
    {

        private string batchNumber1 = "";
        private string batchNumber2 = "";

        private Floor floor;

        private Oven _oven;
        private Oven oven {
            get
            {
                if(_oven == null)
                {
                    _oven = floor.GetOven();
                }
                return _oven;
            }
        }
        public FloorUC()
        {
            InitializeComponent();

            if (System.Windows.SystemParameters.PrimaryScreenHeight > 800)
            {
                this.lbInfoTop.Font = new System.Drawing.Font("Consolas", 9.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.lbStatus.Font = new System.Drawing.Font("Consolas", 9.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }
        }

        public void Init(Floor floor)
        {
            this.floor = floor;
        }

        public void UpdateUI()
        {
            this.pbRunTime.Maximum = floor.RunMinutesSet;
            this.pbRunTime.Value = floor.IsAlive ? (floor.RunMinutesSet > floor.RunMinutes ? floor.RunMinutes : floor.RunMinutesSet) : 0;

            if (!string.IsNullOrEmpty(floor.AlarmStr) && floor.IsAlive)
            {
                if (floor.PreAlarmStr != floor.AlarmStr)
                {
                    this.lbInfoTop.Text = floor.AlarmStr.TrimEnd(',') + "...";
                }
                else
                {
                    string alarmStr = this.lbInfoTop.Text;
                    this.lbInfoTop.Text = alarmStr.Substring(1, alarmStr.Length - 1) + alarmStr.Substring(0, 1);
                }
                this.lbInfoTop.ForeColor = Color.White;
                this.lbInfoTop.BackColor = Color.Red;
            }
            else
            {
                var centerStr = floor.IsEnable ? "" : "禁用";

                var ss = new List<Station>() { oven.ClampOri == ClampOri.A ? floor.Stations[0] : floor.Stations[1], oven.ClampOri == ClampOri.A ? floor.Stations[1] : floor.Stations[0] };
                var strs = new List<string>() { "", "" };

                for (var x = 0; x < ss.Count; x++)
                {
                    if (ss[x].FloorStatus == FloorStatus.待出)
                    {
                        strs[x] = ss[x].SampleStatus == SampleStatus.待结果 ? "待测" : ss[x].SampleStatus == SampleStatus.水分OK ? "OK" : ss[x].SampleStatus == SampleStatus.水分NG ? "NG" : "未知";
                    }
                    else if (ss[x].FloorStatus == FloorStatus.待烤 && ss[x].SampleStatus == SampleStatus.水分NG)
                    {
                        strs[x] = "NG";
                    }
                    else
                    {
                        strs[x] = ss[x].FloorStatus.ToString();
                    }
                }

                if (floor.IsBaking)
                {
                    this.lbInfoTop.Text = string.Format("{0}℃{3} {1} {4}{2}℃",
                         floor.Temperatures[Current.option.DisplayTemperIndex].ToString().PadLeft(2),
                         centerStr,
                         floor.Temperatures[Current.option.DisplayTemperIndex].ToString().PadLeft(2),
                         ss[0].HasSampleFlag ? "■" : "",
                         ss[1].HasSampleFlag ? "■" : ""
                         );
                }
                else
                {
                    this.lbInfoTop.Text = string.Format("{0}{3} {1} {4}{2}",
                         strs[0],
                         centerStr,
                         strs[1],
                         ss[0].HasSampleFlag ? "■" : "",
                         ss[1].HasSampleFlag ? "■" : "");
                }

                //}
                this.lbInfoTop.ForeColor = Color.Red;
                this.lbInfoTop.BackColor = Color.Transparent;
            }

            if (Current.option.FloorShowInfoType == "默认信息")
            {
                lbStatus.Text =
                    string.Format("{0} {1}/{2} {3}",
                    //oven.ClampOri == ClampOri.A ? "左" : "右",
                    //oven.ClampOri == ClampOri.B ? floor.Stations[0].RGVGetCode : floor.Stations[1].RGVGetCode,
                    floor.DoorStatus,
                    floor.RunMinutes.ToString().PadLeft(3),
                    floor.RunMinutesSet.ToString().PadLeft(3),
                    ""
                    //oven.ClampOri == ClampOri.A ? floor.Stations[0].RGVGetCode : floor.Stations[1].RGVGetCode
                    );
            }
            else if (Current.option.FloorShowInfoType == "开始烘烤时间")
            {
                var startBakingTime = "";
                if (floor.Stations[0].Clamp.BakingStartTime > Common.DefaultTime)
                {
                    startBakingTime = floor.Stations[0].Clamp.BakingStartTime.ToString("yyyy-MM-dd HH:mm");
                }
                else if (floor.Stations[1].Clamp.BakingStartTime > Common.DefaultTime)
                {
                    startBakingTime = floor.Stations[1].Clamp.BakingStartTime.ToString("yyyy-MM-dd HH:mm");
                }
                lbStatus.Text = startBakingTime;
            }
            else if (Current.option.FloorShowInfoType == "出烤箱时间")
            {
                lbStatus.Text = floor.OutOvenTime > TengDa.Common.DefaultTime ? floor.OutOvenTime.ToString("yyyy-MM-dd HH:mm") : "";
            }
            else if (Current.option.FloorShowInfoType == "夹具条码")
            {
                lbStatus.Text =
                    string.Format("{0} {1}",
                    (oven.ClampOri == ClampOri.A ? floor.Stations[0].Clamp.Code : floor.Stations[1].Clamp.Code).PadRight(6),
                    (oven.ClampOri == ClampOri.A ? floor.Stations[1].Clamp.Code : floor.Stations[0].Clamp.Code).PadLeft(6)
                    );
            }
            else if (Current.option.FloorShowInfoType == "批次号")
            {
                if (string.IsNullOrEmpty(batchNumber1) && string.IsNullOrEmpty(batchNumber1))
                {
                    Station s1 = oven.ClampOri == ClampOri.A ? floor.Stations[0] : floor.Stations[1];
                    Station s2 = oven.ClampOri == ClampOri.A ? floor.Stations[1] : floor.Stations[0];

                    if (s1.Clamp.Batteries.Count > 0 && s1.Clamp.Batteries[0].Code.Length > 12)
                    {
                        batchNumber1 = s1.Clamp.Batteries[0].Code.Substring(8, 5);
                    }

                    if (s2.Clamp.Batteries.Count > 0 && s2.Clamp.Batteries[0].Code.Length > 12)
                    {
                        batchNumber2 = s2.Clamp.Batteries[0].Code.Substring(8, 5);
                    }
                }

                lbStatus.Text = batchNumber1.PadLeft(5) + "  " + batchNumber2.PadLeft(5);
            }


            if (Current.option.FloorShowInfoType != "批次号")
            {
                batchNumber1 = "";
                batchNumber1 = "";
            }


            switch (floor.DoorStatus)
            {
                case DoorStatus.打开:
                    lbStatus.ForeColor = Color.White;
                    lbStatus.BackColor = SystemColors.WindowText;
                    break;
                case DoorStatus.异常:
                    lbStatus.ForeColor = Color.White;
                    lbStatus.BackColor = Color.Red;
                    break;
                default:
                    lbStatus.ForeColor = SystemColors.WindowText;
                    lbStatus.BackColor = Color.Transparent;
                    break;
            }

            floor.PreAlarmStr = floor.AlarmStr;
        }

        public void Invalidate4ClampStatus()
        {
            this.tlpFloor.Invalidate();
        }

        private void CmsFloor_Opening(object sender, CancelEventArgs e)
        {
            this.tsmRemoteControl.Enabled = oven.IsAlive;
            this.tsmWatContentResult.Enabled = oven.IsAlive;
            this.tsmParamSetting.Enabled = oven.IsAlive;
            this.tsmShowTandV.Enabled = oven.IsAlive;

            this.tsmOvenOpenDoor.Enabled =
                floor.IsNetControlOpen
                && Current.TaskMode == TaskMode.手动任务
                && Current.runStstus == RunStatus.运行
                && oven.IsAlive
                && floor.DoorStatus != DoorStatus.打开
                && !floor.IsBaking
                && !floor.IsVacuum
                && !floor.Stations[0].IsOpenDoorIntervene;
            this.tsmOvenCloseDoor.Enabled =
                floor.IsNetControlOpen
                && Current.TaskMode == TaskMode.手动任务
                && Current.runStstus == RunStatus.运行
                && oven.IsAlive
                && floor.DoorStatus != DoorStatus.关闭;
            //this.tsmLoadVacuum.Enabled =
            //    floor.IsNetControlOpen
            //    && Current.runStstus == RunStatus.运行
            //    && oven.IsAlive
            //    && floor.DoorStatus == DoorStatus.关闭
            //    && !floor.VacuumIsLoading
            //    && !floor.IsVacuum;
            //this.tsmCancelLoadVacuum.Enabled =
            //    floor.IsNetControlOpen
            //    && Current.runStstus == RunStatus.运行
            //    && oven.IsAlive
            //    && floor.VacuumIsLoading;
            //this.tsmUploadVacuum.Enabled =
            //    floor.IsNetControlOpen
            //    && Current.runStstus == RunStatus.运行
            //    && oven.IsAlive
            //    && !floor.IsBaking
            //    && !floor.VacuumIsUploading
            //    && floor.IsVacuum;
            //this.tsmCancelUploadVacuum.Enabled =
            //    floor.IsNetControlOpen
            //    && Current.runStstus == RunStatus.运行
            //    && oven.IsAlive
            //    && floor.VacuumIsUploading;
            this.tsmOpenNetControl.Enabled = !floor.IsNetControlOpen;
            this.tsmStartBaking.Enabled =
                floor.IsNetControlOpen
                && Current.runStstus == RunStatus.运行
                && oven.IsAlive
                && !floor.IsBaking
                && floor.DoorStatus == DoorStatus.关闭;
            this.tsmStopBaking.Enabled =
                floor.IsNetControlOpen
                && Current.runStstus == RunStatus.运行
                && oven.IsAlive
                && floor.IsBaking;
            this.tsmAlarmReset.Enabled = oven.IsAlive;
            this.tsmWatContentResult.Enabled = floor.Stations.Count(s => s.FloorStatus == FloorStatus.待出 || (s.FloorStatus == FloorStatus.待烤 && s.SampleStatus == SampleStatus.水分NG)) > 0;
        }

        private void TsmAlarmReset_Click(object sender, EventArgs e)
        {
            floor.AddLog("手动报警复位");
            oven.AlarmReset(oven.Floors.IndexOf(floor));
        }

        private void TsmOvenOpenDoor_Click(object sender, EventArgs e)
        {
            floor.AddLog("手动开门");
            oven.OpenDoor(oven.Floors.IndexOf(floor));
        }

        private void TsmOvenCloseDoor_Click(object sender, EventArgs e)
        {

            if (floor.Stations.Count(s => s.Id == Current.Task.FromStationId) > 0 && Current.Task.Status != TaskStatus.完成)
            {
                Tip.Alert(Current.Task.FromStationName + "正在取盘，无法关门！");
                return;
            }

            if (floor.Stations.Count(s => s.Id == Current.Task.ToStationId) > 0 && Current.Task.Status != TaskStatus.完成)
            {
                Tip.Alert(Current.Task.ToStationName + "正在放盘，无法关门！");
                return;
            }

            floor.AddLog("手动关门");
            oven.CloseDoor(oven.Floors.IndexOf(floor));
        }

        private void TsmStartBaking_Click(object sender, EventArgs e)
        {
            floor.AddLog("手动启动运行");
            oven.StartBaking(oven.Floors.IndexOf(floor));
        }

        private void TsmLoadVacuum_Click(object sender, EventArgs e)
        {
            floor.AddLog("手动抽真空");
            oven.LoadVacuum(oven.Floors.IndexOf(floor));
        }

        private void TsmCancelLoadVacuum_Click(object sender, EventArgs e)
        {
            floor.AddLog("手动取消泄真空");
            oven.CancelLoadVacuum(oven.Floors.IndexOf(floor));
        }

        private void TsmUploadVacuum_Click(object sender, EventArgs e)
        {
            floor.AddLog("手动泄真空");
            oven.UploadVacuum(oven.Floors.IndexOf(floor));
        }

        private void TsmCancelUploadVacuum_Click(object sender, EventArgs e)
        {
            floor.AddLog("手动取消抽真空");
            oven.CancelUploadVacuum(oven.Floors.IndexOf(floor));
        }

        private void TsmStopBaking_Click(object sender, EventArgs e)
        {
            floor.AddLog("手动停止运行");
            oven.StopBaking(oven.Floors.IndexOf(floor));
        }

        private void TsmClearRunTime_Click(object sender, EventArgs e)
        {
            floor.AddLog("手动运行时间清零");
            oven.ClearRunTime(oven.Floors.IndexOf(floor));
        }

        private void TsmOpenNetControl_Click(object sender, EventArgs e)
        {
            floor.AddLog("手动打开网控");
            oven.OpenNetControl(oven.Floors.IndexOf(floor));
        }

        private void TsmWatContentTestOK_Click(object sender, EventArgs e)
        {
            var floorName = this.floor.Name;
            DialogResult dr = MessageBox.Show("确定" + floorName + "水分测试结果OK？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            if (dr == DialogResult.OK)
            {
                this.floor.Stations.ForEach(s =>
                {
                    if (s.ClampStatus == ClampStatus.满夹具)
                    {
                        s.SampleStatus = SampleStatus.水分OK;
                        s.FloorStatus = FloorStatus.待出;
                        s.SampleInfo = SampleInfo.无样品;
                    }
                    else
                    {
                        s.SampleStatus = SampleStatus.未知;
                    }
                });
            }
        }

        private void TsmWatContentTestNG_Click(object sender, EventArgs e)
        {

            var floorName = this.floor.Name;
            DialogResult dr = MessageBox.Show("确定" + floorName + "水分测试结果NG？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            if (dr == DialogResult.OK)
            {
                this.floor.Stations.ForEach(s =>
                {
                    if (s.ClampStatus == ClampStatus.满夹具)
                    {
                        s.SampleStatus = SampleStatus.水分NG;
                        s.FloorStatus = FloorStatus.待烤;
                    }
                });
            }
        }

        private void TlpFloor_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            if (this.floor == null) return;
            Graphics g = e.Graphics;
            Rectangle r = e.CellBounds;
            Brush brush = Brushes.White;

            for (int k = 0; k < this.floor.Stations.Count; k++)
            {
                if (e.Column == (this.oven.ClampOri == ClampOri.A ? k * 2 : this.floor.Stations.Count - k * 2))
                {

                    Station station = this.floor.Stations[k];

                    bool canChangeColor = DateTime.Now.Second % 3 == 1;

                    if (canChangeColor && station.Id == Current.Task.FromStationId/* && (Current.Task.Status == TaskStatus.就绪 || Current.Task.Status == TaskStatus.可取 || Current.Task.Status == TaskStatus.正取)*/)
                    {
                        brush = Brushes.White;
                    }
                    else if (canChangeColor && station.ClampStatus == ClampStatus.异常)
                    {
                        brush = Brushes.Red;
                    }
                    else if (canChangeColor && station.Id == Current.Task.ToStationId)
                    {
                        brush = Current.Task.FromClampStatus == ClampStatus.空夹具 ? Brushes.Cyan : Brushes.LightBlue;
                    }
                    else
                    {
                        if (!station.IsAlive)
                        {
                            brush = Brushes.LightGray;
                        }
                        else
                        {
                            if (station.SampleStatus == SampleStatus.待结果 && station.FloorStatus == FloorStatus.待出)
                            {
                                brush = Brushes.DeepSkyBlue;
                            }
                            else
                            {
                                switch (station.FloorStatus)
                                {
                                    case FloorStatus.无盘: brush = Brushes.White; break;
                                    case FloorStatus.空盘: brush = Brushes.Cyan; break;
                                    case FloorStatus.待烤: brush = Brushes.LightBlue; break;
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

        private void TsmParamSetting_Click(object sender, EventArgs e)
        {
            var ovenParamSettingForm = new ParamSettingForm(this.floor);
            ovenParamSettingForm.ShowDialog();
        }

        private void TsmFloorEnabled_Click(object sender, EventArgs e)
        {
            this.floor.IsEnable = !this.floor.IsEnable;
        }

        private void TsmShowTandV_Click(object sender, EventArgs e)
        {
            var showTandVForm = new ShowTandVForm(this.floor);
            showTandVForm.ShowDialog();
        }

    }
}
