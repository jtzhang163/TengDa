using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using TengDa;
using TengDa.WF;

namespace Outstanding.Dispatcher
{
    /// <summary>
    /// 电池缓存位
    /// </summary>
    public class BatteryCache : Service
    {
        #region 属性
        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".BatteryCaches";
                }
                return tableName;
            }
        }


        private string batteryIdsStr = string.Empty;
        [ReadOnly(true)]
        [DisplayName("电池Id序列")]
        public string BatteryIdsStr
        {
            get { return batteryIdsStr; }
            private set
            {
                if (batteryIdsStr != value)
                {
                    UpdateDbField("BatteryIdsStr", value);
                }
                batteryIdsStr = value;
            }
        }

        public const int BatteryCacheCount = 16;//电池缓存位总个数
                                                /// <summary>
                                                /// ID
                                                /// -1：无电池
                                                /// 0：有电池，未知Id
                                                /// >0：有电池ID
                                                /// </summary>
        public int[] BatteryIds
        {
            get
            {
                var batteryIds = new int[BatteryCacheCount];
                var batteryIdStrings = BatteryIdsStr.Split(',');
                for (int i = 0; i < BatteryCacheCount && i < batteryIdStrings.Length; i++)
                {
                    batteryIds[i] = TengDa._Convert.StrToInt(batteryIdStrings[i], -1);
                }
                return batteryIds;
            }
            set
            {
                BatteryIdsStr = string.Join(",", Array.ConvertAll<int, string>(value, delegate (int i) { return i.ToString(); }));
            }
        }
        #endregion

        #region 构造方法
        public BatteryCache() : this(-1) { }

        public BatteryCache(int id)
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

        #region 系统PLC列表
        private static List<BatteryCache> batteryCacheList = new List<BatteryCache>();
        public static List<BatteryCache> BatteryCacheList
        {
            get
            {
                if (batteryCacheList.Count < 1)
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
                            BatteryCache batteryCache = new BatteryCache();
                            batteryCache.InitFields(dt.Rows[i]);
                            batteryCacheList.Add(batteryCache);
                        }
                    }
                }
                return batteryCacheList;
            }
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
            this.batteryIdsStr = rowInfo["BatteryIdsStr"].ToString();
        }
        #endregion

        public void SetCount(int count)
        {
            var b = BatteryIds;
            for (int i = count; i < BatteryCacheCount; i++)
            {
                b[i] = -1;
            }
            BatteryIds = b;
        }

        public void Push(int batteryId)
        {
            var b = BatteryIds;
            for (int i = 0; i < BatteryCacheCount; i++)
            {
                if (b[i] < 0)
                {
                    b[i] = batteryId;
                    break;
                }
            }
            BatteryIds = b;
        }

        public void Push(string batteryIds)
        {
            var bids = batteryIds.Split(',');
            for (int i = 0; i < bids.Length; i++)
            {
                Push(TengDa._Convert.StrToInt(bids[i], -1));
            }
        }

        public int Pop()
        {
            var r = -1;
            var b = BatteryIds;
            for (int i = 0; i < BatteryCacheCount; i++)
            {
                if (b[i] > 0)
                {
                    b[i] = -1;
                    r = b[i];
                }
            }
            return r;
        }
    }
}

/*
 _____________↓___________________
_|o|o|o|o|o|o|_|_|_|_|_|_|_|_|_|_|_
  1 2 3 4 5 6 7 8 9 10 12  14  16
   */
