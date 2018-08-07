using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TengDa.WF
{

    /// <summary>
    /// 操作日志
    /// </summary>
    public class Operation
    {
        private static string tableName = string.Empty;
        private static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".Operation";
                }
                return tableName;
            }
        }
        /// <summary>
        /// 增加一条操作日志
        /// </summary>
        /// <param name="userId">当前用户Id</param>
        /// <param name="description">操作内容字符串</param>
        /// <returns>返回是否插入日志成功</returns>
        public static bool Add(string description)
        {
            string msg = string.Empty;
            bool isSuccess = Database.NonQuery(string.Format("INSERT INTO [dbo].[{0}] ([UserId],[Description],[TIME]) VALUES ({1},'{2}',GETDATE());", TableName, Current.user.Id, description), out msg);
            if (!isSuccess)
            {
                Error.Alert(msg);
            }
            return isSuccess;
        }
    }
}
