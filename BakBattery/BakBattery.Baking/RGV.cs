using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using TengDa;
using TengDa.WF;

namespace BakBattery.Baking
{
    /// <summary>
    /// RGV搬运车
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
                    tableName = Config.DbTableNamePre + ".RGVs";
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
            set { clampId = value; }
        }

        private int position = 1;

        [ReadOnly(true)]
        [DisplayName("当前位置")]
        public int Position
        {
            get
            {
                if (position > 19)
                {
                    return 19;
                }
                else if (position < 1)
                {
                    return 1;
                }
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

        [ReadOnly(true), DisplayName("目标标识1：D3410")]
        public int D3410 { get; set; } = -1;

        [ReadOnly(true), DisplayName("目标标识2：D3411")]
        public int D3411 { get; set; } = -1;

        [ReadOnly(true), DisplayName("准备取盘：M70")]
        public bool M70 { get; set; } = false;

        [ReadOnly(true), DisplayName("准备放盘：M71")]
        public bool M71 { get; set; } = false;

        [ReadOnly(true), DisplayName("伸出货叉：M76")]
        public bool M76 { get; set; } = false;

        [ReadOnly(true), DisplayName("可发送取盘指令")]
        public bool IsReadyGet { get; set; } = false;

        [ReadOnly(true), DisplayName("可发送放盘指令")]
        public bool IsReadyPut { get; set; } = false;
        /// <summary>
        /// RGV货叉正在运动
        /// </summary>
        [ReadOnly(true), DisplayName("RGV货叉正在运动")]
        public bool IsGettingOrPutting { get; set; } = false;

        [ReadOnly(true), DisplayName("RGV正在移动")]
        public bool IsMoving { get; set; } = false;

        [ReadOnly(true), DisplayName("可确认是否取放夹具到位")]
        public bool CanCheckGetPutClampIsOk { get; set; } = false;

        [ReadOnly(true), DisplayName("可确认放夹具到位信号出现次数")]
        public int CanCheckPutClampIsOkCount { get; set; } = 0;

        [DisplayName("X轴坐标：D0500")]
        public int D0500 { get; set; } = -1;

        public int PreD0500 = -1;

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

        [Browsable(false)]
        [ReadOnly(true)]
        public PLC Plc
        {
            get
            {
                return PLC.PlcList.First(p => p.Id == this.PlcId);
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

            string msg = string.Empty;
            string output = string.Empty;

            try
            {

                int d0500 = -1;
                if (!this.Plc.GetInfo(false, PlcCompany.Mitsubishi, true, Current.option.RgvXasixAddress, 0, out d0500, out msg))
                {
                    Error.Alert(msg);
                    this.Plc.IsAlive = false;
                    return false;
                }
                this.D0500 = d0500;

                RgvPosition rp = RgvPosition.RgvPositionList.FirstOrDefault(r => r.XMinValue < this.D0500 && r.XMaxValue > this.D0500);

                this.Position = rp == null ? this.position : rp.Position;

                if (this.D0500 < this.PreD0500) { this.MovingDirection = MovingDirection.前进; }
                else if (this.D0500 > this.PreD0500) { this.MovingDirection = MovingDirection.后退; }
                else { this.MovingDirection = MovingDirection.停止; }

                this.PreD0500 = this.D0500;

                bool isNotGettingOrPutting = false;
                if (!this.Plc.GetInfo(false, PlcCompany.Mitsubishi, true, Current.option.RgvIsGettingOrPuttingAdd, false, out isNotGettingOrPutting, out msg))
                {
                    Error.Alert(msg);
                    this.Plc.IsAlive = false;
                    return false;
                }

                this.IsGettingOrPutting = !isNotGettingOrPutting;

                bool isMoving = false;
                if (!this.Plc.GetInfo(false, PlcCompany.Mitsubishi, true, Current.option.RgvIsMovingAdd, false, out isMoving, out msg))
                {
                    Error.Alert(msg);
                    this.Plc.IsAlive = false;
                    return false;
                }
                this.IsMoving = isMoving;

                bool isReadyGet = false;
                if (!this.Plc.GetInfo(false, PlcCompany.Mitsubishi, true, Current.option.RgvIsReadyGetPutAdds.Split(',')[0], false, out isReadyGet, out msg))
                {
                    Error.Alert(msg);
                    this.Plc.IsAlive = false;
                    return false;
                }
                this.IsReadyGet = isReadyGet;

                bool isReadyPut = false;
                if (!this.Plc.GetInfo(false, PlcCompany.Mitsubishi, true, Current.option.RgvIsReadyGetPutAdds.Split(',')[1], false, out isReadyPut, out msg))
                {
                    Error.Alert(msg);
                    this.Plc.IsAlive = false;
                    return false;
                }
                this.IsReadyPut = isReadyPut;


                bool canCheckPutClampIsOk = false;
                if (!this.Plc.GetInfo(false, PlcCompany.Mitsubishi, true, Current.option.CanCheckPutClampIsOkAdd, false, out canCheckPutClampIsOk, out msg))
                {
                    Error.Alert(msg);
                    this.Plc.IsAlive = false;
                    return false;
                }

                if (canCheckPutClampIsOk)
                {
                    this.CanCheckPutClampIsOkCount++;
                }
                else
                {
                    this.CanCheckPutClampIsOkCount = 0;
                }

                this.CanCheckGetPutClampIsOk = CanCheckPutClampIsOkCount > 1;

                int d3410 = -1;
                if (!this.Plc.GetInfo(false, PlcCompany.Mitsubishi, true, Current.option.RgvToPositionAdds.Split(',')[0], 0, out d3410, out msg))
                {
                    Error.Alert(msg);
                    this.Plc.IsAlive = false;
                    return false;
                }
                this.D3410 = d3410;


                int d3411 = -1;
                if (!this.Plc.GetInfo(false, PlcCompany.Mitsubishi, true, Current.option.RgvToPositionAdds.Split(',')[1], 0, out d3411, out msg))
                {
                    Error.Alert(msg);
                    this.Plc.IsAlive = false;
                    return false;
                }
                this.D3411 = d3411;

                bool m70 = false;
                if (!this.Plc.GetInfo(false, PlcCompany.Mitsubishi, true, Current.option.RgvToGetPutAdds.Split(',')[0], false, out m70, out msg))
                {
                    Error.Alert(msg);
                    this.Plc.IsAlive = false;
                    return false;
                }
                this.M70 = m70;

                bool m71 = false;
                if (!this.Plc.GetInfo(false, PlcCompany.Mitsubishi, true, Current.option.RgvToGetPutAdds.Split(',')[0], false, out m71, out msg))
                {
                    Error.Alert(msg);
                    this.Plc.IsAlive = false;
                    return false;
                }
                this.M71 = m71;

                bool m76 = false;
                if (!this.Plc.GetInfo(false, PlcCompany.Mitsubishi, true, Current.option.RgvStartGetPutAdd, false, out m76, out msg))
                {
                    Error.Alert(msg);
                    this.Plc.IsAlive = false;
                    return false;
                }
                this.M76 = m76;

                System.Threading.Thread.Sleep(100);

            }
            catch (Exception ex)
            {
                Error.Alert(ex);
            }

            this.Plc.IsAlive = true;
            this.AlreadyGetAllInfo = true;
            return true;
        }

        public bool Move(int D3410, int D3411, bool isGet)
        {
            if (!this.Plc.IsPingSuccess)
            {
                LogHelper.WriteError("无法连接到 " + this.Plc.IP);
                return false;
            }

            string msg = string.Empty;
            try
            {
                int o1 = 0;
                if (!this.Plc.GetInfo(false, PlcCompany.Mitsubishi, false, Current.option.RgvToPositionAdds.Split(',')[0], D3410, out o1, out msg))
                {
                    Error.Alert(msg);
                    this.Plc.IsAlive = false;
                    return false;
                }


                int o2 = 0;
                if (!this.Plc.GetInfo(false, PlcCompany.Mitsubishi, false, Current.option.RgvToPositionAdds.Split(',')[1], D3411, out o2, out msg))
                {
                    Error.Alert(msg);
                    this.Plc.IsAlive = false;
                    return false;
                }

                bool m = false;
                if (!this.Plc.GetInfo(false, PlcCompany.Mitsubishi, false, isGet ? Current.option.RgvToGetPutAdds.Split(',')[0] : Current.option.RgvToGetPutAdds.Split(',')[1], true, out m, out msg))
                {
                    Error.Alert(msg);
                    this.Plc.IsAlive = false;
                    return false;
                }

                LogHelper.WriteInfo(string.Format("给RGV发送到位取放指令------D3410：{0}   D3411：{1}   {2}：true  ", D3410, D3411, isGet ? Current.option.RgvToGetPutAdds.Split(',')[0] : Current.option.RgvToGetPutAdds.Split(',')[1]));
            }
            catch (Exception ex)
            {
                Error.Alert(ex);
            }

            return true;
        }

        public bool StartGetPut()
        {
            if (!this.Plc.IsPingSuccess)
            {
                LogHelper.WriteError("无法连接到 " + this.Plc.IP);
                return false;
            }

            //if (this.M76)
            //{
            //    return true;
            //}

            string msg = string.Empty;
            try
            {
                bool m = false;
                if (!this.Plc.GetInfo(false, PlcCompany.Mitsubishi, false, Current.option.RgvStartGetPutAdd, true, out m, out msg))
                {
                    Error.Alert(msg);
                    this.Plc.IsAlive = false;
                    return false;
                }
                LogHelper.WriteInfo(string.Format("给RGV发送开始取放指令------{0}：true  ", Current.option.RgvStartGetPutAdd));
            }
            catch (Exception ex)
            {
                Error.Alert(ex);
            }

            return true;
        }

        public bool PutClampIsNotOkAlarm()
        {
            if (!this.Plc.IsPingSuccess)
            {
                LogHelper.WriteError("无法连接到 " + this.Plc.IP);
                return false;
            }

            string msg = string.Empty;
            try
            {
                bool m = false;
                if (!this.Plc.GetInfo(false, PlcCompany.Mitsubishi, false, Current.option.PutClampIsNotOkAlarmAdd, true, out m, out msg))
                {
                    Error.Alert(msg);
                    this.Plc.IsAlive = false;
                    return false;
                }
                LogHelper.WriteInfo(string.Format("给RGV发送报警指令------{0}：true  ", Current.option.PutClampIsNotOkAlarmAdd));
            }
            catch (Exception ex)
            {
                Error.Alert(ex);
            }

            return true;
        }
        #endregion
    }

    public class RgvPosition : Service
    {
        #region 属性字段

        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".RgvPositions";
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

        public RgvPosition() : this(-1) { }

        public RgvPosition(int id)
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
        private static List<RgvPosition> rgvPositionList = new List<RgvPosition>();
        public static List<RgvPosition> RgvPositionList
        {
            get
            {
                if (rgvPositionList.Count < 1)
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
                            RgvPosition rgvPosition = new RgvPosition();
                            rgvPosition.InitFields(dt.Rows[i]);
                            rgvPositionList.Add(rgvPosition);
                        }
                    }
                }
                return rgvPositionList;
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
