using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using TengDa;
using TengDa.WF;

namespace BYD.Scan
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

        [ReadOnly(true)]
        [DisplayName("准备就绪")]
        public bool IsReady { get; set; } = false;

        /// <summary>
        /// 可扫码，防止 IsReady 复位不及时导致重复扫码
        /// </summary>
        [ReadOnly(true)]
        [DisplayName("可扫码")]
        public bool CanScan { get; set; } = false;

        [ReadOnly(true)]
        [DisplayName("所在拉线Id")]
        public int LineId { get; private set; }

        [ReadOnly(true)]
        [DisplayName("是否为自动扫码枪")]
        public bool IsAuto { get; private set; }
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
            this.LineId = TengDa._Convert.StrToInt(rowInfo["LineId"].ToString(), -1);
            this.IsAuto = Convert.ToBoolean(rowInfo["IsAuto"]);
        }
        #endregion

        #region 方法

        public ScanResult StartBatteryScan(out string code, out string msg)
        {
            code = string.Empty;
            string output = string.Empty;

            if (!GetInfo(Current.option.BatteryScanerTriggerStr, 300, out output, out msg))
            {
                if (msg.Contains("超时"))
                {
                    StopBatteryScan();
                    return ScanResult.Timeout;
                }
                return ScanResult.Error;
            }

            if (string.IsNullOrEmpty(output))
            {
                msg = "指定时间未接收到串口数据！";
                return ScanResult.Error;
            }

            if (!string.IsNullOrEmpty(output) && output.Length > 5)
            {
                code = output;
                // code = Regex.Match(output, Current.option.BatteryCodeRegularExpression).Value;
                return ScanResult.OK;
            }

            code = Regex.Match(output, Current.option.BatteryScanerFailedStr).Value;
            if (!string.IsNullOrEmpty(code))
            {
                return ScanResult.NG;
            }

            this.StopBatteryScan();
            msg = "扫码枪返回字符串无法识别！";
            LogHelper.WriteError(string.Format("获得电池条码：{0}，不满足正则表达式：{1}", output, Current.option.BatteryCodeRegularExpression));
            return ScanResult.Unknown;
        }

        public ScanResult StartClampScan(out string code, out string msg)
        {
            code = string.Empty;
            string output = string.Empty;

            if (!GetInfo(Current.option.ClampScanerTriggerStr, 200, out output, out msg))
            {
                if (msg.Contains("超时"))
                {
                    StopClampScan();
                    return ScanResult.Timeout;
                }
                return ScanResult.Error;
            }

            if (string.IsNullOrEmpty(output))
            {
                msg = "指定时间未接收到串口数据！";
                return ScanResult.Error;
            }

            code = Regex.Match(output, Current.option.ClampCodeRegularExpression).Value;
            if (!string.IsNullOrEmpty(code))
            {
                return ScanResult.OK;
            }

            code = Regex.Match(output, Current.option.ClampScanerFailedStr).Value;
            if (!string.IsNullOrEmpty(code))
            {
                return ScanResult.NG;
            }

            msg = "扫码枪返回字符串无法识别！";
            LogHelper.WriteError(string.Format("获得夹具条码：{0}，不满足正则表达式：{1}", output, Current.option.ClampCodeRegularExpression));
            return ScanResult.Unknown;
        }

        public void StopBatteryScan()
        {
            string output = string.Empty;
            string msg = string.Empty;
            SetInfo(Current.option.BatteryScanerStopStr, out msg);
        }

        public void StopClampScan()
        {
            string output = string.Empty;
            string msg = string.Empty;
            SetInfo(Current.option.ClampScanerStopStr, out msg);
        }


        #endregion
    }
}
