using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using TengDa;
using TengDa.WF;

namespace Soundon.Dispatcher
{
    /// <summary>
    /// 工位
    /// </summary>
    public class Station : TengDa.WF.Terminals.Terminal
    {
        #region 属性字段

        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".Stations";
                }
                return tableName;
            }
        }

        private StationStatus status = StationStatus.不可用;
        /// <summary>
        /// 工位状态
        /// </summary>
        [ReadOnly(true)]
        [DisplayName("工位状态")]
        public StationStatus Status
        {
            get
            {
                if (this.GetPutType == GetPutType.缓存架 || this.GetPutType == GetPutType.转移台)
                {
                    var _status = StationStatus.不可用;
                    if (this.ClampStatus == ClampStatus.无夹具)
                    {
                        _status = StationStatus.可放;
                    }
                    else
                    {
                        _status = StationStatus.可取;
                    }
                    if (status != _status)
                    {
                        this.GetPutTime = DateTime.Now;
                        status = _status;
                    }
                }
                return status;
            }
            set
            {
                if (status != value && (value == StationStatus.可取 || value == StationStatus.可放))
                {
                    this.GetPutTime = DateTime.Now;
                }
                status = value;
            }
        }

        private GetPutType getPutType = GetPutType.未知;
        /// <summary>
        /// 取放类型
        /// </summary>
        [ReadOnly(true)]
        [DisplayName("取放类型")]
        public GetPutType GetPutType
        {
            get { return getPutType; }
            set
            {
                if (getPutType != value)
                {
                    UpdateDbField("GetPutType", value);
                }
                getPutType = value;
            }
        }

        private ClampStatus clampStatus = ClampStatus.未知;
        /// <summary>
        /// 夹具状态
        /// </summary>
        [DisplayName("夹具状态")]
        public ClampStatus ClampStatus
        {
            get
            {
                return clampStatus;
            }
            set
            {
                if (TengDa.WF.Current.IsTerminalInitFinished && clampStatus != value)
                {
                    if (value == ClampStatus.满夹具 && clampStatus == ClampStatus.空夹具 && this.GetPutType == GetPutType.上料机)
                    {
                        //上料计数逻辑
                        Current.Yields.First(y => y.ClampOri == this.ClampOri).FeedingOK++;

                        //电池夹具绑定
                        if (this.Clamp.Batteries.Count == 0)
                        {
                            var msg = string.Empty;
                            if (!Battery.Update(this.Clamp.Id, Current.feeders.First(f => f.Stations.Contains(this)).Id, out msg))
                            {
                                Error.Alert(msg);
                            }
                        }
                    }

                    //下料计数逻辑
                    if (value == ClampStatus.空夹具 && clampStatus == ClampStatus.满夹具 && this.GetPutType == GetPutType.下料机)
                    {
                        Current.Yields.First(y => y.ClampOri == this.ClampOri).BlankingOK++;
                    }

                    //解决取放料时夹具状态反复变化问题
                    if (value != ClampStatus.无夹具 && (this.Id == Current.Task.FromStationId || this.Id == Current.Task.ToStationId))
                    {
                        value = Current.Task.FromClampStatus;
                    }

                    AddLog(string.Format("{0}——>{1}", clampStatus, value));

                }

                if (clampStatus != value)
                {
                    UpdateDbField("ClampStatus", value);
                }
                clampStatus = value;
            }
        }

        private int clampId = -1;
        /// <summary>
        /// 夹具Id
        /// </summary>
        [ReadOnly(true)]
        [DisplayName("夹具Id")]
        public int ClampId
        {
            get { return clampId; }
            set
            {
                if (clampId != value)
                {
                    UpdateDbField("ClampId", value);
                }
                clampId = value;
            }
        }

        private int fromStationId = -1;
        /// <summary>
        /// 来源工位ID
        /// </summary>
        [ReadOnly(true)]
        [DisplayName("来源工位ID")]
        public int FromStationId
        {
            get { return fromStationId; }
            set
            {
                if (fromStationId != value)
                {
                    UpdateDbField("FromStationId", value);
                }
                fromStationId = value;
            }
        }

        private Clamp clamp = new Clamp();
        [Browsable(false)]
        public Clamp Clamp
        {
            get
            {
                if (clamp.Id != ClampId)
                {
                    clamp = new Clamp(this.ClampId);
                }
                return clamp;
            }
            set
            {
                this.ClampId = value.Id;
            }
        }

        private ClampOri clampOri = ClampOri.未知;
        /// <summary>
        /// 夹具方向
        /// </summary>
        [ReadOnly(true)]
        [DisplayName("夹具方向")]
        public ClampOri ClampOri
        {
            get { return clampOri; }
            set
            {
                if (clampOri != value)
                {
                    UpdateDbField("ClampOri", value);
                }
                clampOri = value;
            }
        }

        private DateTime getPutTime = TengDa.Common.DefaultTime;
        /// <summary>
        /// 取放时间
        /// </summary>
        [DisplayName("取放时间")]
        public DateTime GetPutTime
        {
            get { return getPutTime; }
            set
            {
                if (getPutTime != value)
                {
                    UpdateDbField("GetPutTime", value);
                }
                getPutTime = value;
            }
        }

        private int priority = 1;
        /// <summary>
        /// 优先级
        /// </summary>
        [DisplayName("优先级")]
        public int Priority
        {
            get { return priority; }
            set
            {
                if (priority != value)
                {
                    UpdateDbField("Priority", value);
                }
                priority = value;
            }
        }

        public DoorStatus PreDoorStatus;

        public DoorStatus doorStatus = DoorStatus.未知;
        /// <summary>
        /// 门状态（下料机为干涉状态）
        /// </summary>
        [ReadOnly(true)]
        [DisplayName("门状态")]
        public DoorStatus DoorStatus
        {
            get
            {
                if (this.GetPutType == GetPutType.上料机 || this.GetPutType == GetPutType.缓存架 || this.GetPutType == GetPutType.转移台)
                {
                    doorStatus = DoorStatus.打开;
                }
                return doorStatus;
            }
            set
            {
                if (TengDa.WF.Current.IsTerminalInitFinished)
                {
                    if (doorStatus != value)
                    {
                        if (value == DoorStatus.打开 || value == DoorStatus.关闭)
                        {
                            AddLog("门：" + value);
                        }
                    }
                }
                doorStatus = value;
            }
        }

        /// <summary>
        /// 可开门（烤箱真空未泄完成，则不可开门）
        /// </summary>
        [DisplayName("可开门")]
        public bool CanOpenDoor
        {
            get
            {
                if (this.GetPutType == GetPutType.上料机 || this.GetPutType == GetPutType.缓存架 || this.GetPutType == GetPutType.转移台 || this.GetPutType == GetPutType.下料机)
                {
                    return true;
                }

                Floor oFloor = this.GetFloor();
                return !oFloor.IsVacuum;
            }
        }

        private string robotValues = string.Empty;
        /// <summary>
        /// 机器人取放位置编码(取编码,放编码)
        /// </summary>
        [ReadOnly(true), DisplayName("机器人取放位置编码")]
        [Description("机器人取放位置编码(取编码,放编码)")]
        public string RobotValues
        {
            get { return robotValues; }
            set
            {
                if (robotValues != value)
                {
                    UpdateDbField("RobotValues", value);
                }
                robotValues = value;
            }
        }

        /// <summary>
        /// 机器人取盘位置编码
        /// </summary>
        [ReadOnly(true), DisplayName("机器人取盘位置编码")]
        public int RobotGetCode
        {
            get
            {
                return TengDa._Convert.StrToInt(RobotValues.Split(',')[0], 0);
            }
        }

        /// <summary>
        /// 机器人放盘位置编码
        /// </summary>
        [ReadOnly(true), DisplayName("机器人放盘位置编码")]
        public int RobotPutCode
        {
            get
            {
                return TengDa._Convert.StrToInt(RobotValues.Split(',')[1], 0);
            }
        }

        public FloorStatus PreFloorStatus;

        protected FloorStatus floorStatus = FloorStatus.未知;
        [DisplayName("当前腔体状态")]
        public FloorStatus FloorStatus
        {
            get
            {
                return floorStatus;
            }
            set
            {

                if (this.GetPutType == GetPutType.烤箱 && floorStatus != value)
                {
                    if (value == FloorStatus.待烤)
                    {
                        this.Clamp.OvenStationId = this.Id;
                        this.Clamp.InOvenTime = DateTime.Now;
                    }
                    else if (value == FloorStatus.烘烤)
                    {
                        var floor = this.GetFloor();

                        this.Clamp.BakingStartTime = DateTime.Now;

                        this.Clamp.ProcessTemperSet = floor.ProcessTemperSet;
                        this.Clamp.TsSet = floor.TemperatureSets;
                        this.Clamp.YunFengTSet = floor.YunfengTemperatureSet;
                        this.Clamp.VacuumSet = floor.VacuumSet;

                        this.Clamp.PreheatTimeSet = floor.PreheatTimeSet;
                        this.Clamp.BakingTimeSet = floor.BakingTimeSet;
                        this.Clamp.BreathingCycleSet = floor.BreathingCycleSet;

                        this.Clamp.IsInFinished = true;

                        //改变另一夹具样品状态
                        if (this.SampleStatus == SampleStatus.待测试)
                        {
                            this.GetFloor().Stations.ForEach(s =>
                            {
                                if (s.Id != this.Id)
                                {
                                    s.SampleStatus = SampleStatus.待结果;
                                }
                            });
                        }
                    }
                    else if (value == FloorStatus.待出)
                    {
                        this.Clamp.BakingStopTime = DateTime.Now;

                        //要测试水分的炉子设置状态
                        //var floor = this.GetFloor();
                        //if (floor.IsTestWaterContent)
                        //{
                        //    var index = floor.Stations.IndexOf(this);
                        //    this.SampleStatus = index == 0 ? SampleStatus.待测试 : SampleStatus.待结果;
                        //}
                    }

                    if (value == FloorStatus.无盘 && floorStatus == FloorStatus.待出)
                    {
                        this.Clamp.OutOvenTime = DateTime.Now;
                        this.Clamp.IsOutFinished = true; 
                    }
                }

                if (floorStatus != value && this.IsAlive)
                {
                    PreFloorStatus = value;
                    UpdateDbField("FloorStatus", value);
                }
                floorStatus = value;
            }
        }

        /// <summary>
        /// 是否开门时会干涉（若为烤箱工位，当同一烤箱中其他门未关闭或有搬运任务，此门不能打开）
        /// </summary>
        [Description("是否开门时会干涉（若为烤箱工位，当同一烤箱中其他门未关闭或有搬运任务，此门不能打开)")]
        [DisplayName("是否开门时会干涉")]
        public bool IsOpenDoorIntervene
        {
            get
            {
                if (this.GetPutType != GetPutType.烤箱)
                {
                    return false;
                }

                if (this.DoorStatus == DoorStatus.打开)
                {
                    return false;
                }

                Floor oFloor = this.GetFloor();
                Oven oOven = this.GetOven();
                if (oOven.Floors.Count(f => f.Id != oFloor.Id && f.DoorStatus != DoorStatus.关闭) > 0)
                {
                    return true;
                }

                var result = false;

                if (Current.TaskMode == TaskMode.自动任务)
                {
                    oOven.Floors.ForEach(f =>
                    {
                        if (f.Id != oFloor.Id)
                        {
                            f.Stations.ForEach(s =>
                            {
                                if (s.Id == Current.Task.FromStationId || s.Id == Current.Task.ToStationId)
                                {
                                    result = true;
                                }
                            });
                        }
                    });
                }

                return result;
            }
        }

        /// <summary>
        /// 放满夹具优先级
        /// </summary>
        [DisplayName("放满夹具优先级")]
        public int PutFillClampPriority
        {
            get
            {
                if (this.GetPutType != GetPutType.烤箱)
                {
                    return 0;
                }
                Floor floor = this.GetFloor();
                if (floor.Stations.Count(s => s.FloorStatus == FloorStatus.待烤) > 0)
                {
                    return 1;
                }
                if (floor.Stations.Count(s => s.FloorStatus == FloorStatus.无盘) > 1)
                {
                    return 2;
                }
                if (floor.Stations.Count(s => s.FloorStatus == FloorStatus.待出) > 0)
                {
                    return 3;
                }
                if (floor.Stations.Count(s => s.FloorStatus == FloorStatus.空盘) > 0)
                {
                    return 4;
                }
                return 5;
            }
        }

        /// <summary>
        /// 放空夹具优先级
        /// </summary>
        [DisplayName("放空夹具优先级")]
        public int PutEmptyClampPriority
        {
            get
            {
                if (this.GetPutType != GetPutType.烤箱)
                {
                    return 0;
                }
                Floor floor = this.GetFloor();
                if (floor.Stations.Count(s => s.FloorStatus == FloorStatus.空盘) > 0)
                {
                    return 1;
                }
                if (floor.Stations.Count(s => s.FloorStatus == FloorStatus.无盘) > 1)
                {
                    return 2;
                }
                if (floor.Stations.Count(s => s.FloorStatus == FloorStatus.待出) > 0)
                {
                    return 3;
                }
                if (floor.Stations.Count(s => s.FloorStatus == FloorStatus.待烤) > 0)
                {
                    return 4;
                }
                return 5;
            }
        }

        /// <summary>
        /// 可以入空夹具
        /// 用来防止已经入了一个待烤夹具的炉层另一侧入空夹具导致最终该炉层无水含量夹具或者均为水含量夹具的问题
        /// </summary>
        [DisplayName("可以入空夹具"), Description("用来防止已经入了一个待烤夹具的炉层另一侧入空夹具导致最终该炉层无水含量夹具或者均为水含量夹具的问题")]
        public bool CanPutEmptyClamp
        {
            get
            {
                if (this.GetPutType != GetPutType.烤箱)
                {
                    return true;
                }

                Floor floor = this.GetFloor();
                if (floor.Stations.Count(s => s.FloorStatus == FloorStatus.待烤) > 0)
                {
                    return false;
                }

                return true;
            }
        }


        /// <summary>
        /// 取空夹具优先级
        /// </summary>
        [DisplayName("取空夹具优先级")]
        public int GetEmptyClampPriority
        {
            get
            {
                if (this.GetPutType != GetPutType.烤箱)
                {
                    return 0;
                }
                Floor floor = this.GetFloor();
                if (floor.Stations.Count(s => s.FloorStatus == FloorStatus.待烤) > 0)
                {
                    return 1;
                }
                if (floor.Stations.Count(s => s.FloorStatus == FloorStatus.待出) > 0)
                {
                    return 2;
                }
                if (floor.Stations.Count(s => s.FloorStatus == FloorStatus.无盘) > 0)
                {
                    return 3;
                }
                return 4;
            }
        }

        /// <summary>
        /// 取满夹具优先级
        /// </summary>
        [DisplayName("取满夹具优先级")]
        public int GetFillClampPriority
        {
            get
            {
                if (this.GetPutType != GetPutType.烤箱)
                {
                    return 0;
                }
                var floor = this.GetFloor();
                if (floor.Stations.Count(s => s.FloorStatus == FloorStatus.待烤) > 0)
                {
                    return 1;
                }
                if (floor.Stations.Count(s => s.FloorStatus == FloorStatus.空盘) > 0)
                {
                    return 2;
                }
                if (floor.Stations.Count(s => s.FloorStatus == FloorStatus.待出) == 1)
                {
                    return 3;
                }
                return 4;
            }
        }

        /// <summary>
        /// 出炉优先级
        /// </summary>
        [DisplayName("出炉优先级"), Description("出炉优先级,优先出当前无其他可能任务的烤箱")]
        public int OutPriority
        {
            get
            {
                if (this.GetPutType != GetPutType.烤箱)
                {
                    return 0;
                }
                return this.GetOven().GetCount(FloorStatus.无盘) + this.GetOven().GetCount(FloorStatus.空盘);
            }
        }

        [ReadOnly(true)]
        [DisplayName("是否可夹具扫码")]
        public bool IsClampScanReady { get; set; }

        /// <summary>
        /// 是否为样品信息标记
        /// </summary>
        [ReadOnly(true)]
        [DisplayName("是否为样品信息标记")]
        public bool HasSampleFlag
        {
            get
            {
                return this.ClampId > 0 && (this.Clamp.SampleInfo != SampleInfo.未知 && this.Clamp.SampleInfo != SampleInfo.无样品);
            }
        }


        [ReadOnly(true)]
        [DisplayName("温度数组")]
        public float[] Temperatures
        {
            get
            {
                return temperatures;
            }
            set
            {
                temperatures = value;
            }
        }
        private float[] temperatures = new float[Option.TemperaturePointCount];

        #region 绘制温度曲线相关
        public List<float>[] sampledDatas = new List<float>[Option.TemperaturePointCount];//采样数据1
        #endregion

        protected SampleStatus sampleStatus = SampleStatus.未知;

        /// <summary>
        /// 工位样品状态
        /// </summary>
        [DisplayName("工位样品状态")]
        public SampleStatus SampleStatus
        {
            get
            {
                return sampleStatus;
            }
            set
            {
                if (sampleStatus != value)
                {
                    UpdateDbField("SampleStatus", value);
                }
                sampleStatus = value;
            }
        }

        /// <summary>
        /// 夹具样品信息
        /// </summary>
        [DisplayName("夹具样品信息")]
        public SampleInfo SampleInfo
        {
            get
            {
                return this.Clamp.SampleInfo;
            }
            set
            {
                this.Clamp.SampleInfo = value;
            }
        }

        private bool sampleIsGet = false;
        /// <summary>
        /// 水分已被取走（下料机）
        /// </summary>
        [DisplayName("水分已被取走（下料机）")]
        public bool SampleIsGet
        {
            get
            {
                return sampleIsGet;
            }
            set
            {
                if (value)
                {
                    this.SampleStatus = SampleStatus.待测试;
                }
                sampleIsGet = value;
            }
        }


        #region 控制标志
        public bool toOpenDoor = false;
        public bool toCloseDoor = false;
        #endregion

        #endregion

        #region 构造方法

        public Station() : this(-1) { }

        public Station(int id)
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
            this.getPutType = (GetPutType)Enum.Parse(typeof(GetPutType), rowInfo["GetPutType"].ToString());
            this.clampStatus = (ClampStatus)Enum.Parse(typeof(ClampStatus), rowInfo["ClampStatus"].ToString());
            this.clampOri = (ClampOri)Enum.Parse(typeof(ClampOri), rowInfo["ClampOri"].ToString());
            this.clampId = TengDa._Convert.StrToInt(rowInfo["ClampId"].ToString(), -1);
            this.getPutTime = DateTime.Parse(rowInfo["GetPutTime"].ToString());
            this.priority = TengDa._Convert.StrToInt(rowInfo["Priority"].ToString(), 1);
            this.name = rowInfo["Name"].ToString();
            this.company = rowInfo["Company"].ToString();
            this.model = rowInfo["Model"].ToString();
            this.number = rowInfo["Number"].ToString();
            this.location = rowInfo["Location"].ToString();
            this.isEnable = Convert.ToBoolean(rowInfo["IsEnable"]);
            this.robotValues = rowInfo["RobotValues"].ToString();
            this.floorStatus = (FloorStatus)Enum.Parse(typeof(FloorStatus), rowInfo["FloorStatus"].ToString());
            this.sampleStatus = (SampleStatus)Enum.Parse(typeof(SampleStatus), rowInfo["SampleStatus"].ToString());
            this.PreFloorStatus = this.floorStatus;
            this.fromStationId = TengDa._Convert.StrToInt(rowInfo["FromStationId"].ToString(), -1);
            for (int i = 0; i < Option.TemperaturePointCount; i++)
            {
                this.sampledDatas[i] = new List<float>();
            }
        }
        #endregion

        #region 系统工位列表
        private static List<Station> stationList = new List<Station>();
        public static List<Station> StationList
        {
            get
            {
                if (stationList.Count < 1)
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
                            Station station = new Station();
                            station.InitFields(dt.Rows[i]);
                            stationList.Add(station);
                        }
                    }
                }
                return stationList;
            }
        }

        public static List<Station> CanGetPutStationList
        {
            get
            {
                return StationList.Where(s => s.IsAlive && !s.IsOpenDoorIntervene && s.CanOpenDoor).ToList();
            }
        }

        public static Station GetStation(int id)
        {
            return StationList.First(s => s.Id == id);
        }

        /// <summary>
        /// 烤箱 获取相邻工位
        /// </summary>
        /// <returns></returns>
        public Station GetLabStation()
        {
            return this.GetFloor().Stations.First(s => s.Id != this.Id);
        }
        /// <summary>
        /// 该工位所在炉层
        /// </summary>
        /// <returns></returns>
        public Floor GetFloor()
        {
            return Floor.FloorList.First(f => f.Stations.Contains(this));
        }
        /// <summary>
        /// 该工位所在烤箱
        /// </summary>
        /// <returns></returns>
        public Oven GetOven()
        {
            return Oven.OvenList.First(o => o.Floors.Contains(GetFloor()));
        }

        /// <summary>
        /// 该工位所在下料机
        /// </summary>
        /// <returns></returns>
        public Blanker GetBlanker()
        {
            return Blanker.BlankerList.First(b => b.Stations.Contains(this));
        }

        #endregion

        #region 控制方法
        /// <summary>
        /// 开门
        /// </summary>
        /// <returns></returns>
        public void OpenDoor()
        {
            if (this.DoorStatus != DoorStatus.打开)
            {
                if (this.GetPutType == GetPutType.烤箱)
                {
                    Floor floor = Floor.FloorList.First(f => f.StationIds.Contains(this.Id.ToString()));
                    Oven oven = Oven.OvenList.First(o => o.Id == floor.OvenId);
                    oven.OpenDoor(oven.Floors.IndexOf(floor));
                }
                else if (this.GetPutType == GetPutType.下料机)
                {
                    Blanker blanker = Blanker.BlankerList.First(b => b.StationIds.Contains(this.Id.ToString()));
                    blanker.OpenDoor();
                }
            }
        }

        /// <summary>
        /// 关门
        /// </summary>
        /// <returns></returns>
        public void CloseDoor()
        {
            if (this.DoorStatus != DoorStatus.关闭)
            {
                if (this.GetPutType == GetPutType.烤箱)
                {
                    Floor floor = Floor.FloorList.First(f => f.StationIds.Contains(this.Id.ToString()));
                    Oven oven = Oven.OvenList.First(o => o.Id == floor.OvenId);
                 //   oven.CloseDoor(oven.Floors.IndexOf(floor));
                }
                else if (this.GetPutType == GetPutType.下料机)
                {
                    Blanker blanker = Blanker.BlankerList.First(b => b.StationIds.Contains(this.Id.ToString()));
                    blanker.CloseDoor();
                }
            }
        }

        #endregion

        public void AddLog(string message)
        {
            string msg = string.Empty;
            if (!AddLog(message, out msg))
            {
                Error.Alert("工位日志保存失败，原因：" + msg);
            }
        }

        /// <summary>
        /// 保存日志
        /// </summary>
        /// <param name="message">内容</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool AddLog(string message, out string msg)
        {
            StationLog log = new StationLog
            {
                StationId = this.Id,
                Message = message
            };
            return StationLog.Add(new List<StationLog>() { log }, out msg);
        }
    }


    public class StationLog
    {
        #region 属性
        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".StationLog";
                }
                return tableName;
            }
        }

        /// <summary>
        /// Id
        /// </summary>
        [ReadOnly(true)]
        public int Id { get; set; }

        public int StationId { get; set; }

        public string Message { get; set; }

        public DateTime RecodeTime { get; set; }

        #endregion

        #region 添加任务日志

        /// <summary>
        /// 增加多个，数据库一次插入多行
        /// </summary>
        /// <param name="addTaskLogs"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool Add(List<StationLog> addLogs, out string msg)
        {
            if (addLogs.Count < 1)
            {
                msg = string.Empty;
                return true;
            }

            StringBuilder sb = new StringBuilder();

            foreach (StationLog stationLog in addLogs)
            {
                sb.Append(string.Format("({0}, '{1}', '{2}', {3}),", stationLog.StationId, stationLog.Message, DateTime.Now, TengDa.WF.Current.user.Id));
            }

            return Database.NonQuery(string.Format("INSERT INTO [dbo].[{0}] ([StationId], [Message], [RecodeTime], [UserId]) VALUES {1}", TableName, sb.ToString().TrimEnd(',')), out msg);
        }

        #endregion
    }


    public enum StationStatus
    {
        不可用 = 1,
        可取 = 2,
        可放 = 3,
        工作中 = 4
    }

    public enum DoorStatus
    {
        未知 = 1,
        打开 = 2,
        关闭 = 3,
        异常 = 4,
        正在打开 = 5,
        正在关闭 = 6
    }

    public enum FloorStatus
    {
        无盘,
        待烤,
        烘烤,
        待出,
        空盘,
        未知
    }

    public enum SampleStatus
    {
        待测试,
        测试OK,
        测试NG,
        待结果,
        未知
    }

    public enum RelativeMove
    {
        未知,
        靠近,
        远离,
        不变
    }
}
