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
    /// 产量
    /// </summary>
    public class Yield : Service
    {
        #region 属性字段

        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".Yields";
                }
                return tableName;
            }
        }

        /// <summary>
        /// 夹具方向
        /// </summary>
        [DisplayName("夹具方向")]
        public ClampOri ClampOri { get; set; } = ClampOri.未知;


        private int feedingOK = 0;
        /// <summary>
        /// 上料良品数
        /// </summary>
        [DisplayName("上料良品数")]
        public int FeedingOK
        {
            get { return feedingOK; }
            set
            {
                if (feedingOK != value)
                {
                    UpdateDbField("FeedingOK", value);
                }
                feedingOK = value;
            }
        }


        private int feedingNG = 0;
        /// <summary>
        /// 上料不良品数
        /// </summary>
        [DisplayName("上料不良品数")]
        public int FeedingNG
        {
            get { return feedingNG; }
            set
            {
                if (feedingNG != value)
                {
                    UpdateDbField("FeedingNG", value);
                }
                feedingNG = value;
            }
        }

        private int blankingOK = 0;
        /// <summary>
        /// 下料良品数
        /// </summary>
        [DisplayName("下料良品数")]
        public int BlankingOK
        {
            get { return blankingOK; }
            set
            {
                if (blankingOK != value)
                {
                    UpdateDbField("BlankingOK", value);
                }
                blankingOK = value;
            }
        }


        private int blankingNG = 0;
        /// <summary>
        /// 下料不良品数
        /// </summary>
        [DisplayName("下料不良品数")]
        public int BlankingNG
        {
            get { return blankingNG; }
            set
            {
                if (blankingNG != value)
                {
                    UpdateDbField("BlankingNG", value);
                }
                blankingNG = value;
            }
        }

        /// <summary>
        /// 备注
        /// </summary>
        [DisplayName("备注")]
        public string Remark { get; set; } = string.Empty;

        #endregion

        #region 构造方法

        public Yield() : this(-1) { }

        public Yield(int id)
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
            this.ClampOri = (ClampOri)Enum.Parse(typeof(ClampOri), rowInfo["ClampOri"].ToString());
            this.FeedingOK = TengDa._Convert.StrToInt(rowInfo["FeedingOK"].ToString(), 0);
            this.FeedingNG = TengDa._Convert.StrToInt(rowInfo["FeedingNG"].ToString(), 0);
            this.BlankingOK = TengDa._Convert.StrToInt(rowInfo["BlankingOK"].ToString(), 0);
            this.BlankingNG = TengDa._Convert.StrToInt(rowInfo["BlankingNG"].ToString(), 0);
            this.Remark = rowInfo["Remark"].ToString();
        }
        #endregion

        #region 列表
        private static List<Yield> yieldList = new List<Yield>();
        public static List<Yield> YieldList
        {
            get
            {
                if (yieldList.Count < 1)
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
                            Yield yield = new Yield();
                            yield.InitFields(dt.Rows[i]);
                            yieldList.Add(yield);
                        }
                    }
                }
                return yieldList;
            }
        }

        #endregion

        #region 方法

        public static void Clear()
        {
            Current.Yields.ForEach(y => { y.ClearYield(); });
        }

        public void ClearYield()
        {
            this.FeedingOK = 0;
            this.FeedingNG = 0;
            this.BlankingOK = 0;
            this.BlankingNG = 0;
        }

        /// <summary>
        /// 判断时间是否是当前班次所在时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool IsCurrentShift(DateTime dt)
        {
            DateTime dtShiftStart = Common.DefaultTime;
            DateTime dtShiftStop = Common.DefaultTime;

            if (DateTime.Now.Hour < 8)
            {
                dtShiftStart = _Convert.StrToDateTime(string.Format("{0} 20:00:00", DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")), Common.DefaultTime);
                dtShiftStop = _Convert.StrToDateTime(string.Format("{0} 08:00:00", DateTime.Now.ToString("yyyy-MM-dd")), Common.DefaultTime);
            }
            else if (DateTime.Now.Hour < 20)
            {
                dtShiftStart = _Convert.StrToDateTime(string.Format("{0} 08:00:00", DateTime.Now.ToString("yyyy-MM-dd")), Common.DefaultTime);
                dtShiftStop = _Convert.StrToDateTime(string.Format("{0} 20:00:00", DateTime.Now.ToString("yyyy-MM-dd")), Common.DefaultTime);
            }
            else
            {
                dtShiftStart = _Convert.StrToDateTime(string.Format("{0} 20:00:00", DateTime.Now.ToString("yyyy-MM-dd")), Common.DefaultTime);
                dtShiftStop = _Convert.StrToDateTime(string.Format("{0} 08:00:00", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")), Common.DefaultTime);
            }

            if (dt > dtShiftStart && dt < dtShiftStop)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
