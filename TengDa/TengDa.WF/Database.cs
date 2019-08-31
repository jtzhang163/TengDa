using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Configuration;

namespace TengDa.WF
{
    /// <summary>
    /// 数据库操作公用类
    /// </summary>
    public class Database
    {
        private static string connectionString = string.Empty;
        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = ConfigurationManager.ConnectionStrings["TengDaDb"].ToString();
                }
                return connectionString;
            }
        }

        private static int openDbTimeout = -1;
        /// <summary>
        /// 打开数据库超时时间
        /// </summary>
        private static int OpenDbTimeout
        {
            get
            {
                if (openDbTimeout < 0)
                {
                    openDbTimeout = TengDa._Convert.StrToInt(ConfigurationManager.AppSettings["OpenDbTimeout"], -1);
                }
                return openDbTimeout;
            }
        }

        private static SqlConnection sqlConnection;
        public static SqlConnection SqlConnection
        {
            get
            {
                if (sqlConnection == null)
                {
                    sqlConnection = new SqlConnection(ConnectionString);
                    //con.Open(); 该方法连接数据库超时太久
                    SqlExtensions.QuickOpen(SqlConnection, OpenDbTimeout);
                }
                return sqlConnection;
            }
        }

        /// <summary>
        /// 查询数据库
        /// </summary>
        /// <param name="Sql">查询数据库SQL语句</param>
        /// <param name="msg">出现异常时的返回字符串</param>
        /// <returns>查询结果转化为DataTable类型</returns>
        public static DataTable Query(string sql, out string msg)
        {
            try
            {
                SqlCommand com = new SqlCommand();
                com.Connection = SqlConnection;
                com.CommandType = CommandType.Text;
                com.CommandText = sql;

                com.CommandTimeout = SqlConnection.ConnectionTimeout;//超时

                SqlDataAdapter SqlDap = new SqlDataAdapter(com);
                DataSet dataset = new DataSet();
                SqlDap.Fill(dataset);

                SqlDap.Dispose();
                com.Dispose();
                msg = string.Empty;
                return dataset.Tables[0];

            }
            catch (Exception ex)
            {
                msg = ex.Message + "SQL:" + sql;
                return null;
            }
        }
        /// <summary>
        /// 写数据库
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="msg">出现异常时的返回字符串</param>
        /// <returns>返回布尔类型表示操作是否成功</returns>
        public static bool NonQuery(string sql, out string msg)
        {
            try
            {
                SqlCommand com = new SqlCommand();
                com.Connection = SqlConnection;
                com.CommandType = CommandType.Text;
                com.CommandText = sql;

                com.CommandTimeout = SqlConnection.ConnectionTimeout;//超时

                com.ExecuteNonQuery();

                com.Dispose();
                msg = string.Empty;
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message + "SQL:" + sql;
                return false;
            }
        }
        /// <summary>
        /// 插入数据库并返回Id
        /// </summary>
        /// <param name="Sql">SQL语句</param>
        /// <param name="msg">出现异常时的返回字符串</param>
        /// <returns>返回新插入行Id</returns>
        public static int Insert(string sql, out string msg)
        {
            try
            {
                SqlCommand com = new SqlCommand();
                com.Connection = SqlConnection;
                com.CommandType = CommandType.Text;
                com.CommandText = sql + " Select @@Identity";

                com.CommandTimeout = SqlConnection.ConnectionTimeout;//超时

                int id = Convert.ToInt32(com.ExecuteScalar());

                com.Dispose();
                msg = string.Empty;
                return id;
            }
            catch (Exception ex)
            {
                msg = ex.Message + "SQL:" + sql;
                return -1;
            }
        }

        public static bool UpdateField(int id, string tableName, string field, string value, out string msg)
        {
            if (id < 1)
            {
                // msg = "id < 1";
                msg = string.Format("Datebase, UpdateDbField: id < 1, tableName: {0}, field : {1} value: {2} ", tableName, field, value);
                return false;
            }
            return NonQuery(string.Format("UPDATE [dbo].[{0}] set [{1}] = '{2}' WHERE Id = {3}", tableName, field, value, id), out msg);
        }
    }
}
