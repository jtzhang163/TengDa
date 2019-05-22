using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Anchitech.Baking.Controls
{
    public partial class FloorUC : UserControl
    {
        public FloorUC()
        {
            InitializeComponent();
        }

        public void Init()
        {

        }

        public void Update(Oven oven,Floor floor)
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
                var centerStr = floor.IsEnable ? floor.Vacuum.ToString("#0").PadLeft(6) + "Pa" : "炉层禁用";

                if (floor.IsBaking)
                {
                    this.lbInfoTop.Text = string.Format("{0}℃ {1} {2}℃",
                         floor.Stations[0].Temperatures[Current.option.DisplayTemperIndex].ToString("#0.0").PadLeft(4),
                         centerStr,
                         floor.Stations[1].Temperatures[Current.option.DisplayTemperIndex].ToString("#0.0").PadLeft(4));
                }
                else
                {

                    var ss = new List<Station>() { oven.ClampOri == ClampOri.B ? floor.Stations[0] : floor.Stations[1], oven.ClampOri == ClampOri.B ? floor.Stations[1] : floor.Stations[0] };
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

                    this.lbInfoTop.Text = string.Format("{0} {3}{1} {4}{2}",
                         strs[0],
                         centerStr,
                         strs[1],
                         ss[0].HasSampleFlag ? "★ " : "",
                         ss[1].HasSampleFlag ? "★ " : "");
                }

                this.lbInfoTop.ForeColor = Color.Red;
                this.lbInfoTop.BackColor = Color.Transparent;
            }

            if (!Current.option.IsDisplayOvenCode)
            {
                lbStatus.Text =
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
                lbStatus.Text =
                    string.Format("{0} {1}",
                    (oven.ClampOri == ClampOri.B ? floor.Stations[0].Clamp.Code : floor.Stations[1].Clamp.Code).PadRight(8),
                    (oven.ClampOri == ClampOri.A ? floor.Stations[0].Clamp.Code : floor.Stations[1].Clamp.Code).PadLeft(8)
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
    }
}
