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
                    this.lbInfo.Text = Current.Robot.MovingDirection == MovingDirection.前进 ? string.Format("{0}移动", robot.MovingDirSign) : string.Format("移动{0}", robot.MovingDirSign);
                    this.lbInfo.ForeColor = Color.Blue;
                }
                else if (robot.IsGettingOrPutting)
                {
                    this.lbInfo.Text = Current.Task.Status == TaskStatus.取完 || Current.Task.Status == TaskStatus.可取 || Current.Task.Status == TaskStatus.正取 ? "取盘中" : "放盘中";
                    this.lbInfo.ForeColor = Color.Blue;
                }
                else if (!Current.Robot.IsExecuting)
                {
                    this.lbInfo.Text = "未就绪";
                    this.lbInfo.ForeColor = Color.Red;
                }
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
    }
}
