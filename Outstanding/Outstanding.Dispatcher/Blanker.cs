using System;
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
    /// 下料机/冷却炉
    /// </summary>
    public class Blanker : TengDa.WF.Terminals.Terminal
    {
        #region 属性字段

        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".Blankers";
                }
                return tableName;
            }
        }

        private int rotaterId = -1;
        [ReadOnly(true), Description("旋转台ID")]
        [DisplayName("旋转台ID")]
        public int RotaterId
        {
            get { return rotaterId; }
            set { rotaterId = value; }
        }

        private int cacheId = -1;
        [ReadOnly(true), Description("缓存架ID")]
        [DisplayName("缓存架ID")]
        public int CacheId
        {
            get { return cacheId; }
            set { cacheId = value; }
        }

        private int plcId = -1;
        [ReadOnly(true), Description("PLC ID")]
        [DisplayName("PLC ID")]
        public int PlcId
        {
            get { return plcId; }
            set { plcId = value; }
        }

        private string stationIds = string.Empty;
        [ReadOnly(true), Description("工位ID集合")]
        [DisplayName("工位ID集合")]
        public string StationIds
        {
            get { return stationIds; }
            private set { stationIds = value; }
        }

        #endregion

        #region 构造方法

        public Blanker() : this(-1) { }

        public Blanker(int id)
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
            this.rotaterId = TengDa._Convert.StrToInt(rowInfo["RotaterId"].ToString(), -1);
            this.cacheId = TengDa._Convert.StrToInt(rowInfo["CacheId"].ToString(), -1);
            this.company = rowInfo["Company"].ToString();
            this.model = rowInfo["Model"].ToString();
            this.number = rowInfo["Number"].ToString();
            this.isEnable = Convert.ToBoolean(rowInfo["IsEnable"]);
            this.StationIds = rowInfo["StationIds"].ToString();
        }
        #endregion

        #region 系统下料机列表
        private static List<Blanker> blankerList = new List<Blanker>();
        public static List<Blanker> BlankerList
        {
            get
            {
                if (blankerList.Count < 1)
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
                        blankerList.Clear();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            Blanker blanker = new Blanker();
                            blanker.InitFields(dt.Rows[i]);
                            blankerList.Add(blanker);
                        }
                    }
                }
                return blankerList;
            }
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

        #region 该设备上的旋转台
        [Browsable(false)]
        [ReadOnly(true)]
        public Rotater Rotater
        {
            get
            {
                return Rotater.RotaterList.FirstOrDefault(r => r.Id == this.RotaterId);
            }
        }
        #endregion

        #region 该设备上的缓存架
        [Browsable(false)]
        [ReadOnly(true)]
        public Cache Cache
        {
            get
            {
                return Cache.CacheList.FirstOrDefault(c => c.Id == this.CacheId);
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
            if (!this.Plc.IsPingSuccess)
            {
                if (this.RotaterId > 0)
                {
                    this.Rotater.IsAlive = false;
                }
                this.Plc.IsAlive = false;
                LogHelper.WriteError("无法连接到 " + this.Plc.IP);
                return false;
            }

            string msg = string.Empty;
            string output = string.Empty;

            try
            {
                if (!this.Plc.GetInfo(false, Current.option.GetBlankerInfoStr, out output, out msg))
                {
                    Error.Alert(msg);
                    this.Rotater.IsAlive = false;
                    this.Plc.IsAlive = false;
                    return false;
                }
                if (output.Substring(3, 1) != "$")
                {
                    LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.GetBlankerInfoStr, output));
                    return false;
                }

                int[] iOut = new int[13];
                output = PanasonicPLC.ConvertHexStr(output.TrimEnd('\r'), false);
                for (int j = 0; j < iOut.Length; j++)
                {
                    iOut[j] = int.Parse(output.Substring(j * 4, 4), System.Globalization.NumberStyles.AllowHexSpecifier);
                }

                for (int j = 0; j < this.Stations.Count; j++)
                {
                    switch (iOut[j])
                    {
                        case 1:
                            this.Stations[j].ClampStatus = ClampStatus.无夹具;
                            this.Stations[j].Status = StationStatus.可放;
                            break;
                        case 2:
                            this.Stations[j].ClampStatus = ClampStatus.满夹具;
                            this.Stations[j].Status = StationStatus.工作中;
                            break;
                        case 3:
                            this.Stations[j].ClampStatus = ClampStatus.空夹具;
                            this.Stations[j].Status = StationStatus.可取;
                            break;
                        default:
                            this.Stations[j].ClampStatus = ClampStatus.未知;
                            this.Stations[j].Status = StationStatus.不可用;
                            break;
                    }
                }

                if (this.RotaterId > 0)
                {
                    switch (iOut[6])
                    {
                        case 1: this.Rotater.Station.ClampStatus = ClampStatus.无夹具; break;
                        case 2:
                        case 3: this.Rotater.Station.ClampStatus = ClampStatus.空夹具; break;
                        default: this.Rotater.Station.ClampStatus = ClampStatus.异常; break;
                    }

                    switch (iOut[7])
                    {
                        case 1: this.Rotater.Station.DoorStatus = DoorStatus.打开; break;
                        case 2: this.Rotater.Station.DoorStatus = DoorStatus.关闭; break;
                        default: this.Rotater.Station.DoorStatus = DoorStatus.异常; break;
                    }

                    if (this.Rotater.Station.ClampStatus == ClampStatus.无夹具 && this.Rotater.Station.DoorStatus == DoorStatus.打开)
                    {
                        this.Rotater.Station.Status = StationStatus.可放;
                    }
                    else if (this.Rotater.Station.ClampStatus == ClampStatus.空夹具 && this.Rotater.Station.DoorStatus == DoorStatus.打开)
                    {
                        this.Rotater.Station.Status = StationStatus.可取;
                    }
                    else
                    {
                        this.Rotater.Station.Status = StationStatus.工作中;
                    }

                    ClampOri clampOri = this.Stations[0].ClampOri;
                    switch (iOut[12])
                    {
                        case 1: this.Rotater.Station.ClampOri = clampOri == ClampOri.A ? ClampOri.B : ClampOri.A; break;
                        case 2: this.Rotater.Station.ClampOri = clampOri; break;
                        default: this.Rotater.Station.ClampOri = ClampOri.未知; break;
                    }

                    #region 控制开门
                    if (this.Rotater.Station.toOpenDoor)
                    {
                        output = string.Empty;
                        if (!this.Plc.GetInfo(false, Current.option.RotaterOpenDoorStr, out output, out msg))
                        {
                            Error.Alert(msg);
                            this.Plc.IsAlive = false;
                            return false;
                        }

                        if (output.Substring(3, 1) != "$")
                        {
                            LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.RotaterOpenDoorStr, output));
                            return false;
                        }
                        LogHelper.WriteInfo(string.Format("成功发送旋转台开门指令到{0}:{1}", this.Rotater.Station.Name, Current.option.RotaterOpenDoorStr));
                        this.Rotater.Station.toOpenDoor = false;
                    }
                    #endregion

                    #region 控制关门
                    if (this.Rotater.Station.toCloseDoor)
                    {
                        output = string.Empty;
                        if (!this.Plc.GetInfo(false, Current.option.RotaterCloseDoorStr, out output, out msg))
                        {
                            Error.Alert(msg);
                            this.Plc.IsAlive = false;
                            return false;
                        }

                        if (output.Substring(3, 1) != "$")
                        {
                            LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.RotaterCloseDoorStr, output));
                            return false;
                        }
                        LogHelper.WriteInfo(string.Format("成功发送旋转台关门指令到{0}:{1}", this.Rotater.Station.Name, Current.option.RotaterCloseDoorStr));
                        this.Rotater.Station.toCloseDoor = false;
                    }

                    #endregion

                    #region 控制旋转

                    for (int x = 0; x < 2; x++)
                    {
                        if (this.Rotater.Station.toRotate[x])
                        {
                            output = string.Empty;
                            if (!this.Plc.GetInfo(false, Current.option.RotaterRotateStrs.Split(',')[x], out output, out msg))
                            {
                                Error.Alert(msg);
                                this.Plc.IsAlive = false;
                                return false;
                            }

                            if (output.Substring(3, 1) != "$")
                            {
                                LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.RotaterRotateStrs.Split(',')[x], output));
                                return false;
                            }
                            LogHelper.WriteInfo(string.Format("成功发送旋转台旋转指令到{0}:{1}", this.Rotater.Station.Name, Current.option.RotaterRotateStrs.Split(',')[x]));
                            this.Rotater.Station.toRotate[x] = false;
                        }
                    }
                    #endregion
                }

                if (this.CacheId > 0)
                {
                    for (int j = 0; j < this.Cache.Stations.Count; j++)
                    {
                        switch (iOut[8 + j])
                        {
                            case 1:
                                this.Cache.Stations[j].ClampStatus = ClampStatus.无夹具;
                                this.Cache.Stations[j].Status = StationStatus.可放;
                                break;
                            case 2:
                                this.Cache.Stations[j].ClampStatus = this.Cache.Stations[j].Clamp.Batteries.Count > 0 ? ClampStatus.满夹具 : ClampStatus.空夹具;
                                this.Cache.Stations[j].Status = StationStatus.工作中;
                                break;
                            case 3:
                                this.Cache.Stations[j].ClampStatus = this.Cache.Stations[j].Clamp.Batteries.Count > 0 ? ClampStatus.满夹具 : ClampStatus.空夹具;
                                this.Cache.Stations[j].Status = StationStatus.可取;
                                break;
                            case 4:
                                this.Cache.Stations[j].ClampStatus = ClampStatus.异常;
                                this.Cache.Stations[j].Status = StationStatus.不可用;
                                break;
                            default:
                                this.Cache.Stations[j].ClampStatus = ClampStatus.未知;
                                this.Cache.Stations[j].Status = StationStatus.不可用;
                                break;
                        }
                        this.Cache.Stations[j].Status = StationStatus.工作中;
                    }
                }

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

        public bool SetScanResult(ScanResult scanResult)
        {
            string input = string.Empty;
            if (scanResult == ScanResult.OK)
            {
                input = Current.option.SendScanOkStr;
            }
            else if (scanResult == ScanResult.NG)
            {
                input = Current.option.SendScanNgStr;
            }
            else
            {
                throw new ArgumentOutOfRangeException("bool SetScanResult(ScanResult scanResult) 参数值与预期不符！");
            }

            string output = string.Empty;
            string msg = string.Empty;
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
}
