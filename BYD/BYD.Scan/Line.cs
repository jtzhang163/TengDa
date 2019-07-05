using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using TengDa;
using TengDa.WF;

namespace BYD.Scan
{
    /// <summary>
    /// 拉线
    /// </summary>
    public class Line : Service
    {
        #region 属性字段

        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".Lines";
                }
                return tableName;
            }
        }


        private string name = string.Empty;
        /// <summary>
        /// 拉线名称
        /// </summary>
        [DisplayName("拉线名称")]
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    UpdateDbField("Name", value);
                }
                name = value;
            }
        }


        /// <summary>
        /// 父拉线ID
        /// </summary>
        [DisplayName("父拉线ID")]
        public int ParentId { get; set; }

        /// <summary>
        /// 触摸屏ID
        /// </summary>
        [DisplayName("触摸屏ID")]
        public int TouchscreenId { get; set; }

        /// <summary>
        /// 子拉线
        /// </summary>
        public List<Line> ChildLines
        {
            get
            {
                return LineList.Where(o => o.ParentId == this.Id).ToList();
            }
        }

        /// <summary>
        /// 父拉线
        /// </summary>
        public Line Parent
        {
            get
            {
                return LineList.FirstOrDefault(o => o.Id == this.ParentId);
            }
        }

        /// <summary>
        /// 触摸屏
        /// </summary>
        public Touchscreen Touchscreen
        {
            get
            {
                return Touchscreen.TouchscreenList.FirstOrDefault(o => o.Id == this.TouchscreenId);
            }
        }

        /// <summary>
        /// 自动扫码枪
        /// </summary>
        public Scaner AutoScaner
        {
            get
            {
                return Scaner.ScanerList.FirstOrDefault(o => o.LineId == this.Id && o.IsAuto);
            }
        }


        /// <summary>
        /// 手动扫码枪
        /// </summary>
        public Scaner ManuScaner
        {
            get
            {
                return Scaner.ScanerList.FirstOrDefault(o => o.LineId == this.Id && !o.IsAuto);
            }
        }

        public List<Scaner> Scaners
        {
            get
            {
                return new List<Scaner>() { AutoScaner, ManuScaner };
            }
        }



        #endregion

        #region 构造方法

        public Line() : this(-1) { }

        public Line(int id)
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
            this.ParentId = TengDa._Convert.StrToInt(rowInfo["ParentId"].ToString(), -1);
            this.TouchscreenId = TengDa._Convert.StrToInt(rowInfo["TouchscreenId"].ToString(), -1);
        }
        #endregion

        #region 系统拉线列表
        private static List<Line> lineList = new List<Line>();
        public static List<Line> LineList
        {
            get
            {
                if (lineList.Count < 1)
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
                            Line line = new Line();
                            line.InitFields(dt.Rows[i]);
                            lineList.Add(line);
                        }
                    }

                }
                return lineList;
            }
        }
        #endregion

        public bool GetInfo()
        {
            lock (this)
            {
                if (!this.Touchscreen.IsPingSuccess)
                {
                    this.Touchscreen.IsAlive = false;
                    LogHelper.WriteError("无法连接到 " + this.Touchscreen.IP);
                    return false;
                }

                try
                {

                    #region 获取信息

                    if (!this.Touchscreen.GetInfo("0", 10, out ushort[] output, out string msg))
                    {
                        Error.Alert(msg);
                        this.Touchscreen.IsAlive = false;
                        return false;
                    }
                    if (output.Length < 10)
                    {
                        LogHelper.WriteError(string.Format("与PLC通信出现错误，msg：{0}", msg));
                        return false;
                    }


                    for (int j = 0; j < 2; j++)
                    {
                        if (output[1 + j * 6] == 1)
                        {
                            if (!this.ChildLines[j].AutoScaner.IsReady)
                            {
                                this.ChildLines[j].AutoScaner.CanScan = true;
                                LogHelper.WriteInfo(string.Format("【扫码日志】收到上料机给的请求 {0} 扫码信号！", this.ChildLines[j].AutoScaner.Name));
                            }
                            this.ChildLines[j].AutoScaner.IsReady = true;
                        }
                        else
                        {
                            this.ChildLines[j].AutoScaner.IsReady = false;
                            this.ChildLines[j].AutoScaner.CanScan = false;
                        }
                    }

                    #endregion
                    Thread.Sleep(20);
                }
                catch (Exception ex)
                {
                    Error.Alert(ex);
                }

                this.Touchscreen.IsAlive = true;
            }
            return true;
        }

        public bool WriteScanFinishInfo(int j, out string msg)
        {
            return this.Touchscreen.SetInfo((1 + 6 * j).ToString(), (ushort)2, out msg);
        }

        public bool WriteScanResultInfo(int j, ScanResult scanResult, out string msg)
        {
            return this.Touchscreen.SetInfo((6 * j).ToString(), scanResult == ScanResult.OK ? (ushort)1 : (ushort)2, out msg);
        }

        public bool WriteScanResultInfoManu(int j, ScanResult scanResult, out string msg)
        {
            return this.Touchscreen.SetInfo((6 * j).ToString(), scanResult == ScanResult.OK ? (ushort)6 : (ushort)7, out msg);
        }
    }
}
