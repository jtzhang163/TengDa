using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TengDa;
using System.Threading;
using System.ComponentModel;
using TengDa.WF;

namespace Soundon.Dispatcher
{
    public class Oven : TengDa.WF.Terminals.Terminal
    {
        #region 属性字段

        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".Ovens";
                }
                return tableName;
            }
        }

        public static string[] GetRuntimeStrs
        {
            get
            {
                return Current.option.GetRuntimeStrs.Split(',');
            }
        }

        public string Alarm2BinString = string.Empty;

        public string PreAlarm2BinString = string.Empty;

        [ReadOnly(true), DisplayName("三色灯")]
        public TriLamp TriLamp
        {
            get
            {
                if (this.IsAlive)
                {
                    var tmp = this.Floors.Select(f => f.TriLamp);
                    if (tmp.Count(t => t == TriLamp.Red) > 0)
                    {
                        return TriLamp.Red;
                    }
                    if (tmp.Count(t => t == TriLamp.Green) > 0)
                    {
                        return TriLamp.Green;
                    }
                    if (tmp.Count(t => t == TriLamp.Yellow) > 0)
                    {
                        return TriLamp.Yellow;
                    }
                }
                return TriLamp.Unknown;
            }
        }

        public float[] Temperature = new float[Option.TemperaturePointCount];

        private int plcId = -1;
        [ReadOnly(true), DisplayName("PLC ID")]
        public int PlcId
        {
            get { return plcId; }
            private set { plcId = value; }
        }

        public int getInfoNum = 0;

        private ClampOri clampOri = ClampOri.未知;
        /// <summary>
        /// 夹具方向
        /// </summary>
        [ReadOnly(true), DisplayName("夹具方向")]
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

        /// <summary>
        /// 要设置参数的指令
        /// </summary>
        [Browsable(false)]
        public List<string> ControlCommands = new List<string>();

        #endregion

        #region 构造方法

        public Oven() : this(-1) { }

        public Oven(int id)
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
            this.clampOri = (ClampOri)Enum.Parse(typeof(ClampOri), rowInfo["ClampOri"].ToString());
        }
        #endregion

        #region 系统烤箱列表
        private static List<Oven> ovenList = new List<Oven>();
        public static List<Oven> OvenList
        {
            get
            {
                if (ovenList.Count < 1)
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
                            Oven oven = new Oven();
                            oven.InitFields(dt.Rows[i]);
                            ovenList.Add(oven);
                        }
                    }

                }

                return ovenList;
            }
        }

        #endregion

        #region 获取烤箱

        public static Oven GetOven(string name, out string msg)
        {
            try
            {
                List<Oven> list = (from oven in OvenList where oven.Name == name select oven).ToList();
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
                    plc = new PLC(PlcId);
                }
                return plc;
            }
        }
        #endregion

        #region 该烤箱腔体列表
        private List<Floor> floors = new List<Floor>();
        [ReadOnly(true)]
        [Browsable(false)]
        public List<Floor> Floors
        {
            get
            {
                if (floors.Count < 1)
                {
                    floors = Floor.FloorList.Where(f => f.OvenId == this.Id).ToList();
                }
                return floors;
            }
        }

        public bool AlreadyGetAllInfo = false;

        #endregion

        #region 通信
        public bool GetInfo()
        {

            if (!this.Plc.IsPingSuccess)
            {
                this.Plc.IsAlive = false;
                LogHelper.WriteError("无法连接到 " + this.Plc.IP);
                return false;
            }

            string msg = string.Empty;
            string output = string.Empty;
            string input = string.Empty;
            try
            {

                #region 获取门状态
                output = string.Empty;
                for (int j = 0; j < this.Floors.Count; j++)
                {
                    var bOutputs = new short[] { };
                    if (!this.Plc.GetInfo(Current.option.OvenDoorStatusAddrs.Split(',')[j], (ushort)1, out bOutputs, out msg))
                    {
                        Error.Alert(msg);
                        this.Plc.IsAlive = false;
                        return false;
                    }

                    output = OmronPLC.GetBitStr(bOutputs[0], 8);

                    if (output.Substring(6, 1) == "1")
                    {
                        this.Floors[j].DoorStatusNotFinal = DoorStatus.打开; this.Floors[j].DoorIsOpenning = false;
                    }
                    else if (output.Substring(5, 1) == "1")
                    {
                        this.Floors[j].DoorStatusNotFinal = DoorStatus.关闭; this.Floors[j].DoorIsClosing = false;
                    }
                    else
                    {
                        this.Floors[j].DoorStatusNotFinal = DoorStatus.异常;
                    }
                }
                #endregion


                /*

                if (getInfoNum == 1)
                {
                    #region 获取温度
                    for (int j = 0; j < this.Floors.Count; j++)
                    {

                        output = string.Empty;
                        if (!this.Plc.GetInfo(false, Current.option.GetTemStrs.Split(',')[j], out output, out msg))
                        {
                            Error.Alert(msg);
                            this.Plc.IsAlive = false;
                            return false;
                        }
                        if (output.Substring(3, 1) != "$")
                        {
                            LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.GetTemStrs.Split(',')[j], output));
                            return false;
                        }

                        output = PanasonicPLC.ConvertHexStr(output.TrimEnd('\r'), false);

                        for (int k = 0; k < this.floors[j].Temperatures.Length; k++)
                        {
                            this.Floors[j].Temperatures[k] = (float)int.Parse(output.Substring(k * 4, 4), System.Globalization.NumberStyles.AllowHexSpecifier) / 10;
                        }
                    }
                    #endregion

                    #region 获取烤箱设置参数
                    for (int j = 0; j < this.Floors.Count; j++)
                    {

                        output = string.Empty;
                        if (!this.Plc.GetInfo(false, Current.option.GetParamSettingStrs.Split(',')[j], out output, out msg))
                        {
                            Error.Alert(msg);
                            this.Plc.IsAlive = false;
                            return false;
                        }
                        if (output.Substring(3, 1) != "$")
                        {
                            LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.GetParamSettingStrs.Split(',')[j], output));
                            return false;
                        }

                        output = PanasonicPLC.ConvertHexStr(output.TrimEnd('\r'), false);

                        this.Floors[j].PreheatTimeSet = int.Parse(output.Substring(4, 4), System.Globalization.NumberStyles.AllowHexSpecifier);
                        this.Floors[j].BakingTimeSet = int.Parse(output.Substring(12, 4), System.Globalization.NumberStyles.AllowHexSpecifier);
                        this.Floors[j].BreathingCycleSet = int.Parse(output.Substring(20, 4), System.Globalization.NumberStyles.AllowHexSpecifier);
                        this.Floors[j].VacuumSet = int.Parse(output.Substring(84, 4) + output.Substring(80, 4), System.Globalization.NumberStyles.AllowHexSpecifier);
                    }
                    #endregion

                }
                else if (getInfoNum == 3)
                {
                    #region 获取网控状态
                    output = string.Empty;
                    if (!this.Plc.GetInfo(false, Current.option.GetNetControlStatusStr, out output, out msg))
                    {
                        Error.Alert(msg);
                        this.Plc.IsAlive = false;
                        return false;
                    }

                    if (output.Substring(3, 1) != "$")
                    {
                        LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.GetNetControlStatusStr, output));
                        return false;
                    }

                    for (int j = 0; j < this.Floors.Count; j++)
                    {
                        this.Floors[j].IsNetControlOpen = output.Substring(6 + j, 1) == "1";
                    }
                    #endregion

                    #region 获取运行时间
                    for (int j = 0; j < this.Floors.Count; j++)
                    {
                        output = string.Empty;
                        if (!this.Plc.GetInfo(false, GetRuntimeStrs[j], out output, out msg))
                        {
                            Error.Alert(msg);
                            this.Plc.IsAlive = false;
                            return false;
                        }

                        if (output.Substring(3, 1) != "$")
                        {
                            LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.GetRuntimeStrs[j], output));
                            return false;
                        }
                        output = PanasonicPLC.ConvertHexStr(output.TrimEnd('\r'), false);
                        this.Floors[j].RunMinutes = int.Parse(output.Substring(0, 4), System.Globalization.NumberStyles.AllowHexSpecifier);
                        this.Floors[j].RunMinutesSet = int.Parse(output.Substring(4, 4), System.Globalization.NumberStyles.AllowHexSpecifier);
                    }
                    #endregion
                }
                if (getInfoNum % 2 == 0)
                {

                    #region 获取真空状态
                    output = string.Empty;
                    if (!this.Plc.GetInfo(false, Current.option.GetVacuumStatusStr, out output, out msg))
                    {
                        Error.Alert(msg);
                        this.Plc.IsAlive = false;
                        return false;
                    }

                    if (output.Substring(3, 1) != "$")
                    {
                        LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.GetVacuumStatusStr, output));
                        return false;
                    }

                    for (int j = 0; j < this.Floors.Count; j++)
                    {
                        this.Floors[j].IsVacuum = output.Substring(6 + j, 1) == "0";
                    }
                    #endregion

                    #region 报警信息
                    output = string.Empty;
                    if (!this.Plc.GetInfo(false, Current.option.GetAlarmStr, out output, out msg))
                    {
                        Error.Alert(msg);
                        this.Plc.IsAlive = false;
                        return false;
                    }

                    if (output.Substring(3, 1) != "$")
                    {
                        LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.GetAlarmStr, output));
                        return false;
                    }
                    this.Alarm2BinString = PanasonicPLC.Convert2BinStringForAlarm(output.TrimEnd('\r'));


                    if (this.Alarm2BinString != this.PreAlarm2BinString)
                    {
                        this.AlarmStr = string.Empty;
                        for (int j = 0; j < this.Floors.Count; j++)
                        {
                            this.Floors[j].AlarmStr = string.Empty;
                        }

                        List<AlarmLog> alarmLogs = new List<AlarmLog>();

                        for (int x = 0; x < this.Alarm2BinString.Length; x++)
                        {
                            if (x > Alarm.Alarms.Count - 1)
                            {
                                break;
                            }
                            char c = this.Alarm2BinString[x];
                            char cPre = this.PreAlarm2BinString.Length < this.Alarm2BinString.Length ? '0' : this.PreAlarm2BinString[x];
                            if (c == '1')
                            {
                                Alarm alarm = (from a in Alarm.Alarms where a.Id == x + 1 select a).ToList()[0];
                                if (alarm.FloorNum == 0)
                                {
                                    if (cPre == '0')
                                    {
                                        AlarmLog.Stop(AlarmType.Oven, x + 1, this.Id, out msg);
                                    }
                                }
                                else if (alarm.FloorNum > 0 && alarm.FloorNum <= this.Floors.Count)
                                {
                                    this.Floors[alarm.FloorNum - 1].AlarmStr += alarm.AlarmStr + ",";

                                    if (cPre == '0')
                                    {
                                        AlarmLog alarmLog = new AlarmLog();
                                        alarmLog.AlarmId = x + 1;
                                        alarmLog.AlarmType = AlarmType.Floor;
                                        alarmLog.TypeId = this.Floors[alarm.FloorNum - 1].Id;
                                        alarmLogs.Add(alarmLog);
                                    }
                                }
                            }
                        }

                        if (!AlarmLog.Add(alarmLogs, out msg))
                        {
                            Error.Alert(msg);
                        }

                    }

                    this.PreAlarm2BinString = this.Alarm2BinString;
                    #endregion

                    #region 获取烤箱设定温度
                    for (int j = 0; j < this.Floors.Count; j++)
                    {

                        output = string.Empty;
                        if (!this.Plc.GetInfo(false, Current.option.GetTemSetStrs.Split(',')[j], out output, out msg))
                        {
                            Error.Alert(msg);
                            this.Plc.IsAlive = false;
                            return false;
                        }
                        if (output.Substring(3, 1) != "$")
                        {
                            LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.GetTemSetStrs.Split(',')[j], output));
                            return false;
                        }

                        output = PanasonicPLC.ConvertHexStr(output.TrimEnd('\r'), false);

                        for (int k = 0; k < this.floors[j].TemperatureSets.Length; k++)
                        {
                            this.Floors[j].TemperatureSets[k] = (float)int.Parse(output.Substring(k * 4, 4), System.Globalization.NumberStyles.AllowHexSpecifier);
                        }

                        this.Floors[j].YunfengTemperatureSet = (float)int.Parse(output.Substring(44, 4), System.Globalization.NumberStyles.AllowHexSpecifier);
                    }
                    #endregion
                }

                #region 获取门控制指令信息
                output = string.Empty;
                if (!this.Plc.GetInfo(false, Current.option.GetDoorCommandStr, out output, out msg))
                {
                    Error.Alert(msg);
                    this.Plc.IsAlive = false;
                    return false;
                }

                if (output.Substring(3, 1) != "$")
                {
                    LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.GetDoorCommandStr, output));
                    return false;
                }

                for (int j = 0; j < this.Floors.Count; j++)
                {
                    if (output.Substring(6 + j, 1) == "1")
                    {
                        //this.Floors[j].DoorIsOpenning = true;
                    }
                    if (output.Substring(9 + j, 1) == "1")
                    {
                        //this.Floors[j].DoorIsClosing = true;
                    }
                }
                #endregion

                #region 获取门状态、真空和三色灯
                for (int j = 0; j < this.Floors.Count; j++)
                {
                    output = string.Empty;
                    if (!this.Plc.GetInfo(false, Current.option.GetMultiInfoStrs.Split(',')[j], out output, out msg))
                    {
                        Error.Alert(msg);
                        this.Plc.IsAlive = false;
                        return false;
                    }

                    if (output.Substring(3, 1) != "$")
                    {
                        LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.GetMultiInfoStrs.Split(',')[j], output));
                        return false;
                    }

                    var tmpStr = output;

                    output = PanasonicPLC.ConvertHexStr(output.TrimEnd('\r'), false);

                    switch (int.Parse(output.Substring(0, 4), System.Globalization.NumberStyles.AllowHexSpecifier))
                    {
                        case 1: this.Floors[j].DoorStatusNotFinal = DoorStatus.打开; this.Floors[j].DoorIsOpenning = false; break;
                        case 2: this.Floors[j].DoorStatusNotFinal = DoorStatus.关闭; this.Floors[j].DoorIsClosing = false; break;
                        case 3: break;
                        case 4: this.Floors[j].DoorStatusNotFinal = DoorStatus.异常; break;
                        default: this.Floors[j].DoorStatusNotFinal = DoorStatus.未知; break;
                    }

                    switch (int.Parse(output.Substring(4, 4), System.Globalization.NumberStyles.AllowHexSpecifier))
                    {
                        case 1: this.Floors[j].TriLamp = TriLamp.Green; break;
                        case 2: this.Floors[j].TriLamp = TriLamp.Yellow; break;
                        case 3: this.Floors[j].TriLamp = TriLamp.Red; break;
                        default: this.Floors[j].TriLamp = TriLamp.Unknown; break;
                    }

                    var tmpStr2 = PanasonicPLC.ConvertHexStr(tmpStr.TrimEnd('\r'), true, true);

                    this.Floors[j].Vacuum = (float)int.Parse(tmpStr2.Substring(16, 8), System.Globalization.NumberStyles.AllowHexSpecifier);

                }

                #endregion

                #region 获取真空控制指令信息
                output = string.Empty;
                if (!this.Plc.GetInfo(false, Current.option.GetVacuumCommandStr, out output, out msg))
                {
                    Error.Alert(msg);
                    this.Plc.IsAlive = false;
                    return false;
                }

                if (output.Substring(3, 1) != "$")
                {
                    LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.GetVacuumCommandStr, output));
                    return false;
                }

                for (int j = 0; j < this.Floors.Count; j++)
                {
                    this.Floors[j].VacuumIsLoading = output.Substring(6 + j, 1) == "1";
                    this.Floors[j].VacuumIsUploading = output.Substring(9 + j, 1) == "1";
                }
                #endregion

                #region 获取运行状态
                output = string.Empty;
                if (!this.Plc.GetInfo(false, Current.option.GetRunStatusStr, out output, out msg))
                {
                    Error.Alert(msg);
                    this.Plc.IsAlive = false;
                    return false;
                }

                if (output.Substring(3, 1) != "$")
                {
                    LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.GetRunStatusStr, output));
                    return false;
                }

                for (int j = 0; j < this.Floors.Count; j++)
                {
                    this.Floors[j].IsBaking = output.Substring(6 + j, 1) == "1";
                }

                Thread.Sleep(100);
                #endregion

                #region 获取是否运行完成

                output = string.Empty;
                if (!this.Plc.GetInfo(false, Current.option.GetBakingIsFinishedStr, out output, out msg))
                {
                    Error.Alert(msg);
                    this.Plc.IsAlive = false;
                    return false;
                }

                if (output.Substring(3, 1) != "$")
                {
                    LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.GetBakingIsFinishedStr, output));
                    return false;
                }

                for (int j = 0; j < this.Floors.Count; j++)
                {
                    this.Floors[j].IsBakeFinished = output.Substring(6 + j, 1) == "1";
                }

                #endregion
                */

                #region 写指令 控制开关门、启动运行、抽卸真空
                for (int j = 0; j < this.Floors.Count; j++)
                {

                    if (this.Floors[j].DoorIsOpenning && this.Floors[j].DoorStatusNotFinal != DoorStatus.打开)
                    {
                        this.Floors[j].DoorStatus = DoorStatus.正在打开;
                    }
                    else if (this.Floors[j].DoorIsClosing && this.Floors[j].DoorStatusNotFinal != DoorStatus.关闭)
                    {
                        this.Floors[j].DoorStatus = DoorStatus.正在关闭;
                    }
                    else
                    {
                        this.Floors[j].DoorStatus = this.Floors[j].DoorStatusNotFinal;
                    }

                    #region 控制开门
                    if (this.Floors[j].DoorStatus == DoorStatus.打开)
                    {
                        this.Floors[j].toOpenDoor = false;
                    }

                    if (this.Floors[j].toOpenDoor)
                    {
                        output = string.Empty;
                        if (!this.Plc.GetInfo(false, Current.option.OpenOvenDoorStrs.Split(',')[j], out output, out msg))
                        {
                            Error.Alert(msg);
                            this.Plc.IsAlive = false;
                            return false;
                        }

                        if (output.Substring(3, 1) != "$")
                        {
                            LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.OpenOvenDoorStrs.Split(',')[j], output));
                            return false;
                        }
                        LogHelper.WriteInfo(string.Format("成功发送开门指令到{0}:{1}", this.Floors[j].Name, Current.option.OpenOvenDoorStrs.Split(',')[j]));
                        this.Floors[j].toOpenDoor = false;
                        this.Floors[j].DoorIsOpenning = true;
                    }
                    #endregion

                    #region 控制关门
                    if (this.Floors[j].DoorStatus == DoorStatus.关闭)
                    {
                        this.Floors[j].toCloseDoor = false;
                    }

                    if (this.Floors[j].toCloseDoor)
                    {
                        output = string.Empty;
                        if (!this.Plc.GetInfo(false, Current.option.CloseOvenDoorStrs.Split(',')[j], out output, out msg))
                        {
                            Error.Alert(msg);
                            this.Plc.IsAlive = false;
                            return false;
                        }

                        if (output.Substring(3, 1) != "$")
                        {
                            LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.CloseOvenDoorStrs.Split(',')[j], output));
                            return false;
                        }
                        LogHelper.WriteInfo(string.Format("成功发送关门指令到{0}:{1}", this.Floors[j].Name, Current.option.CloseOvenDoorStrs.Split(',')[j]));
                        this.Floors[j].toCloseDoor = false;
                        this.Floors[j].DoorIsClosing = true;
                    }
                    #endregion

                    #region 启动运行
                    if (this.Floors[j].toStartBaking)
                    {
                        output = string.Empty;
                        if (!this.Plc.GetInfo(false, Current.option.StartBakingStrs.Split(',')[j], out output, out msg))
                        {
                            Error.Alert(msg);
                            this.Plc.IsAlive = false;
                            return false;
                        }

                        if (output.Substring(3, 1) != "$")
                        {
                            LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.StartBakingStrs.Split(',')[j], output));
                            return false;
                        }
                        LogHelper.WriteInfo(string.Format("成功发送启动运行指令到{0}:{1}", this.Floors[j].Name, Current.option.StartBakingStrs.Split(',')[j]));
                        this.Floors[j].toStartBaking = false;
                    }
                    #endregion

                    #region 结束运行
                    if (this.Floors[j].toStopBaking)
                    {
                        output = string.Empty;
                        if (!this.Plc.GetInfo(false, Current.option.StopBakingStrs.Split(',')[j], out output, out msg))
                        {
                            Error.Alert(msg);
                            this.Plc.IsAlive = false;
                            return false;
                        }

                        if (output.Substring(3, 1) != "$")
                        {
                            LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.StopBakingStrs.Split(',')[j], output));
                            return false;
                        }
                        LogHelper.WriteInfo(string.Format("成功发送停止运行指令到{0}:{1}", this.Floors[j].Name, Current.option.StopBakingStrs.Split(',')[j]));
                        this.Floors[j].toStopBaking = false;
                    }
                    #endregion

                    #region 开启网控
                    if (this.Floors[j].toOpenNetControl)
                    {
                        output = string.Empty;
                        if (!this.Plc.GetInfo(false, Current.option.OpenNetControlStrs.Split(',')[j], out output, out msg))
                        {
                            Error.Alert(msg);
                            this.Plc.IsAlive = false;
                            return false;
                        }

                        if (output.Substring(3, 1) != "$")
                        {
                            LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.OpenNetControlStrs.Split(',')[j], output));
                            return false;
                        }
                        LogHelper.WriteInfo(string.Format("成功发送开启网控指令到{0}:{1}", this.Floors[j].Name, Current.option.OpenNetControlStrs.Split(',')[j]));
                        this.Floors[j].toOpenNetControl = false;
                    }
                    #endregion

                    #region 报警复位
                    if (this.Floors[j].toAlarmReset)
                    {
                        output = string.Empty;
                        if (!this.Plc.GetInfo(false, Current.option.OvenAlarmResetStrs.Split(',')[j], out output, out msg))
                        {
                            Error.Alert(msg);
                            this.Plc.IsAlive = false;
                            return false;
                        }

                        if (output.Substring(3, 1) != "$")
                        {
                            LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.OvenAlarmResetStrs.Split(',')[j], output));
                            return false;
                        }
                        LogHelper.WriteInfo(string.Format("成功发送报警复位指令到{0}:{1}", this.Floors[j].Name, Current.option.OvenAlarmResetStrs.Split(',')[j]));
                        this.Floors[j].toAlarmReset = false;
                    }
                    #endregion

                    #region 抽真空
                    if (this.Floors[j].toLoadVacuum)
                    {
                        var command = Current.option.LoadVacuumStrs.Split(',')[j];
                        output = string.Empty;
                        if (!this.Plc.GetInfo(false, command, out output, out msg))
                        {
                            Error.Alert(msg);
                            this.Plc.IsAlive = false;
                            return false;
                        }

                        if (output.Substring(3, 1) != "$")
                        {
                            LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", command, output));
                            return false;
                        }
                        LogHelper.WriteInfo(string.Format("成功发送抽真空指令到{0}:{1}", this.Floors[j].Name, command));
                        this.Floors[j].toLoadVacuum = false;
                    }
                    #endregion

                    #region 取消抽真空
                    if (this.Floors[j].toCancelLoadVacuum)
                    {
                        var command = Current.option.LoadVacuumStrs.Split(',')[j];
                        command = command.Replace("1**", "0**");
                        output = string.Empty;
                        if (!this.Plc.GetInfo(false, command, out output, out msg))
                        {
                            Error.Alert(msg);
                            this.Plc.IsAlive = false;
                            return false;
                        }

                        if (output.Substring(3, 1) != "$")
                        {
                            LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", command, output));
                            return false;
                        }
                        LogHelper.WriteInfo(string.Format("成功发送取消抽真空指令到{0}:{1}", this.Floors[j].Name, command));
                        this.Floors[j].toCancelLoadVacuum = false;
                    }
                    #endregion

                    #region 泄真空
                    if (this.Floors[j].toUploadVacuum)
                    {
                        var command = Current.option.UnloadVacuumStrs.Split(',')[j];
                        output = string.Empty;
                        if (!this.Plc.GetInfo(false, command, out output, out msg))
                        {
                            Error.Alert(msg);
                            this.Plc.IsAlive = false;
                            return false;
                        }

                        if (output.Substring(3, 1) != "$")
                        {
                            LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", command, output));
                            return false;
                        }
                        LogHelper.WriteInfo(string.Format("成功发送泄真空指令到{0}:{1}", this.Floors[j].Name, command));
                        this.Floors[j].toUploadVacuum = false;
                    }
                    #endregion

                    #region 取消泄真空
                    if (this.Floors[j].toCancelUploadVacuum)
                    {
                        var command = Current.option.UnloadVacuumStrs.Split(',')[j];

                        command = command.Replace("1**", "0**");

                        output = string.Empty;
                        if (!this.Plc.GetInfo(false, command, out output, out msg))
                        {
                            Error.Alert(msg);
                            this.Plc.IsAlive = false;
                            return false;
                        }

                        if (output.Substring(3, 1) != "$")
                        {
                            LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", command, output));
                            return false;
                        }
                        LogHelper.WriteInfo(string.Format("成功发送取消泄真空指令到{0}:{1}", this.Floors[j].Name, command));
                        this.Floors[j].toCancelUploadVacuum = false;
                    }
                    #endregion

                    #region 运行时间清零
                    if (this.Floors[j].toClearRunTime)
                    {
                        output = string.Empty;
                        if (!this.Plc.GetInfo(false, Current.option.ClearRunTimeStrs.Split(',')[j], out output, out msg))
                        {
                            Error.Alert(msg);
                            this.Plc.IsAlive = false;
                            return false;
                        }

                        if (output.Substring(3, 1) != "$")
                        {
                            LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.ClearRunTimeStrs.Split(',')[j], output));
                            return false;
                        }
                        LogHelper.WriteInfo(string.Format("成功发送运行时间清零指令到{0}:{1}", this.Floors[j].Name, Current.option.ClearRunTimeStrs.Split(',')[j]));
                        this.Floors[j].toClearRunTime = false;
                    }
                    #endregion
                }
                #endregion

                #region 参数设置

                for (int x = 0; x < this.ControlCommands.Count; x++)
                {
                    output = string.Empty;
                    if (!this.Plc.GetInfo(false, this.ControlCommands[x], out output, out msg))
                    {
                        Error.Alert(msg);
                        this.Plc.IsAlive = false;
                        return false;
                    }

                    if (output.Substring(3, 1) != "$")
                    {
                        LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", this.ControlCommands[x], output));
                        return false;
                    }
                    LogHelper.WriteInfo(string.Format("成功发送参数设置指令到{0}:{1}", this.Name, this.ControlCommands[x]));

                    this.ControlCommands.Remove(this.ControlCommands[x]);
                    x--;
                }

                #endregion

              
            }
            catch (Exception ex)
            {
                Error.Alert(ex);
            }

            this.Plc.IsAlive = true;

            this.getInfoNum++;
            if (getInfoNum >= 4)
            {
                this.AlreadyGetAllInfo = true;
            }
            this.getInfoNum %= 4;
            return true;
        }

        /// <summary>
        /// 开门
        /// </summary>
        /// <param name="j">炉腔序号</param>
        /// <returns></returns>
        public void OpenDoor(int j)
        {
            if (!this.Plc.IsPingSuccess)
            {
                this.Plc.IsAlive = false;
                LogHelper.WriteError("无法连接到 " + this.Plc.IP);
                return;
            }

            if (!this.Floors[j].IsNetControlOpen)
            {
                Tip.Alert(this.Floors[j].Name + "网控未开启，无法远程开门 ");
                return;
            }

            if (this.Floors[j].IsBaking)
            {
                LogHelper.WriteError("在运行，无法开门 " + this.Floors[j].Name);
                return;
            }

            if (this.Floors[j].IsVacuum)
            {
                LogHelper.WriteError("有真空，无法开门 " + this.Floors[j].Name);
                return;
            }

            if (this.Floors[j].Stations[0].IsOpenDoorIntervene)
            {
                LogHelper.WriteError("开门会干涉，无法开门 " + this.Floors[j].Name);
                return;
            }

            //Floor floor = this.Floors.FirstOrDefault(f => f.Id != this.Floors[j].Id && f.DoorStatus != DoorStatus.关闭);

            //if (floor != null)
            //{
            //    Error.Alert(String.Format("{0}炉门{1}，{2}炉门无法打开！", floor.Name, floor.DoorStatus, this.Floors[j].Name));
            //    return;
            //}

            if (!this.Floors[j].DoorIsOpenning)
            {
                this.Floors[j].toOpenDoor = true;
            }
        }

        /// <summary>
        /// 关门
        /// </summary>
        /// <param name="j">炉腔序号</param>
        /// <returns></returns>
        public void CloseDoor(int j)
        {
            if (!this.Plc.IsPingSuccess)
            {
                this.Plc.IsAlive = false;
                LogHelper.WriteError("无法连接到 " + this.Plc.IP);
                return;
            }

            if (!this.Floors[j].IsNetControlOpen)
            {
                Tip.Alert(this.Floors[j].Name + "网控未开启，无法远程关门 ");
                return;
            }

            //Floor floor = this.Floors.FirstOrDefault(f => f.Id != this.Floors[j].Id && f.DoorStatus != DoorStatus.打开);

            //if (floor != null)
            //{
            //    Error.Alert(String.Format("{0}炉门{1}，{2}炉门无法关闭！", floor.Name, floor.DoorStatus, this.Floors[j].Name));
            //    return;
            //}

            if (!this.Floors[j].DoorIsClosing)
            {
                this.Floors[j].toCloseDoor = true;
            }
        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="j">炉腔序号</param>
        /// <returns></returns>
        public void StartBaking(int j)
        {
            if (!this.Plc.IsPingSuccess)
            {
                this.Plc.IsAlive = false;
                LogHelper.WriteError("无法连接到 " + this.Plc.IP);
                return;
            }

            if (!this.Floors[j].IsNetControlOpen)
            {
                Tip.Alert(this.Floors[j].Name + "网控未开启，无法远程启动 ");
                return;
            }

            if (this.Floors[j].DoorStatus != DoorStatus.关闭)
            {
                Tip.Alert(this.Floors[j].Name + "门未关闭，无法启动！");
                return;
            }

            this.Floors[j].toStartBaking = true;
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="j">炉腔序号</param>
        /// <returns></returns>
        public void StopBaking(int j)
        {
            if (!this.Plc.IsPingSuccess)
            {
                this.Plc.IsAlive = false;
                LogHelper.WriteError("无法连接到 " + this.Plc.IP);
                return;
            }

            if (!this.Floors[j].IsNetControlOpen)
            {
                Tip.Alert(this.Floors[j].Name + "网控未开启，无法远程停止运行 ");
                return;
            }

            this.Floors[j].toStopBaking = true;
        }

        /// <summary>
        /// 抽真空
        /// </summary>
        /// <param name="j">炉腔序号</param>
        /// <returns></returns>
        public void LoadVacuum(int j)
        {
            if (!this.Plc.IsPingSuccess)
            {
                this.Plc.IsAlive = false;
                LogHelper.WriteError("无法连接到 " + this.Plc.IP);
                return;
            }

            if (!this.Floors[j].IsNetControlOpen)
            {
                Tip.Alert(this.Floors[j].Name + "网控未开启，无法远程抽真空 ");
                return;
            }


            this.Floors[j].toLoadVacuum = true;
        }

        /// <summary>
        /// 泄真空
        /// </summary>
        /// <param name="j">炉腔序号</param>
        /// <returns></returns>
        public void UploadVacuum(int j)
        {
            if (!this.Plc.IsPingSuccess)
            {
                this.Plc.IsAlive = false;
                LogHelper.WriteError("无法连接到 " + this.Plc.IP);
                return;
            }

            if (!this.Floors[j].IsNetControlOpen)
            {
                Tip.Alert(this.Floors[j].Name + "网控未开启，无法远程卸真空 ");
                return;
            }

            if (this.Floors[j].IsBaking)
            {
                Tip.Alert(this.Floors[j].Name + "运行未完成，无法远程卸真空 ");
                return;
            }

            this.Floors[j].toUploadVacuum = true;
        }

        /// <summary>
        /// 取消抽真空
        /// </summary>
        /// <param name="j">炉腔序号</param>
        /// <returns></returns>
        public void CancelLoadVacuum(int j)
        {
            if (!this.Plc.IsPingSuccess)
            {
                this.Plc.IsAlive = false;
                LogHelper.WriteError("无法连接到 " + this.Plc.IP);
                return;
            }

            if (!this.Floors[j].IsNetControlOpen)
            {
                Tip.Alert(this.Floors[j].Name + "网控未开启，无法远程取消抽真空 ");
                return;
            }


            this.Floors[j].toCancelLoadVacuum = true;
        }

        /// <summary>
        /// 取消泄真空
        /// </summary>
        /// <param name="j">炉腔序号</param>
        /// <returns></returns>
        public void CancelUploadVacuum(int j)
        {
            if (!this.Plc.IsPingSuccess)
            {
                this.Plc.IsAlive = false;
                LogHelper.WriteError("无法连接到 " + this.Plc.IP);
                return;
            }

            if (!this.Floors[j].IsNetControlOpen)
            {
                Tip.Alert(this.Floors[j].Name + "网控未开启，无法远程取消卸真空 ");
                return;
            }

            if (this.Floors[j].IsBaking)
            {
                Tip.Alert(this.Floors[j].Name + "运行未完成，无法远程取消卸真空 ");
                return;
            }

            this.Floors[j].toCancelUploadVacuum = true;
        }

        /// <summary>
        /// 运行时间清零
        /// </summary>
        /// <param name="j">炉腔序号</param>
        /// <returns></returns>
        public void ClearRunTime(int j)
        {
            if (!this.Plc.IsPingSuccess)
            {
                this.Plc.IsAlive = false;
                LogHelper.WriteError("无法连接到 " + this.Plc.IP);
                return;
            }

            if (!this.Floors[j].IsNetControlOpen)
            {
                Tip.Alert(this.Floors[j].Name + "网控未开启，无法清零运行时间 ");
                return;
            }


            this.Floors[j].toClearRunTime = true;
        }

        /// <summary>
        /// 打开网控
        /// </summary>
        /// <param name="j">炉腔序号</param>
        /// <returns></returns>
        public void OpenNetControl(int j)
        {
            if (!this.Plc.IsPingSuccess)
            {
                this.Plc.IsAlive = false;
                LogHelper.WriteError("无法连接到 " + this.Plc.IP);
                return;
            }

            this.Floors[j].toOpenNetControl = true;
        }

        /// <summary>
        /// 报警复位
        /// </summary>
        /// <param name="j">炉腔序号</param>
        /// <returns></returns>
        public void AlarmReset(int j)
        {
            if (!this.Plc.IsPingSuccess)
            {
                this.Plc.IsAlive = false;
                LogHelper.WriteError("无法连接到 " + this.Plc.IP);
                return;
            }

            this.Floors[j].toAlarmReset = true;
        }

        #endregion
        /// <summary>
        /// 获取该烤箱下某种炉层状态的工位个数
        /// </summary>
        /// <param name="floorStatus"></param>
        /// <returns></returns>
        public int GetCount(FloorStatus floorStatus)
        {
            int count = 0;
            this.Floors.ForEach(f => f.Stations.ForEach(s =>
            {
                if (s.FloorStatus == floorStatus) count++;
            }));
            return count;
        }
        /// <summary>
        /// 改变该烤箱测试炉层
        /// </summary>
        public void ChangeWaterContentTestFloor()
        {
            Floor floor = this.Floors.FirstOrDefault(f => f.IsTestWaterContent);
            if (floor == null)
            {
                this.Floors[0].IsTestWaterContent = true;
                return;
            }
            int j = this.Floors.IndexOf(floor);
            j = (++j) % this.Floors.Count;
            this.Floors[j].IsTestWaterContent = true;
        }
    }
}
