﻿using System;
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
    /// 冷却架
    /// </summary>
    public class Cache : TengDa.WF.Terminals.Terminal
    {
        #region 属性字段

        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".Caches";
                }
                return tableName;
            }
        }


        private string stationIds = string.Empty;
        [ReadOnly(true), Description("工位ID集合")]
        [DisplayName("工位ID集合")]
        public string StationIds
        {
            get { return stationIds; }
            private set { stationIds = value; }
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

        public Cache() : this(-1) { }

        public Cache(int id)
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

            this.IsAlive = true;
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
            this.stationIds = rowInfo["StationIds"].ToString();
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
                if (stations.Count < 1)
                {
                    stations = Station.StationList.Where(s => Array.IndexOf(this.StationIds.Split(','), s.Id.ToString()) > -1).ToList();
                }
                return stations;
            }
        }
        #endregion

        #region 系统冷却架列表
        private static List<Cache> cacheList = new List<Cache>();
        public static List<Cache> CacheList
        {
            get
            {
                if (cacheList.Count < 1)
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
                            Cache cache = new Cache();
                            cache.InitFields(dt.Rows[i]);
                            cacheList.Add(cache);
                        }
                    }

                }

                return cacheList;
            }
        }

        #endregion
    }
}
