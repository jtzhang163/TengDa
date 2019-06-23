using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using TengDa;
using TengDa.WF;

namespace Anchitech.Baking
{
    public class OvenParam : Service
    {
        #region 属性字段
        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".OvenParam";
                }
                return tableName;
            }
        }

        public string Content { get; set; }

        public string Unit { get; set; }

        public int Floor1Addr { get; set; }

        public int Floor2Addr { get; set; }

        public int Floor3Addr { get; set; }
        #endregion

        #region 列表
        private static List<OvenParam> ovenParamList = new List<OvenParam>();
        public static List<OvenParam> OvenParamList
        {
            get
            {
                if (ovenParamList.Count < 1)
                {
                    string msg = string.Empty;

                    DataTable dt = Database.Query("SELECT * FROM [dbo].[" + TableName + "]", out msg);
                    if (!string.IsNullOrEmpty(msg))
                    {
                        Error.Alert(msg);
                        return null;
                    }

                    if (dt != null && dt.Rows.Count > 0)
                    {

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            OvenParam ovenParam = new OvenParam();
                            ovenParam.InitFields(dt.Rows[i]);
                            ovenParamList.Add(ovenParam);
                        }
                    }

                }

                return ovenParamList;
            }
        }
        #endregion

        #region 构造方法
        public OvenParam() : this(-1) { }

        public OvenParam(int id)
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
            this.Id = TengDa._Convert.StrToInt(rowInfo["Id"].ToString(), -1);
            this.Content = rowInfo["Content"].ToString();
            this.Unit = rowInfo["Unit"].ToString();
            this.Floor1Addr = TengDa._Convert.StrToInt(rowInfo["Floor1Addr"].ToString(), -1);
            this.Floor2Addr = TengDa._Convert.StrToInt(rowInfo["Floor2Addr"].ToString(), -1);
            this.Floor3Addr = TengDa._Convert.StrToInt(rowInfo["Floor3Addr"].ToString(), -1);
        }
        #endregion

    }
}