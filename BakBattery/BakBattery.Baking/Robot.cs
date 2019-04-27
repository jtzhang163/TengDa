using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using TengDa;
using TengDa.WF;

namespace BakBattery.Baking
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

        /// <summary>
        /// 启动成功
        /// </summary>
        [ReadOnly(true), DisplayName("启动成功")]
        public bool IsStarting
        {
            get { return IsExecuting || IsPausing; }
        }

        /// <summary>
        /// 程序执行中
        /// </summary>
        [ReadOnly(true), DisplayName("程序执行中")]
        public bool IsExecuting { get; set; } = false;

        [ReadOnly(true), DisplayName("可发送取盘指令")]
        public bool IsReadyGet { get; set; } = false;

        [ReadOnly(true), DisplayName("可发送放盘指令")]
        public bool IsReadyPut { get; set; } = false;

        [ReadOnly(true), DisplayName("防撞报警中")]
        public bool IsBumpAlarming { get; set; } = false;
        
        /// <summary>
        /// 机器人货叉正在运动
        /// </summary>
        [ReadOnly(true), DisplayName("机器人货叉正在运动")]
        public bool IsGettingOrPutting { get; set; } = false;

        [ReadOnly(true), DisplayName("机器人正在移动")]
        public bool IsMoving { get; set; } = false;

        //[ReadOnly(true), DisplayName("可确认是否取放夹具到位")]
        //public bool CanCheckGetPutClampIsOk { get; set; } = false;

        [ReadOnly(true), DisplayName("可确认放夹具到位信号出现次数")]
        public int CanCheckPutClampIsOkCount { get; set; } = 0;

        /// <summary>
        /// 坐标值
        /// </summary>
        [DisplayName("轴坐标：I5")]
        public int CoordinateValue { get; set; } = -1;

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
            lock (this)
            {
                if (!this.Plc.IsPingSuccess)
                {
                    this.Plc.IsAlive = false;
                    LogHelper.WriteError("无法连接到 " + this.Plc.IP);
                    return false;
                }

                string msg = string.Empty;
                string output = string.Empty;

                try
                {

                    var plcCompany = (PlcCompany)Enum.Parse(typeof(PlcCompany), this.Plc.Company);

                    #region 获取是否启动完成

                    bool isRequestStart = false;

                    if (!this.Plc.GetInfo(false, plcCompany, true, "I10.1", false, out isRequestStart, out msg))
                    {
                        Error.Alert(msg);
                        this.Plc.IsAlive = false;
                        return false;
                    }
                    this.IsRequestStart = isRequestStart;

                    #endregion

                    #region 获取是否执行中


                    if (!this.Plc.GetInfo(false, plcCompany, true, "I1.2", false, out bool isExecuting, out msg))
                    {
                        Error.Alert(msg);
                        this.Plc.IsAlive = false;
                        return false;
                    }
                    IsExecuting = isExecuting;

                    #endregion

                    #region 获取报警状态

                    bool isAlarming = false;

                    if (!this.Plc.GetInfo(false, plcCompany, true, "I1.5", false, out isAlarming, out msg))
                    {
                        Error.Alert(msg);
                        this.Plc.IsAlive = false;
                        return false;
                    }

                    this.IsAlarming = isAlarming;

                    #endregion

                    #region 获取防撞报警状态

                    bool isBumpAlarming = false;

                    if (!this.Plc.GetInfo(false, plcCompany, true, "I10.6", false, out isBumpAlarming, out msg))
                    {
                        Error.Alert(msg);
                        this.Plc.IsAlive = false;
                        return false;
                    }

                    this.IsBumpAlarming = isBumpAlarming;

                    this.AlarmStr = isAlarming ? this.Name + "报警中" : (this.IsBumpAlarming ? "防撞报警中" : "");

                    #endregion

                    #region 获取暂停状态

                    bool isPausing = false;

                    if (!this.Plc.GetInfo(false, plcCompany, true, "I1.3", false, out isPausing, out msg))
                    {
                        Error.Alert(msg);
                        this.Plc.IsAlive = false;
                        return false;
                    }

                    this.IsPausing = isPausing;

                    #endregion

                    #region 获取夹具状态

                    int clampStatus = -1;

                    if (!this.Plc.GetInfo(false, plcCompany, true, "Q15", (byte)0, out clampStatus, out msg))
                    {
                        Error.Alert(msg);
                        this.Plc.IsAlive = false;
                        return false;
                    }

                    switch (clampStatus)
                    {
                        case 1: this.ClampStatus = this.ClampStatus == ClampStatus.空夹具 ? ClampStatus.空夹具 : ClampStatus.满夹具; break;
                        case 2: this.ClampStatus = ClampStatus.无夹具; break;
                        case 4: this.ClampStatus = ClampStatus.异常; break;
                        default: this.ClampStatus = ClampStatus.未知; break;
                    }

                    this.IsPausing = isPausing;

                    #endregion

                    #region 获取正在执行取放的位置编号

                    int stationNum = -1;
                    if (!this.Plc.GetInfo(false, plcCompany, true, "Q4", 0, out stationNum, out msg))
                    {
                        Error.Alert(msg);
                        this.Plc.IsAlive = false;
                        return false;
                    }

                    this.IsGettingOrPutting = stationNum != 0;

                    this.IsReadyGet = IsExecuting && (stationNum == 0);
                    this.IsReadyPut = IsExecuting && (stationNum == 0);

                    #endregion

                    #region 获取位置
                    int i5 = -1;
                    if (!this.Plc.GetInfo(false, plcCompany, true, "I5", (byte)0, out i5, out msg))
                    {
                        Error.Alert(msg);
                        this.Plc.IsAlive = false;
                        return false;
                    }

                    if (i5 < 0)
                    {
                        this.CoordinateValue = 0;
                    }
                    else if (i5 < 170)
                    {
                        this.CoordinateValue = i5;
                    }
                    else
                    {
                        this.CoordinateValue = 170;
                    }

                    // RobotPosition rp = RobotPosition.RobotPositionList.FirstOrDefault(r => r.XMinValue < this.I4 && r.XMaxValue > this.I4);
                    this.Position = (int)(this.CoordinateValue * Current.option.RobotPositionAmplify);
                    //  this.Position = rp == null ? this.position : rp.Position;

                    if (this.CoordinateValue < this.PreCoordinateValue) { this.MovingDirection = MovingDirection.前进; this.IsMoving = true; }
                    else if (this.CoordinateValue > this.PreCoordinateValue) { this.MovingDirection = MovingDirection.后退; this.IsMoving = true; }
                    else { this.MovingDirection = MovingDirection.停止; this.IsMoving = false; }

                    this.PreCoordinateValue = this.CoordinateValue;

                    #endregion

                    System.Threading.Thread.Sleep(50);

                }
                catch (Exception ex)
                {
                    Error.Alert(ex);
                }

                this.Plc.IsAlive = true;
                this.AlreadyGetAllInfo = true;

            }
            return true;
        }

        /// <summary>
        /// 执行取放
        /// </summary>
        /// <param name="pos">取放位置编号</param>
        /// <returns></returns>
        public bool GetOrPut(byte pos)
        {
            if (!this.Plc.IsPingSuccess)
            {
                LogHelper.WriteError("无法连接到 " + this.Plc.IP);
                return false;
            }

            string msg = string.Empty;
            try
            {

                var plcCompany = (PlcCompany)Enum.Parse(typeof(PlcCompany), this.Plc.Company);

                #region 获取正在执行取放的位置编号

                int stationNum = -1;
                if (!this.Plc.GetInfo(false, plcCompany, true, "Q4", 0, out stationNum, out msg))
                {
                    Error.Alert(msg);
                    this.Plc.IsAlive = false;
                    return false;
                }

                #endregion

                if ((int)pos == stationNum)
                {
                    return true;
                }

                int o1 = 0;
                if (!this.Plc.GetInfo(false, plcCompany, false, "Q4", pos, out o1, out msg))
                {
                    Error.Alert(msg);
                    this.Plc.IsAlive = false;
                    return false;
                }

                LogHelper.WriteInfo(string.Format("给机器人发送到位取放指令------{0}：{1}  ", "Q4", pos));
            }
            catch (Exception ex)
            {
                Error.Alert(ex);
            }

            return true;
        }

        /// <summary>
        /// 首次开机启动机器人
        /// </summary>
        /// <returns></returns>
        public bool Start(out string msg)
        {

            msg = string.Empty;
            var plcCompany = (PlcCompany)Enum.Parse(typeof(PlcCompany), this.Plc.Company);

            if (this.IsPausing)
            {
                this.Restart(out msg);
            }

            if (!this.Plc.GetInfo(false, plcCompany, true, "I1.0", false, out bool tmpBool1, out msg))
            {
                this.Plc.IsAlive = false;
                return false;
            }

            if (!tmpBool1)
            {
                msg = "启动条件不满足！";
                this.Plc.IsAlive = false;
                return false;
            }

            System.Threading.Thread.Sleep(10);

            //将I10.0置为ture
            if (!this.Plc.GetInfo(false, plcCompany, false, "I10.0", true, out bool tmpBool2, out msg))
            {
                this.Plc.IsAlive = false;
                return false;
            }

            System.Threading.Thread.Sleep(200);

            if (!this.Plc.GetInfo(false, plcCompany, true, "I1.2", false, out bool tmpBool3, out msg))
            {
                this.Plc.IsAlive = false;
                return false;
            }

            if (!tmpBool3)
            {
                msg = "启动成功标识为False";
                return false;
            }

            return true;
        }

        /// <summary>
        /// 机器人暂停运行
        /// </summary>
        /// <returns></returns>
        public bool Pause(out string msg)
        {

            var plcCompany = (PlcCompany)Enum.Parse(typeof(PlcCompany), this.Plc.Company);

            bool tmp = false;
            if (!this.Plc.GetInfo(false, plcCompany, false, "I10.2", true, out tmp, out msg))
            {
                Error.Alert("暂停运行失败！原因：" + msg);
                this.Plc.IsAlive = false;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 机器人继续运行
        /// </summary>
        /// <returns></returns>
        public bool Restart(out string msg)
        {

            var plcCompany = (PlcCompany)Enum.Parse(typeof(PlcCompany), this.Plc.Company);

            bool tmp = false;
            if (!this.Plc.GetInfo(false, plcCompany, false, "I10.4", true, out tmp, out msg))
            {
                Error.Alert("继续运行失败！原因：" + msg);
                this.Plc.IsAlive = false;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 机器人报警复位
        /// </summary>
        /// <returns></returns>
        public bool AlarmReset(out string msg)
        {

            var plcCompany = (PlcCompany)Enum.Parse(typeof(PlcCompany), this.Plc.Company);

            bool tmp = false;
            if (!this.Plc.GetInfo(false, plcCompany, false, "I10.3", true, out tmp, out msg))
            {
                Error.Alert("机器人报警复位失败！原因：" + msg);
                this.Plc.IsAlive = false;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 机器人急停
        /// </summary>
        /// <returns></returns>
        public bool Stop(out string msg)
        {

            var plcCompany = (PlcCompany)Enum.Parse(typeof(PlcCompany), this.Plc.Company);

            if (!this.Plc.GetInfo(false, plcCompany, false, "I10.1", true, out bool tmp, out msg))
            {
                Error.Alert("机器人急停失败！原因：" + msg);
                this.Plc.IsAlive = false;
                return false;
            }

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
