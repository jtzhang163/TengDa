using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading;
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

        public string[] Codes
        {
            get
            {
                return new string[2] { Code1, Code2 };
            }
        }


        private string code1 = string.Empty;
        /// <summary>
        /// 条码1
        /// </summary>
        [DisplayName("条码1")]
        public string Code1
        {
            get { return code1; }
            set
            {
                if (code1 != value)
                {
                    UpdateDbField("Code1", value);
                }
                code1 = value;
            }
        }

        private string code2 = string.Empty;
        /// <summary>
        /// 条码1
        /// </summary>
        [DisplayName("条码1")]
        public string Code2
        {
            get { return code2; }
            set
            {
                if (code2 != value)
                {
                    UpdateDbField("Code2", value);
                }
                code2 = value;
            }
        }


        #endregion

        #region 扫码枪列表
        private static List<Scaner> scanerList = new List<Scaner>();
        public static List<Scaner> ScanerList
        {
            get
            {
                if (scanerList.Count < 1)
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
            this.code1 = rowInfo["Code1"].ToString();
            this.code2 = rowInfo["Code2"].ToString();
        }
        #endregion

        #region 方法

        public ScanResult StartScan(out string code, out string msg)
        {
            code = "";
            SetInfo("LON", out msg);

            Thread.Sleep(800);
            var receiveData = this.GetReceiveData();
            if (receiveData.Length > 18)
            {
                code = receiveData;
                this.ClearReceiveData();
                return ScanResult.OK;
            }

            code = "ERROR";
            StopScan();
            return ScanResult.NG;
        }


        public void StopScan()
        {
            SetInfo("LOFF", out string msg);
        }

        /// <summary>
        /// 扫码结束逻辑
        /// </summary>
        /// <param name="scanResult"></param>
        /// <returns>扫码结束，True：第二个扫码结束（可发送结果至触摸屏）</returns>
        public bool ScanFinish(ScanResult scanResult, string code, out ScanResult finalScanResult)
        {
            finalScanResult = ScanResult.NG;

            if (!string.IsNullOrEmpty(this.Code1) && !string.IsNullOrEmpty(this.Code2))
            {
                this.Code1 = "";
                this.Code2 = "";
            }

            if (string.IsNullOrEmpty(this.Code1))
            {
                this.Code1 = scanResult == ScanResult.OK ? code : "ERROR";

                return false;
            }
            else if (string.IsNullOrEmpty(this.Code2))
            {
                this.Code2 = scanResult == ScanResult.OK ? code : "ERROR";

                if (!this.Code1.Contains("ERROR") && !this.Code2.Contains("ERROR"))
                {
                    LogHelper.WriteInfo(string.Format("获得先后两个条码：{0}，{1}", this.Code1, this.Code2));

                    Battery.Add(new Battery() { Code = this.Code1, ScanerId = this.Id }, out string msg);

                    Battery.Add(new Battery() { Code = this.Code2, ScanerId = this.Id }, out msg);

                    finalScanResult = ScanResult.OK;
                }

                return true;
            }
            return true;
        }

        public bool Manu(out ScanResult finalScanResult)
        {
            var code = "";
            finalScanResult = ScanResult.NG;
            var receiveData = this.GetReceiveData();
            if (receiveData.Length > 8)
            {
                code = receiveData;
                this.ClearReceiveData();
                return ScanFinish(ScanResult.OK, code, out finalScanResult);
            }
            return false;
        }

        #endregion
    }
}
