﻿using System;
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
        /// 就绪
        /// 可进行任务
        /// </summary>
        public bool IsReady { get; set; } = false;

        /// <summary>
        /// RGV状态
        /// 1：为正常运转，2：正常等待，3：停机中，4：异常处理中
        /// </summary>
        public int Status { get; set; }

        public bool IsAuto { get; set; } = false;

        /// <summary>
        /// 调度有效
        /// </summary>
        public bool IsDispatchEnabled { get; set; } = false;

        /// <summary>
        /// 任务结束
        /// </summary>
        public bool IsTaskFinished { get; set; } = false;

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

            //心跳
            if (bOutputs[0] == 0)
            {
                this.Plc.SetInfo("D1000", (ushort)1, out msg);
            }

            //手动自动信号
            this.IsAuto = bOutputs[5] == 2;

            //报警
            this.IsAlarming = bOutputs[11] == 1;
            this.AlarmStr = bOutputs[11] == 1 ? "有异常" : "";

            //有无料
            switch (bOutputs[12])
            {
                case 2: this.ClampStatus = this.ClampStatus == ClampStatus.空夹具 ? ClampStatus.空夹具 : ClampStatus.满夹具; break;
                case 1: this.ClampStatus = ClampStatus.无夹具; break;
                case 3: this.ClampStatus = ClampStatus.异常; break;
                default: this.ClampStatus = ClampStatus.未知; break;
            }

            //调度有效
            this.IsDispatchEnabled = bOutputs[13] == 0;

            //任务完成
            this.IsTaskFinished = bOutputs[15] == 1;

            //X轴位置
            this.CoordinateValue = bOutputs[20];
            if (this.CoordinateValue < this.PreCoordinateValue) { this.MovingDirection = MovingDirection.前进; this.IsMoving = true; }
            else if (this.CoordinateValue > this.PreCoordinateValue) { this.MovingDirection = MovingDirection.后退; this.IsMoving = true; }
            else { this.MovingDirection = MovingDirection.停止; this.IsMoving = false; }
            this.PreCoordinateValue = this.CoordinateValue;

            //rgv状态
            this.Status = bOutputs[60];

            this.IsReady = this.Status == 2 && this.IsDispatchEnabled && this.IsAuto && this.IsTaskFinished;

            this.AlreadyGetAllInfo = true;
            this.Plc.IsAlive = true;
            return true;      
        }

        /// <summary>
        /// 取盘
        /// </summary>
        public bool Get(Station fromStation)
        {
            var msg = "";
            if (!this.Plc.IsPingSuccess)
            {
                LogHelper.WriteError("无法连接到 " + this.Plc.IP);
                return false;
            }

            if (!this.IsReady)
            {
                Tip.Alert("RGV未就绪！");
                return false;
            }

            //清掉任务完成信号
            if (this.IsTaskFinished)
            {
                if (!this.Plc.SetInfo("D1015", (ushort)0, out msg))
                {
                    Error.Alert(msg);
                    this.Plc.IsAlive = false;
                    return false;
                }
            }

            //发送位置编号
            if (!this.Plc.SetInfo("D1009", ushort.Parse(fromStation.RgvValue), out msg))
            {
                Error.Alert(msg);
                this.Plc.IsAlive = false;
                return false;
            }

            Thread.Sleep(30);

            //启动
            return this.Start(out msg);
        }

        /// <summary>
        /// 放盘
        /// </summary>
        public bool Put(Station toStation)
        {
            var msg = "";
            if (!this.Plc.IsPingSuccess)
            {
                LogHelper.WriteError("无法连接到 " + this.Plc.IP);
                return false;
            }

            if (!this.IsReady)
            {
                Tip.Alert("RGV未就绪！");
                return false;
            }

            //清掉任务完成信号
            if (this.IsTaskFinished)
            {
                if (!this.Plc.SetInfo("D1015", (ushort)0, out msg))
                {
                    Error.Alert(msg);
                    this.Plc.IsAlive = false;
                    return false;
                }
            }

            //发送位置编号
            if (!this.Plc.SetInfo("D1008", ushort.Parse(toStation.RgvValue), out msg))
            {
                Error.Alert(msg);
                this.Plc.IsAlive = false;
                return false;
            }

            Thread.Sleep(30);

            //启动
            return this.Start(out msg);
        }


        /// <summary>
        /// 启动RGV
        /// </summary>
        /// <returns></returns>
        public bool Start(out string msg)
        {
            if (!this.Plc.SetInfo("D1001", (ushort)1, out msg))
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
            if (!this.Plc.SetInfo("D1002", (ushort)1, out msg))
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
            if (!this.Plc.SetInfo("D1003", (ushort)1, out msg))
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
            if (!this.Plc.SetInfo("D1004", (ushort)1, out msg))
            {
                Error.Alert(msg);
                this.Plc.IsAlive = false;
                return false;
            }

            LogHelper.WriteInfo(string.Format("给RGV发送急停指令------{0}：{1}  ", "D1004", 1));
            return true;
        }

        /// <summary>
        /// 切换手动/自动
        /// </summary>
        /// <returns></returns>
        public bool TransAutoManu(out string msg)
        {
            var val = (ushort)(this.IsAuto ? 1 : 2);
            if (!this.Plc.SetInfo("D1005", val, out msg))
            {
                Error.Alert(msg);
                this.Plc.IsAlive = false;
                return false;
            }

            LogHelper.WriteInfo(string.Format("给RGV发送切换手动/自动指令------{0}：{1}  ", "D1005", val));
            return true;
        }

        /// <summary>
        /// 控制货叉伸入
        /// </summary>
        /// <returns></returns>
        public bool Enter(out string msg)
        {
            if (!this.Plc.SetInfo("D1010", (ushort)1, out msg))
            {
                Error.Alert(msg);
                this.Plc.IsAlive = false;
                return false;
            }

            LogHelper.WriteInfo(string.Format("给RGV发送急停指令------{0}：{1}  ", "D1010", 1));
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
