using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using TengDa;
using TengDa.WF;

namespace Tafel.ScanSystem
{
    /// <summary>
    /// 工位
    /// </summary>
    public class Station : TengDa.WF.Terminals.Terminal
    {
        #region 属性字段

        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".Stations";
                }
                return tableName;
            }
        }


        private int feederId = -1;
        public int FeederId
        {
            get { return feederId; }
            private set { feederId = value; }
        }

        private int clampId = -1;

        [ReadOnly(true)]
        [DisplayName("夹具Id")]
        public int ClampId
        {
            get { return clampId; }
            set
            {
                if(clampId != value)
                {
                    UpdateDbField("ClampId", value);
                }
                clampId = value;
            }
        }

        [Browsable(false)]
        public Clamp Clamp
        {
            get
            {
                return new Clamp(this.ClampId);
            }
            set
            {
                this.ClampId = value.Id;
            }
        }

        #endregion

        #region 构造方法

        public Station() : this(-1) { }

        public Station(int id)
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
            this.ClampId = TengDa._Convert.StrToInt(rowInfo["ClampId"].ToString(), -1);
        }
        #endregion

        #region 系统工位列表
        private static List<Station> stationList = new List<Station>();
        public static List<Station> StationList
        {
            get
            {
                if(stationList.Count < 1)
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
                            Station station = new Station();
                            station.InitFields(dt.Rows[i]);
                            stationList.Add(station);
                        }
                    }
                }
                return stationList;
            }
        }

        #endregion

    }
}
