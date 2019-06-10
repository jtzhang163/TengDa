using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using TengDa;
using TengDa.WF;

namespace Anchitech.Baking
{
    /// <summary>
    /// 下料机
    /// </summary>
    public class Blanker : TengDa.WF.Terminals.Terminal
    {
        #region 属性字段

        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".Blankers";
                }
                return tableName;
            }
        }

        private bool canTestWatContent = false;
        /// <summary>
        /// 可以放置测水分夹具
        /// </summary>
        [DisplayName("可以放置测水分夹具")]
        public bool CanTestWatContent
        {
            get { return canTestWatContent; }
            set
            {
                if (canTestWatContent != value)
                {
                    UpdateDbField("CanTestWatContent", value);
                }
                canTestWatContent = value;
            }
        }

        private int plcId = -1;
        [ReadOnly(true), Description("PLC ID")]
        [DisplayName("PLC ID")]
        public int PlcId
        {
            get { return plcId; }
            set { plcId = value; }
        }

        private string stationIds = string.Empty;
        [ReadOnly(true), Description("工位ID集合")]
        [DisplayName("工位ID集合")]
        public string StationIds
        {
            get { return stationIds; }
            private set { stationIds = value; }
        }

        private TriLamp triLamp = TriLamp.Unknown;

        /// <summary>
        /// 三色灯
        /// </summary>
        [ReadOnly(true), DisplayName("三色灯")]
        public TriLamp TriLamp
        {
            get
            {
                if (!this.IsAlive)
                {
                    triLamp = TriLamp.Unknown;
                }
                return triLamp;
            }
            set
            {
                if (value == TriLamp.Red)
                {
                    this.AlarmStr = this.Name + "报警";
                }
                else
                {
                    this.AlarmStr = "";
                }
                triLamp = value;
            }
        }

        public ushort D2027;

        /// <summary>
        /// 人员处于安全光栅感应区
        /// 感应到此信号时，若搬运机器人运行至下料附近，则发送急停信号给搬运机器人
        /// </summary>
        [ReadOnly(true), DisplayName("人员处于安全光栅感应区")]
        public bool IsRasterInductive { get; set; }

        /// <summary>
        /// 复位光栅报警
        /// </summary>
        public bool toCancelRasterInductive = false;

        #endregion

        #region 构造方法

        public Blanker() : this(-1) { }

        public Blanker(int id)
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
            this.plcId = TengDa._Convert.StrToInt(rowInfo["PlcId"].ToString(), -1);
            this.company = rowInfo["Company"].ToString();
            this.model = rowInfo["Model"].ToString();
            this.number = rowInfo["Number"].ToString();
            this.isEnable = Convert.ToBoolean(rowInfo["IsEnable"]);
            this.StationIds = rowInfo["StationIds"].ToString();
            this.canTestWatContent = TengDa._Convert.StrToBool(rowInfo["CanTestWatContent"].ToString(), false);
        }
        #endregion

        #region 系统下料机列表
        private static List<Blanker> blankerList = new List<Blanker>();
        public static List<Blanker> BlankerList
        {
            get
            {
                if (blankerList.Count < 1)
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
                        blankerList.Clear();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            Blanker blanker = new Blanker();
                            blanker.InitFields(dt.Rows[i]);
                            blankerList.Add(blanker);
                        }
                    }
                }
                return blankerList;
            }
        }

        #endregion

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

        #region 该设备上的工位列表
        private List<Station> stations = new List<Station>();
        [Browsable(false)]
        public List<Station> Stations
        {
            get
            {
                if (stations.Count < 1)
                {
                    stations.Clear();
                    for (int i = 0; i < StationIds.Split(',').Length; i++)
                    {
                        stations.Add(Station.StationList.First(s => s.Id.ToString() == StationIds.Split(',')[i]));
                    }
                }
                return stations;
            }
        }
        #endregion

        #region 通信

        public bool AlreadyGetAllInfo = false;

        public bool GetInfo()
        {
            lock (this)
            {
                if (!this.Plc.IsPingSuccess)
                {
                    this.Plc.IsAlive = false;
                    LogHelper.WriteError("无法连接到 " + this.Plc.IP);
                    return false;
                }

                string msg = string.Empty;
                string output = string.Empty;

                try
                {

                    if (!this.Plc.GetInfo(false, "%01#RDD0001400016**", out output, out msg))
                    {
                        Error.Alert(msg);
                        this.Plc.IsAlive = false;
                        return false;
                    }
                    if (output.Substring(3, 1) != "$")
                    {
                        LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", "%01#RDD0001400016**", output));
                        return false;
                    }

                    int[] iOut = new int[3];
                    output = PanasonicPLC.ConvertHexStr(output.TrimEnd('\r'), false);
                    for (int j = 0; j < iOut.Length; j++)
                    {
                        iOut[j] = int.Parse(output.Substring(j * 4, 4), System.Globalization.NumberStyles.AllowHexSpecifier);
                    }

                    for (int j = 0; j < this.Stations.Count; j++)
                    {
                        switch (iOut[j])
                        {
                            case 1:
                                this.Stations[j].ClampStatus = ClampStatus.无夹具;
                                this.Stations[j].Status = StationStatus.可放;
                                break;
                            case 2:
                                this.Stations[j].ClampStatus = ClampStatus.满夹具;
                                this.Stations[j].Status = StationStatus.工作中;
                                break;
                            case 3:
                                this.Stations[j].ClampStatus = ClampStatus.空夹具;
                                this.Stations[j].Status = StationStatus.可取;
                                break;
                            case 4:
                                this.Stations[j].ClampStatus = ClampStatus.异常;
                                this.Stations[j].Status = StationStatus.不可用;
                                break;
                            default:
                                this.Stations[j].ClampStatus = ClampStatus.未知;
                                this.Stations[j].Status = StationStatus.不可用;
                                break;
                        }
                    }

                    switch (iOut[2])
                    {
                        case 1: this.TriLamp = TriLamp.Green; break;
                        case 2: this.TriLamp = TriLamp.Yellow; break;
                        case 3: this.TriLamp = TriLamp.Red; break;
                        default: this.TriLamp = TriLamp.Unknown; break;
                    }


                    #region 获取信息

                    //var bOutputs = new ushort[] { };
                    //if (!this.Plc.GetInfo("D2000", (ushort)30, out bOutputs, out msg))
                    //{
                    //    Error.Alert(msg);
                    //    this.Plc.IsAlive = false;
                    //    return false;
                    //}

                    //for (int j = 0; j < this.Stations.Count; j++)
                    //{
                    //    int jj = 1 - j;

                    //    if (bOutputs[10 + j] == 1)
                    //    {
                    //        this.Stations[jj].ClampStatus = ClampStatus.空夹具;
                    //        this.Stations[jj].Status = StationStatus.可取;
                    //        this.Stations[jj].SampleInfo = SampleInfo.未知;
                    //    }
                    //    else if (bOutputs[10 + j] == 2)
                    //    {
                    //        this.Stations[jj].ClampStatus = ClampStatus.无夹具;
                    //        this.Stations[jj].Status = StationStatus.可放;
                    //    }
                    //    else if (bOutputs[10 + j] == 3)
                    //    {
                    //        this.Stations[jj].ClampStatus = ClampStatus.满夹具;
                    //        this.Stations[jj].Status = StationStatus.工作中;
                    //    }
                    //    else if (bOutputs[10 + j] == 4)
                    //    {
                    //        this.Stations[jj].ClampStatus = ClampStatus.异常;
                    //        this.Stations[jj].Status = StationStatus.不可用;
                    //    }
                    //    else if (bOutputs[10 + j] == 5) //NG回炉
                    //    {
                    //        this.Stations[jj].ClampStatus = ClampStatus.满夹具;
                    //        this.Stations[jj].Status = StationStatus.可取;
                    //        this.Stations[jj].SampleStatus = SampleStatus.水分NG;
                    //    }
                    //    else
                    //    {
                    //        this.Stations[jj].ClampStatus = ClampStatus.未知;
                    //        this.Stations[jj].Status = StationStatus.不可用;
                    //    }

                    //    var samResultVal = bOutputs[21 + j];
                    //    if (samResultVal == 3 || samResultVal == 4)
                    //    {

                    //        var sampleStatus = samResultVal == 3 ? SampleStatus.水分OK : SampleStatus.水分NG;

                    //        if (this.Stations[jj].ClampStatus != ClampStatus.无夹具)
                    //        {
                    //            this.Stations[jj].SampleStatus = sampleStatus;
                    //        }

                    //        var floorStation = Station.GetStation(this.Stations[jj].FromStationId);
                    //        if (floorStation != null && floorStation.GetPutType == GetPutType.烤箱)
                    //        {
                    //            floorStation.GetFloor().Stations.ForEach(s =>
                    //            {
                    //                if(s.ClampStatus == ClampStatus.满夹具)
                    //                {
                    //                    s.SampleStatus = sampleStatus;
                    //                }
                    //            });
                    //        }
                    //    }
                    //}

                    //switch (bOutputs[18])
                    //{
                    //    case 1: this.TriLamp = TriLamp.Red; break;
                    //    case 2: this.TriLamp = TriLamp.Yellow; break;
                    //    case 3: this.TriLamp = TriLamp.Green; break;
                    //    default: this.TriLamp = TriLamp.Unknown; break;
                    //}

                    //this.Stations[0].DoorStatus = DoorStatus.打开;

                    //this.Stations[1].DoorStatus = DoorStatus.打开;

                    //if (bOutputs[25] == 1)
                    //{
                    //    if (!this.IsRasterInductive)
                    //    {
                    //        LogHelper.WriteInfo(string.Format("{0} --> 人员进入安全光栅感应区", this.Name));

                    //        if (!Current.Robot.IsPausing && Current.Robot.Position <= Current.option.RobotStopPosition4RasterInductive)
                    //        {
                    //            if (Current.Robot.Stop(out msg))
                    //            {
                    //                Error.Alert(string.Format("人员进入 {0} 安全光栅感应区域，已远程发送急停信号给 {1}", this.Name, Current.Robot.Name));
                    //            }
                    //            else
                    //            {
                    //                Error.Alert(string.Format("人员进入 {0} 安全光栅感应区域，远程发送急停信号给 {1} 失败！", this.Name, Current.Robot.Name));
                    //            }
                    //        }

                    //    }
                    //    this.IsRasterInductive = true;
                    //    this.AlarmStr = "安全光栅报警！";
                    //}
                    //else
                    //{
                    //    if (this.IsRasterInductive)
                    //    {
                    //        LogHelper.WriteInfo(string.Format("{0} --> 安全光栅感应报警结束", this.Name));


                    //        //if (!otherBlanker.IsRasterInductive && Current.Robot.IsPausing)
                    //        //{
                    //        //    if (Current.Robot.Restart(out msg))
                    //        //    {
                    //        //        Tip.Alert(string.Format("{0} 安全光栅感应报警结束，已远程发送继续运动信号给 {1}", this.Name, Current.Robot.Name));
                    //        //    }
                    //        //    else
                    //        //    {
                    //        //        Error.Alert(string.Format("{0} 安全光栅感应报警结束，远程发送继续运动信号给 {1} 失败！", this.Name, Current.Robot.Name));
                    //        //    }
                    //        //}
                    //    }
                    //    this.IsRasterInductive = false;
                    //    this.AlarmStr = "";
                    //}

                    //this.D2027 = bOutputs[27];
                    //if (this.toCancelRasterInductive)
                    //{
                    //    if (!this.Plc.SetInfo("D2019", (ushort)1, out msg))
                    //    {
                    //        Error.Alert(msg);
                    //        this.Plc.IsAlive = false;
                    //        return false;
                    //    }

                    //    LogHelper.WriteInfo(string.Format("成功发送光栅感应报警复位指令到{0}:{1}", this.Name, "D2019:1"));
                    //    this.toCancelRasterInductive = false;
                    //}


                    //for (int j = 0; j < this.Stations.Count; j++)
                    //{
                    //    int jj = 1 - j;

                    //    if (this.Stations[j].SampleInfo == SampleInfo.无样品)
                    //    {
                    //        if (bOutputs[21 + jj] == 0)
                    //        {
                    //            if (!this.Plc.SetInfo("D" + (2021 + jj).ToString("D4"), (ushort)1, out msg))
                    //            {
                    //                Error.Alert(msg);
                    //                this.Plc.IsAlive = false;
                    //                return false;
                    //            }

                    //            LogHelper.WriteInfo(string.Format("成功发送无水分夹具指令到{0}:{1}", this.Name, "D" + (2021 + jj).ToString("D4") + "1"));
                    //        }
                    //    }

                    //    if (this.Stations[j].SampleInfo == SampleInfo.有样品)
                    //    {
                    //        if (bOutputs[21 + jj] == 0)
                    //        {
                    //            if (!this.Plc.SetInfo("D" + (2021 + jj).ToString("D4"), (ushort)2, out msg))
                    //            {
                    //                Error.Alert(msg);
                    //                this.Plc.IsAlive = false;
                    //                return false;
                    //            }

                    //            LogHelper.WriteInfo(string.Format("成功发送有水分夹具指令到{0}:{1}", this.Name, "D" + (2021 + jj).ToString("D4") + "2"));
                    //        }
                    //    }
                    //}


                    //避让功能
                    //if (Current.Feeder.Plc.IsAlive && bOutputs[26] != Current.Feeder.D1025)
                    //{
                    //    if (!this.Plc.SetInfo("D2026", Current.Feeder.D1025, out msg))
                    //    {
                    //        Error.Alert(msg);
                    //        this.Plc.IsAlive = false;
                    //        return false;
                    //    }
                    //}

                    #endregion

                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    Error.Alert(ex);
                }

                this.Plc.IsAlive = true;

                this.AlreadyGetAllInfo = true;

            }
            return true;
        }

        public void CancelRasterInductive()
        {
            if (!this.Plc.IsPingSuccess)
            {
                this.Plc.IsAlive = false;
                LogHelper.WriteError("无法连接到 " + this.Plc.IP);
                return;
            }

            this.toCancelRasterInductive = true;
        }

        public bool SetScanResult(ScanResult scanResult)
        {
            string input = string.Empty;
            if (scanResult == ScanResult.OK)
            {
                input = Current.option.SendScanOkStr;
            }
            else if (scanResult == ScanResult.NG)
            {
                input = Current.option.SendScanNgStr;
            }
            else
            {
                throw new ArgumentOutOfRangeException("bool SetScanResult(ScanResult scanResult) 参数值与预期不符！");
            }

            string output = string.Empty;
            string msg = string.Empty;
            if (this.Plc.GetInfo(input, out output, out msg))
            {
                if (output.Substring(3, 1) == "$")
                {
                    return true;
                }
                else
                {
                    LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", input, output));
                }
            }
            return false;
        }
        #endregion
    }
}
