﻿using System;
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

        //public static string[] GetRuntimeStrs
        //{
        //    get
        //    {
        //        return Current.option.GetRuntimeStrs.Split(',');
        //    }
        //}

        public string Alarm2BinString = string.Empty;

        public string PreAlarm2BinString = string.Empty;

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
                    plc = PLC.PlcList.First(p => p.Id == this.PlcId);
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
            lock (this)
            {
                if (!this.Plc.IsPingSuccess)
                {
                    this.Plc.IsAlive = false;
                    LogHelper.WriteError("无法连接到 " + this.Plc.IP);
                    return false;
                }

                string msg = string.Empty;
                try
                {

                    var bOutputs0 = new ushort[] { };
                    if (!this.Plc.GetInfo(true, "D3000", (ushort)999, out bOutputs0, out msg))
                    {
                        Error.Alert(msg);
                        this.Plc.IsAlive = false;
                        return false;
                    }

                    for (int j = 0; j < this.Floors.Count; j++)
                    {
                        switch (bOutputs0[209 - j * 104])
                        {
                            case 1: this.Floors[j].DoorStatusNotFinal = DoorStatus.关闭; this.Floors[j].DoorIsClosing = false; break;
                            case 2: this.Floors[j].DoorStatusNotFinal = DoorStatus.打开; this.Floors[j].DoorIsOpenning = false; break;
                            default: this.Floors[j].DoorStatusNotFinal = DoorStatus.异常; break;
                        }

                        for (int k = 0; k < this.Floors[j].Stations.Count; k++)
                        {
                            if (bOutputs0[210 - j * 104 + k] == 1)
                            {
                                if (this.Floors[j].Stations[k].Id == Current.Task.FromStationId && Current.Task.Status == TaskStatus.正取)
                                {

                                }
                                else
                                {
                                    this.Floors[j].Stations[k].ClampStatus = this.Floors[j].Stations[k].ClampStatus == ClampStatus.空夹具 ? ClampStatus.空夹具 : ClampStatus.满夹具;
                                }
                            }
                            else
                            {
                                this.Floors[j].Stations[k].ClampStatus = ClampStatus.无夹具;
                            }

                        }

                        this.Floors[j].IsNetControlOpen = bOutputs0[692 - 44 * j] == 0;

                        this.Floors[j].ProcessTemperSet = bOutputs0[463 - 36 * j] / 10;
                        this.Floors[j].PreheatTimeSet = bOutputs0[465 - 36 * j];
                        this.Floors[j].BakingTimeSet = bOutputs0[466 - 36 * j];
                        this.Floors[j].BreathingCycleSet = bOutputs0[474 - 36 * j];

                        this.Floors[j].RunMinutes = bOutputs0[687 - 44 * j];
                        this.Floors[j].Vacuum = bOutputs0[689 - 44 * j] + bOutputs0[690 - 44 * j] * 65535;
                        this.Floors[j].IsVacuum = this.Floors[j].Vacuum < Current.option.VacuumStandard;

                        if (j == 0)
                        {
                            switch (bOutputs0[693])
                            {
                                case 1: this.TriLamp = TriLamp.Red; break;
                                case 2: this.TriLamp = TriLamp.Yellow; break;
                                case 3: this.TriLamp = TriLamp.Green; break;
                                default: this.TriLamp = TriLamp.Unknown; break;
                            }
                        }

                        var output = OmronPLC.GetBitStr(bOutputs0[250 - 104 * j], 8);
                        this.Floors[j].IsBaking = bOutputs0[688 - 44 * j] == 1 || output.Substring(2, 1) == "1" && output.Substring(3, 1) == "1";
                    }


                    var bOutputs1 = new ushort[] { };
                    if (!this.Plc.GetInfo(true, "D3500", (ushort)999, out bOutputs1, out msg))
                    {
                        Error.Alert(msg);
                        this.Plc.IsAlive = false;
                        return false;
                    }

                    for (int j = 0; j < this.Floors.Count; j++)
                    {
                        for (int n = 0; n < Option.TemperaturePointCount; n++)
                        {
                            this.Floors[j].Stations[0].Temperatures[n] = bOutputs1[587 - 128 * j + n] / 10f;
                        }
                        for (int n = 0; n < Option.TemperaturePointCount; n++)
                        {
                            this.Floors[j].Stations[1].Temperatures[n] = bOutputs1[651 - 128 * j + n] / 10f;
                        }
                    }

                    #region 报警信息


                    if (this.TriLamp == TriLamp.Red)
                    {
                        var bOutputs2 = new ushort[] { };
                        if (!this.Plc.GetInfo(true, "D4600", (ushort)400, out bOutputs2, out msg))
                        {
                            Error.Alert(msg);
                            this.Plc.IsAlive = false;
                            return false;
                        }

                        StringBuilder sb = new StringBuilder();
                        for (int n = 0; n < bOutputs2.Length; n++)
                        {
                            sb.Append(_Convert.Revert(OmronPLC.GetBitStr(bOutputs2[n], 16)));
                        }

                        this.Alarm2BinString = sb.ToString();
                    }
                    else
                    {
                        this.Alarm2BinString = new String('0', 6400);
                    }


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
                                        alarmLog.Clamp1Id = this.Floors[alarm.FloorNum - 1].Stations[0].ClampId > 0 ? this.Floors[alarm.FloorNum - 1].Stations[0].ClampId : 60;
                                        alarmLog.Clamp2Id = this.Floors[alarm.FloorNum - 1].Stations[1].ClampId > 0 ? this.Floors[alarm.FloorNum - 1].Stations[1].ClampId : 60;
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

                    #region 写指令 控制开关门、启动运行、抽破真空
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
                            var addr = Current.option.OvenOpenDoorAddrVals.Split(',')[j].Split(':')[0];
                            var val = Convert.ToUInt16(Current.option.OvenOpenDoorAddrVals.Split(',')[j].Split(':')[1]);

                            if (!this.Plc.SetInfo(addr, val, out msg))
                            {
                                Error.Alert(msg);
                                this.Plc.IsAlive = false;
                                return false;
                            }
                            LogHelper.WriteInfo(string.Format("成功发送开门指令到{0}:{1}", this.Floors[j].Name, Current.option.OvenOpenDoorAddrVals.Split(',')[j]));
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
                            //关门前判断该处有无任务

                            // cannotClose : 不能关门
                            bool cannotClose = false;
                            if (this.Floors[j].Stations.Count(s => s.Id == Current.Task.FromStationId) > 0 && (Current.Task.Status == TaskStatus.就绪 || Current.Task.Status == TaskStatus.可取 || Current.Task.Status == TaskStatus.正取))
                            {
                                Tip.Alert(Current.Task.FromStationName + "正在取盘，无法关门！");
                                cannotClose = true;
                            }

                            if (!cannotClose)
                            {
                                if (this.Floors[j].Stations.Count(s => s.Id == Current.Task.ToStationId) > 0 && (Current.Task.Status == TaskStatus.可放 || Current.Task.Status == TaskStatus.正放))
                                {
                                    Tip.Alert(Current.Task.FromStationName + "正在放盘，无法关门！");
                                    cannotClose = true;
                                }
                            }

                            if (cannotClose)
                            {
                                this.Floors[j].toCloseDoor = false;
                            }
                            else
                            {
                                var addr = Current.option.OvenCloseDoorAddrVals.Split(',')[j].Split(':')[0];
                                var val = Convert.ToUInt16(Current.option.OvenCloseDoorAddrVals.Split(',')[j].Split(':')[1]);

                                if (!this.Plc.SetInfo(addr, val, out msg))
                                {
                                    Error.Alert(msg);
                                    this.Plc.IsAlive = false;
                                    return false;
                                }

                                LogHelper.WriteInfo(string.Format("成功发送关门指令到{0}:{1}", this.Floors[j].Name, Current.option.OvenCloseDoorAddrVals.Split(',')[j]));
                                this.Floors[j].toCloseDoor = false;
                                this.Floors[j].DoorIsClosing = true;
                                this.Floors[j].DoorStatusNotFinal = DoorStatus.正在关闭;
                                this.Floors[j].DoorStatus = DoorStatus.正在关闭;
                            }
                        }
                        #endregion

                        #region 启动运行
                        if (this.Floors[j].toStartBaking)
                        {
                            var addr = Current.option.OvenStartBakingAddrVals.Split(',')[j].Split(':')[0];
                            var val = Convert.ToUInt16(Current.option.OvenStartBakingAddrVals.Split(',')[j].Split(':')[1]);

                            if (!this.Plc.SetInfo(addr, val, out msg))
                            {
                                Error.Alert(msg);
                                this.Plc.IsAlive = false;
                                return false;
                            }

                            LogHelper.WriteInfo(string.Format("成功发送启动运行指令到{0}:{1}", this.Floors[j].Name, Current.option.OvenStartBakingAddrVals.Split(',')[j]));
                            this.Floors[j].toStartBaking = false;
                        }
                        #endregion

                        #region 结束运行
                        if (this.Floors[j].toStopBaking)
                        {
                            var addr = Current.option.OvenStopBakingAddrVals.Split(',')[j].Split(':')[0];
                            var val = Convert.ToUInt16(Current.option.OvenStopBakingAddrVals.Split(',')[j].Split(':')[1]);

                            if (!this.Plc.SetInfo(addr, val, out msg))
                            {
                                Error.Alert(msg);
                                this.Plc.IsAlive = false;
                                return false;
                            }

                            LogHelper.WriteInfo(string.Format("成功发送停止运行指令到{0}:{1}", this.Floors[j].Name, Current.option.OvenStopBakingAddrVals.Split(',')[j]));
                            this.Floors[j].toStopBaking = false;
                        }
                        #endregion

                        #region 开启网控

                        #endregion

                        #region 报警复位
                        if (this.Floors[j].toAlarmReset)
                        {

                            if (!this.Plc.SetInfo("D5303", (ushort)1, out msg))
                            {
                                Error.Alert(msg);
                                this.Plc.IsAlive = false;
                                return false;
                            }

                            LogHelper.WriteInfo(string.Format("成功发送报警复位指令到{0}:{1}", this.Name, "D5303"));
                            this.Floors[j].toAlarmReset = false;
                        }
                        #endregion

                        #region 抽真空
                        if (this.Floors[j].toLoadVacuum)
                        {
                            var addr = Current.option.OvenLoadVacuumAddrVals.Split(',')[j].Split(':')[0];
                            var val = Convert.ToUInt16(Current.option.OvenLoadVacuumAddrVals.Split(',')[j].Split(':')[1]);

                            if (!this.Plc.SetInfo(addr, val, out msg))
                            {
                                Error.Alert(msg);
                                this.Plc.IsAlive = false;
                                return false;
                            }

                            LogHelper.WriteInfo(string.Format("成功发送抽真空指令到{0}:{1}", this.Floors[j].Name, Current.option.OvenLoadVacuumAddrVals.Split(',')[j]));
                            this.Floors[j].toLoadVacuum = false;
                        }
                        #endregion

                        #region 停止抽真空
                        if (this.Floors[j].toCancelLoadVacuum)
                        {
                            var addr = Current.option.OvenStopLoadVacuumAddrVals.Split(',')[j].Split(':')[0];
                            var val = Convert.ToUInt16(Current.option.OvenStopLoadVacuumAddrVals.Split(',')[j].Split(':')[1]);

                            if (!this.Plc.SetInfo(addr, val, out msg))
                            {
                                Error.Alert(msg);
                                this.Plc.IsAlive = false;
                                return false;
                            }

                            LogHelper.WriteInfo(string.Format("成功发送取消抽真空指令到{0}:{1}", this.Floors[j].Name, Current.option.OvenStopLoadVacuumAddrVals.Split(',')[j]));
                            this.Floors[j].toCancelLoadVacuum = false;
                        }
                        #endregion

                        #region 破真空
                        if (this.Floors[j].toUploadVacuum)
                        {
                            var addr = Current.option.OvenUploadVacuumAddrVals.Split(',')[j].Split(':')[0];
                            var val = Convert.ToUInt16(Current.option.OvenUploadVacuumAddrVals.Split(',')[j].Split(':')[1]);

                            if (!this.Plc.SetInfo(addr, val, out msg))
                            {
                                Error.Alert(msg);
                                this.Plc.IsAlive = false;
                                return false;
                            }
                            LogHelper.WriteInfo(string.Format("成功发送破真空指令到{0}:{1}", this.Floors[j].Name, Current.option.OvenUploadVacuumAddrVals.Split(',')[j]));
                            this.Floors[j].toUploadVacuum = false;
                        }
                        #endregion

                        #region 停止破真空
                        if (this.Floors[j].toCancelUploadVacuum)
                        {
                            var addr = Current.option.OvenStopUploadVacuumAddrVals.Split(',')[j].Split(':')[0];
                            var val = Convert.ToUInt16(Current.option.OvenStopUploadVacuumAddrVals.Split(',')[j].Split(':')[1]);

                            if (!this.Plc.SetInfo(addr, val, out msg))
                            {
                                Error.Alert(msg);
                                this.Plc.IsAlive = false;
                                return false;
                            }
                            LogHelper.WriteInfo(string.Format("成功发送取消破真空指令到{0}:{1}", this.Floors[j].Name, Current.option.OvenStopUploadVacuumAddrVals.Split(',')[j]));
                            this.Floors[j].toCancelUploadVacuum = false;
                        }
                        #endregion

                        #region 运行时间清零

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
                if (getInfoNum >= 2)
                {
                    this.AlreadyGetAllInfo = true;
                }
                this.getInfoNum %= 2;
            }
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
        /// 破真空
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
                Tip.Alert(this.Floors[j].Name + "网控未开启，无法远程破真空 ");
                return;
            }

            if (this.Floors[j].IsBaking)
            {
                Tip.Alert(this.Floors[j].Name + "运行未完成，无法远程破真空 ");
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
        /// 取消破真空
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
                Tip.Alert(this.Floors[j].Name + "网控未开启，无法远程取消破真空 ");
                return;
            }

            if (this.Floors[j].IsBaking)
            {
                Tip.Alert(this.Floors[j].Name + "运行未完成，无法远程取消破真空 ");
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
        //public void ChangeWaterContentTestFloor()
        //{
        //    Floor floor = this.Floors.FirstOrDefault(f => f.IsTestWaterContent);
        //    if (floor == null)
        //    {
        //        this.Floors[0].IsTestWaterContent = true;
        //        return;
        //    }
        //    int j = this.Floors.IndexOf(floor);
        //    j = (++j) % this.Floors.Count;
        //    this.Floors[j].IsTestWaterContent = true;
        //}
    }
}
