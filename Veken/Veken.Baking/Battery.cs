using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using TengDa;
using TengDa.WF;

namespace Veken.Baking
{
    /// <summary>
    /// 电池
    /// </summary>
    public class Battery :TengDa.WF.Products.Product
    {
        #region 属性
        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".Batteries";
                }
                return tableName;
            }
        }

        /// <summary>
        /// 料盒Id
        /// </summary>
        [ReadOnly(true)]
        public int ClampId { get; set; }
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
            this.code = rowInfo["Code"].ToString().Trim();
            this.ClampId = TengDa._Convert.StrToInt(rowInfo["ClampId"].ToString(), -1);
            this.location = rowInfo["Location"].ToString().Trim();
            this.Id = TengDa._Convert.StrToInt(rowInfo["Id"].ToString(), -1);
        }
        #endregion

        #region 增删查改

        public static List<Battery> GetList(string sql, out string msg)
        {
            List<Battery> list = new List<Battery>();
            DataTable dt = Database.Query(sql, out msg);

            if (!string.IsNullOrEmpty(msg))
            {
                Error.Alert(msg);
                return list;
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                return list;
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Battery battery = new Battery();
                    battery.InitFields(dt.Rows[i]);
                    list.Add(battery);
                }
            }
            return list;
        }

        public static bool Verify(string code, out Battery outUser, out string msg)
        {
            try
            {
                List<Battery> list = GetList(string.Format("SELECT * FROM [dbo].[{0}] WHERE [Code] = '{1}'", TableName, code), out msg);
                if (list.Count() > 0)
                {
                    outUser = list[0];
                    msg = string.Empty;
                    return true;
                }
                msg = "用户名或密码错误";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            outUser = null;
            return false;
        }

        public static int Add(Battery addBattery, out string msg)
        {

            Yield.FeedingOK += 1;
            return Database.Insert(string.Format("INSERT INTO [dbo].[{0}] ([Code], [ClampId], [Location]) VALUES ('{1}', {2}, '{3}')", TableName, addBattery.Code, addBattery.ClampId, addBattery.Location), out msg);

        }

        /// <summary>
        /// 增加多个，数据库一次插入多行
        /// </summary>
        /// <param name="addBatteries"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool Add(List<Battery> addBatteries, out string msg)
        {
            if (addBatteries.Count < 1)
            {
                msg = string.Empty;
                return true;
            }

            StringBuilder sb = new StringBuilder();

            foreach (Battery battery in addBatteries)
            {
                sb.Append(string.Format("('{0}', {1}, '{2}'),", battery.Code, battery.ClampId, battery.Location));
            }

            Yield.FeedingOK += addBatteries.Count;
            return Database.NonQuery(string.Format("INSERT INTO [dbo].[{0}] ([Code], [ClampId], [Location]) VALUES {1}", TableName, sb.ToString().TrimEnd(',')), out msg);                 
        }

        public static bool Delete(Battery delBattery, out string msg)
        {
            List<Battery> list = GetList(string.Format("SELECT * FROM [dbo].[{0}] WHERE [Code] = '{1}'", TableName, delBattery.Code), out msg);
            if (list.Count() < 1)
            {
                msg = "不存在电池！ Code：" + delBattery.Code;
                return false;
            }
            return Database.NonQuery(string.Format("DELETE FROM	[dbo].[{0}] WHERE [Code] = '{1}'", TableName, delBattery.Code), out msg);
        }

        public static bool Update(Battery newBattery, out string msg)
        {
            List<Battery> list = GetList(string.Format("SELECT * FROM [dbo].[{0}] WHERE [Code] = '{1}'", TableName, newBattery.Code), out msg);
            if (list.Count() < 1)
            {
                msg = "不存在电池！ Code：" + newBattery.Code;
                return false;
            }
            return Database.NonQuery(string.Format("UPDATE [dbo].[{0}] SET [ClampId] = {1} WHERE [Code] = '{2}'", TableName, newBattery.ClampId, newBattery.Code), out msg);
        }
        #endregion
    }
}
