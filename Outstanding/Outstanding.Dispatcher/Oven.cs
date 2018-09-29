using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TengDa;
using System.Threading;
using System.ComponentModel;
using TengDa.WF;

namespace Outstanding.Dispatcher
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

                if (getInfoNum == 1)
                {
                    #region 获取温度左前25
                    for (int j = 0; j < this.Floors.Count; j++)
                    {

                        output = string.Empty;
                        if (!this.Plc.GetInfo(false, Current.option.GetTemStrs1.Split(',')[j], out output, out msg))
                        {
                            Error.Alert(msg);
                            this.Plc.IsAlive = false;
                            return false;
                        }
                        if (output.Substring(3, 1) != "$")
                        {
                            LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.GetTemStrs1.Split(',')[j], output));
                            return false;
                        }

                        output = PanasonicPLC.ConvertHexStr(output.TrimEnd('\r'), false);

                        for (int k = 0; k < this.floors[j].Stations[0].Temperatures.Length / 2; k++)
                        {
                            this.Floors[j].Stations[0].Temperatures[k] = (float)int.Parse(output.Substring(k * 4, 4), System.Globalization.NumberStyles.AllowHexSpecifier) / 10;
                        }
                    }
                    #endregion

                    #region 获取真空
                    output = string.Empty;
                    if (!this.Plc.GetInfo(false, Current.option.GetVacuumStr, out output, out msg))
                    {
                        Error.Alert(msg);
                        this.Plc.IsAlive = false;
                        return false;
                    }

                    if (output.Substring(3, 1) != "$")
                    {
                        LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.GetVacuumStr, output));
                        return false;
                    }

                    output = PanasonicPLC.ConvertHexStr(output.TrimEnd('\r'), true, true);
                    for (int j = 0; j < this.Floors.Count; j++)
                    {
                        this.Floors[j].Vacuum = (float)int.Parse(output.Substring(j * 8, 8), System.Globalization.NumberStyles.AllowHexSpecifier);
                    }
                    //for (int j = 0; j < this.Floors.Count; j++)
                    //{
                    //    uint num = uint.Parse(output.Substring(32 - j * 8, 8), System.Globalization.NumberStyles.AllowHexSpecifier);
                    //    byte[] floatVals = BitConverter.GetBytes(num);
                    //    this.Floors[j].Vacuum = BitConverter.ToSingle(floatVals, 0);
                    //}
                    #endregion
                }
                else if (getInfoNum == 3)
                {
                    #region 获取温度左后25
                    for (int j = 0; j < this.Floors.Count; j++)
                    {

                        output = string.Empty;
                        if (!this.Plc.GetInfo(false, Current.option.GetTemStrs2.Split(',')[j], out output, out msg))
                        {
                            Error.Alert(msg);
                            this.Plc.IsAlive = false;
                            return false;
                        }
                        if (output.Substring(3, 1) != "$")
                        {
                            LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.GetTemStrs2.Split(',')[j], output));
                            return false;
                        }

                        output = PanasonicPLC.ConvertHexStr(output.TrimEnd('\r'), false);

                        for (int k = 0; k < this.floors[j].Stations[0].Temperatures.Length / 2; k++)
                        {
                            this.Floors[j].Stations[0].Temperatures[Option.TemperaturePointCount / 2 + k] = (float)int.Parse(output.Substring(k * 4, 4), System.Globalization.NumberStyles.AllowHexSpecifier) / 10;
                        }
                    }
                    #endregion

                    #region 获取三色灯状态
                    output = string.Empty;
                    if (!this.Plc.GetInfo(false, Current.option.GetTrichromaticLampStr, out output, out msg))
                    {
                        Error.Alert(msg);
                        this.Plc.IsAlive = false;
                        return false;
                    }

                    if (output.Substring(3, 1) != "$")
                    {
                        LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.GetTrichromaticLampStr, output));
                        return false;
                    }

                    if (output.Substring(6, 1) == "1")
                    {
                        this.TriLamp = TriLamp.Red;
                    }
                    else if (output.Substring(7, 1) == "1")
                    {
                        this.TriLamp = TriLamp.Green;
                    }
                    else if (output.Substring(8, 1) == "1")
                    {
                        this.TriLamp = TriLamp.Yellow;
                    }
                    else
                    {
                        this.TriLamp = TriLamp.Unknown;
                    }

                    #endregion
                }
                else if (getInfoNum == 5)
                {
                    #region 获取温度右前25
                    for (int j = 0; j < this.Floors.Count; j++)
                    {

                        output = string.Empty;
                        if (!this.Plc.GetInfo(false, Current.option.GetTemStrs3.Split(',')[j], out output, out msg))
                        {
                            Error.Alert(msg);
                            this.Plc.IsAlive = false;
                            return false;
                        }
                        if (output.Substring(3, 1) != "$")
                        {
                            LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.GetTemStrs3.Split(',')[j], output));
                            return false;
                        }

                        output = PanasonicPLC.ConvertHexStr(output.TrimEnd('\r'), false);

                        for (int k = 0; k < this.floors[j].Stations[1].Temperatures.Length / 2; k++)
                        {
                            this.Floors[j].Stations[1].Temperatures[k] = (float)int.Parse(output.Substring(k * 4, 4), System.Globalization.NumberStyles.AllowHexSpecifier) / 10;
                        }
                    }
                    #endregion

                }
                else if (getInfoNum == 7)
                {
                    #region 获取温度右后25
                    for (int j = 0; j < this.Floors.Count; j++)
                    {

                        output = string.Empty;
                        if (!this.Plc.GetInfo(false, Current.option.GetTemStrs4.Split(',')[j], out output, out msg))
                        {
                            Error.Alert(msg);
                            this.Plc.IsAlive = false;
                            return false;
                        }
                        if (output.Substring(3, 1) != "$")
                        {
                            LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.GetTemStrs4.Split(',')[j], output));
                            return false;
                        }

                        output = PanasonicPLC.ConvertHexStr(output.TrimEnd('\r'), false);

                        for (int k = 0; k < this.floors[j].Stations[1].Temperatures.Length / 2; k++)
                        {
                            this.Floors[j].Stations[1].Temperatures[Option.TemperaturePointCount / 2 + k] = (float)int.Parse(output.Substring(k * 4, 4), System.Globalization.NumberStyles.AllowHexSpecifier) / 10;
                        }
                    }
                    #endregion

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
                }
                else if (getInfoNum == 9)
                {
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
                        this.Floors[j].RunMinutesSet = int.Parse(output.Substring(8, 4), System.Globalization.NumberStyles.AllowHexSpecifier);
                    }
                    #endregion

                    #region 获取剩余时间

                    output = string.Empty;
                    if (!this.Plc.GetInfo(false, Current.option.GetRemainTimeStr, out output, out msg))
                    {
                        Error.Alert(msg);
                        this.Plc.IsAlive = false;
                        return false;
                    }

                    if (output.Substring(3, 1) != "$")
                    {
                        LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.GetRemainTimeStr, output));
                        return false;
                    }
                    output = PanasonicPLC.ConvertHexStr(output.TrimEnd('\r'), false);
                    for (int j = 0; j < this.Floors.Count; j++)
                    {
                        this.Floors[j].RunRemainMinutes = int.Parse(output.Substring(j * 8, 4), System.Globalization.NumberStyles.AllowHexSpecifier);
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
                            else if (c == '0')
                            {
                                Alarm alarm = (from a in Alarm.Alarms where a.Id == x + 1 select a).ToList()[0];
                                if (alarm.FloorNum == 0)
                                {
                                    this.AlarmStr += alarm.AlarmStr + ",";
                                    if (cPre == '1')
                                    {
                                        AlarmLog alarmLog = new AlarmLog();
                                        alarmLog.AlarmId = x + 1;
                                        alarmLog.AlarmType = AlarmType.Oven;
                                        alarmLog.TypeId = this.Id;
                                        alarmLogs.Add(alarmLog);
                                    }
                                }
                                else if (alarm.FloorNum > 0 && alarm.FloorNum <= this.Floors.Count)
                                {
                                    if (cPre == '1')
                                    {
                                        AlarmLog.Stop(AlarmType.Floor, x + 1, this.Floors[alarm.FloorNum - 1].Id, out msg);
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

                    #region 获取烤箱门状态
                    output = string.Empty;
                    if (!this.Plc.GetInfo(false, Current.option.GetDoorStatusStr, out output, out msg))
                    {
                        Error.Alert(msg);
                        this.Plc.IsAlive = false;
                        return false;
                    }

                    if (output.Substring(3, 1) != "$")
                    {
                        LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.GetDoorStatusStr, output));
                        return false;
                    }
                    output = PanasonicPLC.ConvertHexStr(output.TrimEnd('\r'), false);

                    for (int j = 0; j < this.Floors.Count; j++)
                    {
                        switch (int.Parse(output.Substring(j * 4, 4), System.Globalization.NumberStyles.AllowHexSpecifier))
                        {
                            case 0: this.Floors[j].DoorStatusNotFinal = DoorStatus.关闭; break;
                            case 1: this.Floors[j].DoorStatusNotFinal = DoorStatus.打开; break;
                            case 2: this.Floors[j].DoorStatusNotFinal = DoorStatus.异常; break;
                            default: this.Floors[j].DoorStatusNotFinal = DoorStatus.未知; break;
                        }
                    }

                    //Thread.Sleep(100);
                    #endregion

                    #region 获取烤箱门的运动状态

                    for (int k = 0; k < 2; k++)
                    {
                        input = Current.option.GetOvenDoorRunStrs.Split(',')[k];
                        output = string.Empty;
                        if (!this.Plc.GetInfo(false, input, out output, out msg))
                        {
                            Error.Alert(msg);
                            this.Plc.IsAlive = false;
                            return false;
                        }

                        if (output.Substring(3, 1) != "$")
                        {
                            LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", input, output));
                            return false;
                        }

                        for (int j = 0; j < this.Floors.Count; j++)
                        {
                            if (output.Substring(6 + j, 1) == "1")
                            {
                                if (k == 0)
                                {
                                    this.Floors[j].DoorIsOpenning = true;
                                }
                                else
                                {
                                    this.Floors[j].DoorIsClosing = true;
                                }
                            }
                            else
                            {
                                if (k == 0)
                                {
                                    this.Floors[j].DoorIsOpenning = false;
                                }
                                else
                                {
                                    this.Floors[j].DoorIsClosing = false;
                                }
                            }
                        }
                    }

                    #endregion

                }

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

                #region 获取烤箱夹具状态

                if (!this.Plc.GetInfo(false, Current.option.GetOvenClampStatusStr, out output, out msg))
                {
                    Error.Alert(msg);
                    this.Plc.IsAlive = false;
                    return false;
                }
                if (output.Substring(3, 1) != "$")
                {
                    LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.GetOvenClampStatusStr, output));
                    return false;
                }

                int[] iOut = new int[10];
                output = PanasonicPLC.ConvertHexStr(output.TrimEnd('\r'), false);
                for (int j = 0; j < iOut.Length; j++)
                {
                    iOut[j] = int.Parse(output.Substring(j * 4, 4), System.Globalization.NumberStyles.AllowHexSpecifier);
                }

                for (int j = 0; j < this.Floors.Count; j++)
                {
                    for (int k = 0; k < this.Floors[j].Stations.Count; k++)
                    {
                        switch (iOut[j * 2 + k])
                        {
                            case 1: this.Floors[j].Stations[k].ClampStatus = ClampStatus.无夹具; break;
                            case 2: this.Floors[j].Stations[k].ClampStatus = this.Floors[j].Stations[k].ClampStatus == ClampStatus.空夹具 ? ClampStatus.空夹具 : ClampStatus.满夹具; break;
                            case 4: this.Floors[j].Stations[k].ClampStatus = ClampStatus.异常; break;
                            default: this.Floors[j].Stations[k].ClampStatus = ClampStatus.未知; break;
                        }
                    }
                }

                #endregion

                #region 获取是否运行完成
                for (int k = 0; k < 2; k++)
                {
                    output = string.Empty;
                    if (!this.Plc.GetInfo(false, Current.option.GetBakingIsFinishedStrs.Split(',')[k], out output, out msg))
                    {
                        Error.Alert(msg);
                        this.Plc.IsAlive = false;
                        return false;
                    }

                    if (output.Substring(3, 1) != "$")
                    {
                        LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.GetBakingIsFinishedStrs.Split(',')[k], output));
                        return false;
                    }

                    for (int j = 0; j < this.Floors.Count; j++)
                    {
                        this.Floors[j].Stations[k].IsBakeFinished = output.Substring(6 + j, 1) == "1";
                    }
                }
                #endregion

                #region 写指令 控制开关门、启动运行、卸真空
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
                    }
                    #endregion

                    #region 控制关门
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

                    #region 泄真空
                    if (this.Floors[j].toUploadVacuum)
                    {
                        output = string.Empty;
                        if (!this.Plc.GetInfo(false, Current.option.UnloadVacuumStrs.Split(',')[j], out output, out msg))
                        {
                            Error.Alert(msg);
                            this.Plc.IsAlive = false;
                            return false;
                        }

                        if (output.Substring(3, 1) != "$")
                        {
                            LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.UnloadVacuumStrs.Split(',')[j], output));
                            return false;
                        }
                        LogHelper.WriteInfo(string.Format("成功发送泄真空指令到{0}:{1}", this.Floors[j].Name, Current.option.UnloadVacuumStrs.Split(',')[j]));
                        this.Floors[j].toUploadVacuum = false;
                    }
                    #endregion
                }
                #endregion
            }
            catch (Exception ex)
            {
                Error.Alert(ex);
            }

            this.Plc.IsAlive = true;

            this.getInfoNum++;
            if (getInfoNum >= 10)
            {
                this.AlreadyGetAllInfo = true;
            }
            this.getInfoNum %= 10;
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
    }
}
