using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading;
using TengDa;
using TengDa.WF;

namespace Soundon.Dispatcher
{
    public class Scaner : TengDa.WF.Terminals.EthernetTerminal
    {
        #region 属性字段
        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".Scaners";
                }
                return tableName;
            }
        }

        private static int clampScanerTimeout = -1;
        /// <summary>
        /// 扫码超时时间
        /// </summary>
        private static int ClampScanerTimeout
        {
            get
            {
                if (clampScanerTimeout < 0)
                {
                    clampScanerTimeout = TengDa._Convert.StrToInt(ConfigurationManager.AppSettings["ClampScanerTimeout"], -1);
                }
                return clampScanerTimeout;
            }
        }

        [ReadOnly(true)]
        [DisplayName("准备就绪")]
        public bool IsReady { get; set; } = false;

        /// <summary>
        /// 可扫码，防止 IsReady 复位不及时导致重复扫码
        /// </summary>
        [ReadOnly(true)]
        [DisplayName("可扫码")]
        public bool CanScan { get; set; } = false;
        #endregion

        #region 扫码枪列表
        private static List<Scaner> scanerList = new List<Scaner>();
        public static List<Scaner> ScanerList
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

                if (dt == null || dt.Rows.Count == 0)
                {
                    scanerList = null;
                }
                else
                {
                    scanerList.Clear();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Scaner scaner = new Scaner();
                        scaner.InitFields(dt.Rows[i]);
                        if(scanerList.Count < 5)
                        {
                            scanerList.Add(scaner);
                        }
                    }
                }
                return scanerList;
            }
        }

        #endregion

        #region 构造函数

        public Scaner() : this(-1) { }

        public Scaner(int id)
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

        #region 初始化对象
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

        #region 方法

        public ScanResult StartBatteryScan(out string code, out string msg)
        {
            code = "";
            if (!SetInfo("LON", out msg))
            {
                return ScanResult.Error;
            }

            Thread.Sleep(1000);

            var receiveData = this.GetReceiveData();
            if (receiveData.Length > 18)
            {
                code = receiveData;
                this.ClearReceiveData();
                return ScanResult.OK;
            }

            code = "ERROR";
            StopBatteryScan();
            return ScanResult.Error;
        }

        public ScanResult StartClampScan(out string code, out string msg)
        {
            code = "";
            if (!SetInfo("LON", out msg))
            {
                return ScanResult.Error;
            }

            Thread.Sleep(1000);

            var receiveData = this.GetReceiveData();
            if (receiveData.Length > 10)
            {
                code = receiveData;
                this.ClearReceiveData();
                return ScanResult.OK;
            }

            code = "ERROR";
            StopBatteryScan();
            return ScanResult.Error;
        }

        public void StopBatteryScan()
        {
            SetInfo("LOFF", out string msg);
        }

        public void StopClampScan()
        {
            SetInfo("LOFF", out string msg);
        }


        #endregion
    }
}
