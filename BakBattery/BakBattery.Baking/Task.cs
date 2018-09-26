using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using TengDa;
using TengDa.WF;

namespace BakBattery.Baking
{
    /// <summary>
    /// 搬运任务
    /// </summary>
    public class Task : Service
    {
        #region 属性字段

        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".Tasks";
                }
                return tableName;
            }
        }


        private ClampOri clampOri = ClampOri.未知;
        /// <summary>
        /// 夹具方向
        /// </summary>
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

        private GetPutType fromType = GetPutType.未知;
        /// <summary>
        /// 源工位类型
        /// </summary>
        [DisplayName("源工位类型")]
        public GetPutType FromType
        {
            get { return fromType; }
            set
            {
                if (fromType != value)
                {
                    UpdateDbField("FromType", value);
                }
                fromType = value;
            }
        }

        private ClampStatus fromClampStatus = ClampStatus.未知;
        /// <summary>
        /// 源夹具状态
        /// </summary>
        [DisplayName("源夹具状态")]
        public ClampStatus FromClampStatus
        {
            get { return fromClampStatus; }
            set
            {
                if (fromClampStatus != value)
                {
                    UpdateDbField("FromClampStatus", value);
                }
                fromClampStatus = value;
            }
        }

        private GetPutType toType = GetPutType.未知;
        /// <summary>
        /// 目的工位类型
        /// </summary>
        [DisplayName("目的工位类型")]
        public GetPutType ToType
        {
            get { return toType; }
            set
            {
                if (toType != value)
                {
                    UpdateDbField("ToType", value);
                }
                toType = value;
            }
        }

        private ClampStatus toClampStatus = ClampStatus.未知;
        /// <summary>
        /// 目的夹具状态
        /// </summary>
        [ReadOnly(true)]
        [DisplayName("目的夹具状态")]
        public ClampStatus ToClampStatus
        {
            get { return toClampStatus; }
            set
            {
                if (toClampStatus != value)
                {
                    UpdateDbField("ToClampStatus", value);
                }
                toClampStatus = value;
            }
        }

        protected SampleStatus sampleStatus = SampleStatus.未知;
        [Description("样品工位信息")]
        [DisplayName("样品工位信息")]
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

        private int cycleOrder = 1;
        /// <summary>
        /// 任务循环次序
        /// </summary>
        [DisplayName("任务循环次序")]
        public int CycleOrder
        {
            get { return cycleOrder; }
            set
            {
                if (cycleOrder != value)
                {
                    UpdateDbField("CycleOrder", value);
                }
                cycleOrder = value;
            }
        }

        private string description = string.Empty;
        /// <summary>
        /// 描述
        /// </summary>
        [DisplayName("描述")]
        public string Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    UpdateDbField("Description", value);
                }
                description = value;
            }
        }

        protected bool isEnable = false;
        /// <summary>
        /// 是否启用
        /// </summary>
        [Description("是否启用")]
        [DisplayName("是否启用")]
        public bool IsEnable
        {
            get
            {
                return isEnable;
            }
            set
            {
                if (isEnable != value)
                {
                    UpdateDbField("IsEnable", value);
                }
                isEnable = value;
            }
        }

        #endregion

        #region 构造方法

        public Task() : this(-1) { }

        public Task(int id)
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
            this.clampOri = (ClampOri)Enum.Parse(typeof(ClampOri), rowInfo["ClampOri"].ToString());
            this.fromType = (GetPutType)Enum.Parse(typeof(GetPutType), rowInfo["FromType"].ToString());
            this.fromClampStatus = (ClampStatus)Enum.Parse(typeof(ClampStatus), rowInfo["FromClampStatus"].ToString());
            this.toType = (GetPutType)Enum.Parse(typeof(GetPutType), rowInfo["ToType"].ToString());
            this.toClampStatus = (ClampStatus)Enum.Parse(typeof(ClampStatus), rowInfo["ToClampStatus"].ToString());
            this.sampleStatus = (SampleStatus)Enum.Parse(typeof(SampleStatus), rowInfo["SampleStatus"].ToString());
            this.priority = TengDa._Convert.StrToInt(rowInfo["Priority"].ToString(), 1);
            this.cycleOrder = TengDa._Convert.StrToInt(rowInfo["CycleOrder"].ToString(), 1);
            this.description = rowInfo["Description"].ToString();
            this.isEnable = Convert.ToBoolean(rowInfo["IsEnable"]);
        }
        #endregion

        #region 系统任务列表
        private static List<Task> taskList = new List<Task>();
        public static List<Task> TaskList
        {
            get
            {
                if (taskList.Count < 1)
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
                            Task task = new Task();
                            task.InitFields(dt.Rows[i]);
                            taskList.Add(task);
                        }
                    }
                }
                return taskList;
            }
        }

        private static List<Task> canGetPutTaskList = new List<Task>();
        public static List<Task> CanGetPutTaskList
        {
            get
            {
                canGetPutTaskList.Clear();
                List<Task> tasks = TaskList.OrderBy(t => t.CycleOrder).ToList();
                for (int i = 0; i < tasks.Count; i++)
                {
                    if (TaskList[i].IsEnable && TaskList[i].CycleOrder > Current.Task.PreCycleOrder)
                    {
                        canGetPutTaskList.Add(TaskList[i]);
                    }
                }

                for (int i = 0; i < tasks.Count; i++)
                {
                    if (TaskList[i].IsEnable && TaskList[i].CycleOrder <= Current.Task.PreCycleOrder)
                    {
                        canGetPutTaskList.Add(TaskList[i]);
                    }
                }
                return canGetPutTaskList;
            }
        }
        #endregion

        public bool IsSuitSampleStatus(Station station)
        {
            if (this.SampleStatus != SampleStatus.未知 && this.SampleStatus == station.SampleStatus)
            {
                return true;
            }
            else if (this.SampleStatus == SampleStatus.未知)
            {
                return true;
            }
            return false;
        }

        public static void Run()
        {
            if (Current.TaskMode == TaskMode.自动任务)
            {

                //烤箱只有一个空位时，不要搬运空夹具至烤箱
                List<ClampOri> ClampOris = new List<ClampOri> { ClampOri.A, ClampOri.B };
                foreach (ClampOri clampOri in ClampOris)
                {
                    List<Station> stations = Station.StationList.Where(s =>
                            s.ClampOri == clampOri
                            && s.IsAlive
                            && s.GetPutType == GetPutType.烤箱
                            && s.FloorStatus == FloorStatus.无盘).ToList();

                    Task.TaskList.Where(t => t.FromClampStatus == ClampStatus.空夹具 && t.ToType == GetPutType.烤箱 && t.ClampOri == clampOri).ToList().
                        ForEach(t => t.IsEnable = stations.Count > 1);
                }


                if (Current.Task.Status == TaskStatus.完成)
                {
                    if (CurrentTask.ToSwitchManuTaskMode)
                    {
                        Current.Task.StartTime = TengDa.Common.DefaultTime;
                        Current.Task.TaskId = -1;
                        Current.Task.FromStationId = -1;
                        Current.Task.ToStationId = -1;
                        Current.Task.FromClampStatus = ClampStatus.未知;

                        Current.TaskMode = TaskMode.手动任务;
                        CurrentTask.ToSwitchManuTaskMode = false;
                        return;
                    }
                    ///任务遍历
                    foreach (Task task in Task.CanGetPutTaskList.Where(t => t.IsEnable))
                    {
                        List<Station> fromStations = Station.CanGetPutStationList
                            .Where(s => s.ClampOri == task.ClampOri
                            && s.GetPutType == task.FromType && s.ClampStatus == task.FromClampStatus
                            && s.Status == StationStatus.可取
                            && task.IsSuitSampleStatus(s))
                            .OrderBy(s => s.Distance(Current.Robot))
                            .OrderBy(s => s.Priority)
                            .OrderBy(s => s.GetPutTime)
                            .ToList();
                        List<Station> toStations = Station.CanGetPutStationList
                            .Where(s => s.ClampOri == task.ClampOri
                            && s.GetPutType == task.ToType && s.ClampStatus == task.ToClampStatus
                            && s.Status == StationStatus.可放
                            && task.IsSuitSampleStatus(s))
                            .OrderBy(s => s.Priority)
                            //.OrderBy(s => s.Distance(Current.Robot))
                            .OrderBy(s => s.GetPutTime)
                            .ToList();

                        if (fromStations.Count > 0 && toStations.Count > 0)
                        {
                            if (task.FromClampStatus == ClampStatus.满夹具 && task.FromType == GetPutType.烤箱)
                            {
                                fromStations = fromStations.OrderBy(s => s.GetFillClampPriority).ToList();
                            }
                            if (task.FromClampStatus == ClampStatus.空夹具 && task.FromType == GetPutType.烤箱)
                            {
                                fromStations = fromStations.OrderBy(s => s.GetEmptyClampPriority).ToList();
                            }
                            if (task.FromClampStatus == ClampStatus.满夹具 && task.ToType == GetPutType.烤箱)
                            {
                                toStations = toStations.OrderBy(s => s.PutFillClampPriority).ToList();
                            }
                            if (task.FromClampStatus == ClampStatus.空夹具 && task.ToType == GetPutType.烤箱)
                            {
                                toStations = toStations.Where(s => s.CanPutEmptyClamp).OrderBy(s => s.PutEmptyClampPriority).ToList();
                            }

                            Station fromStation = fromStations.First();
                            Station toStation = toStations.First();
                            Current.Task.StartTime = DateTime.Now;
                            Current.Task.TaskId = task.Id;
                            Current.Task.FromStationId = fromStation.Id;
                            Current.Task.ToStationId = toStation.Id;
                            Current.Task.FromClampStatus = fromStation.ClampStatus;
                            Current.Task.PreCycleOrder = task.CycleOrder;
                            Current.Task.Status = TaskStatus.就绪;
                            break;
                        }
                        else
                        {
                            Current.Task.StartTime = TengDa.Common.DefaultTime;
                            Current.Task.TaskId = -1;
                            Current.Task.FromStationId = -1;
                            Current.Task.ToStationId = -1;
                            Current.Task.FromClampStatus = ClampStatus.未知;
                            Current.Task.Status = TaskStatus.完成;
                        }
                    }
                }

                else if (Current.Task.FromStationId > 0 && Current.Task.ToStationId > 0 && Current.Task.FromStation != null && Current.Task.ToStation != null)
                {

                    if (Current.Task.Status == TaskStatus.就绪)
                    {
                        int d3410 = int.Parse(Current.Task.FromStation.RobotValues.Split(',')[0]);
                        int d3411 = int.Parse(Current.Task.FromStation.RobotValues.Split(',')[1]);
                        if (Current.Robot.IsReadyGet)
                        {
                            if (Current.Robot.Move(d3410, d3411, isGet: true))
                            {
                                Current.Task.Status = TaskStatus.可取;
                            }
                        }

                    }
                    else if (Current.Task.Status == TaskStatus.可取 && Current.Task.FromStation != null)
                    {
                        if (Current.Task.FromStation.DoorStatus != DoorStatus.打开)
                        {
                            Current.Task.FromStation.OpenDoor();
                        }

                        if (Current.Task.ToStation.DoorStatus != DoorStatus.打开 && Current.Task.ToStation != null && Current.Task.ToStation.GetPutType == GetPutType.烤箱 && Current.Robot.IsGettingOrPutting)
                        {
                            Current.Task.ToStation.OpenDoor();
                        }

                        if (Current.Task.FromStation.DoorStatus == DoorStatus.打开 && Current.Task.FromStation.ClampStatus != ClampStatus.无夹具)
                        {
                            Current.Robot.StartGetPut();
                        }

                        if (Current.Task.FromStation.ClampStatus == ClampStatus.无夹具)
                        {
                            Current.Robot.ClampStatus = Current.Task.FromClampStatus;
                            Current.Robot.Location = Current.Task.FromStation.Location;
                            if (Current.Task.FromStation.ClampId > 0)
                            {
                                Current.Robot.ClampId = Current.Task.FromStation.ClampId;
                                Current.Task.FromStation.ClampId = -1;
                            }

                            if (!Current.Robot.IsGettingOrPutting)
                            {
                                Current.Task.Status = TaskStatus.取完;
                            }
                        }
        
                    }
                    else if (Current.Task.Status == TaskStatus.取完 && Current.Task.ToStation != null)
                    {
                        int d3410 = int.Parse(Current.Task.ToStation.RobotValues.Split(',')[0]);
                        int d3411 = int.Parse(Current.Task.ToStation.RobotValues.Split(',')[1]);

                        if (Current.Robot.IsReadyPut)
                        {
                            if (Current.Robot.Move(d3410, d3411, isGet: false))
                            {
                                Current.Task.Status = TaskStatus.可放;
                            }
                        }
                    }
                    else if (Current.Task.Status == TaskStatus.可放 && Current.Task.ToStation != null)
                    {
                        if (Current.Task.ToStation.DoorStatus != DoorStatus.打开)
                        {
                            Current.Task.ToStation.OpenDoor();
                        }

                        if (Current.Task.ToStation.DoorStatus == DoorStatus.打开 && !Current.Robot.IsGettingOrPutting && Current.Task.ToStation.ClampStatus == ClampStatus.无夹具)
                        {
                            Current.Robot.StartGetPut();
                        }

                        if (Current.Task.ToStation.ClampStatus != ClampStatus.无夹具)
                        {
                            Current.Task.ToStation.ClampStatus = Current.Task.FromClampStatus;
                            Current.Task.ToStation.FromStationId = Current.Task.FromStationId;

                            if (Current.Task.ToStation.ClampId < 1 && Current.Task.ToStation.GetPutType == GetPutType.上料机 && Current.Task.FromClampStatus == ClampStatus.空夹具)
                            {
                                string msg = string.Empty;
                                int clampId = Clamp.Add(new Clamp(Current.Robot.ClampId).Code, out msg);
                                if (clampId > 0)
                                {
                                    Current.Task.ToStation.ClampId = clampId;
                                }
                                else
                                {
                                    LogHelper.WriteError(msg);
                                }
                            }
                            else if (Current.Robot.ClampId > 0)
                            {
                                Current.Task.ToStation.ClampId = Current.Robot.ClampId;
                            }

                            Current.Robot.ClampId = -1;
                            Current.Robot.ClampStatus = ClampStatus.无夹具;
                            Current.Robot.Location = Current.Task.ToStation.Location;
                        }

                        if (Current.Robot.CanCheckGetPutClampIsOk)
                        {
                            Current.Robot.ClampStatus = ClampStatus.无夹具;
                            if (Current.Task.ToStation.ClampStatus == ClampStatus.无夹具)
                            {
                                //放盘无效报警
                                Current.Robot.PutClampIsNotOkAlarm();
                            }
                        }

                        if (!Current.Robot.IsGettingOrPutting && Current.Task.ToStation.ClampStatus != ClampStatus.无夹具)
                        {
                            string msg = string.Empty;
                            if (TaskLog.Add(out msg))//记录
                            {
                                Current.Task.Status = TaskStatus.完成;
                            }
                            else
                            {
                                Error.Alert("保存搬运记录失败：" + msg);
                            }
                        }
                    }
                }
            }
            else if (Current.TaskMode == TaskMode.手动任务)
            {
                if (Current.Task.Status == TaskStatus.完成)
                {
                    Current.Task.StartTime = TengDa.Common.DefaultTime;
                    Current.Task.TaskId = -1;
                    Current.Task.FromStationId = -1;
                    Current.Task.ToStationId = -1;
                    Current.Task.FromClampStatus = ClampStatus.未知;
                    Current.Task.Status = TaskStatus.完成;

                    if (Current.Task.NextFromStationId > 0)
                    {
                        Current.Task.FromStationId = Current.Task.NextFromStationId;
                        Current.Task.NextFromStationId = -1;
                        Current.Task.FromClampStatus = Current.Task.FromStation.ClampStatus;
                        Current.Task.Status = TaskStatus.就绪;
                    }
                }
                else if (Current.Task.Status == TaskStatus.就绪 && Current.Task.FromStation != null)
                {
                    if (Current.Task.FromStation.DoorStatus != DoorStatus.打开)
                    {
                        Tip.Alert(Current.Task.FromStation.Name + "门未打开！");
                    }

                    if (Current.Task.FromStation.DoorStatus == DoorStatus.打开)
                    {
                        int d3410 = int.Parse(Current.Task.FromStation.RobotValues.Split(',')[0]);
                        int d3411 = int.Parse(Current.Task.FromStation.RobotValues.Split(',')[1]);
                        if (Current.Robot.IsReadyGet)
                        {
                            if (Current.Robot.Move(d3410, d3411, isGet: true))
                            {
                                if (Current.Task.FromStation.DoorStatus == DoorStatus.打开)
                                {
                                    Current.Task.Status = TaskStatus.可取;
                                }
                            }
                        }
                    }
                }
                else if (Current.Task.Status == TaskStatus.可取 && Current.Task.FromStation != null)
                {

                    if (Current.Task.FromStation.DoorStatus == DoorStatus.打开 && Current.Task.FromStation.ClampStatus != ClampStatus.无夹具)
                    {
                        Current.Robot.StartGetPut();
                    }

                    if (Current.Task.FromStation.ClampStatus == ClampStatus.无夹具)
                    {
                        Current.Robot.ClampStatus = Current.Task.FromClampStatus;
                        Current.Robot.Location = Current.Task.FromStation.Location;
                        if (Current.Task.FromStation.ClampId > 0)
                        {
                            Current.Robot.ClampId = Current.Task.FromStation.ClampId;
                            Current.Task.FromStation.ClampId = -1;
                        }
                        
                        if (!Current.Robot.IsGettingOrPutting)
                        {
                            Current.Task.Status = TaskStatus.取完;
                        }
                    }
                }
                else if (Current.Task.Status == TaskStatus.取完)
                {
                    if (Current.Task.NextToStationId > 0)
                    {
                        Current.Task.ToStationId = Current.Task.NextToStationId;
                        Current.Task.NextToStationId = -1;
                    }
                    if (Current.Task.ToStation != null)
                    {
                        if (Current.Task.ToStation.DoorStatus != DoorStatus.打开)
                        {
                            Tip.Alert(Current.Task.ToStation.Name + "门未打开！");
                        }
                        if (Current.Task.ToStation.DoorStatus == DoorStatus.打开)
                        {
                            int d3410 = int.Parse(Current.Task.ToStation.RobotValues.Split(',')[0]);
                            int d3411 = int.Parse(Current.Task.ToStation.RobotValues.Split(',')[1]);

                            if (Current.Robot.IsReadyPut)
                            {
                                if (Current.Robot.Move(d3410, d3411, isGet: false))
                                {
                                    if (Current.Task.ToStation.DoorStatus == DoorStatus.打开)
                                    {
                                        Current.Task.Status = TaskStatus.可放;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (Current.Task.Status == TaskStatus.可放 && Current.Task.ToStation != null)
                {

                    if (Current.Task.ToStation.DoorStatus == DoorStatus.打开 && !Current.Robot.IsGettingOrPutting && Current.Task.ToStation.ClampStatus == ClampStatus.无夹具)
                    {
                        Current.Robot.StartGetPut();
                    }

                    if (Current.Task.ToStation.ClampStatus != ClampStatus.无夹具)
                    {
                        Current.Task.ToStation.ClampStatus = Current.Task.FromClampStatus;
                        Current.Task.ToStation.FromStationId = Current.Task.FromStationId;

                        if (Current.Task.ToStation.ClampId < 1 && Current.Task.ToStation.GetPutType == GetPutType.上料机 && Current.Task.FromClampStatus == ClampStatus.空夹具)
                        {
                            string msg = string.Empty;
                            int clampId = Clamp.Add(new Clamp(Current.Robot.ClampId).Code, out msg);
                            if (clampId > 0)
                            {
                                Current.Task.ToStation.ClampId = clampId;
                            }
                            else
                            {
                                LogHelper.WriteError(msg);
                            }
                        }
                        else if (Current.Robot.ClampId > 0)
                        {
                            Current.Task.ToStation.ClampId = Current.Robot.ClampId;
                        }
                        Current.Robot.ClampId = -1;

                        Current.Robot.ClampStatus = ClampStatus.无夹具;
                        Current.Robot.Location = Current.Task.ToStation.Location;
                    }

                    if (Current.Robot.CanCheckGetPutClampIsOk)
                    {
                        Current.Robot.ClampStatus = ClampStatus.无夹具;
                        if (Current.Task.ToStation.ClampStatus == ClampStatus.无夹具)
                        {
                            //放盘无效报警
                            Current.Robot.PutClampIsNotOkAlarm();
                        }
                    }

                    if (!Current.Robot.IsGettingOrPutting && Current.Task.ToStation.ClampStatus != ClampStatus.无夹具)
                    {

                        string msg = string.Empty;
                        if (TaskLog.Add(out msg))//记录
                        {
                            Current.Task.Status = TaskStatus.完成;
                        }
                        else
                        {
                            Error.Alert("保存搬运记录失败：" + msg);
                        }
                    }
                }
            }
        }
    }

    public enum GetPutType
    {
        未知,
        上料机,
        烤箱,
        下料机,
        旋转台,
        缓存架
    }

    public class TaskLog
    {
        #region 属性
        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".TaskLog";
                }
                return tableName;
            }
        }

        /// <summary>
        /// Id
        /// </summary>
        [ReadOnly(true)]
        public int Id { get; set; }

        public int TaskId { get; set; }

        public int FromStationId { get; set; }

        public int ToStationId { get; set; }

        public ClampStatus ClampStatus { get; set; }

        public TaskMode TaskMode { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime FinishedTime { get; set; }

        #endregion

        #region 添加任务日志

        public static bool Add(out string msg)
        {
            TaskLog taskLog = new TaskLog
            {
                FinishedTime = DateTime.Now,
                FromStationId = Current.Task.FromStationId,
                ToStationId = Current.Task.ToStationId,
                StartTime = Current.Task.StartTime,
                TaskId = Current.Task.TaskId,
                ClampStatus = Current.Task.FromClampStatus,
                TaskMode = Current.TaskMode
            };
            return Add(new List<TaskLog>() { taskLog }, out msg);
        }

        /// <summary>
        /// 增加多个，数据库一次插入多行
        /// </summary>
        /// <param name="addTaskLogs"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool Add(List<TaskLog> addTaskLogs, out string msg)
        {
            if (addTaskLogs.Count < 1)
            {
                msg = string.Empty;
                return true;
            }

            StringBuilder sb = new StringBuilder();

            foreach (TaskLog taskLog in addTaskLogs)
            {
                sb.Append(string.Format("({0}, {1}, {2}, '{3}', '{4}', {5}, '{6}', '{7}'),", taskLog.TaskId, taskLog.FromStationId, taskLog.ToStationId, taskLog.StartTime, taskLog.FinishedTime, TengDa.WF.Current.user.Id, taskLog.TaskMode, taskLog.ClampStatus));
            }

            return Database.NonQuery(string.Format("INSERT INTO [dbo].[{0}] ([TaskId], [FromStationId], [ToStationId], [StartTime], [FinishedTime], [UserId], [TaskMode], [ClampStatus]) VALUES {1}", TableName, sb.ToString().TrimEnd(',')), out msg);
        }

        #endregion
    }


    public class CurrentTask : Service
    {
        #region 属性
        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".CurrentTask";
                }
                return tableName;
            }
        }

        private int taskId = -1;
        /// <summary>
        /// 任务Id
        /// </summary>
        [DisplayName("任务ID")]
        public int TaskId
        {
            get { return taskId; }
            set
            {
                if (taskId != value)
                {
                    UpdateDbField("TaskId", value);
                }
                taskId = value;
            }
        }

        //public Task RunningTask
        //{
        //    get
        //    {
        //        return Task.TaskList.FirstOrDefault(t => t.Id == this.TaskId);
        //    }
        //}

        private int fromStationId = -1;
        /// <summary>
        /// 源工位Id
        /// </summary>
        [DisplayName("源工位ID")]
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

        private int toStationId = -1;
        /// <summary>
        /// 目的工位Id
        /// </summary>
        [DisplayName("目的工位ID")]
        public int ToStationId
        {
            get { return toStationId; }
            set
            {
                if (toStationId != value)
                {
                    UpdateDbField("ToStationId", value);
                }
                toStationId = value;
            }
        }

        private int nextFromStationId = -1;
        /// <summary>
        /// 手动下一任务源工位ID
        /// </summary>
        [ReadOnly(true), DisplayName("手动下一任务源工位ID")]
        public int NextFromStationId
        {
            get { return nextFromStationId; }
            set { nextFromStationId = value; }
        }

        private int nextToStationId = -1;
        /// <summary>
        /// 手动下一任务目的工位ID
        /// </summary>
        [ReadOnly(true), DisplayName("手动下一任务目的工位ID")]
        public int NextToStationId
        {
            get { return nextToStationId; }
            set { nextToStationId = value; }
        }

        private int preCycleOrder = 1;
        /// <summary>
        /// 上次任务循环次序
        /// </summary>
        [DisplayName("上次任务循环次序")]
        public int PreCycleOrder
        {
            get { return preCycleOrder; }
            set
            {
                if (preCycleOrder != value)
                {
                    UpdateDbField("PreCycleOrder", value);
                }
                preCycleOrder = value;
            }
        }


        private DateTime startTime = TengDa.Common.DefaultTime;
        /// <summary>
        /// 当前任务开始时间
        /// </summary>
        [ReadOnly(true)]
        [DisplayName("当前任务开始时间")]
        public DateTime StartTime
        {
            get { return startTime; }
            set
            {
                if (startTime != value)
                {
                    UpdateDbField("StartTime", value);
                }
                startTime = value;
            }
        }


        private TaskStatus status = TaskStatus.未知;
        /// <summary>
        /// 任务状态
        /// </summary>
        [DisplayName("任务状态")]
        public TaskStatus Status
        {
            get { return status; }
            set
            {
                if (status != value)
                {
                    UpdateDbField("Status", value);
                    LogHelper.WriteInfo(string.Format("当前任务状态: {0}—>{1}", status, value));
                }
                status = value;
            }
        }

        /// <summary>
        /// 源工位
        /// </summary>
        [Browsable(false)]
        public Station FromStation
        {
            get
            {
                return Station.StationList.FirstOrDefault(s => s.Id == this.FromStationId);
            }
        }

        /// <summary>
        /// 目的工位
        /// </summary>
        [Browsable(false)]
        public Station ToStation
        {
            get
            {
                return Station.StationList.FirstOrDefault(s => s.Id == this.ToStationId);
            }
        }

        /// <summary>
        /// 源工位名称
        /// </summary>
        [DisplayName("源工位名称")]
        public string FromStationName
        {
            get
            {
                return FromStation == null ? "" : FromStation.Name;
            }
        }

        /// <summary>
        /// 目的工位名称
        /// </summary>
        [DisplayName("目的工位名称")]
        public string ToStationName
        {
            get
            {
                return ToStation == null ? "" : ToStation.Name;
            }
        }

        private ClampStatus fromClampStatus = ClampStatus.未知;
        /// <summary>
        /// 源夹具状态
        /// </summary>
        [ReadOnly(true), DisplayName("源夹具状态")]
        public ClampStatus FromClampStatus
        {
            get { return fromClampStatus; }
            set
            {
                if (fromClampStatus != value)
                {
                    UpdateDbField("FromClampStatus", value);
                }
                fromClampStatus = value;
            }
        }

        public static bool ToSwitchManuTaskMode = false;

        #endregion

        #region 构造方法

        public CurrentTask() : this(-1) { }

        public CurrentTask(int id)
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
            this.taskId = TengDa._Convert.StrToInt(rowInfo["TaskId"].ToString(), -1);
            this.fromStationId = TengDa._Convert.StrToInt(rowInfo["FromStationId"].ToString(), -1);
            this.toStationId = TengDa._Convert.StrToInt(rowInfo["ToStationId"].ToString(), -1);
            this.preCycleOrder = TengDa._Convert.StrToInt(rowInfo["PreCycleOrder"].ToString(), 1);
            this.status = (TaskStatus)Enum.Parse(typeof(TaskStatus), rowInfo["Status"].ToString());
            this.fromClampStatus = (ClampStatus)Enum.Parse(typeof(ClampStatus), rowInfo["FromClampStatus"].ToString());
            this.startTime = DateTime.Parse(rowInfo["StartTime"].ToString());
        }
        #endregion
    }
    public enum TaskStatus
    {
        未知,
        就绪,
        可取,
        取完,
        可放,
        完成
    }
}
