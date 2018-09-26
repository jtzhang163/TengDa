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
    /// 旋转机构
    /// </summary>
    public class Rotater : TengDa.WF.Terminals.Terminal
    {
        #region 属性字段

        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".Rotaters";
                }
                return tableName;
            }
        }


        private int stationId = -1;
        [ReadOnly(true), Description("工位Id")]
        [DisplayName("工位Id")]
        public int StationId
        {
            get { return stationId; }
            private set { stationId = value; }
        }

        //[Browsable(false)]
        //public bool PreIsReady { get; set; } = false;

        //public string Alarm2BinString = string.Empty;

        //public string PreAlarm2BinString = string.Empty;

        //public TriLamp triLamp = TriLamp.Unknown;

        //[ReadOnly(true)]
        //[DisplayName("电池是否到位")]
        //public bool IsReady { get; set; } = false;

        ///// <summary>
        ///// 当前扫描NG已累计次数
        ///// </summary>
        //[Browsable(false)]
        //public int CurrentScanNgCount { get; set; }

        ///// <summary>
        ///// 扫描NG总次数设置
        ///// </summary>
        //[Browsable(false)]
        //public int ScanNgCount { get { return Current.option.ScanNgCount; } }

        #endregion

        #region 构造方法

        public Rotater() : this(-1) { }

        public Rotater(int id)
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
            this.company = rowInfo["Company"].ToString();
            this.model = rowInfo["Model"].ToString();
            this.number = rowInfo["Number"].ToString();
            this.isEnable = Convert.ToBoolean(rowInfo["IsEnable"]);
            this.stationId = TengDa._Convert.StrToInt(rowInfo["StationId"].ToString(), -1);
        }
        #endregion

        #region 系统旋转机构列表
        private static List<Rotater> rotaterList = new List<Rotater>();
        public static List<Rotater> RotaterList
        {
            get
            {
                if (rotaterList.Count < 1)
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
                            Rotater rotater = new Rotater();
                            rotater.InitFields(dt.Rows[i]);
                            rotaterList.Add(rotater);
                        }
                    }

                }

                return rotaterList;
            }
        }

        #endregion

        #region 该设备关联的下料机

        [Browsable(false)]
        public Blanker Blanker
        {
            get
            {
                return Current.blankers.First(b => b.RotaterId > 0);
            }
        }
        #endregion

        #region 该设备的工位
        [Browsable(false)]
        public Station Station
        {
            get
            {
                return Station.StationList.First(s => s.Id == this.StationId);
            }
        }
        #endregion

        /// <summary>
        /// 开门指令
        /// </summary>
        /// <returns></returns>
        public void OpenDoor()
        {

            if (!this.Blanker.Plc.IsPingSuccess)
            {
                IsAlive = false;
                LogHelper.WriteError("无法连接到 " + this.Blanker.Plc.IP);
                return;
            }

            this.Station.toOpenDoor = true;
        }

        /// <summary>
        /// 关门指令
        /// </summary>
        /// <returns></returns>
        public void CloseDoor()
        {
            if (!this.Blanker.Plc.IsPingSuccess)
            {
                IsAlive = false;
                LogHelper.WriteError("无法连接到 " + this.Blanker.Plc.IP);
                return;
            }

            if (Current.rotater.Station.ClampOri != this.Blanker.Stations[0].ClampOri)
            {
                Tip.Alert("当前状态无法关门！");
                return;
            }
            this.Station.toCloseDoor = true;
        }

        /// <summary>
        /// 关门指令
        /// </summary>
        /// <returns></returns>
        public void Rotate(ClampOri clampOri)
        {
            if (!this.Blanker.Plc.IsPingSuccess)
            {
                IsAlive = false;
                LogHelper.WriteError("无法连接到 " + this.Blanker.Plc.IP);
                return;
            }

            this.Station.toRotate[0] = clampOri != this.Blanker.Stations[0].ClampOri;
            this.Station.toRotate[1] = clampOri == this.Blanker.Stations[0].ClampOri;
        }
    }
}
