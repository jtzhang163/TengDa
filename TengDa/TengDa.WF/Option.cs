using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Configuration;

namespace TengDa.WF
{
    /// <summary>
    /// 公用配置类
    /// </summary>
    public static class Option
    {
        private static string tableName = string.Empty;
        private static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".Options";
                }
                return tableName;
            }
        }

        /// <summary>
        /// 读取配置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetOption(string key)
        {
            string msg = string.Empty;
            DataTable dt = Database.Query(string.Format("SELECT [Value] FROM [dbo].[{0}] WHERE [Key] = '{1}'", TableName, key), out msg);
            if (!string.IsNullOrEmpty(msg))
            {
                Error.Alert(msg);
                Environment.Exit(0);
                return string.Empty;
            }

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            return string.Empty;
        }
        /// <summary>
        /// 更新配置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetOption(string key, string value)
        {
            string msg = string.Empty;
            bool isSuccess = Database.NonQuery(string.Format("UPDATE [dbo].[{0}] SET [Value] = '{1}' WHERE [Key] = '{2}'", TableName, value, key), out msg);
            if (!isSuccess)
            {
                Error.Alert(msg);
                return false;
            }
            return isSuccess;
        }
    }
}
