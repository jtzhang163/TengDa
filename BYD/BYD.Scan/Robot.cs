using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using TengDa;
using TengDa.WF;

namespace BYD.Scan
{
    /// <summary>
    /// 机器人搬运车
    /// </summary>
    public class Robot : TengDa.WF.Terminals.Terminal
    {
        #region 属性字段

        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".Robots";
                }
                return tableName;
            }
        }

        private int plcId = -1;
        [ReadOnly(true), DisplayName("PLC ID")]
        public int PlcId
        {
            get { return plcId; }
            set { plcId = value; }
        }


        private int clampId = -1;
        [ReadOnly(true), DisplayName("夹具ID")]
        public int ClampId
        {
            get { return clampId; }
            set
            {
                if (clampId != value)
                {
                    UpdateDbField("ClampId", value);
                }
                clampId = value;
            }
        }

        private int position = 1;

        [ReadOnly(true)]
        [DisplayName("当前位置")]
        public int Position
        {
            get
            {
                position = (int)((this.CoordinateValue - Current.option.RobotMinCoordinate) * Current.option.RobotPositionAmplify);
                return position;
            }
            set
            {
                if (position != value)
                {
                    this.Location = string.Format("{0},1", value);
                    UpdateDbField("Position", value);
                }
                position = value;
            }
        }
        [Browsable(false)]
        public int PrePosition { get; set; }

        private ClampStatus clampStatus = ClampStatus.未知;
        /// <summary>
        /// 夹具状态
        /// </summary>

        [DisplayName("夹具状态")]
        public ClampStatus ClampStatus
        {
            get { return clampStatus; }
            set
            {
                if (clampStatus != value)
                {
                    UpdateDbField("ClampStatus", value);
                }
                clampStatus = value;
            }
        }

        [ReadOnly(true), DisplayName("机器人正在移动")]
        public bool IsMoving { get; set; } = false;

        [ReadOnly(true), DisplayName("可确认放夹具到位信号出现次数")]
        public int CanCheckPutClampIsOkCount { get; set; } = 0;

        private int coordinateValue = -1;
        /// <summary>
        /// 坐标值
        /// </summary>
        [DisplayName("坐标值")]
        public int CoordinateValue
        {
            get
            {
                if (coordinateValue < Current.option.RobotMinCoordinate)
                {
                    coordinateValue = Current.option.RobotMinCoordinate;
                }
                return coordinateValue;
            }
            set
            {
                coordinateValue = value;
            }
        }

        public int PreCoordinateValue = -1;

        [ReadOnly(true), DisplayName("运动方向")]
        public MovingDirection MovingDirection { get; set; } = MovingDirection.未知;

        [Browsable(false)]
        public string MovingDirSign
        {
            get
            {
                if (this.MovingDirection == MovingDirection.停止 || this.MovingDirection == MovingDirection.未知)
                {
                    return string.Empty;
                }
                return this.MovingDirection == MovingDirection.前进 ^ Option.LayoutType == 1 ? "←" : "→";
            }
        }

        /// <summary>
        /// 已请求启动
        /// </summary>
        [ReadOnly(true), DisplayName("已请求启动")]
        public bool IsRequestStart { get; set; } = false;

        [Browsable(false)]
        public bool IsAlreadySendCmd { get; set; } = false;

        #endregion

        #region 构造方法

        public Robot() : this(-1) { }

        public Robot(int id)
        {
            if (id < 0)
            {
                this.Id = -1;
                return;
            }

            string msg = string.Empty;

            DataTable dt = Database.Query("SELECT * FROM [dbo].[" + TableName + "] WHERE Id = " + id, out msg);

            if (!string.IsNullOrEmpty(msg))
            {
                Error.Alert(msg);
                return;
            }

            if (dt == null || dt.Rows.Count == 0) { return; }

            Init(dt.Rows[0]);
            //释放资源
            dt.Dispose();
        }

        #endregion

        #region 初始化方法
        protected void Init(DataRow rowInfo)
        {
            if (rowInfo == null)
            {
                this.Id = -1;
                return;
            }

            InitFields(rowInfo);
        }

        protected void InitFields(DataRow rowInfo)
        {
            this.Id = TengDa._Convert.StrToInt(rowInfo["Id"].ToString(), -1);
            this.name = rowInfo["Name"].ToString();
            this.plcId = TengDa._Convert.StrToInt(rowInfo["PlcId"].ToString(), -1);
            this.company = rowInfo["Company"].ToString();
            this.model = rowInfo["Model"].ToString();
            this.number = rowInfo["Number"].ToString();
            this.location = rowInfo["Location"].ToString();
            this.isEnable = Convert.ToBoolean(rowInfo["IsEnable"]);
            this.clampId = TengDa._Convert.StrToInt(rowInfo["ClampId"].ToString(), -1);
            this.position = TengDa._Convert.StrToInt(rowInfo["Position"].ToString(), 1);
            this.clampStatus = (ClampStatus)Enum.Parse(typeof(ClampStatus), rowInfo["ClampStatus"].ToString());
        }
        #endregion

        #region 该设备上的PLC

        private PLC plc = new PLC();

        [Browsable(false)]
        [ReadOnly(true)]
        public PLC Plc
        {
            get
            {
                if(plc.Id < 1)
                {
                    plc = PLC.PlcList.First(p => p.Id == this.PlcId);
                }
                return plc;
            }
        }
        #endregion

        #region 该设备上的工位列表
        private Clamp clamp = new Clamp();
        [Browsable(false)]
        public Clamp Clamp
        {
            get
            {
                if (clamp.Id != ClampId)
                {
                    clamp = new Clamp(this.ClampId);
                }
                return clamp;
            }
            set
            {
                this.ClampId = value.Id;
            }
        }
        #endregion

        #region 通信

        public bool AlreadyGetAllInfo = false;

        public bool GetInfo()
        {
            if (!this.Plc.IsPingSuccess)
            {
                this.Plc.IsAlive = false;
                LogHelper.WriteError("无法连接到 " + this.Plc.IP);
                return false;
            }
            this.Plc.IsAlive = true;
            return true;      
        }

        /// <summary>
        /// 执行取放
        /// </summary>
        public bool Move(Station fromStation, Station toStation)
        {
            if (!this.Plc.IsPingSuccess)
            {
                LogHelper.WriteError("无法连接到 " + this.Plc.IP);
                return false;
            }

            string cmd = string.Format("<Sensor><WORKNUM>{6}</WORKNUM><N1>{0}</N1><N2>{1}</N2><N3>{2}</N3><N4>{3}</N4><N5>{4}</N5><N6>{5}</N6><CMD>1</CMD></Sensor>",
                fromStation.RobotValue1, fromStation.RobotValue2, fromStation.RobotValue3, toStation.RobotValue1, toStation.RobotValue2, toStation.RobotValue3,Current.option.CurrentWorkNum);

            LogHelper.WriteInfo(string.Format("给机器人发送取放盘指令------：{0}", cmd));

            if (!Current.Robot.Plc.SetInfo(cmd, out string msg))
            {
                LogHelper.WriteInfo(string.Format("发送取放盘指令失败--：{0}", msg));
                Error.Alert(msg);
                this.Plc.IsAlive = false;
                return false;
            }

            return true;
        }


        public bool IsReceived()
        {
            var recevieData = this.Plc.GetReceiveData();
            if (recevieData.Contains("REC"))
            {
                LogHelper.WriteInfo(string.Format("收到机器人REC指令------：{0}", recevieData));
                this.Plc.ClearReceiveData();
                return true;
            }
            return false;
        }

        public bool IsFinished()
        {
            var recevieData = this.Plc.GetReceiveData();
            if (recevieData.Contains("FINISH"))
            {
                LogHelper.WriteInfo(string.Format("收到机器人FINISH指令------：{0}", recevieData));
                this.Plc.ClearReceiveData();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 首次开机启动机器人
        /// </summary>
        /// <returns></returns>
        public bool Start(out string msg)
        {
            msg = "";
            return true;
        }


        /// <summary>
        /// 机器人暂停运行
        /// </summary>
        /// <returns></returns>
        public bool Pause(out string msg)
        {

            if (!Current.Robot.Plc.SetInfo("D1012", (ushort)1, out msg))
            {
                Error.Alert(msg);
                this.Plc.IsAlive = false;
                return false;
            }

            LogHelper.WriteInfo(string.Format("给机器人发送暂停运行指令------{0}：{1}  ", "D1012", 1));

            return true;
        }



        /// <summary>
        /// 机器人继续运行
        /// </summary>
        /// <returns></returns>
        public bool Restart(out string msg)
        {

            if (!Current.Robot.Plc.SetInfo("D1012", (ushort)0, out msg))
            {
                Error.Alert(msg);
                this.Plc.IsAlive = false;
                return false;
            }

            LogHelper.WriteInfo(string.Format("给机器人发送继续运行指令------{0}：{1}  ", "D1012", 0));

            return true;
        }

        /// <summary>
        /// 机器人急停
        /// </summary>
        /// <returns></returns>
        public bool Stop(out string msg)
        {
            Current.Robot.Pause(out msg);
            return true;
        }

        #endregion
    }

    public class RobotPosition : Service
    {
        #region 属性字段

        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".RobotPositions";
                }
                return tableName;
            }
        }

        private int position = 1;

        [ReadOnly(true)]
        [DisplayName("位置编号")]
        public int Position
        {
            get
            {
                return position;
            }
            set
            {
                if (position != value)
                {
                    UpdateDbField("Position", value);
                }
                position = value;
            }
        }


        private int xValue = 1;

        [DisplayName("坐标值")]
        public int XValue
        {
            get
            {
                return xValue;
            }
            set
            {
                if (xValue != value)
                {
                    UpdateDbField("XValue", value);
                }
                xValue = value;
            }
        }

        private int xMinValue = 1;

        [DisplayName("最小坐标值")]
        public int XMinValue
        {
            get
            {
                return xMinValue;
            }
            set
            {
                if (xMinValue != value)
                {
                    UpdateDbField("XMinValue", value);
                }
                xMinValue = value;
            }
        }

        private int xMaxValue = 1;

        [DisplayName("最大坐标值")]
        public int XMaxValue
        {
            get
            {
                return xMaxValue;
            }
            set
            {
                if (xMaxValue != value)
                {
                    UpdateDbField("XMaxValue", value);
                }
                xMaxValue = value;
            }
        }

        #endregion

        #region 构造方法

        public RobotPosition() : this(-1) { }

        public RobotPosition(int id)
        {
            if (id < 0)
            {
                this.Id = -1;
                return;
            }

            string msg = string.Empty;

            DataTable dt = Database.Query("SELECT * FROM [dbo].[" + TableName + "] WHERE Id = " + id, out msg);

            if (!string.IsNullOrEmpty(msg))
            {
                Error.Alert(msg);
                return;
            }

            if (dt == null || dt.Rows.Count == 0) { return; }

            Init(dt.Rows[0]);

            //释放资源
            dt.Dispose();
        }

        #endregion

        #region 初始化方法
        protected void Init(DataRow rowInfo)
        {
            if (rowInfo == null)
            {
                this.Id = -1;
                return;
            }

            InitFields(rowInfo);
        }

        protected void InitFields(DataRow rowInfo)
        {
            this.Id = TengDa._Convert.StrToInt(rowInfo["Id"].ToString(), -1);
            this.position = TengDa._Convert.StrToInt(rowInfo["Position"].ToString(), -1);
            this.xValue = TengDa._Convert.StrToInt(rowInfo["XValue"].ToString(), -1);
            this.xMinValue = TengDa._Convert.StrToInt(rowInfo["XMinValue"].ToString(), -1);
            this.xMaxValue = TengDa._Convert.StrToInt(rowInfo["XMaxValue"].ToString(), -1);
        }
        #endregion

        #region 列表
        private static List<RobotPosition> robotPositionList = new List<RobotPosition>();
        public static List<RobotPosition> RobotPositionList
        {
            get
            {
                if (robotPositionList.Count < 1)
                {
                    string msg = string.Empty;

                    DataTable dt = Database.Query("SELECT * FROM [dbo].[" + TableName + "]", out msg);
                    if (!string.IsNullOrEmpty(msg))
                    {
                        Error.Alert(msg);
                        return null;
                    }

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            RobotPosition robotPosition = new RobotPosition();
                            robotPosition.InitFields(dt.Rows[i]);
                            robotPositionList.Add(robotPosition);
                        }
                    }
                }
                return robotPositionList;
            }
        }
        #endregion
    }

    public enum MovingDirection
    {
        前进 = 1,
        后退 = 2,
        停止 = 3,
        未知 = 4
    }
}
