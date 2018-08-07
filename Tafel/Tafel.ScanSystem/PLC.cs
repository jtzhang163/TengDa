using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using TengDa;
using TengDa.WF;

namespace Tafel.ScanSystem
{
    public class PLC : TengDa.WF.Terminals.EthernetTerminal
    {
        #region 属性字段
        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".PLCs";
                }
                return tableName;
            }
        }

        #endregion

        #region 系统PLC列表
        private static List<PLC> plcList = new List<PLC>();
        public static List<PLC> PlcList
        {
            get
            {
                if(plcList.Count < 1)
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
                            PLC plc = new PLC();
                            plc.InitFields(dt.Rows[i]);
                            plcList.Add(plc);
                        }
                    }

                }

                return plcList;
            }
        }
        #endregion

        #region 构造方法
        public PLC() : this(-1) { }

        public PLC(int id)
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
        }
        #endregion

        private static string JawLocation
        {
            get
            {
                return Current.option.JawLocation;
            }
            set
            {
                Current.option.JawLocation = value;
            }
        }

        public static bool GetJawLocation(out int i, out int j, out int k)
        {
            if (Current.feeder.Stations.Count(s => s.IsEnable) < 1)
            {
                Error.Alert("所有工位均被禁用！");
                i = j = k = -1;
                return false;
            }
            try
            {
                string[] locations = JawLocation.Split(',');
                i = TengDa._Convert.StrToInt(locations[0], -1);
                j = TengDa._Convert.StrToInt(locations[1], -1);
                k = TengDa._Convert.StrToInt(locations[2], -1);

                if (!Current.feeder.Stations[i].IsEnable)
                {
                    i = GetNextEnableStationIndex(i);
                    j = 0;
                    k = 0;
                    return true;
                }

                ///跳过1个空位，用来放置水分电池
                if (j == Current.ClampRowCount - 1 && k == Current.ClampColCount - 2)
                {
                    k = Current.ClampColCount - 1;
                }
                return true;
            }
            catch (Exception ex)
            {
                Error.Alert("GetJawLocation()出错! ex:" + ex.Message);
            }
            i = j = k = -1;
            return false;
        }

        /// <summary>
        /// 找到下一个启用的工位序号
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static int GetNextEnableStationIndex(int i)
        {
            i = (i + 1) % Current.ClampCount;
            if (Current.feeder.Stations[i].IsEnable)
            {
                return i;
            }
            else
            {
                return GetNextEnableStationIndex(i);
            }
        }

        public static int GetJawStationIndex()
        {
            try
            {
                string[] locations = JawLocation.Split(',');
                return TengDa._Convert.StrToInt(locations[0], -1);
            }
            catch (Exception ex)
            {
                Error.Alert("GetJawLocation()出错! ex:" + ex.Message);
            }
            return -1;
        }

        public void SetJawLocation(int i, int j, int k)
        {
            JawLocation = string.Format("{0},{1},{2}", i.ToString("D2"), j.ToString("D2"), k.ToString("D2"));
        }

    }
}
