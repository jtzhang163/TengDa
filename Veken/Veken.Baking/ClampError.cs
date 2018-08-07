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
    public class ClampError
    {
        #region 属性
        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".ClampError";
                }
                return tableName;
            }
        }

        public int Id { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDesc { get; set; }
        #endregion


        #region 构造方法

        public ClampError() : this(-1) { }

        public ClampError(int id)
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
            this.ErrorCode = rowInfo["ErrorCode"].ToString().Trim();
            this.ErrorDesc = rowInfo["ErrorDesc"].ToString().Trim();
            this.Id = TengDa._Convert.StrToInt(rowInfo["Id"].ToString(), -1);
        }
        #endregion

        #region 系统腔体不良列表
        private static List<ClampError> errorList = new List<ClampError>();
        public static List<ClampError> ErrorList
        {
            get
            {
                string msg = string.Empty;

                DataTable dt = Database.Query("SELECT * FROM [dbo].[" + TableName + "]", out msg);
                if (!string.IsNullOrEmpty(msg))
                {
                    Error.Alert(msg);
                    return null;
                }

                if (dt != null && dt.Rows.Count < 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        ClampError error = new ClampError();
                        error.InitFields(dt.Rows[i]);
                        errorList.Add(error);
                    }
                }
                return errorList;
            }
        }

        #endregion
    }

    public enum ErrorCode
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
