using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using TengDa;
using TengDa.WF;

namespace CAMEL.Baking
{
    /// <summary>
    /// RGV
    /// </summary>
    public class RGV : TengDa.WF.Terminals.Terminal
    {
        #region 属性字段

        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".RGV";
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
                position = (int)((this.CoordinateValue - Current.option.RGVMinCoordinate) * Current.option.RGVPositionAmplify);
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

        [ReadOnly(true), DisplayName("RGV正在移动")]
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
                if (coordinateValue < Current.option.RGVMinCoordinate)
                {
                    coordinateValue = Current.option.RGVMinCoordinate;
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

        public RGV() : this(-1) { }

        public RGV(int id)
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

            var bOutputs = new ushort[] { };
            if (!this.Plc.GetInfo("D1000", (ushort)100, out bOutputs, out string msg))
            {
                Error.Alert(msg);
                this.Plc.IsAlive = false;
                return false;
            }

            //#region 获取正在执行取放的位置编号

            //// this.GetPutNumber = bOutputs[0];

            //#endregion

            //#region 获取是否启动完成

            ////Current.RGV.IsStartting = true;
            //this.IsStarting = bOutputs[13] == 1;

            //this.IsExecuting = Current.RGV.IsStarting && bOutputs[16] == 1;

            //#endregion

            //#region 获取报警状态


            //Current.RGV.IsAlarming = false;
            //Current.RGV.AlarmStr = Current.RGV.IsAlarming ? this.Name + "报警中" : "";

            //#endregion

            //#region 获取暂停状态


            //Current.RGV.IsPausing = bOutputs[12] == 1;

            //#endregion

            #region 获取夹具状态

            switch (bOutputs[12])
            {
                case 2: Current.RGV.ClampStatus = Current.RGV.ClampStatus == ClampStatus.空夹具 ? ClampStatus.空夹具 : ClampStatus.满夹具; break;
                case 1: Current.RGV.ClampStatus = ClampStatus.无夹具; break;
                case 3: Current.RGV.ClampStatus = ClampStatus.异常; break;
                default: Current.RGV.ClampStatus = ClampStatus.未知; break;
            }

            #endregion

            //#region 获取位置

            //Current.RGV.CoordinateValue = bOutputs[15];

            //if (Current.RGV.CoordinateValue < Current.RGV.PreCoordinateValue) { Current.RGV.MovingDirection = MovingDirection.前进; Current.RGV.IsMoving = true; }
            //else if (Current.RGV.CoordinateValue > Current.RGV.PreCoordinateValue) { Current.RGV.MovingDirection = MovingDirection.后退; Current.RGV.IsMoving = true; }
            //else { Current.RGV.MovingDirection = MovingDirection.停止; Current.RGV.IsMoving = false; }

            //Current.RGV.PreCoordinateValue = Current.RGV.CoordinateValue;

            //#endregion

            this.AlreadyGetAllInfo = true;
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

            string cmd = "";
            LogHelper.WriteInfo(string.Format("给RGV发送取放盘指令------：{0}", cmd));

            if (!Current.RGV.Plc.SetInfo(cmd, out string msg))
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
                LogHelper.WriteInfo(string.Format("收到RGVREC指令------：{0}", recevieData));
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
                LogHelper.WriteInfo(string.Format("收到RGVFINISH指令------：{0}", recevieData));
                this.Plc.ClearReceiveData();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 启动RGV
        /// </summary>
        /// <returns></returns>
        public bool Start(out string msg)
        {
            if (!Current.RGV.Plc.SetInfo("D1001", (ushort)1, out msg))
            {
                Error.Alert(msg);
                this.Plc.IsAlive = false;
                return false;
            }

            LogHelper.WriteInfo(string.Format("给RGV发送启动指令------{0}：{1}  ", "D1001", 1));
            return true;
        }


        /// <summary>
        /// 停止RGV
        /// </summary>
        /// <returns></returns>
        public bool Pause(out string msg)
        {
            if (!Current.RGV.Plc.SetInfo("D1002", (ushort)1, out msg))
            {
                Error.Alert(msg);
                this.Plc.IsAlive = false;
                return false;
            }

            LogHelper.WriteInfo(string.Format("给RGV发送停止指令------{0}：{1}  ", "D1002", 1));
            return true;
        }



        /// <summary>
        /// 复位RGV
        /// </summary>
        /// <returns></returns>
        public bool Reset(out string msg)
        {
            if (!Current.RGV.Plc.SetInfo("D1003", (ushort)1, out msg))
            {
                Error.Alert(msg);
                this.Plc.IsAlive = false;
                return false;
            }

            LogHelper.WriteInfo(string.Format("给RGV发送复位指令------{0}：{1}  ", "D1003", 1));
            return true;
        }

        /// <summary>
        /// 急停RGV
        /// </summary>
        /// <returns></returns>
        public bool Stop(out string msg)
        {
            if (!Current.RGV.Plc.SetInfo("D1004", (ushort)1, out msg))
            {
                Error.Alert(msg);
                this.Plc.IsAlive = false;
                return false;
            }

            LogHelper.WriteInfo(string.Format("给RGV发送急停指令------{0}：{1}  ", "D1004", 1));
            return true;
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
