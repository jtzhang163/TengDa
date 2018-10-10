using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TengDa;
using System.ComponentModel;
using System.Drawing;
using TengDa.WF;

namespace BakBattery.Baking
{
    public class Floor : TengDa.WF.Terminals.Terminal
    {

        #region 属性字段

        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".Floors";
                }
                return tableName;
            }
        }

        [ReadOnly(true), Description("已运行时间，单位：min")]
        [DisplayName("已运行时间")]
        public int RunMinutes
        {
            get
            {
                return runMinutes;
            }
            set
            {
                runMinutes = value;
            }
        }

        [ReadOnly(true), Description("剩余时间，单位：min")]
        [DisplayName("剩余时间")]
        public int RunRemainMinutes
        {
            get
            {
                return RunMinutesSet - RunMinutes;
            }
        }
        private int runMinutes = 0;

        [ReadOnly(true), Description("总运行时间设置，单位：min")]
        [DisplayName("总运行时间设置")]
        public int RunMinutesSet
        {
            get
            {
                return runMinutesSet;
            }
            set
            {
                runMinutesSet = value;
            }
        }
        private int runMinutesSet = 0;


        [ReadOnly(true), Description("真空度，单位：Pa")]
        [DisplayName("真空度")]
        public float Vacuum
        {
            get
            {
                return vacuum;
            }
            set
            {
                vacuum = value;
            }
        }
        private float vacuum = 0f;


        [ReadOnly(true), Description("是否正在烘烤")]
        [DisplayName("是否正在烘烤")]
        public bool IsBaking { get; set; }

        private bool isVacuum = false;
        [ReadOnly(true), Description("是否腔内有真空")]
        [DisplayName("是否腔内有真空")]
        public bool IsVacuum
        {
            get
            {
                return isVacuum;
            }
            set
            {
                if (TengDa.WF.Current.IsTerminalInitFinished)
                {
                    if (isVacuum != value)
                    {
                        if (value)
                        {
                            AddLog("开始抽真空");
                        }
                        else
                        {
                            AddLog("卸真空完成");
                        }
                    }
                }
                isVacuum = value;
            }
        }

        [ReadOnly(true), Description("网控已开启")]
        [DisplayName("网控已开启")]
        public bool IsNetControlOpen { get; set; }

        [ReadOnly(true), Description("所在烤箱Id")]
        [DisplayName("所在烤箱Id")]
        public int OvenId { get; private set; }

        /// <summary>
        /// 该炉腔所在烤箱
        /// </summary>
        /// <returns></returns>
        public Oven GetOven()
        {
            return Oven.OvenList.First(o => o.Floors.Contains(this));
        }

        private string stationIds = string.Empty;
        [ReadOnly(true), Description("工位Id集合")]
        [DisplayName("工位Id集合")]
        public string StationIds
        {
            get { return stationIds; }
            set { stationIds = value; }
        }

        private DoorStatus doorStatus = DoorStatus.未知;

        [ReadOnly(true), Description("门状态")]
        [DisplayName("门状态")]
        public DoorStatus DoorStatus
        {
            get
            {
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
                foreach (Station station in Stations)
                {
                    station.DoorStatus = value;
                }
                doorStatus = value;
            }
        }

        public DoorStatus DoorStatusNotFinal = DoorStatus.未知;

        [ReadOnly(true), Description("是否烘烤完成（分辨是待出还是待烤状态）")]
        [DisplayName("是否烘烤完成")]
        public bool IsBakeFinished { get; set; }


        public TriLamp triLamp = TriLamp.Unknown;
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
                triLamp = value;
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


        public bool DoorIsOpenning = false;
        public bool DoorIsClosing = false;

        /// <summary>
        /// 正在抽真空
        /// </summary>
        public bool VacuumIsLoading = false;

        /// <summary>
        /// 正在泄真空
        /// </summary>
        public bool VacuumIsUploading = false;

        #region 控制标志
        public bool toOpenDoor = false;
        public bool toCloseDoor = false;
        public bool toStartBaking = false;
        public bool toStopBaking = false;
        public bool toLoadVacuum = false;
        public bool toUploadVacuum = false;
        public bool toCancelLoadVacuum = false;
        public bool toCancelUploadVacuum = false;
        public bool toClearRunTime = false;
        public bool toOpenNetControl = false;
        public bool toAlarmReset = false;
        #endregion

        #endregion

        #region 要设置的参数

        private int preheatTimeSet = -1;

        /// <summary>
        /// 设置预热时间
        /// </summary>
        [Description("设置预热时间，单位：min")]
        [DisplayName("设置预热时间")]
        public int PreheatTimeSet
        {
            get
            {
                return preheatTimeSet;
            }
            set
            {
                preheatTimeSet = value;
            }
        }


        private int bakingTimeSet = -1;

        /// <summary>
        /// 设置烘烤时间
        /// </summary>
        [Description("设置烘烤时间，单位：min")]
        [DisplayName("设置烘烤时间")]
        public int BakingTimeSet
        {
            get
            {
                return bakingTimeSet;
            }
            set
            {
                bakingTimeSet = value;
            }
        }


        private int breathingCycleSet = -1;

        /// <summary>
        /// 设置呼吸周期
        /// </summary>
        [Description("设置呼吸周期，单位：min")]
        [DisplayName("设置呼吸周期")]
        public int BreathingCycleSet
        {
            get
            {
                return breathingCycleSet;
            }
            set
            {
                breathingCycleSet = value;
            }
        }

        #endregion

        #region 该腔体下的工位
        private List<Station> stations = new List<Station>();
        [Browsable(false)]
        public List<Station> Stations
        {
            get
            {
                if (stations.Count < 1)
                {
                    //string[] stationIds = StationIds.Split(',');
                    //stations = Station.StationList.Where(s => Array.IndexOf(stationIds, s.Id.ToString()) > -1).ToList();
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

        #region 该腔体下的夹具
        private List<Clamp> clamps = new List<Clamp>();
        [Browsable(false)]
        public List<Clamp> Clamps
        {
            get
            {
                clamps.Clear();
                foreach (Station station in Stations)
                {
                    if (station.Clamp.Id > 0)
                    {
                        clamps.Add(station.Clamp);
                    }
                }
                return clamps;
            }
        }

        #endregion

        #region 系统腔体列表
        private static List<Floor> floorList = new List<Floor>();
        public static List<Floor> FloorList
        {
            get
            {
                if (floorList.Count < 1)
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
                        floorList.Clear();
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            Floor floor = new Floor();
                            floor.InitFields(dt.Rows[i]);
                            floorList.Add(floor);
                        }
                    }
                }

                return floorList;
            }
        }

        #endregion

        #region 获取腔体

        public static Floor GetFloor(string name, out string msg)
        {
            try
            {
                List<Floor> list = (from floor in FloorList where floor.Name == name select floor).ToList();
                if (list.Count() > 0)
                {
                    msg = string.Empty;
                    return list[0];
                }
                msg = string.Format("数据库不存在名称为 {0} 的烤箱！", name);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return null;
        }

        #endregion

        #region 构造方法

        public Floor() : this(-1) { }

        public Floor(int id)
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
            this.stationIds = rowInfo["StationIds"].ToString();
            this.company = rowInfo["Company"].ToString();
            this.model = rowInfo["Model"].ToString();
            this.number = rowInfo["Number"].ToString();
            this.OvenId = TengDa._Convert.StrToInt(rowInfo["OvenId"].ToString(), -1);
            this.isEnable = Convert.ToBoolean(rowInfo["IsEnable"]);
            for (int i = 0; i < Option.TemperaturePointCount; i++)
            {
                this.sampledDatas[i] = new List<float>();
            }
        }
        #endregion

        public void AddLog(string message)
        {
            string msg = string.Empty;
            if (!AddLog(message, out msg))
            {
                Error.Alert("炉层日志保存失败，原因：" + msg);
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
            FloorLog log = new FloorLog
            {
                FloorId = this.Id,
                Message = message
            };
            return FloorLog.Add(new List<FloorLog>() { log }, out msg);
        }
    }

    public class FloorLog
    {
        #region 属性
        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".FloorLog";
                }
                return tableName;
            }
        }

        /// <summary>
        /// Id
        /// </summary>
        [ReadOnly(true)]
        public int Id { get; set; }

        public int FloorId { get; set; }

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
        public static bool Add(List<FloorLog> addLogs, out string msg)
        {
            if (addLogs.Count < 1)
            {
                msg = string.Empty;
                return true;
            }

            StringBuilder sb = new StringBuilder();

            foreach (FloorLog floorLog in addLogs)
            {
                sb.Append(string.Format("({0}, '{1}', '{2}', {3}),", floorLog.FloorId, floorLog.Message, DateTime.Now, TengDa.WF.Current.user.Id));
            }

            return Database.NonQuery(string.Format("INSERT INTO [dbo].[{0}] ([FloorId], [Message], [RecodeTime], [UserId]) VALUES {1}", TableName, sb.ToString().TrimEnd(',')), out msg);
        }

        #endregion
    }

}
