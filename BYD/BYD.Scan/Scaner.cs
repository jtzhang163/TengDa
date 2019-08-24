﻿using System;
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


        public string[] MES_RESULTs
        {
            get
            {
                return new string[2] { MES_RESULT1, MES_RESULT2 };
            }
        }

        private string mes_RESULT1 = string.Empty;

        public string MES_RESULT1
        {
            get
            {
                if (string.IsNullOrEmpty(mes_RESULT1) && !string.IsNullOrEmpty(Code1))
                {
                    mes_RESULT1 = Battery.GetBattery(this.Code1).Location;
                }
                return mes_RESULT1;
            }
        }


        private string mes_RESULT2 = string.Empty;

        public string MES_RESULT2
        {
            get
            {
                if (string.IsNullOrEmpty(mes_RESULT2) && !string.IsNullOrEmpty(Code2))
                {
                    mes_RESULT2 = Battery.GetBattery(this.Code2).Location;
                }
                return mes_RESULT2;
            }
        }


        [Browsable(false)]
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
                    if (string.IsNullOrEmpty(value))
                    {
                        this.mes_RESULT1 = "";
                    }
                    UpdateDbField("Code1", value);
                }
                code1 = value;
            }
        }

        private string code2 = string.Empty;
        /// <summary>
        /// 条码2
        /// </summary>
        [DisplayName("条码2")]
        public string Code2
        {
            get { return code2; }
            set
            {
                if (code2 != value)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        this.mes_RESULT2 = "";
                    }
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
            if (!SetInfo("LON", out msg))
            {
                return ScanResult.Error;
            }

            Thread.Sleep(Current.option.ScanDelayTimeSpan);

            var receiveData = this.GetReceiveData();
            if (receiveData.Length > 18)
            {
                code = receiveData;
                this.ClearReceiveData();
                return ScanResult.OK;
            }

            code = "ERROR";
            StopScan();
            return ScanResult.Error;
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
            finalScanResult = ScanResult.Error;

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

                    var mesResult1 = MES.CheckBattery(this.Code1);
                    var mesResult2 = MES.CheckBattery(this.Code2);

                    Battery.Add(new Battery() { Code = this.Code1, ScanerId = this.Id, Location = mesResult1 }, out string msg);

                    Battery.Add(new Battery() { Code = this.Code2, ScanerId = this.Id, Location = mesResult2 }, out msg);

                    if (mesResult1.ToLower().Contains("ok") && mesResult2.ToLower().Contains("ok"))
                    {
                        finalScanResult = ScanResult.OK;
                    }
                    else
                    {
                        finalScanResult = ScanResult.NG;
                    }        
                }

                return true;
            }
            return true;
        }

        public bool Manu(out ScanResult finalScanResult)
        {
            var code = "";
            finalScanResult = ScanResult.Error;
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
