using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using TengDa;
using TengDa.WF;

namespace Soundon.Dispatcher
{
    /// <summary>
    /// 夹具品质状态
    /// </summary>
    public class ClampQualityStatus
    {
        #region 属性
        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".ClampQualityStatus";
                }
                return tableName;
            }
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Desc { get; set; }
        #endregion

        #region 构造方法

        public ClampQualityStatus() : this(-1) { }

        public ClampQualityStatus(int id)
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
            this.Code = rowInfo["Code"].ToString().Trim();
            this.Desc = rowInfo["Desc"].ToString().Trim();
            this.Id = TengDa._Convert.StrToInt(rowInfo["Id"].ToString(), -1);
        }
        #endregion

        #region 系统夹具品质状态列表
        private static List<ClampQualityStatus> statusList = new List<ClampQualityStatus>();
        public static List<ClampQualityStatus> StatusList
        {
            get
            {
                if (statusList.Count < 1)
                {
                    string msg = string.Empty;
                    DataTable dt = Database.Query("SELECT * FROM [dbo].[" + TableName + "]", out msg);
                    if (!string.IsNullOrEmpty(msg))
                    {
                        Error.Alert(msg);
                        return statusList;
                    }

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        statusList.Clear();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            ClampQualityStatus status = new ClampQualityStatus();
                            status.InitFields(dt.Rows[i]);
                            statusList.Add(status);
                        }
                    }
                }

                return statusList;
            }
        }

        #endregion
    }

    public enum QualityStatus
    {
        OK,
        HK001,
        HK002,
        HK003,
        HK004,
        HK005,
        HK006,
        HK007
    }
}
