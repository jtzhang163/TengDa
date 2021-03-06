﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using TengDa;
using TengDa.WF;

namespace CAMEL.Baking
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
                    tableName = Config.DbTableNamePre + ".Battery";
                }
                return tableName;
            }
        }

        /// <summary>
        /// 料盒Id
        /// </summary>
        [ReadOnly(true), DisplayName("所在夹具Id")]
        public int ClampId { get; set; } = -1;

        /// <summary>
        /// 上料机Id
        /// </summary>
        [ReadOnly(true), DisplayName("上料机Id")]
        public int FeederId { get; set; } = -1;

        #endregion

        public Battery() : this(-1) { }


        public Battery(int id)
        {
            this.Id = id;
        }

        public Battery(string code) : this(code, -1, -1, "")
        {

        }

        public Battery(string code, int feederId, int clampId, string location)
        {
            this.code = code;
            this.FeederId = feederId;
            this.ClampId = clampId;
            this.location = location;
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
            this.ClampId = TengDa._Convert.StrToInt(rowInfo["ClampId"].ToString(), -1);
            this.FeederId = TengDa._Convert.StrToInt(rowInfo["FeederId"].ToString(), -1);
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

        public static int GetCountByClampId(int clampId, out string msg)
        {
            DataTable dt = Database.Query(string.Format("SELECT COUNT(*) FROM [dbo].[{0}] WHERE ClampId = {1};", TableName, clampId), out msg);
            if (dt.Rows.Count > 0)
            {
                return TengDa._Convert.StrToInt(dt.Rows[0][0].ToString(), -1);
            }
            return 0;
        }

        public static int Add(Battery addBattery, out string msg)
        {
            // msg = "";

            //if (!addBattery.Code.Contains("0000000000"))
            //{
            //    var addedBattery = GetBattery(addBattery.Code);
            //    if (addedBattery.Id > 0)
            //    {
            //        return addedBattery.Id;
            //    }
            //}

            return Database.Insert(string.Format("INSERT INTO [dbo].[{0}] ([Code], [ClampId], [FeederId], [Location], [ScanTime]) VALUES ('{1}', {2}, {3}, '{4}', '{5}')", TableName, addBattery.Code, addBattery.ClampId, addBattery.FeederId, addBattery.Location, DateTime.Now), out msg);
        }

        //public static bool Update(int clampId, int feederId, out string msg)
        //{
        //    return Database.NonQuery(string.Format("UPDATE [dbo].[{0}] SET [ClampId] = {1} WHERE [Id] IN (SELECT TOP {2} [Id] FROM [dbo].[{0}] WHERE [ClampId] = -1 AND [FeederId] = {3} ORDER BY [Id])",
        //         TableName, clampId, Current.option.ClampBatteryCount, feederId), out msg);
        //}

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
                sb.Append(string.Format("('{0}', {1}, {2}, '{3}', '{4}'),", battery.Code, battery.FeederId, battery.ClampId, battery.Location, DateTime.Now));
            }

            return Database.NonQuery(string.Format("INSERT INTO [dbo].[{0}] ([Code], [FeederId], [ClampId], [Location], [ScanTime]) VALUES {1}", TableName, sb.ToString().TrimEnd(',')), out msg);
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

        public static bool UpdateClampId(string batteryIds, int clampId, out string msg)
        {
            return Database.NonQuery(string.Format("UPDATE [dbo].[{0}] SET [ClampId] = {1} WHERE [Id] IN ({2})", TableName, clampId, batteryIds), out msg);
        }

        /// <summary>
        /// 电池绑定初始化
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool BatteryBindInit(out string msg)
        {
            return Database.NonQuery(string.Format("update dbo.[{0}] SET ClampId = (SELECT MAX(ClampId) FROM dbo.[{0}]) WHERE ClampId = -1", TableName), out msg);
        }


        public static int GetCount(out string msg)
        {
            DataTable dt = Database.Query(string.Format("SELECT COUNT(*) FROM [dbo].[{0}];", TableName), out msg);
            if (dt.Rows.Count > 0)
            {
                return TengDa._Convert.StrToInt(dt.Rows[0][0].ToString(), -1);
            }
            return 0;
        }

        /// <summary>
        /// 删除很久之前数据库中的电池数据，保留最近的100000条数据
        /// </summary>
        public static bool DeleteLongAgo(out string msg)
        {
            return Database.NonQuery(string.Format("DELETE FROM dbo.[{0}] WHERE Id <= ((SELECT MAX(Id) from dbo.[{0}]) - 100000)", TableName), 60, out msg);
        }
        #endregion
    }
}
