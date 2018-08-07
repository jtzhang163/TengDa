using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using TengDa;
using TengDa.WF;

namespace Veken.Baking
{
    public class Scaner : TengDa.WF.Terminals.SerialTerminal
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

        #region 获取扫码枪

        static string[] sysPortNames = SerialPort.GetPortNames();

        public static Scaner GetScaner(out string msg)
        {
            if (ScanerList.Count() > 0)
            {
                if (Array.IndexOf<string>(sysPortNames, scanerList[0].SerialPort.PortName) > -1)
                {
                    msg = string.Empty;
                    return scanerList[0];
                }

                msg = "当前电脑不存在串口： " + scanerList[0].SerialPort.PortName;
                Error.Alert(msg);
                Scaner scaner = new Scaner();
                scaner.name = "扫码枪";
                scaner.portName = "COM0";
                return scaner;
            }
            msg = "数据库不存在扫码枪信息！";
            Error.Alert(msg);
            return new Scaner();
        }

        public static Scaner GetScaner(SerialPort serialPort, out string msg)
        {
            try
            {
                List<Scaner> scanerList = (from scaner in ScanerList where scaner.SerialPort.PortName == serialPort.PortName select scaner).ToList();
                if (scanerList.Count() > 0)
                {
                    if (Array.IndexOf<string>(sysPortNames, scanerList[0].SerialPort.PortName) > -1)
                    {
                        msg = string.Empty;
                        return scanerList[0];
                    }

                    msg = "当前电脑不存在串口： " + scanerList[0].SerialPort.PortName;
                    return null;
                }
                msg = string.Format("数据库不存在IP为 {0} 的扫码枪！", serialPort);
                return null;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return null;
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
            this.portName = rowInfo["PortName"].ToString();
            this.baudRate = TengDa._Convert.StrToInt(rowInfo["BaudRate"].ToString(), 9600);
            this.SerialPort = new SerialPort(this.PortName, this.BaudRate);
            this.number = rowInfo["Number"].ToString();
            this.IsEnable = Convert.ToBoolean(rowInfo["IsEnable"]);

            if (Array.IndexOf<string>(sysPortNames, this.SerialPort.PortName) < 0)
            {
                Error.Alert("当前电脑不存在串口： " + this.SerialPort.PortName);
            }
        }

        #endregion

    }
}
