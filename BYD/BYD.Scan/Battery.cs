using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using TengDa;
using TengDa.WF;

namespace BYD.Scan
{
    /// <summary>
    /// 电池
    /// </summary>
    public class Battery : TengDa.WF.Products.Product
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
        /// 扫码枪ID
        /// </summary>
        [ReadOnly(true), DisplayName("扫码枪ID")]
        public int ScanerId { get; set; } = -1;


        #endregion

        public Battery() : this(-1) { }


        public Battery(int id)
        {
            this.Id = id;
        }

        public Battery(string code) : this(code, -1)
        {

        }

        public Battery(string code, int scanerId)
        {
            this.code = code;
            this.ScanerId = scanerId;
        }

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
            this.ScanerId = TengDa._Convert.StrToInt(rowInfo["ScanerId"].ToString(), -1);
            this.location = rowInfo["Location"].ToString().Trim();
            this.scanTime = TengDa._Convert.StrToDateTime(rowInfo["ScanTime"].ToString(), Common.DefaultTime);
            this.Id = TengDa._Convert.StrToInt(rowInfo["Id"].ToString(), -1);
        }
        #endregion

        #region 增删查改

        public static Battery GetBattery(int id)
        {
            var msg = string.Empty;
            var batteries = GetList(string.Format("SELECT * FROM [dbo].[{0}] WHERE [Id] = {1}", TableName, id), out msg);
            if (batteries.Count < 1)
                return new Battery();
            return batteries[0];
        }

        public static Battery GetBattery(string code)
        {
            var msg = string.Empty;
            var batteries = GetList(string.Format("SELECT * FROM [dbo].[{0}] WHERE [Code] = '{1}'", TableName, code), out msg);
            if (batteries.Count < 1)
                return new Battery();
            return batteries[0];
        }

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

        public static bool Verify(string code, out Battery outBattery, out string msg)
        {
            try
            {
                List<Battery> list = GetList(string.Format("SELECT * FROM [dbo].[{0}] WHERE [Code] = '{1}'", TableName, code), out msg);
                if (list.Count() > 0)
                {
                    outBattery = list[0];
                    msg = string.Empty;
                    return true;
                }
                msg = string.Format("系统不存在电芯{0}", code);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            outBattery = null;
            return false;
        }

        public static int Add(Battery newBattery, out string msg)
        {
            msg = "";

            if (!newBattery.Code.Contains("0000000000"))
            {
                var addedBattery = GetBattery(newBattery.Code);
                if (addedBattery.Id > 0)
                {
                    LogHelper.WriteError("重复扫码，Code：" + newBattery.Code);
                }
            }

            return Database.Insert(string.Format("INSERT INTO [dbo].[{0}] ([Code], [ScanerId], [Location], [ScanTime]) VALUES ('{1}', {2}, '{3}', '{4}')", TableName, newBattery.Code, newBattery.ScanerId, newBattery.Location, DateTime.Now), out msg);
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
                sb.Append(string.Format("('{0}', {1}, '{2}', '{3}'),", battery.Code, battery.ScanerId, battery.Location, DateTime.Now));
            }

            //Yield.FeedingOK += addBatteries.Count;
            return Database.NonQuery(string.Format("INSERT INTO [dbo].[{0}] ([Code], [ScanerId], [Location], [ScanTime]) VALUES {1}", TableName, sb.ToString().TrimEnd(',')), out msg);
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

        #endregion
    }
}
