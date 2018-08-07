using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TengDa;
using System.Threading;
using System.ComponentModel;
using TengDa.WF;

namespace Tafel.ScanSystem
{
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
        [Browsable(false)]
        public bool PreIsReady { get; set; } = false;

        public string Alarm2BinString = string.Empty;

        public string PreAlarm2BinString = string.Empty;

        public TriLamp triLamp = TriLamp.Unknown;

        private string plcIds = string.Empty;
        [ReadOnly(true)]
        public string PlcIds
        {
            get { return plcIds; }
            private set { plcIds = value; }
        }

        private string stationIds = string.Empty;
        [ReadOnly(true)]
        public string StationIds
        {
            get { return stationIds; }
            private set { stationIds = value; }
        }

        [ReadOnly(true)]
        [DisplayName("电池是否到位")]
        public bool IsReady { get; set; } = false;

        /// <summary>
        /// 当前扫描NG已累计次数
        /// </summary>
        [Browsable(false)]
        public int CurrentScanNgCount { get; set; }

        /// <summary>
        /// 扫描NG总次数设置
        /// </summary>
        [Browsable(false)]
        public int ScanNgCount { get { return Current.option.ScanNgCount; } }

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
            this.plcIds = rowInfo["PlcIds"].ToString();
            this.company = rowInfo["Company"].ToString();
            this.model = rowInfo["Model"].ToString();
            this.number = rowInfo["Number"].ToString();
            this.isEnable = Convert.ToBoolean(rowInfo["IsEnable"]);
            this.StationIds = rowInfo["StationIds"].ToString();
        }
        #endregion

        #region 该设备上的PLC列表
        private List<PLC> plcs = new List<PLC>();

        [Browsable(false)]
        [ReadOnly(true)]
        public List<PLC> Plcs
        {
            get
            {
                if (plcs.Count < 1)
                {
                    List<PLC> list = new List<PLC>();
                    string[] plcIds = PlcIds.Split(',');
                    for (int i = 0; i < plcIds.Length; i++)
                    {
                        PLC plc = new PLC(TengDa._Convert.StrToInt(plcIds[i], -1));
                        list.Add(plc);
                    }
                    plcs = list;
                }
                return plcs;
            }
        }
        #endregion

        #region 该设备上的工位列表
        private List<Station> stations = new List<Station>();
        [Browsable(false)]
        [ReadOnly(true)]
        public List<Station> Stations
        {
            get
            {
                if(stations.Count < 1)
                {
                    stations = Station.StationList.Where(s => Array.IndexOf(this.StationIds.Split(','), s.Id.ToString()) > -1).ToList();
                }
                return stations;
            }
        }
        #endregion

        #region 通信
        public bool GetInfo()
        {
            if (!this.Plcs[0].IsPingSuccess)
            {
                IsAlive = false;
                LogHelper.WriteError("无法连接到 " + this.Plcs[0].IP);
                return false;
            }

            this.Plcs[0].IsAlive = true;

            string msg = string.Empty;
            string output = string.Empty;
            try
            {
                if (this.Plcs[0].GetInfo(false, Current.option.GetInfoStr, out output, out msg))
                {
                    if (output.Substring(3, 1) == "$")
                    {

                        this.IsReady = output.Substring(6, 1) == "1";

                        for (int j = 0; j < Stations.Count; j++)
                        {
                            this.Stations[j].IsEnable = output.Substring(7 + j, 1) != "1";
                        }

                        return true;
                    }
                    else
                    {
                        LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.GetInfoStr, output));
                    }

                }
                else
                {
                    Error.Alert(msg);
                }

            }
            catch (Exception ex)
            {
                Error.Alert(ex);
            }
            //this.IsReady = false;
            return false;
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
            if (this.Plcs[0].GetInfo(input, out output, out msg))
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
