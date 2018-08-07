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
    /// 料盒
    /// </summary>
    public class Clamp :TengDa.WF.Products.Product
    {
        #region 属性字段
        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".Clamps";
                }
                return tableName;
            }
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        [ReadOnly(true)]
        public int UserId { get; set; }

        /// <summary>
        /// 工位Id
        /// </summary>
        [ReadOnly(true)]
        public int StationId { get; set; }

        #endregion

        #region 构造方法
        public Clamp() : this(-1) { }

        public Clamp(int id)
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

        public Clamp(string code, int stationId,string location )
        {
            this.code = code;
            this.StationId = stationId;
            this.location = location;
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
            this.code = rowInfo["Code"].ToString().Trim();
            this.UserId = TengDa._Convert.StrToInt(rowInfo["UserId"].ToString(), -1);
            this.StationId = TengDa._Convert.StrToInt(rowInfo["StationId"].ToString(), -1);
            this.Id = TengDa._Convert.StrToInt(rowInfo["Id"].ToString(), -1);
        }
        #endregion

        #region 该料盒下的电池列表
        private List<Battery> batteries = new List<Battery>();

        [Browsable(false)]
        public List<Battery> Batteries
        {
            get
            {
                if (this.Id > 0 && batteries.Count < 1)
                {
                    string msg = string.Empty;
                    batteries = Battery.GetList(string.Format("SELECT * FROM [dbo].[{0}] WHERE ClampId = {1}", Battery.TableName, this.Id), out msg);
                }
                return batteries;
            }
        }
        #endregion

        #region 增删查改

        public static List<Clamp> GetList(string sql, out string msg)
        {
            List<Clamp> list = new List<Clamp>();
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
                    Clamp clamp = new Clamp();
                    clamp.InitFields(dt.Rows[i]);
                    list.Add(clamp);
                }
            }
            return list;
        }

        public static int Add(Clamp addClamp, out string msg)
        {
            return Database.Insert(string.Format("INSERT INTO [dbo].[{0}] ([Code], [UserId], [StationId], [BindTime]) VALUES ('{1}', {2}, '{3}', GETDATE())", TableName, addClamp.Code, TengDa.WF.Current.user.Id, addClamp.StationId), out msg);
        }

        public static bool Update(Clamp newClamp, out string msg)
        {
            return Database.NonQuery(string.Format("UPDATE [dbo].[{0}] SET [BindTime] = '{1}' WHERE [Id] = {2}", TableName, newClamp.BindTime, newClamp.Id), out msg);
        }
        #endregion

        /// <summary>
        /// 绑盘时间
        /// </summary>
        [ReadOnly(true)]
        public DateTime BindTime { get; set; }

        #region 其他

        public static bool Verify(string code, out Clamp outClamp, out string msg)
        {
            try
            {
                List<Clamp> list = GetList(string.Format("SELECT * FROM [dbo].[{0}] WHERE [Code] = '{1}'", TableName, code), out msg);
                if (list.Count() > 0)
                {
                    outClamp = list[0];
                    msg = string.Empty;
                    return true;
                }
                msg = "用户名或密码错误";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            outClamp = null;
            return false;
        }


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
                Current.clampId = TengDa._Convert.StrToInt(dt.Rows[0][0].ToString(), -1);
            }
        }
        #endregion
    }
}
