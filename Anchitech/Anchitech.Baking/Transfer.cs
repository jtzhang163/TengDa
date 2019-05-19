using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using TengDa;
using TengDa.WF;

namespace Anchitech.Baking
{
    /// <summary>
    /// 转移台
    /// </summary>
    public class Transfer : TengDa.WF.Terminals.Terminal
    {
        #region 属性字段

        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".Transfers";
                }
                return tableName;
            }
        }


        private int stationId = -1;
        [ReadOnly(true), Description("工位Id")]
        [DisplayName("工位Id")]
        public int StationId
        {
            get { return stationId; }
            private set { stationId = value; }
        }

        [ReadOnly(true)]
        [DisplayName("PLC Id")]
        public int PlcId { get; set; } = -1;

        #region 该设备上的PLC

        private PLC plc = new PLC();

        [Browsable(false)]
        [ReadOnly(true)]
        public PLC Plc
        {
            get
            {
                if (plc.Id < 1)
                {
                    plc = PLC.PlcList.First(p => p.Id == this.PlcId);
                }
                return plc;
            }
        }
        #endregion


        #endregion

        #region 构造方法

        public Transfer() : this(-1) { }

        public Transfer(int id)
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
            this.number = rowInfo["Number"].ToString();
            this.isEnable = Convert.ToBoolean(rowInfo["IsEnable"]);
            this.stationId = TengDa._Convert.StrToInt(rowInfo["StationId"].ToString(), -1);
            this.PlcId = TengDa._Convert.StrToInt(rowInfo["PlcId"].ToString(), -1);
        }
        #endregion

        #region 系统旋转机构列表
        private static List<Transfer> rotaterList = new List<Transfer>();
        public static List<Transfer> RotaterList
        {
            get
            {
                if (rotaterList.Count < 1)
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
                            Transfer rotater = new Transfer();
                            rotater.InitFields(dt.Rows[i]);
                            rotaterList.Add(rotater);
                        }
                    }

                }

                return rotaterList;
            }
        }

        #endregion

        #region 该设备的工位
        [Browsable(false)]
        public Station Station
        {
            get
            {
                return Station.StationList.First(s => s.Id == this.StationId);
            }
        }
        #endregion

    }
}
