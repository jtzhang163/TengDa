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
    /// 上料机
    /// </summary>
    public class Feeder : TengDa.WF.Terminals.Terminal
    {
        #region 属性字段

        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".Feeder";
                }
                return tableName;
            }
        }

        private TriLamp triLamp = TriLamp.Unknown;

        /// <summary>
        /// 三色灯
        /// </summary>
        [ReadOnly(true), DisplayName("三色灯")]
        public TriLamp TriLamp
        {
            get
            {
                if (!this.IsAlive)
                {
                    triLamp = TriLamp.Unknown;
                }
                return triLamp;
            }
            set
            {
                if (value == TriLamp.Red)
                {
                    this.AlarmStr = this.Name + "报警";
                }
                else
                {
                    this.AlarmStr = "";
                }
                triLamp = value;
            }
        }

        private int plcId = -1;
        [ReadOnly(true), Description("PLC ID")]
        [DisplayName("PLC Id")]
        public int PlcId
        {
            get { return plcId; }
            set { plcId = value; }
        }

        private string scanerIds = string.Empty;
        [ReadOnly(true), Description("扫码枪Id集合")]
        [DisplayName("扫码枪Id集合")]
        public string ScanerIds
        {
            get { return scanerIds; }
            private set { scanerIds = value; }
        }

        private string stationIds = string.Empty;
        [ReadOnly(true), Description("工位Id集合")]
        [DisplayName("工位Id集合")]
        public string StationIds
        {
            get { return stationIds; }
            private set { stationIds = value; }
        }

        private int batteryCacheId = -1;
        [ReadOnly(true)]
        [DisplayName("电池缓存位Id")]
        public int BatteryCacheId
        {
            get { return batteryCacheId; }
            private set { batteryCacheId = value; }
        }

        public int PreCurrentBatteryCount = 0;
        private int currentBatteryCount = -1;
        /// <summary>
        /// 当前入料夹具电芯个数
        /// </summary>
        [DisplayName("当前入料夹具电芯个数")]
        public int CurrentBatteryCount
        {
            get { return currentBatteryCount; }
            set
            {
                if (currentBatteryCount != value)
                {
                    UpdateDbField("CurrentBatteryCount", value);
                }
                currentBatteryCount = value;
            }
        }

        /// <summary>
        /// 夹爪移动类型
        /// </summary>
        [ReadOnly(true)]
        [DisplayName("夹爪移动类型")]
        public JawMoveType JawMoveType { get; set; }

        private string cacheBatteryIdsStr = string.Empty;
        [ReadOnly(true)]
        [DisplayName("缓存电池Id序列")]
        public string CacheBatteryIdsStr
        {
            get { return cacheBatteryIdsStr; }
            private set
            {
                if (cacheBatteryIdsStr != value)
                {
                    UpdateDbField("CacheBatteryIdsStr", value);
                }
                cacheBatteryIdsStr = value;
            }
        }

        /// <summary>
        /// 缓存电池
        /// </summary>
        [ReadOnly(true), DisplayName("缓存电池")]
        public List<Battery> CacheBatteries
        {
            get
            {
                var cacheBatteries = new List<Battery>();

                var batteryIdStrings = CacheBatteryIdsStr.Split(',');
                for (int i = 0; i < batteryIdStrings.Length; i++)
                {
                    cacheBatteries.Add(Battery.GetBattery(TengDa._Convert.StrToInt(batteryIdStrings[i], -1)));
                }

                return cacheBatteries;
            }
            set
            {
                CacheBatteryIdsStr = string.Join(",", Array.ConvertAll<Battery, string>(value.ToArray(), delegate (Battery b) { return b.Id.ToString(); }));
            }
        }

        /// <summary>
        /// 接收到的无夹具信号计数，防止上料机偶尔返回的异常数据导致生成错误任务
        /// </summary>
        private int[] EmptyClampCount = new int[] { 0, 0, 0 };


        /// <summary>
        /// 接收到的满夹具信号计数，防止上料机偶尔返回的异常数据导致生成错误任务
        /// </summary>
        private int[] FillClampCount = new int[] { 0, 0, 0 };


        /// <summary>
        /// 取盘位
        /// </summary>
        [Browsable(false)]
        public Station GetStation
        {
            get
            {
                return Stations[0];
            }
        }

        /// <summary>
        /// 放盘位
        /// </summary>
        [Browsable(false)]
        public Station PutStation
        {
            get
            {
                return Stations[1];
            }
        }

        [Browsable(false)]
        public ushort HeartValue { get; set; }


        private string cacheClampCodes = string.Empty;
        /// <summary>
        /// 缓存夹具条码序列
        /// LXY00054&LXY00055&
        /// </summary>
        [ReadOnly(true)]
        [DisplayName("缓存夹具条码序列")]
        public string CacheClampCodes
        {
            get { return cacheClampCodes; }
            private set
            {
                if (cacheClampCodes != value)
                {
                    UpdateDbField("CacheClampCodes", value);
                }
                cacheClampCodes = value;
            }
        }

        #endregion

        #region 构造方法

        public Feeder() : this(-1) { }

        public Feeder(int id)
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
            this.isEnable = Convert.ToBoolean(rowInfo["IsEnable"]);
            this.stationIds = rowInfo["StationIds"].ToString();
            this.scanerIds = rowInfo["ScanerIds"].ToString();
            this.cacheClampCodes = rowInfo["CacheClampCodes"].ToString();
        }
        #endregion

        #region 系统上料机列表
        private static List<Feeder> feederList = new List<Feeder>();
        public static List<Feeder> FeederList
        {
            get
            {
                if (feederList.Count < 1)
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
                        feederList.Clear();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            Feeder feeder = new Feeder();
                            feeder.InitFields(dt.Rows[i]);
                            feederList.Add(feeder);
                        }
                    }
                }
                return feederList;
            }
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
                if (plc.Id < 1)
                {
                    plc = PLC.PlcList.First(p => p.Id == this.PlcId);
                }
                return plc;
            }
        }
        #endregion

        #region 该设备上的电池缓存位
        private BatteryCache batteryCache = new BatteryCache();
        [Browsable(false)]
        [ReadOnly(true)]
        public BatteryCache BatteryCache
        {
            get
            {
                if (batteryCache.Id < 1)
                {
                    batteryCache = BatteryCache.BatteryCacheList.First(b => b.Id == this.BatteryCacheId);
                }
                return batteryCache;
            }
        }
        #endregion

        #region 该设备上的工位列表
        private List<Station> stations = new List<Station>();
        [Browsable(false)]
        public List<Station> Stations
        {
            get
            {
                if (stations.Count < 1)
                {
                    // stations = Station.StationList.Where(s => Array.IndexOf(this.StationIds.Split(','), s.Id.ToString()) > -1).ToList();
                    stations.Clear();
                    for (int i = 0; i < StationIds.Split(',').Length; i++)
                    {
                        stations.Add(Station.StationList.First(s => s.Id.ToString() == StationIds.Split(',')[i]));
                    }
                }
                return stations;
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

                    #region 获取信息
                    var plcCompany = (PlcCompany)Enum.Parse(typeof(PlcCompany), this.Plc.Company);

                    var addr = Option.LineNum == 1 ? "DB17.0" : "DB17.4";

                    if (!this.Plc.GetInfo(false, plcCompany, true, addr, (ushort)0, out ushort db17_0, out msg))
                    {
                        Error.Alert(msg);
                        this.Plc.IsAlive = false;
                        return false;
                    }
                    this.PutStation.ClampStatus = db17_0 == 2 ? ClampStatus.无夹具 : ClampStatus.满夹具;
                    this.PutStation.Status = db17_0 == 2 ? StationStatus.可放 : StationStatus.工作中;

                    addr = Option.LineNum == 1 ? "DB17.2" : "DB17.6";
                    if (!this.Plc.GetInfo(false, plcCompany, true, addr, (ushort)0, out ushort db17_2, out msg))
                    {
                        Error.Alert(msg);
                        this.Plc.IsAlive = false;
                        return false;
                    }

                    if (db17_2 == 2 && this.GetStation.ClampStatus != ClampStatus.满夹具)
                    {
                        var clampCode = Current.Feeder.PopClampCode();
                        if (!string.IsNullOrEmpty(clampCode))
                        {
                            this.GetStation.ClampId = Clamp.Add(clampCode, out msg);
                        }
                        else
                        {
                            //没有缓存条码时生成随机码
                            this.GetStation.ClampId = Clamp.Add(Guid.NewGuid().ToString().Substring(0, 8).ToUpper(), out msg);
                        }
                    }

                    this.GetStation.ClampStatus = db17_2 == 2 ? ClampStatus.满夹具 : ClampStatus.无夹具;
                    this.GetStation.Status = db17_2 == 2 ? StationStatus.可取 : StationStatus.工作中;

                    //获取夹具扫码信号
                    if (Current.ClampScaner.IsEnable)
                    {
                        addr = Option.LineNum == 1 ? "DB17.8" : "DB17.12";
                        if (!this.Plc.GetInfo(false, plcCompany, true, addr, (ushort)0, out ushort db17_8, out msg))
                        {
                            Error.Alert(msg);
                            this.Plc.IsAlive = false;
                            return false;
                        }

                        if (db17_8 == 2)
                        {
                            if (!Current.ClampScaner.IsReady)
                            {
                                Current.ClampScaner.CanScan = true;
                            }
                            Current.ClampScaner.IsReady = true;
                        }
                        else
                        {
                            Current.ClampScaner.IsReady = false;
                            Current.ClampScaner.CanScan = false;
                        }
                    }


                    addr = Option.LineNum == 1 ? "DB17.28" : "DB17.30";

                    if (!this.Plc.GetInfo(false, plcCompany, false, addr, this.HeartValue, out ushort db17_28, out msg))
                    {
                        Error.Alert(msg);
                        this.Plc.IsAlive = false;
                    }

                    this.HeartValue = this.HeartValue == 0 ? (ushort)2 : (ushort)0;

                    #endregion
                    Thread.Sleep(20);
                }
                catch (Exception ex)
                {
                    Error.Alert(ex);
                }

                this.Plc.IsAlive = true;
                this.AlreadyGetAllInfo = true;
            }

            this.TriLamp = this.Plc.IsAlive ? TriLamp.Green : TriLamp.Unknown;
            return true;
        }

        public bool SetScanClampResult(ScanResult scanResult, out string msg)
        {
            var val = scanResult == ScanResult.OK ? (ushort)2 : (ushort)2;
            var plcCompany = (PlcCompany)Enum.Parse(typeof(PlcCompany), this.Plc.Company);
            var addr = Option.LineNum == 1 ? "DB17.10" : "DB17.14";
            if (!this.Plc.GetInfo(false, plcCompany, false, addr, val, out ushort db17_10, out msg))
            {
                Error.Alert(msg);
                this.Plc.IsAlive = false;
                return false;
            }
            LogHelper.WriteInfo(string.Format("发送扫码结果信号给{0}，{1}：{2}", this.Plc.Name, addr, val));
            return true;
        }

        public void GetFinished()
        {
            var plcCompany = (PlcCompany)Enum.Parse(typeof(PlcCompany), this.Plc.Company);

            var addr = Option.LineNum == 1 ? "DB17.18" : "DB17.22";
            var val = (ushort)2;

            if (!this.Plc.GetInfo(false, plcCompany, false, addr, val, out ushort db17_18, out string msg))
            {
                Error.Alert(msg);
                this.Plc.IsAlive = false;
            }
            LogHelper.WriteInfo(string.Format("发送取完信号给{0}，{1}：{2}", this.Plc.Name, addr, val));
        }

        public void PutFinished()
        {
            var plcCompany = (PlcCompany)Enum.Parse(typeof(PlcCompany), this.Plc.Company);

            var addr = Option.LineNum == 1 ? "DB17.16" : "DB17.20";
            var val = (ushort)2;

            if (!this.Plc.GetInfo(false, plcCompany, false, addr, val, out ushort db17_18, out string msg))
            {
                Error.Alert(msg);
                this.Plc.IsAlive = false;
            }
            LogHelper.WriteInfo(string.Format("发送放完信号给{0}，{1}：{2}", this.Plc.Name, addr, val));
        }

        public void CacheBatteryIn(Battery battery)
        {
            var c = CacheBatteries;
            c.Add(battery);
            CacheBatteries = c;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Battery的id集合字符串</returns>
        public string CacheBatteryOut()
        {
            if (CacheBatteries.Count < 1)
            {
                return "-1";
            }
            string batteryIds = CacheBatteries[0].Id.ToString();
            var c = CacheBatteries;
            var count = c.Count;
            if (count < 4)
            {
                c.RemoveAt(0);
            }
            else
            {
                batteryIds = string.Join(",", Array.ConvertAll<Battery, string>(c.Take(count - 3).ToArray(), delegate (Battery b) { return b.Id.ToString(); }));
                c.RemoveRange(0, count - 3);
            }

            CacheBatteries = c;
            return batteryIds;
        }

        /// <summary>
        /// 往夹具条码缓存中添加条码
        /// </summary>
        /// <param name="code"></param>
        public void PushClampCode(string code)
        {
            if (string.IsNullOrEmpty(code) || code.Length != 8) return;
            if (this.CacheClampCodes.Contains(code)) return;
            if (this.CacheClampCodes.Length > 20) return;
            this.CacheClampCodes = this.CacheClampCodes + code + "&";
        }

        /// <summary>
        /// 从夹具条码缓存中取出条码
        /// </summary>
        /// <returns></returns>
        public string PopClampCode()
        {
            if (this.CacheClampCodes.Length < 9)
            {
                return string.Empty;
            }
            var codes = this.CacheClampCodes;
            var code = codes.Split('&')[0];
            this.CacheClampCodes = codes.Replace(code, "").TrimStart('&');
            return code;
        }

        #endregion
    }

    /// <summary>
    /// 夹爪移动类型
    /// </summary>
    public enum JawMoveType
    {
        /// <summary>
        /// 静止
        /// </summary>
        Motionless = 0,
        /// <summary>
        /// 拉带——>转接位
        /// </summary>
        PulltabToRotary = 1,
        /// <summary>
        /// 拉带——>缓存位
        /// </summary>
        PulltabToBatteryCache = 2,
        /// <summary>
        /// 缓存位——>转接位
        /// </summary>
        BatteryCacheToRotary = 3,
        /// <summary>
        /// 水含量样品——>转接位
        /// </summary>
        SampleToRotary = 4
    }
}
