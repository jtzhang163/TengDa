using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using TengDa;
using System.Threading;
using System.ComponentModel;
using TengDa.WF;

namespace Veken.Baking
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

        private static string getTemStr = string.Empty;
        private static string[] getTemStrs = new string[3];
        public static string[] GetTemStrs
        {
            get
            {
                if (string.IsNullOrEmpty(getTemStr))
                {
                    getTemStrs = Current.option.GetTemStrs.Split(',');
                    getTemStr = "Get";
                }
                return getTemStrs;
            }
        }

        public string Alarm2BinString = string.Empty;

        public string PreAlarm2BinString = string.Empty;

        public TriLamp triLamp = TriLamp.Unknown;

        public float[] Temperature = new float[Option.TemperaturePointCount];

        private string plcIds = string.Empty;
        public string PlcIds
        {
            get { return plcIds; }
            private set { plcIds = value; }
        }

        private string floorIds = string.Empty;
        public string FloorIds
        {
            get { return floorIds; }
            private set { floorIds = value; }
        }

        private int getInfoNum = 0;

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
            this.plcIds = rowInfo["PlcIds"].ToString();
            this.floorIds = rowInfo["FloorIds"].ToString();
            this.company = rowInfo["Company"].ToString();
            this.model = rowInfo["Model"].ToString();
            this.number = rowInfo["Number"].ToString();
            this.isEnable = Convert.ToBoolean(rowInfo["IsEnable"]);
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

        #region 该烤箱上的PLC列表
        private List<PLC> plcs = new List<PLC>();

        [Browsable(false)]
        [ReadOnly(true)]
        public List<PLC> Plcs
        {
            get
            {
                if (plcs.Count < 1)
                {
                    List<PLC> list = new List<PLC>();
                    string[] plcIds = PlcIds.Split(',');
                    for (int i = 0; i < plcIds.Length; i++)
                    {
                        PLC plc = new PLC(TengDa._Convert.StrToInt(plcIds[i], -1));
                        list.Add(plc);
                    }
                    plcs = list;
                }
                return plcs;
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
                    List<Floor> list = new List<Floor>();
                    string[] floorIds = FloorIds.Split(',');
                    for (int i = 0; i < floorIds.Length; i++)
                    {
                        Floor floor = new Floor(TengDa._Convert.StrToInt(floorIds[i], -1));
                        list.Add(floor);
                    }
                    floors = list;
                }
                return floors;
            }
        }

        public bool AlreadyGetAllInfo = false;

        #endregion

        #region 通信
        public bool GetInfo()
        {
            if (!this.Plcs[0].IsPingSuccess)
            {
                IsAlive = false;
                LogHelper.WriteError("无法连接到 " + this.Plcs[0].IP);
                return false;
            }

            string msg = string.Empty;
            string output = string.Empty;

            try
            {

                if (getInfoNum % 2 == 0)
                {
                    #region 获取温度
                    for (int j = 0; j < this.Floors.Count; j++)
                    {

                        output = string.Empty;
                        if (!this.Plcs[0].GetInfo(false, GetTemStrs[j], out output, out msg))
                        {
                            Error.Alert(msg);
                            this.Plcs[0].IsAlive = false;
                            return false;
                        }
                        if (output.Substring(3, 1) != "$")
                        {
                            LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", GetTemStrs[j], output));
                            return false;
                        }

                        output = PanasonicPLC.ConvertHexStr(output.TrimEnd('\r'), false);

                        for (int k = 0; k < this.floors[j].Temperatures.Length; k++)
                        {
                            this.Floors[j].Temperatures[k] = (float)int.Parse(output.Substring(k * 4, 4), System.Globalization.NumberStyles.AllowHexSpecifier) / 10;
                        }
                        Thread.Sleep(100);
                    }
                    #endregion
                }
                else if (getInfoNum == 1)
                {
                    #region 获取真空
                    output = string.Empty;
                    if (!this.Plcs[0].GetInfo(false, Current.option.GetVacuumStr, out output, out msg))
                    {
                        Error.Alert(msg);
                        this.Plcs[0].IsAlive = false;
                        return false;
                    }

                    if (output.Substring(3, 1) != "$")
                    {
                        LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.GetVacuumStr, output));
                        return false;
                    }

                    output = PanasonicPLC.ConvertHexStr(output.TrimEnd('\r'), true);

                    for (int j = 0; j < this.Floors.Count; j++)
                    {
                        uint num = uint.Parse(output.Substring(16 - j * 8, 8), System.Globalization.NumberStyles.AllowHexSpecifier);
                        byte[] floatVals = BitConverter.GetBytes(num);
                        this.Floors[j].Vacuum = BitConverter.ToSingle(floatVals, 0);
                    }

                    Thread.Sleep(100);
                    #endregion

                    #region 获取运行状态
                    output = string.Empty;
                    if (!this.Plcs[0].GetInfo(false, Current.option.GetRunStatusStr, out output, out msg))
                    {
                        Error.Alert(msg);
                        this.Plcs[0].IsAlive = false;
                        return false;
                    }

                    if (output.Substring(3, 1) != "$")
                    {
                        LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.GetRunStatusStr, output));
                        return false;
                    }

                    for (int j = 0; j < this.Floors.Count; j++)
                    {
                        if (output.Substring(6 + j, 1) == "1")
                        {
                            this.Floors[j].IsBaking = true;
                            this.Floors[j].Runmode = RunMode.自动;
                        }
                        else if (output.Substring(9 + j, 1) == "1")
                        {
                            //this.Floors[j].IsBaking = true;
                            this.Floors[j].IsBaking = false;//手动调试加热不算加热
                            this.Floors[j].Runmode = RunMode.手动;
                        }
                        else
                        {
                            this.Floors[j].IsBaking = false;
                            this.Floors[j].Runmode = RunMode.未运行;
                        }
                    }
                    Thread.Sleep(100);
                    #endregion

                    #region 获取三色灯状态
                    output = string.Empty;
                    if (!this.Plcs[0].GetInfo(false, Current.option.GetTrichromaticLampStr, out output, out msg))
                    {
                        Error.Alert(msg);
                        this.Plcs[0].IsAlive = false;
                        return false;
                    }

                    if (output.Substring(3, 1) != "$")
                    {
                        LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.GetTrichromaticLampStr, output));
                        return false;
                    }


                    if (output.Substring(6, 1) == "1")
                    {
                        this.triLamp = TriLamp.Red;
                    }
                    else if (output.Substring(7, 1) == "1")
                    {
                        this.triLamp = TriLamp.Green;
                    }
                    else if (output.Substring(8, 1) == "1")
                    {
                        this.triLamp = TriLamp.Yellow;
                    }
                    else
                    {
                        this.triLamp = TriLamp.Unknown;
                    }

                    #endregion
                }
                else if (getInfoNum == 3)
                {
                    #region 获取工艺时间
                    output = string.Empty;
                    if (!this.Plcs[0].GetInfo(false, Current.option.GetRunMinutesSetStr, out output, out msg))
                    {
                        Error.Alert(msg);
                        this.Plcs[0].IsAlive = false;
                        return false;
                    }

                    if (output.Substring(3, 1) != "$")
                    {
                        LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.GetRunMinutesSetStr, output));
                        return false;
                    }

                    output = PanasonicPLC.ConvertHexStr(output.TrimEnd('\r'), false);

                    for (int j = 0; j < this.Floors.Count; j++)
                    {
                        this.Floors[j].RunMinutesSet = int.Parse(output.Substring(j * 4, 4), System.Globalization.NumberStyles.AllowHexSpecifier);
                        Thread.Sleep(100);
                    }
                    #endregion

                    #region 获取已运行时间
                    output = string.Empty;
                    if (!this.Plcs[0].GetInfo(false, Current.option.GetRuntimeStr, out output, out msg))
                    {
                        Error.Alert(msg);
                        this.Plcs[0].IsAlive = false;
                        return false;
                    }

                    if (output.Substring(3, 1) != "$")
                    {
                        LogHelper.WriteError(string.Format("与PLC通信格式错误，input：{0}，output：{1}", Current.option.GetRuntimeStr, output));
                        return false;
                    }
                    output = PanasonicPLC.ConvertHexStr(output.TrimEnd('\r'), false);
                    for (int j = 0; j < this.Floors.Count; j++)
                    {
                        this.Floors[j].RunMinutes = int.Parse(output.Substring(j * 4, 4), System.Globalization.NumberStyles.AllowHexSpecifier);
                        Thread.Sleep(100);
                    }
                    #endregion

                    #region 报警信息
                    output = string.Empty;
                    if (!this.Plcs[0].GetInfo(false, Current.option.GetAlarmStr, out output, out msg))
                    {
                        Error.Alert(msg);
                        this.Plcs[0].IsAlive = false;
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
                                    this.AlarmStr += alarm.AlarmStr + ",";
                                    if (cPre == '0')
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
                                    if (cPre == '1')
                                    {
                                        AlarmLog.Stop(AlarmType.Oven, x + 1, this.Id, out msg);
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
                }
            }
            catch (Exception ex)
            {
                Error.Alert(ex);
            }

            this.Plcs[0].IsAlive = true;
            this.getInfoNum++;
            if (getInfoNum >= 4)
            {
                this.AlreadyGetAllInfo = true;
            }
            this.getInfoNum %= 4;

            return true;
        }
        #endregion

    }
}
