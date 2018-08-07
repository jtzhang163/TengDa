using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace TengDa.WF
{
    /// <summary>
    /// 产量
    /// </summary>
    public static class Yield
    {

        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".Yield";
                }
                return tableName;
            }
        }

        private static int feedingOK = -1;
        /// <summary>
        /// 上料OK数
        /// </summary>
        public static int FeedingOK
        {
            get
            {
                if (feedingOK < 0)
                {
                    feedingOK = Get(YieldKey.FeedingOK);
                }
                return feedingOK;
            }
            set
            {
                Set(YieldKey.FeedingOK, value);
                feedingOK = value;
            }
        }

        private static int feedingNG = -1;
        /// <summary>
        /// 上料NG数
        /// </summary>
        public static int FeedingNG
        {
            get
            {
                if (feedingNG < 0)
                {
                    feedingNG = Get(YieldKey.FeedingNG);
                }
                return feedingNG;
            }
            set
            {
                Set(YieldKey.FeedingNG, value);
                feedingNG = value;
            }
        }


        private static int blankingOK = -1;
        /// <summary>
        /// 下料OK数
        /// </summary>
        public static int BlankingOK
        {
            get
            {
                if (blankingOK < 0)
                {
                    blankingOK = Get(YieldKey.BlankingOK);
                }
                return blankingOK;
            }
            set
            {
                Set(YieldKey.BlankingOK, value);
                blankingOK = value;
            }
        }

        private static int blankingNG = -1;
        /// <summary>
        /// 下料NG数
        /// </summary>
        public static int BlankingNG
        {
            get
            {
                if (blankingNG < 0)
                {
                    blankingNG = Get(YieldKey.BlankingNG);
                }
                return blankingNG;
            }
            set
            {
                Set(YieldKey.BlankingNG, value);
                blankingNG = value;
            }
        }


        /// <summary>
        /// 读取产量
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int Get(YieldKey key)
        {
            string msg = string.Empty;
            DataTable dt = Database.Query(string.Format("SELECT [Value] FROM [dbo].[{0}] WHERE [Key] = '{1}'", TableName, key), out msg);
            if (!string.IsNullOrEmpty(msg))
            {
                Error.Alert(msg);
                return -1;
            }

            if (dt.Rows.Count > 0)
            {
                return TengDa._Convert.StrToInt(dt.Rows[0][0].ToString(), -1);
            }
            return -1;
        }
        /// <summary>
        /// 更新产量
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Set(YieldKey key, int value)
        {
            string msg = string.Empty;
            bool isSuccess = Database.NonQuery(string.Format("UPDATE [dbo].[{0}] SET [Value] = {1} WHERE [Key] = '{2}'", TableName, value, key), out msg);
            if (!string.IsNullOrEmpty(msg))
            {
                Error.Alert(msg);
                return false;
            }
            return isSuccess;
        }

        public static void Clear()
        {
            Yield.FeedingOK = 0;
            Yield.FeedingNG = 0;
            Yield.BlankingOK = 0;
            Yield.BlankingNG = 0;
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
    }
}