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

namespace Anchitech.Baking.Controls
{
    public partial class FloorUC : UserControl
    {
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
                var centerStr = floor.IsEnable ? floor.Vacuum.ToString().PadLeft(4) + "" : "炉层禁用";

                var ss = new List<Station>() { oven.ClampOri == ClampOri.A ? floor.Stations[0] : floor.Stations[1], oven.ClampOri == ClampOri.A ? floor.Stations[1] : floor.Stations[0] };
                var strs = new List<string>() { "", "" };

                for (var x = 0; x < ss.Count; x++)
                {
                    if (ss[x].FloorStatus == FloorStatus.待出)
                    {
                        strs[x] = ss[x].SampleStatus == SampleStatus.待结果 ? "待测" : ss[x].SampleStatus == SampleStatus.水分OK ? "水分OK" : ss[x].SampleStatus == SampleStatus.水分NG ? "水分NG" : "未知";
                    }
                    else if (ss[x].FloorStatus == FloorStatus.待烤 && ss[x].SampleStatus == SampleStatus.水分NG)
                    {
                        strs[x] = "水分NG";
                    }
                    else
                    {
                        strs[x] = ss[x].FloorStatus.ToString();
                    }
                }

                if (floor.IsBaking)
                {
                    this.lbInfoTop.Text = string.Format("{0}℃{3} {1} {4}{2}℃",
                         ss[0].Temperatures[Current.option.DisplayTemperIndex].ToString().PadLeft(2),
                         centerStr,
                         ss[1].Temperatures[Current.option.DisplayTemperIndex].ToString().PadLeft(2),
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

            if (!Current.option.IsDisplayOvenCode)
            {
                lbStatus.Text =
                    string.Format("{0} {1} {2}/{3} {4}",
                    oven.ClampOri == ClampOri.A ? "左" : "右",
                    //oven.ClampOri == ClampOri.B ? floor.Stations[0].RobotGetCode : floor.Stations[1].RobotGetCode,
                    floor.DoorStatus,
                    floor.RunMinutes.ToString().PadLeft(3),
                    floor.RunMinutesSet.ToString().PadLeft(3),
                    ""
                    //oven.ClampOri == ClampOri.A ? floor.Stations[0].RobotGetCode : floor.Stations[1].RobotGetCode
                    );
            }
            else
            {
                lbStatus.Text =
                    string.Format("{0} {1}",
                    (oven.ClampOri == ClampOri.A ? floor.Stations[0].Clamp.Code : floor.Stations[1].Clamp.Code).PadRight(8),
                    (oven.ClampOri == ClampOri.B ? floor.Stations[0].Clamp.Code : floor.Stations[1].Clamp.Code).PadLeft(8)
                    );
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

            this.tsmOvenOpenDoor.Enabled =
                floor.IsNetControlOpen
                && Current.runStstus == RunStatus.运行
                && oven.IsAlive
                && floor.DoorStatus != DoorStatus.打开
                && !floor.IsBaking
                && !floor.IsVacuum
                && !floor.Stations[0].IsOpenDoorIntervene;
            this.tsmOvenCloseDoor.Enabled =
                floor.IsNetControlOpen
                && Current.runStstus == RunStatus.运行
                && oven.IsAlive
                && floor.DoorStatus != DoorStatus.关闭;
            this.tsmLoadVacuum.Enabled =
                floor.IsNetControlOpen
                && Current.runStstus == RunStatus.运行
                && oven.IsAlive
                && floor.DoorStatus == DoorStatus.关闭
                && !floor.VacuumIsLoading
                && !floor.IsVacuum;
            this.tsmCancelLoadVacuum.Enabled =
                floor.IsNetControlOpen
                && Current.runStstus == RunStatus.运行
                && oven.IsAlive
                && floor.VacuumIsLoading;
            this.tsmUploadVacuum.Enabled =
                floor.IsNetControlOpen
                && Current.runStstus == RunStatus.运行
                && oven.IsAlive
                && !floor.IsBaking
                && !floor.VacuumIsUploading
                && floor.IsVacuum;
            this.tsmCancelUploadVacuum.Enabled =
                floor.IsNetControlOpen
                && Current.runStstus == RunStatus.运行
                && oven.IsAlive
                && floor.VacuumIsUploading;
            this.tsmClearRunTime.Enabled = false;
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
                Tip.Alert(Current.Task.FromStationName + "正在放盘，无法关门！");
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
            floor.AddLog("手动取消破真空");
            oven.CancelLoadVacuum(oven.Floors.IndexOf(floor));
        }

        private void TsmUploadVacuum_Click(object sender, EventArgs e)
        {
            floor.AddLog("手动破真空");
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
                                    case FloorStatus.待烤: brush = Brushes.Yellow; break;
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
    }
}
