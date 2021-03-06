﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using TengDa;
using TengDa.WF;

namespace Outstanding.Dispatcher
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
                    tableName = Config.DbTableNamePre + ".Feeders";
                }
                return tableName;
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

        private int currentPutStationId = -1;
        /// <summary>
        /// 当前入料夹具Id
        /// </summary>
        [ReadOnly(true)]
        [DisplayName("当前入料夹具Id")]
        public int CurrentPutStationId
        {
            get { return currentPutStationId; }
            set
            {
                if (currentPutStationId != value)
                {
                    UpdateDbField("CurrentPutStationId", value);
                }
                currentPutStationId = value;
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
        /// 接收到空夹具的信息计数，防止上料机急停返回的异常数据导致生成错误指令
        /// </summary>
        private int[] EmptyClampCount = new int[] { 0, 0 };

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
            this.batteryCacheId = TengDa._Convert.StrToInt(rowInfo["BatteryCacheId"].ToString(), -1);
            this.currentPutStationId = TengDa._Convert.StrToInt(rowInfo["CurrentPutStationId"].ToString(), -1);
            this.cacheBatteryIdsStr = rowInfo["CacheBatteryIdsStr"].ToString();
            this.currentBatteryCount = TengDa._Convert.StrToInt(rowInfo["CurrentBatteryCount"].ToString(), 0);
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
                    plc = new PLC(PlcId);
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

        #region 该设备上的扫码枪
        private List<Scaner> scaners = new List<Scaner>();
        [Browsable(false)]
        public List<Scaner> Scaners
        {
            get
            {
                if (scaners.Count < 1)
                {
                    scaners = Scaner.ScanerList.Where(s => Array.IndexOf(this.ScanerIds.Split(','), s.Id.ToString()) > -1).ToList();
                }
                return scaners;
            }
        }
        [Browsable(false)]
        public Scaner BatteryScaner
        {
            get
            {
                if (this.Scaners.Count > 0)
                {
                    return this.Scaners[0];
                }
                return new Scaner();
            }
        }
        [Browsable(false)]
        public Scaner ClampScaner
        {
            get
            {
                if (this.Scaners.Count > 1)
                {
                    return this.Scaners[1];
                }
                return new Scaner();
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
                if (!this.Plc.GetInfo(false, Current.option.GetFeederInfoStr, out output, out msg))
                {
                    Error.Alert(msg);
                    this.Plc.IsAlive = false;
                    return false;
                }
                if (output.Substring(3, 1) != "$")
                {
                    LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.GetFeederInfoStr, output));
                    return false;
                }

                int[] iOut = new int[10];
                output = PanasonicPLC.ConvertHexStr(output.TrimEnd('\r'), false);
                for (int j = 0; j < iOut.Length; j++)
                {
                    iOut[j] = int.Parse(output.Substring(j * 4, 4), System.Globalization.NumberStyles.AllowHexSpecifier);
                }

                for (int j = 0; j < this.Stations.Count; j++)
                {
                    switch (iOut[j])
                    {
                        case 1: this.EmptyClampCount[j]++; break;
                        case 2: this.Stations[j].ClampStatus = ClampStatus.空夹具; break;
                        case 3: this.Stations[j].ClampStatus = ClampStatus.满夹具; this.Stations[j].SampleStatus = SampleStatus.非样品位; break;
                        case 31: this.Stations[j].ClampStatus = ClampStatus.满夹具; this.Stations[j].SampleStatus = SampleStatus.样品位; break;
                        case 4: this.Stations[j].ClampStatus = ClampStatus.异常; break;
                        default: this.Stations[j].ClampStatus = ClampStatus.未知; break;
                    }

                    if (iOut[j] == 1)
                    {
                        if(EmptyClampCount[j] > 2)
                        {
                            this.Stations[j].ClampStatus = ClampStatus.无夹具;
                            EmptyClampCount[j] = 3;
                        }
                    }
                    else
                    {
                        EmptyClampCount[j] = 0;
                    }          

                    switch (iOut[j + 2])
                    {
                        case 1: this.Stations[j].DoorStatus = DoorStatus.打开; break;
                        case 2: this.Stations[j].DoorStatus = DoorStatus.关闭; break;
                        case 4: this.Stations[j].DoorStatus = DoorStatus.异常; break;
                        default: this.Stations[j].DoorStatus = DoorStatus.未知; break;
                    }

                    if (this.Stations[j].ClampStatus == ClampStatus.无夹具)
                    {
                        this.Stations[j].Status = StationStatus.可放;
                    }
                    else if (this.Stations[j].ClampStatus == ClampStatus.满夹具)
                    {
                        this.Stations[j].Status = StationStatus.可取;
                    }
                    else if (this.Stations[j].ClampStatus == ClampStatus.空夹具)
                    {
                        this.Stations[j].Status = StationStatus.工作中;
                    }
                    else
                    {
                        this.Stations[j].Status = StationStatus.不可用;
                    }
                }

                switch (iOut[5])
                {
                    case 0: this.Stations.ForEach(s => s.IsClampScanReady = false); this.ClampScaner.IsReady = false; break;
                    case 1: this.Stations[0].IsClampScanReady = true; this.ClampScaner.IsReady = true; this.CurrentPutStationId = this.Stations[0].Id; break;
                    case 2: this.Stations[1].IsClampScanReady = true; this.ClampScaner.IsReady = true; this.CurrentPutStationId = this.Stations[1].Id; break;
                }

                if (iOut[6] == 1)
                {
                    if (!this.BatteryScaner.IsReady)
                    {
                        this.BatteryScaner.CanScan = true;
                    }
                    this.BatteryScaner.IsReady = true;
                }
                else
                {
                    this.BatteryScaner.IsReady = false;
                    this.BatteryScaner.CanScan = false;
                }


                switch (iOut[7])
                {
                    case 0:
                        if (this.JawMoveType == JawMoveType.PulltabToRotary)
                        {
                            //if(this.CurrentPutClampBatteryCount < Clamp.BatteryCount)
                            //{
                            if (!Battery.UpdateClampId(this.CacheBatteryOut(), Station.GetStation(CurrentPutStationId).ClampId, out msg))
                            {
                                LogHelper.WriteError(msg);
                            }
                            //}
                        }
                        else if (this.JawMoveType == JawMoveType.PulltabToBatteryCache)
                        {
                            this.BatteryCache.Push(this.CacheBatteryOut());
                        }
                        else if (this.JawMoveType == JawMoveType.BatteryCacheToRotary)
                        {
                            if (this.CurrentBatteryCount < Clamp.BatteryCount)
                            {
                                if (!Battery.UpdateClampId(this.BatteryCache.Pop().ToString(), Station.GetStation(CurrentPutStationId).ClampId, out msg))
                                {
                                    LogHelper.WriteError(msg);
                                }
                            }
                        }
                        else if (this.JawMoveType == JawMoveType.SampleToRotary)
                        {
                            ///

                        }
                        this.JawMoveType = JawMoveType.Motionless;
                        break;
                    case 1: this.JawMoveType = JawMoveType.PulltabToRotary; break;
                    case 2: this.JawMoveType = JawMoveType.PulltabToBatteryCache; break;
                    case 3: this.JawMoveType = JawMoveType.BatteryCacheToRotary; break;
                    case 4: this.JawMoveType = JawMoveType.SampleToRotary; break;
                }

                this.CurrentBatteryCount = iOut[8];

                this.BatteryCache.SetCount(iOut[9]);

                #region 写指令 控制开关门
                for (int j = 0; j < this.Stations.Count; j++)
                {
                    #region 控制开门
                    if (this.Stations[j].toOpenDoor)
                    {
                        output = string.Empty;
                        if (!this.Plc.GetInfo(false, Current.option.OpenFeederDoorStrs.Split(',')[j], out output, out msg))
                        {
                            Error.Alert(msg);
                            this.Plc.IsAlive = false;
                            return false;
                        }

                        if (output.Substring(3, 1) != "$")
                        {
                            LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.OpenOvenDoorStrs.Split(',')[j], output));
                            return false;
                        }
                        LogHelper.WriteInfo(string.Format("成功发送开门指令到{0}:{1}", this.Stations[j].Name, Current.option.OpenOvenDoorStrs.Split(',')[j]));
                        this.Stations[j].toOpenDoor = false;
                    }
                    #endregion

                    #region 控制关门
                    if (this.Stations[j].toCloseDoor)
                    {
                        output = string.Empty;
                        if (!this.Plc.GetInfo(false, Current.option.CloseFeederDoorStrs.Split(',')[j], out output, out msg))
                        {
                            Error.Alert(msg);
                            this.Plc.IsAlive = false;
                            return false;
                        }

                        if (output.Substring(3, 1) != "$")
                        {
                            LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.CloseOvenDoorStrs.Split(',')[j], output));
                            return false;
                        }
                        LogHelper.WriteInfo(string.Format("成功发送关门指令到{0}:{1}", this.Stations[j].Name, Current.option.CloseOvenDoorStrs.Split(',')[j]));
                        this.Stations[j].toCloseDoor = false;
                    }
                    #endregion

                }
                #endregion

                Thread.Sleep(100);
            }
            catch (Exception ex)
            {
                Error.Alert(ex);
            }

            this.Plc.IsAlive = true;

            this.AlreadyGetAllInfo = true;

            return true;
        }

        /// <summary>
        /// 开门指令
        /// </summary>
        /// <param name="j">工位序号</param>
        /// <returns></returns>
        public void OpenDoor(int j)
        {

            if (!this.Plc.IsPingSuccess)
            {
                IsAlive = false;
                LogHelper.WriteError("无法连接到 " + this.Plc.IP);
                return;
            }

            this.Stations[j].toOpenDoor = true;
        }

        /// <summary>
        /// 关门指令
        /// </summary>
        /// <param name="j">工位序号</param>
        /// <returns></returns>
        public void CloseDoor(int j)
        {
            if (!this.Plc.IsPingSuccess)
            {
                IsAlive = false;
                LogHelper.WriteError("无法连接到 " + this.Plc.IP);
                return;
            }
            this.Stations[j].toCloseDoor = true;
        }

        public bool SetScanClampResultOK(out string msg)
        {
            string input = Current.option.ScanClampCodeOK;
            string output = string.Empty;
            if (this.Plc.GetInfo(input, out output, out msg))
            {
                if (output.Substring(3, 1) == "$")
                {
                    return true;
                }
                else
                {
                    LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", input, output));
                }
            }
            return false;
        }

        public bool SetScanBatteryResult(ScanResult scanResult, out string msg)
        {
            string input = string.Empty;
            if (scanResult == ScanResult.OK)
            {
                input = Current.option.ScanBatteryCodeOK;
            }
            else if (scanResult == ScanResult.NG)
            {
                input = Current.option.ScanBatteryCodeNG;
            }
            else
            {
                throw new ArgumentOutOfRangeException("bool SetScanResult(ScanResult scanResult) 参数值与预期不符！");
            }

            string output = string.Empty;
            if (this.Plc.GetInfo(input, out output, out msg))
            {
                if (output.Substring(3, 1) == "$")
                {
                    return true;
                }
                else
                {
                    LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", input, output));
                }
            }
            return false;
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
