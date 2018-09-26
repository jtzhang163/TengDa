using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using TengDa;
using TengDa.WF;

namespace BakBattery.Baking
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
                        scanerList.Add(scaner);
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

        public bool StartClampScan(out string code, out string msg)
        {
            return this.GetInfo(Current.option.StartClampScan, ClampScanerTimeout, out code, out msg);
        }

        public bool StopClampScan(out string msg)
        {
            return this.SetInfo(Current.option.StopClampScan, out msg);
        }

        public ScanResult BatteryScan(out string code, out string msg)
        {
            code = string.Empty;
            string output = string.Empty;

            if (!GetInfo(Current.option.BatteryScanerTriggerStr, out output, out msg))
            {
                return ScanResult.Error;
            }

            if (string.IsNullOrEmpty(output))
            {
                msg = "指定时间未接收到串口数据！";
                return ScanResult.Error;
            }

            code = Regex.Match(output, Current.option.BatteryCodeRegularExpression).Value;
            if (!string.IsNullOrEmpty(code))
            {
                return ScanResult.OK;
            }

            code = Regex.Match(output, Current.option.BatteryScanerFailedStr).Value;
            if (!string.IsNullOrEmpty(code))
            {
                return ScanResult.NG;
            }

            msg = "扫码枪返回字符串无法识别！";
            return ScanResult.Unknown;
        }
    }
}
