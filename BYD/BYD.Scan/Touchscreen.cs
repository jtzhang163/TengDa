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


        [Browsable(false)]
        public bool IsDealWithData { get; set; }
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

        #region 通信

        public bool WriteScanFinishInfoAuto(int j, out string msg)
        {
            var addr = 1 + 6 * j;
            var val = (ushort)2;

            var result = this.SetInfo(addr.ToString(), val, out msg);
            if (result) { WriteTouchscreenLog(addr, val); }
            return result;
        }

        public bool WriteScanResultInfo(int j, ScanResult scanResult, out string msg)
        {
            var addr = 6 * j;
            var val = scanResult == ScanResult.OK ? (ushort)1 : (ushort)2;
            WriteTouchscreenLog(addr, val);
            var result = this.SetInfo(addr.ToString(), val, out msg);
            if (result) { WriteTouchscreenLog(addr, val); }
            return result;
        }


        public bool WriteScanResultInfoManu(int j, bool mesOK1, bool batchOK1, bool mesOK2, bool batchOK2, out string msg)
        {
            msg = "";

            var addr1 = j == 0 ? 9 : 11;
            var val1 = GetWriteVal(mesOK1, batchOK1);
            var ret1 = this.SetInfo(addr1.ToString(), val1, out string msg1);
            if (ret1) { WriteTouchscreenLog(addr1, val1); }

            var addr2 = j == 0 ? 2 : 8;
            var val2 = GetWriteVal(mesOK2, batchOK2);
            var ret2 = this.SetInfo(addr2.ToString(), val2, out string msg2);
            if (ret2) { WriteTouchscreenLog(addr2, val2); }

            msg = msg1 + msg2;
            return ret1 && ret2;
        }

        /// <summary>
        /// 写入触摸屏的数据添加日志
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="val"></param>
        private void WriteTouchscreenLog(int addr, ushort val)
        {
            var addrTouchscreen = "4WUB" + (addr + 1).ToString("D3");
            LogHelper.WriteInfo(string.Format("往 {0} 的 {1} 中写入数据:{2}", this.Name, addrTouchscreen, val));
        }

        private ushort GetWriteVal(bool mesOK, bool batchOK)
        {
            if (mesOK && batchOK)
            {
                return (ushort)64;
            }
            else if (mesOK && !batchOK)
            {
                return (ushort)80;
            }
            else if (!mesOK && batchOK)
            {
                return (ushort)128;
            }
            else
            {
                return (ushort)144;
            }
        }
        #endregion

    }
}