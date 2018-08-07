using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using TengDa;
using TengDa.WF;

namespace Tafel.ScanSystem
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

        /// <summary>
        /// 已上传
        /// </summary>
        [DisplayName("是否已上传MES")]
        [Category("基本信息")]
        public bool IsUploaded
        {
            get
            {
                return isUploaded;
            }
            set
            {
                if(isUploaded != value)
                {
                    UpdateDbField("IsUploaded", value);
                }
                isUploaded = value;
            }
        }

        /// <summary>
        /// 已完成Baking
        /// </summary>
        [Description("是否已完成(已出炉，可上传MES)")]
        [DisplayName("是否已完成")]
        [Category("基本信息")]
        public bool IsFinished
        {
            get
            {
                return isFinished;
            }
            set
            {
                if(isFinished != value)
                {
                    UpdateDbField("IsFinished", value);
                }
                isFinished = value;
            }
        }

        public bool isFinished;
        public bool isUploaded;

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
            this.isUploaded = Convert.ToBoolean(rowInfo["IsUploaded"]);
            this.isFinished = Convert.ToBoolean(rowInfo["IsFinished"]);
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
            return Database.Insert(string.Format("INSERT INTO [dbo].[{0}] ([Code], [ClampId], [Location], [ScanTime], [IsFinished], [IsUploaded]) VALUES ('{1}', {2}, '{3}', GETDATE(), 'TRUE', 'FALSE')", TableName, addBattery.Code, addBattery.ClampId, addBattery.Location), out msg);
        }


        public static bool Delete(Battery delBattery, out string msg)
        {
            return Database.NonQuery(string.Format("DELETE FROM	[dbo].[{0}] WHERE [Code] = '{1}'", TableName, delBattery.Code), out msg);
        }

        public static bool Update(Battery newBattery, out string msg)
        {
            return Database.NonQuery(string.Format("UPDATE [dbo].[{0}] SET [IsFinished] = '{1}', [IsUploaded] = '{2}' WHERE [Id] = {3}", TableName, newBattery.IsFinished, newBattery.IsUploaded, newBattery.Id), out msg);
        }
        #endregion

        #region 构造方法
        public Battery() : this(-1) { }

        public Battery(int id)
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

        public Battery(string code, int clampId, string location)
        {
            this.code = code;
            this.ClampId = clampId;
            this.location = location;
        }

        #endregion

        #region 增删查改

        public static void GetLastId()
        {
            string msg = string.Empty;
            DataTable dt = Database.Query(string.Format("SELECT TOP 1 [Id] FROM [dbo].[{0}] ORDER BY [Id] DESC", TableName), out msg);
            if (!string.IsNullOrEmpty(msg))
            {
                Error.Alert(msg);
                return;
            }

            if (dt.Rows.Count > 0)
            {
                Current.batteryId = TengDa._Convert.StrToInt(dt.Rows[0][0].ToString(), -1);
            }
        }
        #endregion
    }
}
