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
    /// 触摸屏
    /// </summary>
    public class Touchscreen : TengDa.WF.Terminals.EthernetTerminal
    {
        #region 属性字段
        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".Touchscreens";
                }
                return tableName;
            }
        }

        [Browsable(false)]
        public bool IsReadyScan1 { get; set; }

        [Browsable(false)]
        public bool IsReadyScan2 { get; set; }
        #endregion

        #region 系统触摸屏列表
        private static List<Touchscreen> touchscreenList = new List<Touchscreen>();
        public static List<Touchscreen> TouchscreenList
        {
            get
            {
                if (touchscreenList.Count < 1)
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
                            Touchscreen touchscreen = new Touchscreen();
                            touchscreen.InitFields(dt.Rows[i]);
                            touchscreenList.Add(touchscreen);
                        }
                    }

                }

                return touchscreenList;
            }
        }
        #endregion

        #region 构造方法
        public Touchscreen() : this(-1) { }

        public Touchscreen(int id)
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
            this.name = rowInfo["Name"].ToString();
            this.company = rowInfo["Company"].ToString();
            this.model = rowInfo["Model"].ToString();
            this.ip = rowInfo["IP"].ToString();
            this.port = TengDa._Convert.StrToInt(rowInfo["Port"].ToString(), -1);
            this.number = rowInfo["Number"].ToString();
            this.isEnable = Convert.ToBoolean(rowInfo["IsEnable"]);
        }
        #endregion

    }
}