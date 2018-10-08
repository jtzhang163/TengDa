﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using TengDa;

namespace BakBattery.Baking
{
    public class Option
    {

        private string appName = string.Empty;
        /// <summary>
        /// 程序名称
        /// </summary>
        [Category("全局")]
        [Description("程序名称")]
        [DisplayName("程序名称")]
        public string AppName
        {
            get
            {
                if (string.IsNullOrEmpty(appName))
                {
                    appName = TengDa.WF.Option.GetOption("AppName");
                }
                return appName;
            }
            set
            {
                if (value != appName)
                {
                    TengDa.WF.Option.SetOption("AppName", value);
                    appName = value;
                }
            }
        }

        /// <summary>
        /// 是否记住密码
        /// </summary>
        [ReadOnly(true), Description("是否记住密码")]
        [DisplayName("是否记住密码")]
        [Category("用户")]
        public bool RememberMe
        {
            get
            {
                return _Convert.StrToBool(TengDa.WF.Option.GetOption("RememberMe"), false);
            }
            set
            {
                TengDa.WF.Option.SetOption("RememberMe", value.ToString());
                if (!value)
                {
                    RememberUserId = -1;
                }
            }
        }

        /// <summary>
        /// 记住的用户Id
        /// </summary>
        [ReadOnly(true), Description("记住用户的Id")]
        [DisplayName("记住用户的Id")]
        [Category("用户")]
        public int RememberUserId
        {
            get
            {
                return _Convert.StrToInt(TengDa.WF.Option.GetOption("RememberUserId"), -1);
            }
            set
            {
                TengDa.WF.Option.SetOption("RememberUserId", value.ToString());
            }
        }

        /// <summary>
        /// 启用MES用户登录
        /// </summary>
        [Description("启用MES用户登录")]
        [DisplayName("启用MES用户登录")]
        [Category("用户")]
        public bool IsMesUserEnable
        {
            get
            {
                return _Convert.StrToBool(TengDa.WF.Option.GetOption("IsMesUserEnable"), false);
            }
            set
            {
                TengDa.WF.Option.SetOption("IsMesUserEnable", value.ToString());
            }
        }

        /// <summary>
        /// 是否记住MES用户
        /// </summary>
        [ReadOnly(true), Description("是否记住MES用户")]
        [DisplayName("是否记住MES用户")]
        [Category("用户")]
        public bool MesRememberMe
        {
            get
            {
                return _Convert.StrToBool(TengDa.WF.Option.GetOption("MesRememberMe"), false);
            }
            set
            {
                TengDa.WF.Option.SetOption("MesRememberMe", value.ToString());
                if (!value)
                {
                    MesRememberUserId = -1;
                }
            }
        }

        /// <summary>
        /// 记住的MES用户Id
        /// </summary>
        [ReadOnly(true), Description("记住的MES用户Id")]
        [DisplayName("记住的MES用户Id")]
        [Category("用户")]
        public int MesRememberUserId
        {
            get
            {
                return _Convert.StrToInt(TengDa.WF.Option.GetOption("MesRememberUserId"), -1);
            }
            set
            {
                TengDa.WF.Option.SetOption("MesRememberUserId", value.ToString());
            }
        }


        public const int TemperaturePointCount = 8;

        private static int layoutType = -1;
        /// <summary>
        /// 设备布局类型    
        /// A/B线：1，C/D线：2（之信遗留）
        /// </summary>
        public static int LayoutType
        {
            get
            {
                if (layoutType < 0)
                {
                    layoutType = TengDa._Convert.StrToInt(ConfigurationManager.AppSettings["LayoutType"], -1);
                }
                return layoutType;
            }
        }

        public string[] TemperNames = new string[Option.TemperaturePointCount]
        {
            "上左空", "上右空", "下左空", "下右空",
            "左侧空", "右侧空", "后左空", "后右空"
        };

        [Description("温度曲线颜色")]
        [DisplayName("温度曲线颜色")]
        [Category("温度曲线")]
        [Browsable(false)]
        public List<Color> CurveColors
        {
            get
            {
                return new List<Color> { Color.Green, Color.Fuchsia, Color.Blue, Color.Lime, Color.Pink, Color.Cyan, Color.Orange, Color.Purple };
            }
        }

        private int displayTemperIndex = -1;
        /// <summary>
        /// 界面显示温度点的序号
        /// </summary>
        [Description("界面显示温度点的序号")]
        [DisplayName("界面显示温度点的序号")]
        public int DisplayTemperIndex
        {
            get
            {
                if (displayTemperIndex < 0)
                {
                    displayTemperIndex = _Convert.StrToInt(TengDa.WF.Option.GetOption("DisplayTemperIndex"), 0) % TemperaturePointCount;
                }
                return displayTemperIndex;
            }
            set
            {
                TengDa.WF.Option.SetOption("DisplayTemperIndex", value.ToString());
                displayTemperIndex = value;
            }
        }

        private int curveFloorId = -1;
        /// <summary>
        /// 温度曲线炉层Id
        /// </summary>
        [ReadOnly(true)]
        [DisplayName("温度曲线炉层Id")]
        [Category("温度曲线")]
        public int CurveFloorId
        {
            get
            {
                if (curveFloorId < 0)
                {
                    curveFloorId = _Convert.StrToInt(TengDa.WF.Option.GetOption("CurveFloorId"), 1);
                }
                return curveFloorId;
            }
            set
            {
                TengDa.WF.Option.SetOption("CurveFloorId", value.ToString());
                curveFloorId = value;
            }
        }



        private int sampleFloorId = -1;
        /// <summary>
        /// 水分手动输入炉层Id
        /// </summary>
        [ReadOnly(true), Description("水分手动输入炉层Id")]
        [DisplayName("水分手动输入炉层Id")]
        public int SampleFloorId
        {
            get
            {
                if (sampleFloorId < 0)
                {
                    sampleFloorId = _Convert.StrToInt(TengDa.WF.Option.GetOption("SampleFloorId"), 1);
                }
                return sampleFloorId;
            }
            set
            {
                TengDa.WF.Option.SetOption("SampleFloorId", value.ToString());
                sampleFloorId = value;
            }
        }


        private string batteryScanerTriggerStr = string.Empty;
        /// <summary>
        /// 电池扫码枪触发字符串
        /// </summary>
        [Description("电池扫码枪触发字符串")]
        [DisplayName("电池扫码枪触发字符串")]
        [Category("扫码枪")]
        public string BatteryScanerTriggerStr
        {
            get
            {
                if (string.IsNullOrEmpty(batteryScanerTriggerStr))
                {
                    batteryScanerTriggerStr = TengDa.WF.Option.GetOption("BatteryScanerTriggerStr");
                }
                return batteryScanerTriggerStr;
            }
            set
            {
                if (value != batteryScanerTriggerStr)
                {
                    TengDa.WF.Option.SetOption("BatteryScanerTriggerStr", value);
                    batteryScanerTriggerStr = value;
                }
            }
        }

        private string batteryCodeRegularExpression = string.Empty;
        /// <summary>
        /// 电池条码正则表达式
        /// </summary>
        [Description("电池条码正则表达式")]
        [DisplayName("电池条码正则表达式")]
        [Category("扫码枪")]
        public string BatteryCodeRegularExpression
        {
            get
            {
                if (string.IsNullOrEmpty(batteryCodeRegularExpression))
                {
                    batteryCodeRegularExpression = TengDa.WF.Option.GetOption("BatteryCodeRegularExpression");
                }
                return batteryCodeRegularExpression;
            }
            set
            {
                if (value != batteryCodeRegularExpression)
                {
                    TengDa.WF.Option.SetOption("BatteryCodeRegularExpression", value);
                    batteryCodeRegularExpression = value;
                }
            }
        }


        private string batteryScanerFailedStr = string.Empty;
        /// <summary>
        /// 电池扫码枪扫码失败返回字符串
        /// </summary>
        [Description("电池扫码枪扫码失败返回字符串")]
        [DisplayName("电池扫码枪扫码失败返回字符串")]
        [Category("扫码枪")]
        public string BatteryScanerFailedStr
        {
            get
            {
                if (string.IsNullOrEmpty(batteryScanerFailedStr))
                {
                    batteryScanerFailedStr = TengDa.WF.Option.GetOption("BatteryScanerFailedStr");
                }
                return batteryScanerFailedStr;
            }
            set
            {
                if (value != batteryScanerFailedStr)
                {
                    TengDa.WF.Option.SetOption("BatteryScanerFailedStr", value);
                    batteryScanerFailedStr = value;
                }
            }
        }

        private string queryBatteryTimeSpan = string.Empty;
        /// <summary>
        /// 查询历史记录时间范围控制，单位：Day
        /// </summary>
        [Description("查询历史记录时间范围控制，单位：Day")]
        [DisplayName("查询历史记录时间范围控制")]
        [Category("查询时间控制")]
        public string QueryBatteryTimeSpan
        {
            get
            {
                if (string.IsNullOrEmpty(queryBatteryTimeSpan))
                {
                    queryBatteryTimeSpan = TengDa.WF.Option.GetOption("QueryBatteryTimeSpan");
                }
                return queryBatteryTimeSpan;
            }
            set
            {
                if (value != queryBatteryTimeSpan)
                {
                    TengDa.WF.Option.SetOption("QueryBatteryTimeSpan", value);
                    queryBatteryTimeSpan = value;
                }
            }
        }

        private string queryTVTimeSpan = string.Empty;
        /// <summary>
        /// 查询真空温度时间范围，单位：Day
        /// </summary>
        [Description("查询真空温度时间范围，单位：Day")]
        [DisplayName("查询真空温度时间范围")]
        [Category("查询时间控制")]
        public string QueryTVTimeSpan
        {
            get
            {
                if (string.IsNullOrEmpty(queryTVTimeSpan))
                {
                    queryTVTimeSpan = TengDa.WF.Option.GetOption("QueryTVTimeSpan");
                }
                return queryTVTimeSpan;
            }
            set
            {
                if (value != queryTVTimeSpan)
                {
                    TengDa.WF.Option.SetOption("QueryTVTimeSpan", value);
                    queryTVTimeSpan = value;
                }
            }
        }

        private string queryAlarmTimeSpan = string.Empty;
        /// <summary>
        /// 查询报警记录时间范围，单位：Day
        /// </summary>
        [Description("查询报警记录时间范围，单位：Day")]
        [DisplayName("查询报警记录时间范围")]
        [Category("查询时间控制")]
        public string QueryAlarmTimeSpan
        {
            get
            {
                if (string.IsNullOrEmpty(queryAlarmTimeSpan))
                {
                    queryAlarmTimeSpan = TengDa.WF.Option.GetOption("QueryAlarmTimeSpan");
                }
                return queryAlarmTimeSpan;
            }
            set
            {
                if (value != queryAlarmTimeSpan)
                {
                    TengDa.WF.Option.SetOption("QueryAlarmTimeSpan", value);
                    queryAlarmTimeSpan = value;
                }
            }
        }

        private string checkPlcPeriod = string.Empty;
        /// <summary>
        /// 检测PLC状态周期，单位：毫秒
        /// </summary>
        [Description("检测PLC状态周期，单位：毫秒")]
        [DisplayName("检测PLC状态周期")]
        [Category("定时器")]
        public string CheckPlcPeriod
        {
            get
            {
                if (string.IsNullOrEmpty(checkPlcPeriod))
                {
                    checkPlcPeriod = TengDa.WF.Option.GetOption("CheckPlcPeriod");
                }
                return checkPlcPeriod;
            }
            set
            {
                if (value != checkPlcPeriod)
                {
                    TengDa.WF.Option.SetOption("CheckPlcPeriod", value);
                    checkPlcPeriod = value;
                }
            }
        }

        private string recordTVInterval = string.Empty;
        /// <summary>
        /// 记录温度真空间隔时间，单位：毫秒
        /// </summary>
        [Description("记录温度真空间隔时间，单位：毫秒")]
        [DisplayName("记录温度真空间隔时间")]
        [Category("定时器")]
        public string RecordTVInterval
        {
            get
            {
                if (string.IsNullOrEmpty(recordTVInterval))
                {
                    recordTVInterval = TengDa.WF.Option.GetOption("RecordTVInterval");
                }
                return recordTVInterval;
            }
            set
            {
                if (value != recordTVInterval)
                {
                    TengDa.WF.Option.SetOption("RecordTVInterval", value);
                    recordTVInterval = value;
                }
            }
        }

        private string uploadMesInterval = string.Empty;
        /// <summary>
        /// MES上传数据间隔时间，单位：毫秒
        /// </summary>
        [Description("MES上传数据间隔时间，单位：毫秒")]
        [DisplayName("MES上传数据间隔时间")]
        [Category("定时器")]
        public string UploadMesInterval
        {
            get
            {
                if (string.IsNullOrEmpty(uploadMesInterval))
                {
                    uploadMesInterval = TengDa.WF.Option.GetOption("UploadMesInterval");
                }
                return uploadMesInterval;
            }
            set
            {
                if (value != uploadMesInterval)
                {
                    TengDa.WF.Option.SetOption("UploadMesInterval", value);
                    uploadMesInterval = value;
                }
            }
        }

        private int paintCurveInterval = -1;
        /// <summary>
        /// 温度曲线绘制间隔时间，单位：毫秒
        /// </summary>
        [Description("温度曲线绘制间隔时间，单位：毫秒")]
        [DisplayName("温度曲线绘制间隔时间")]
        [Category("定时器")]
        public int PaintCurveInterval
        {
            get
            {
                if (paintCurveInterval < 0)
                {
                    paintCurveInterval = _Convert.StrToInt(TengDa.WF.Option.GetOption("PaintCurveInterval"), 3000);
                }
                return paintCurveInterval;
            }
            set
            {
                if (value != paintCurveInterval)
                {
                    TengDa.WF.Option.SetOption("PaintCurveInterval", value.ToString());
                    paintCurveInterval = value;
                }
            }
        }

        private int checkScanerInterval = -1;
        /// <summary>
        /// 检测扫码枪数据时间间隔，单位：毫秒
        /// </summary>
        [Description("检测扫码枪数据时间间隔，单位：毫秒")]
        [DisplayName("检测扫码枪数据时间间隔")]
        [Category("定时器")]
        public int CheckScanerInterval
        {
            get
            {
                if (checkScanerInterval < 0)
                {
                    checkScanerInterval = _Convert.StrToInt(TengDa.WF.Option.GetOption("CheckScanerInterval"), 30);
                }
                return checkScanerInterval;
            }
            set
            {
                if (value != checkScanerInterval)
                {
                    TengDa.WF.Option.SetOption("CheckScanerInterval", value.ToString());
                    checkScanerInterval = value;
                }
            }
        }


        private int taskInterval = -1;
        /// <summary>
        /// 检测是否生成机器人搬运任务时间间隔，单位：毫秒
        /// </summary>
        [Description("检测是否生成机器人搬运任务时间间隔，单位：毫秒")]
        [DisplayName("检测是否生成机器人搬运任务时间间隔")]
        [Category("定时器")]
        public int TaskInterval
        {
            get
            {
                if (taskInterval < 0)
                {
                    taskInterval = _Convert.StrToInt(TengDa.WF.Option.GetOption("TaskInterval"), 1000);
                }
                return taskInterval;
            }
            set
            {
                if (value != taskInterval)
                {
                    TengDa.WF.Option.SetOption("TaskInterval", value.ToString());
                    taskInterval = value;
                }
            }
        }


        private string getRunStatusStr = string.Empty;
        /// <summary>
        /// 获取烤箱运行状态的字符串
        /// </summary>
        [Description("获取烤箱运行状态的字符串")]
        [DisplayName("获取烤箱运行状态的字符串")]
        [Category("烤箱")]
        public string GetRunStatusStr
        {
            get
            {
                if (string.IsNullOrEmpty(getRunStatusStr))
                {
                    getRunStatusStr = TengDa.WF.Option.GetOption("GetRunStatusStr");
                }
                return getRunStatusStr;
            }
            set
            {
                if (value != getRunStatusStr)
                {
                    TengDa.WF.Option.SetOption("GetRunStatusStr", value);
                    getRunStatusStr = value;
                }
            }
        }



        private string getMultiInfoStrs = string.Empty;
        /// <summary>
        /// 获取烤箱门状态、真空和三色灯的指令
        /// </summary>
        [DisplayName("获取烤箱门状态、真空和三色灯的指令")]
        [Category("烤箱")]
        public string GetMultiInfoStrs
        {
            get
            {
                if (string.IsNullOrEmpty(getMultiInfoStrs))
                {
                    getMultiInfoStrs = TengDa.WF.Option.GetOption("GetMultiInfoStrs");
                }
                return getMultiInfoStrs;
            }
            set
            {
                if (value != getMultiInfoStrs)
                {
                    TengDa.WF.Option.SetOption("GetMultiInfoStrs", value);
                    getMultiInfoStrs = value;
                }
            }
        }

        private string getTemStrs = string.Empty;
        /// <summary>
        /// 获取烤箱温度的字符串
        /// </summary>
        [DisplayName("获取烤箱温度的字符串")]
        [Category("烤箱")]
        public string GetTemStrs
        {
            get
            {
                if (string.IsNullOrEmpty(getTemStrs))
                {
                    getTemStrs = TengDa.WF.Option.GetOption("GetTemStrs");
                }
                return getTemStrs;
            }
            set
            {
                if (value != getTemStrs)
                {
                    TengDa.WF.Option.SetOption("GetTemStrs", value);
                    getTemStrs = value;
                }
            }
        }


        private string getParamSettingStrs = string.Empty;
        /// <summary>
        /// 获取烤箱预热时间、烘烤时间、呼吸周期参数的指令
        /// </summary>
        [DisplayName("获取烤箱设置参数的指令")]
        [Category("烤箱")]
        public string GetParamSettingStrs
        {
            get
            {
                if (string.IsNullOrEmpty(getParamSettingStrs))
                {
                    getParamSettingStrs = TengDa.WF.Option.GetOption("GetParamSettingStrs");
                }
                return getParamSettingStrs;
            }
            set
            {
                if (value != getParamSettingStrs)
                {
                    TengDa.WF.Option.SetOption("GetParamSettingStrs", value);
                    getParamSettingStrs = value;
                }
            }
        }

        private string preheatTimeSetAddrs = string.Empty;
        /// <summary>
        /// 烤箱预热时间地址
        /// </summary>
        [DisplayName("烤箱预热时间地址")]
        [Category("烤箱")]
        public string PreheatTimeSetAddrs
        {
            get
            {
                if (string.IsNullOrEmpty(preheatTimeSetAddrs))
                {
                    preheatTimeSetAddrs = TengDa.WF.Option.GetOption("PreheatTimeSetAddrs");
                }
                return preheatTimeSetAddrs;
            }
            set
            {
                if (value != preheatTimeSetAddrs)
                {
                    TengDa.WF.Option.SetOption("PreheatTimeSetAddrs", value);
                    preheatTimeSetAddrs = value;
                }
            }
        }

        private string bakingTimeSetAddrs = string.Empty;
        /// <summary>
        /// 烤箱烘烤时间地址
        /// </summary>
        [DisplayName("烤箱烘烤时间地址")]
        [Category("烤箱")]
        public string BakingTimeSetAddrs
        {
            get
            {
                if (string.IsNullOrEmpty(bakingTimeSetAddrs))
                {
                    bakingTimeSetAddrs = TengDa.WF.Option.GetOption("BakingTimeSetAddrs");
                }
                return bakingTimeSetAddrs;
            }
            set
            {
                if (value != bakingTimeSetAddrs)
                {
                    TengDa.WF.Option.SetOption("BakingTimeSetAddrs", value);
                    bakingTimeSetAddrs = value;
                }
            }
        }

        private string breathingCycleSetAddrs = string.Empty;
        /// <summary>
        /// 烤箱呼吸周期地址
        /// </summary>
        [DisplayName("烤箱呼吸周期地址")]
        [Category("烤箱")]
        public string BreathingCycleSetAddrs
        {
            get
            {
                if (string.IsNullOrEmpty(breathingCycleSetAddrs))
                {
                    breathingCycleSetAddrs = TengDa.WF.Option.GetOption("BreathingCycleSetAddrs");
                }
                return breathingCycleSetAddrs;
            }
            set
            {
                if (value != breathingCycleSetAddrs)
                {
                    TengDa.WF.Option.SetOption("BreathingCycleSetAddrs", value);
                    breathingCycleSetAddrs = value;
                }
            }
        }


        private string getRuntimeStrs = string.Empty;
        /// <summary>
        /// 获取烤箱运行时间的字符串
        /// </summary>
        [Description("获取烤箱运行时间的字符串")]
        [DisplayName("获取烤箱运行时间的字符串")]
        [Category("烤箱")]
        public string GetRuntimeStrs
        {
            get
            {
                if (string.IsNullOrEmpty(getRuntimeStrs))
                {
                    getRuntimeStrs = TengDa.WF.Option.GetOption("GetRuntimeStrs");
                }
                return getRuntimeStrs;
            }
            set
            {
                if (value != getRuntimeStrs)
                {
                    TengDa.WF.Option.SetOption("GetRuntimeStrs", value);
                    getRuntimeStrs = value;
                }
            }
        }

        private string getTrichromaticLampStr = string.Empty;
        /// <summary>
        /// 获取烤箱三色灯的字符串
        /// </summary>
        [Description("获取烤箱三色灯的字符串")]
        [DisplayName("获取烤箱三色灯的字符串")]
        [Category("烤箱")]
        public string GetTrichromaticLampStr
        {
            get
            {
                if (string.IsNullOrEmpty(getTrichromaticLampStr))
                {
                    getTrichromaticLampStr = TengDa.WF.Option.GetOption("GetTrichromaticLampStr");
                }
                return getTrichromaticLampStr;
            }
            set
            {
                if (value != getTrichromaticLampStr)
                {
                    TengDa.WF.Option.SetOption("GetTrichromaticLampStr", value);
                    getTrichromaticLampStr = value;
                }
            }
        }

        private string getAlarmStr = string.Empty;
        /// <summary>
        /// 获取烤箱报警的字符串
        /// </summary>
        [Description("获取烤箱报警的字符串")]
        [DisplayName("获取烤箱报警的字符串")]
        [Category("烤箱")]
        public string GetAlarmStr
        {
            get
            {
                if (string.IsNullOrEmpty(getAlarmStr))
                {
                    getAlarmStr = TengDa.WF.Option.GetOption("GetAlarmStr");
                }
                return getAlarmStr;
            }
            set
            {
                if (value != getAlarmStr)
                {
                    TengDa.WF.Option.SetOption("GetAlarmStr", value);
                    getAlarmStr = value;
                }
            }
        }


        private string temperatureSet = string.Empty;
        /// <summary>
        /// 设定温度（上传MES）
        /// </summary>
        [Description("设定温度（上传MES）")]
        [DisplayName("设定温度（上传MES）")]
        public string TemperatureSet
        {
            get
            {
                if (string.IsNullOrEmpty(temperatureSet))
                {
                    temperatureSet = TengDa.WF.Option.GetOption("TemperatureSet");
                }
                return temperatureSet;
            }
            set
            {
                if (value != temperatureSet)
                {
                    TengDa.WF.Option.SetOption("TemperatureSet", value);
                    temperatureSet = value;
                }
            }
        }


        private string sendScanOkStr = string.Empty;
        /// <summary>
        /// 扫码OK反馈给PLC的指令
        /// </summary>
        [Description("扫码OK反馈给PLC的指令")]
        [DisplayName("扫码OK反馈给PLC的指令")]
        [Category("烤箱")]
        public string SendScanOkStr
        {
            get
            {
                if (string.IsNullOrEmpty(sendScanOkStr))
                {
                    sendScanOkStr = TengDa.WF.Option.GetOption("SendScanOkStr");
                }
                return sendScanOkStr;
            }
            set
            {
                if (value != sendScanOkStr)
                {
                    TengDa.WF.Option.SetOption("SendScanOkStr", value);
                    sendScanOkStr = value;
                }
            }
        }

        private string sendScanNgStr = string.Empty;

        /// <summary>
        /// 扫码NG反馈给PLC的指令
        /// </summary>
        [Description("扫码NG反馈给PLC的指令")]
        [DisplayName("扫码NG反馈给PLC的指令")]
        [Category("烤箱")]
        public string SendScanNgStr
        {
            get
            {
                if (string.IsNullOrEmpty(sendScanNgStr))
                {
                    sendScanNgStr = TengDa.WF.Option.GetOption("SendScanNgStr");
                }
                return sendScanNgStr;
            }
            set
            {
                if (value != sendScanNgStr)
                {
                    TengDa.WF.Option.SetOption("SendScanNgStr", value);
                    sendScanNgStr = value;
                }
            }
        }




        private string getFeederInfoStr = string.Empty;
        /// <summary>
        /// 获取上料机信息的指令
        /// </summary>
        [Description("获取上料机信息的指令")]
        [DisplayName("获取上料机信息的指令")]
        [Category("上料机")]
        public string GetFeederInfoStr
        {
            get
            {
                if (string.IsNullOrEmpty(getFeederInfoStr))
                {
                    getFeederInfoStr = TengDa.WF.Option.GetOption("GetFeederInfoStr");
                }
                return getFeederInfoStr;
            }
            set
            {
                if (value != getFeederInfoStr)
                {
                    TengDa.WF.Option.SetOption("GetFeederInfoStr", value);
                    getFeederInfoStr = value;
                }
            }
        }

        private string getBlankerInfoStr = string.Empty;
        /// <summary>
        /// 获取下料机信息的指令
        /// </summary>
        [Description("获取下料机信息的指令")]
        [DisplayName("获取下料机信息的指令")]
        [Category("下料机")]
        public string GetBlankerInfoStr
        {
            get
            {
                if (string.IsNullOrEmpty(getBlankerInfoStr))
                {
                    getBlankerInfoStr = TengDa.WF.Option.GetOption("GetBlankerInfoStr");
                }
                return getBlankerInfoStr;
            }
            set
            {
                if (value != getBlankerInfoStr)
                {
                    TengDa.WF.Option.SetOption("GetBlankerInfoStr", value);
                    getBlankerInfoStr = value;
                }
            }
        }


        private string openFeederDoorStrs = string.Empty;
        /// <summary>
        /// 上料机开门指令
        /// </summary>
        [Description("上料机开门指令")]
        [DisplayName("上料机开门指令")]
        [Category("上料机")]
        public string OpenFeederDoorStrs
        {
            get
            {
                if (string.IsNullOrEmpty(openFeederDoorStrs))
                {
                    openFeederDoorStrs = TengDa.WF.Option.GetOption("OpenFeederDoorStrs");
                }
                return openFeederDoorStrs;
            }
            set
            {
                if (value != openFeederDoorStrs)
                {
                    TengDa.WF.Option.SetOption("OpenFeederDoorStrs", value);
                    openFeederDoorStrs = value;
                }
            }
        }



        private string closeFeederDoorStrs = string.Empty;
        /// <summary>
        /// 上料机关门指令
        /// </summary>
        [Description("上料机关门指令")]
        [DisplayName("上料机关门指令")]
        [Category("上料机")]
        public string CloseFeederDoorStrs
        {
            get
            {
                if (string.IsNullOrEmpty(closeFeederDoorStrs))
                {
                    closeFeederDoorStrs = TengDa.WF.Option.GetOption("CloseFeederDoorStrs");
                }
                return closeFeederDoorStrs;
            }
            set
            {
                if (value != closeFeederDoorStrs)
                {
                    TengDa.WF.Option.SetOption("CloseFeederDoorStrs", value);
                    closeFeederDoorStrs = value;
                }
            }
        }


        private string feederClampScanerResultBackStrs = string.Empty;
        /// <summary>
        /// 上料机夹具扫码枪扫描结果反馈指令：OK，NG
        /// </summary>
        [Description("上料机夹具扫码枪扫描结果反馈指令：OK，NG")]
        [DisplayName("上料机夹具扫码枪扫描结果反馈指令")]
        [Category("扫码枪")]
        public string FeederClampScanerResultBackStrs
        {
            get
            {
                if (string.IsNullOrEmpty(feederClampScanerResultBackStrs))
                {
                    feederClampScanerResultBackStrs = TengDa.WF.Option.GetOption("FeederClampScanerResultBackStrs");
                }
                return feederClampScanerResultBackStrs;
            }
            set
            {
                if (value != feederClampScanerResultBackStrs)
                {
                    TengDa.WF.Option.SetOption("FeederClampScanerResultBackStrs", value);
                    feederClampScanerResultBackStrs = value;
                }
            }
        }


        private string feederBatteryScanerResultBackStrs = string.Empty;
        /// <summary>
        /// 上料机电池扫码枪扫描结果反馈指令：OK，NG
        /// </summary>
        [Description("上料机电池扫码枪扫描结果反馈指令：OK，NG")]
        [DisplayName("上料机电池扫码枪扫描结果反馈指令")]
        [Category("扫码枪")]
        public string FeederBatteryScanerResultBackStrs
        {
            get
            {
                if (string.IsNullOrEmpty(feederBatteryScanerResultBackStrs))
                {
                    feederBatteryScanerResultBackStrs = TengDa.WF.Option.GetOption("FeederBatteryScanerResultBackStrs");
                }
                return feederBatteryScanerResultBackStrs;
            }
            set
            {
                if (value != feederBatteryScanerResultBackStrs)
                {
                    TengDa.WF.Option.SetOption("FeederBatteryScanerResultBackStrs", value);
                    feederBatteryScanerResultBackStrs = value;
                }
            }
        }



        private string blankerStationHasWaterSampleBackStrs = string.Empty;
        /// <summary>
        /// 下料机工位夹具包含水分测试电池
        /// </summary>
        [Description("下料机工位夹具包含水分测试电池")]
        [DisplayName("下料机工位夹具包含水分测试电池")]
        [Category("下料机")]
        public string BlankerStationHasWaterSampleBackStrs
        {
            get
            {
                if (string.IsNullOrEmpty(blankerStationHasWaterSampleBackStrs))
                {
                    blankerStationHasWaterSampleBackStrs = TengDa.WF.Option.GetOption("BlankerStationHasWaterSampleBackStrs");
                }
                return blankerStationHasWaterSampleBackStrs;
            }
            set
            {
                if (value != blankerStationHasWaterSampleBackStrs)
                {
                    TengDa.WF.Option.SetOption("BlankerStationHasWaterSampleBackStrs", value);
                    blankerStationHasWaterSampleBackStrs = value;
                }
            }
        }

        private string blankerStationNotHasWaterSampleBackStrs = string.Empty;
        /// <summary>
        /// 下料机工位夹具不包含水分测试电池
        /// </summary>
        [Description("下料机工位夹具不包含水分测试电池")]
        [DisplayName("下料机工位夹具不包含水分测试电池")]
        [Category("下料机")]
        public string BlankerStationNotHasWaterSampleBackStrs
        {
            get
            {
                if (string.IsNullOrEmpty(blankerStationNotHasWaterSampleBackStrs))
                {
                    blankerStationNotHasWaterSampleBackStrs = TengDa.WF.Option.GetOption("BlankerStationNotHasWaterSampleBackStrs");
                }
                return blankerStationNotHasWaterSampleBackStrs;
            }
            set
            {
                if (value != blankerStationNotHasWaterSampleBackStrs)
                {
                    TengDa.WF.Option.SetOption("BlankerStationNotHasWaterSampleBackStrs", value);
                    blankerStationNotHasWaterSampleBackStrs = value;
                }
            }
        }

        private string waterSampleTestResultOkStrs = string.Empty;
        /// <summary>
        /// 水分测试OK反馈指令
        /// </summary>
        [Description("水分测试OK反馈指令")]
        [DisplayName("水分测试OK反馈指令")]
        [Category("下料机")]
        public string WaterSampleTestResultOkStrs
        {
            get
            {
                if (string.IsNullOrEmpty(waterSampleTestResultOkStrs))
                {
                    waterSampleTestResultOkStrs = TengDa.WF.Option.GetOption("WaterSampleTestResultOkStrs");
                }
                return waterSampleTestResultOkStrs;
            }
            set
            {
                if (value != waterSampleTestResultOkStrs)
                {
                    TengDa.WF.Option.SetOption("WaterSampleTestResultOkStrs", value);
                    waterSampleTestResultOkStrs = value;
                }
            }
        }

        private string waterSampleTestResultNgStrs = string.Empty;
        /// <summary>
        /// 水分测试NG反馈指令
        /// </summary>
        [Description("水分测试NG反馈指令")]
        [DisplayName("水分测试NG反馈指令")]
        [Category("下料机")]
        public string WaterSampleTestResultNgStrs
        {
            get
            {
                if (string.IsNullOrEmpty(waterSampleTestResultNgStrs))
                {
                    waterSampleTestResultNgStrs = TengDa.WF.Option.GetOption("WaterSampleTestResultNgStrs");
                }
                return waterSampleTestResultNgStrs;
            }
            set
            {
                if (value != waterSampleTestResultNgStrs)
                {
                    TengDa.WF.Option.SetOption("WaterSampleTestResultNgStrs", value);
                    waterSampleTestResultNgStrs = value;
                }
            }
        }

        private string getDoorStatusStr = string.Empty;
        /// <summary>
        /// 获取烤箱门状态指令
        /// </summary>
        [Description("获取烤箱门状态指令")]
        [DisplayName("获取烤箱门状态指令")]
        [Category("烤箱")]
        public string GetDoorStatusStr
        {
            get
            {
                if (string.IsNullOrEmpty(getDoorStatusStr))
                {
                    getDoorStatusStr = TengDa.WF.Option.GetOption("GetDoorStatusStr");
                }
                return getDoorStatusStr;
            }
            set
            {
                if (value != getDoorStatusStr)
                {
                    TengDa.WF.Option.SetOption("GetDoorStatusStr", value);
                    getDoorStatusStr = value;
                }
            }
        }

        private string openOvenDoorStrs = string.Empty;
        /// <summary>
        /// 烤箱开门指令
        /// </summary>
        [Description("烤箱开门指令")]
        [DisplayName("烤箱开门指令")]
        [Category("烤箱")]
        public string OpenOvenDoorStrs
        {
            get
            {
                if (string.IsNullOrEmpty(openOvenDoorStrs))
                {
                    openOvenDoorStrs = TengDa.WF.Option.GetOption("OpenOvenDoorStrs");
                }
                return openOvenDoorStrs;
            }
            set
            {
                if (value != openOvenDoorStrs)
                {
                    TengDa.WF.Option.SetOption("OpenOvenDoorStrs", value);
                    openOvenDoorStrs = value;
                }
            }
        }

        private string closeOvenDoorStrs = string.Empty;
        /// <summary>
        /// 烤箱关门指令
        /// </summary>
        [Description("烤箱关门指令")]
        [DisplayName("烤箱关门指令")]
        [Category("烤箱")]
        public string CloseOvenDoorStrs
        {
            get
            {
                if (string.IsNullOrEmpty(closeOvenDoorStrs))
                {
                    closeOvenDoorStrs = TengDa.WF.Option.GetOption("CloseOvenDoorStrs");
                }
                return closeOvenDoorStrs;
            }
            set
            {
                if (value != closeOvenDoorStrs)
                {
                    TengDa.WF.Option.SetOption("CloseOvenDoorStrs", value);
                    closeOvenDoorStrs = value;
                }
            }
        }

        private string getOvenClampStatusStrs = string.Empty;
        /// <summary>
        /// 获取烤箱夹具状态指令（左，右）
        /// </summary>
        [Description("获取烤箱夹具状态指令（左，右）")]
        [DisplayName("获取烤箱夹具状态指令")]
        [Category("烤箱")]
        public string GetOvenClampStatusStrs
        {
            get
            {
                if (string.IsNullOrEmpty(getOvenClampStatusStrs))
                {
                    getOvenClampStatusStrs = TengDa.WF.Option.GetOption("GetOvenClampStatusStrs");
                }
                return getOvenClampStatusStrs;
            }
            set
            {
                if (value != getOvenClampStatusStrs)
                {
                    TengDa.WF.Option.SetOption("GetOvenClampStatusStrs", value);
                    getOvenClampStatusStrs = value;
                }
            }
        }

        private string startBakingStrs = string.Empty;
        /// <summary>
        /// 开始Baking指令
        /// </summary>
        [Description("开始Baking指令")]
        [DisplayName("开始Baking指令")]
        [Category("烤箱")]
        public string StartBakingStrs
        {
            get
            {
                if (string.IsNullOrEmpty(startBakingStrs))
                {
                    startBakingStrs = TengDa.WF.Option.GetOption("StartBakingStrs");
                }
                return startBakingStrs;
            }
            set
            {
                if (value != startBakingStrs)
                {
                    TengDa.WF.Option.SetOption("StartBakingStrs", value);
                    startBakingStrs = value;
                }
            }
        }

        private string stopBakingStrs = string.Empty;
        /// <summary>
        /// 结束Baking指令
        /// </summary>
        [Description("结束Baking指令")]
        [DisplayName("结束Baking指令")]
        [Category("烤箱")]
        public string StopBakingStrs
        {
            get
            {
                if (string.IsNullOrEmpty(stopBakingStrs))
                {
                    stopBakingStrs = TengDa.WF.Option.GetOption("StopBakingStrs");
                }
                return stopBakingStrs;
            }
            set
            {
                if (value != stopBakingStrs)
                {
                    TengDa.WF.Option.SetOption("StopBakingStrs", value);
                    stopBakingStrs = value;
                }
            }
        }


        private string getBakingIsFinishedStr = string.Empty;
        /// <summary>
        /// 获取烤箱Baking是否结束指令
        /// </summary>
        [DisplayName("获取烤箱Baking是否结束指令")]
        [Category("烤箱")]
        public string GetBakingIsFinishedStr
        {
            get
            {
                if (string.IsNullOrEmpty(getBakingIsFinishedStr))
                {
                    getBakingIsFinishedStr = TengDa.WF.Option.GetOption("GetBakingIsFinishedStr");
                }
                return getBakingIsFinishedStr;
            }
            set
            {
                if (value != getBakingIsFinishedStr)
                {
                    TengDa.WF.Option.SetOption("GetBakingIsFinishedStr", value);
                    getBakingIsFinishedStr = value;
                }
            }
        }


        private string closeNetControlStrs = string.Empty;
        /// <summary>
        /// 关闭网控指令
        /// </summary>
        [Description("关闭网控指令")]
        [DisplayName("关闭网控指令")]
        [Category("烤箱")]
        public string CloseNetControlStrs
        {
            get
            {
                if (string.IsNullOrEmpty(closeNetControlStrs))
                {
                    closeNetControlStrs = TengDa.WF.Option.GetOption("CloseNetControlStrs");
                }
                return closeNetControlStrs;
            }
            set
            {
                if (value != closeNetControlStrs)
                {
                    TengDa.WF.Option.SetOption("CloseNetControlStrs", value);
                    closeNetControlStrs = value;
                }
            }
        }


        private string openNetControlStrs = string.Empty;
        /// <summary>
        /// 打开网控指令
        /// </summary>
        [Description("打开网控指令")]
        [DisplayName("打开网控指令")]
        [Category("烤箱")]
        public string OpenNetControlStrs
        {
            get
            {
                if (string.IsNullOrEmpty(openNetControlStrs))
                {
                    openNetControlStrs = TengDa.WF.Option.GetOption("OpenNetControlStrs");
                }
                return openNetControlStrs;
            }
            set
            {
                if (value != openNetControlStrs)
                {
                    TengDa.WF.Option.SetOption("OpenNetControlStrs", value);
                    openNetControlStrs = value;
                }
            }
        }


        private string getNetControlStatusStr = string.Empty;
        /// <summary>
        /// 获取炉腔网控状态指令
        /// </summary>
        [Description("获取炉腔网控状态指令")]
        [DisplayName("获取炉腔网控状态指令")]
        [Category("烤箱")]
        public string GetNetControlStatusStr
        {
            get
            {
                if (string.IsNullOrEmpty(getNetControlStatusStr))
                {
                    getNetControlStatusStr = TengDa.WF.Option.GetOption("GetNetControlStatusStr");
                }
                return getNetControlStatusStr;
            }
            set
            {
                if (value != getNetControlStatusStr)
                {
                    TengDa.WF.Option.SetOption("GetNetControlStatusStr", value);
                    getNetControlStatusStr = value;
                }
            }
        }

        private string unloadVacuumStrs = string.Empty;
        /// <summary>
        /// 卸真空指令
        /// </summary>
        [Description("卸真空指令")]
        [DisplayName("卸真空指令")]
        [Category("烤箱")]
        public string UnloadVacuumStrs
        {
            get
            {
                if (string.IsNullOrEmpty(unloadVacuumStrs))
                {
                    unloadVacuumStrs = TengDa.WF.Option.GetOption("UnloadVacuumStrs");
                }
                return unloadVacuumStrs;
            }
            set
            {
                if (value != unloadVacuumStrs)
                {
                    TengDa.WF.Option.SetOption("UnloadVacuumStrs", value);
                    unloadVacuumStrs = value;
                }
            }
        }

        private string loadVacuumStrs = string.Empty;
        /// <summary>
        /// 抽真空指令
        /// </summary>
        [DisplayName("抽真空指令")]
        [Category("烤箱")]
        public string LoadVacuumStrs
        {
            get
            {
                if (string.IsNullOrEmpty(loadVacuumStrs))
                {
                    loadVacuumStrs = TengDa.WF.Option.GetOption("LoadVacuumStrs");
                }
                return loadVacuumStrs;
            }
            set
            {
                if (value != loadVacuumStrs)
                {
                    TengDa.WF.Option.SetOption("LoadVacuumStrs", value);
                    loadVacuumStrs = value;
                }
            }
        }


        private string clearRunTimeStrs = string.Empty;
        /// <summary>
        /// 运行时间清零指令
        /// </summary>
        [DisplayName("运行时间清零指令")]
        [Category("烤箱")]
        public string ClearRunTimeStrs
        {
            get
            {
                if (string.IsNullOrEmpty(clearRunTimeStrs))
                {
                    clearRunTimeStrs = TengDa.WF.Option.GetOption("ClearRunTimeStrs");
                }
                return clearRunTimeStrs;
            }
            set
            {
                if (value != clearRunTimeStrs)
                {
                    TengDa.WF.Option.SetOption("ClearRunTimeStrs", value);
                    clearRunTimeStrs = value;
                }
            }
        }

        private string getVacuumStatusStr = string.Empty;
        /// <summary>
        /// 获取炉腔真空状态指令
        /// </summary>
        [Description("获取炉腔真空状态指令")]
        [DisplayName("获取炉腔真空状态指令")]
        [Category("烤箱")]
        public string GetVacuumStatusStr
        {
            get
            {
                if (string.IsNullOrEmpty(getVacuumStatusStr))
                {
                    getVacuumStatusStr = TengDa.WF.Option.GetOption("GetVacuumStatusStr");
                }
                return getVacuumStatusStr;
            }
            set
            {
                if (value != getVacuumStatusStr)
                {
                    TengDa.WF.Option.SetOption("GetVacuumStatusStr", value);
                    getVacuumStatusStr = value;
                }
            }
        }

        private string ovenAlarmResetStrs = string.Empty;
        /// <summary>
        /// 烤箱报警复位指令
        /// </summary>
        [Description("烤箱报警复位指令")]
        [DisplayName("烤箱报警复位指令")]
        [Category("烤箱")]
        public string OvenAlarmResetStrs
        {
            get
            {
                if (string.IsNullOrEmpty(ovenAlarmResetStrs))
                {
                    ovenAlarmResetStrs = TengDa.WF.Option.GetOption("OvenAlarmResetStrs");
                }
                return ovenAlarmResetStrs;
            }
            set
            {
                if (value != ovenAlarmResetStrs)
                {
                    TengDa.WF.Option.SetOption("OvenAlarmResetStrs", value);
                    ovenAlarmResetStrs = value;
                }
            }
        }

        private string getOvenClampStatusStr = string.Empty;
        /// <summary>
        /// 获取烤箱夹具状态指令
        /// </summary>
        [Description("获取烤箱夹具状态指令")]
        [DisplayName("获取烤箱夹具状态指令")]
        [Category("烤箱")]
        public string GetOvenClampStatusStr
        {
            get
            {
                if (string.IsNullOrEmpty(getOvenClampStatusStr))
                {
                    getOvenClampStatusStr = TengDa.WF.Option.GetOption("GetOvenClampStatusStr");
                }
                return getOvenClampStatusStr;
            }
            set
            {
                if (value != getOvenClampStatusStr)
                {
                    TengDa.WF.Option.SetOption("GetOvenClampStatusStr", value);
                    getOvenClampStatusStr = value;
                }
            }
        }


        private string getVacuumCommandStr = string.Empty;
        /// <summary>
        /// 获取真空指令状态
        /// </summary>
        [DisplayName("获取真空指令状态")]
        [Category("烤箱")]
        public string GetVacuumCommandStr
        {
            get
            {
                if (string.IsNullOrEmpty(getVacuumCommandStr))
                {
                    getVacuumCommandStr = TengDa.WF.Option.GetOption("GetVacuumCommandStr");
                }
                return getVacuumCommandStr;
            }
            set
            {
                if (value != getVacuumCommandStr)
                {
                    TengDa.WF.Option.SetOption("GetVacuumCommandStr", value);
                    getVacuumCommandStr = value;
                }
            }
        }



        private string curveIndexs = string.Empty;
        [Browsable(false)]
        public string CurveIndexs
        {
            get
            {
                if (string.IsNullOrEmpty(curveIndexs))
                {
                    curveIndexs = TengDa.WF.Option.GetOption("CurveIndexs");
                }
                return curveIndexs;
            }
            set
            {
                if (value != curveIndexs)
                {
                    TengDa.WF.Option.SetOption("CurveIndexs", value);
                    curveIndexs = value;
                }
            }
        }

        private string clearYieldTime = string.Empty;
        /// <summary>
        /// 产量清除时间
        /// </summary>
        [Browsable(false)]
        public string ClearYieldTime
        {
            get
            {
                if (string.IsNullOrEmpty(clearYieldTime))
                {
                    clearYieldTime = TengDa.WF.Option.GetOption("ClearYieldTime");
                }
                return clearYieldTime;
            }
            set
            {
                if (value != clearYieldTime)
                {
                    TengDa.WF.Option.SetOption("ClearYieldTime", value);
                    clearYieldTime = value;
                }
            }
        }

        private string scanClampCodeOK = string.Empty;
        /// <summary>
        /// 夹具扫码OK返回给上料机的指令
        /// </summary>
        [Description("夹具扫码OK返回给上料机的指令")]
        [DisplayName("夹具扫码OK返回给上料机的指令")]
        [Category("扫码枪")]
        public string ScanClampCodeOK
        {
            get
            {
                if (string.IsNullOrEmpty(scanClampCodeOK))
                {
                    scanClampCodeOK = TengDa.WF.Option.GetOption("ScanClampCodeOK");
                }
                return scanClampCodeOK;
            }
            set
            {
                if (value != scanClampCodeOK)
                {
                    TengDa.WF.Option.SetOption("ScanClampCodeOK", value);
                    scanClampCodeOK = value;
                }
            }
        }




        private string scanBatteryCodeOK = string.Empty;
        /// <summary>
        /// 电池扫码OK返回给上料机的指令
        /// </summary>
        [Description("电池扫码OK返回给上料机的指令")]
        [DisplayName("电池扫码OK返回给上料机的指令")]
        [Category("扫码枪")]
        public string ScanBatteryCodeOK
        {
            get
            {
                if (string.IsNullOrEmpty(scanBatteryCodeOK))
                {
                    scanBatteryCodeOK = TengDa.WF.Option.GetOption("ScanBatteryCodeOK");
                }
                return scanBatteryCodeOK;
            }
            set
            {
                if (value != scanBatteryCodeOK)
                {
                    TengDa.WF.Option.SetOption("ScanBatteryCodeOK", value);
                    scanBatteryCodeOK = value;
                }
            }
        }

        private string scanBatteryCodeNG = string.Empty;
        /// <summary>
        /// 电池扫码NG返回给上料机的指令
        /// </summary>
        [Description("电池扫码NG返回给上料机的指令")]
        [DisplayName("电池扫码NG返回给上料机的指令")]
        [Category("扫码枪")]
        public string ScanBatteryCodeNG
        {
            get
            {
                if (string.IsNullOrEmpty(scanBatteryCodeNG))
                {
                    scanBatteryCodeNG = TengDa.WF.Option.GetOption("ScanBatteryCodeNG");
                }
                return scanBatteryCodeNG;
            }
            set
            {
                if (value != scanBatteryCodeNG)
                {
                    TengDa.WF.Option.SetOption("ScanBatteryCodeNG", value);
                    scanBatteryCodeNG = value;
                }
            }
        }








        private byte startClampScan = 0;
        /// <summary>
        /// 夹具扫码启动信号
        /// </summary>
        [Description("夹具扫码启动信号")]
        [DisplayName("夹具扫码启动信号")]
        [Category("扫码枪")]
        public byte StartClampScan
        {
            get
            {
                if (startClampScan < 1)
                {
                    startClampScan = Byte.Parse(TengDa.WF.Option.GetOption("StartClampScan"));
                }
                return startClampScan;
            }
            set
            {
                if (value != startClampScan)
                {
                    TengDa.WF.Option.SetOption("StartClampScan", value.ToString());
                    startClampScan = value;
                }
            }
        }

        private byte stopClampScan = 0;
        /// <summary>
        /// 夹具扫码停止信号
        /// </summary>
        [Description("夹具扫码停止信号")]
        [DisplayName("夹具扫码停止信号")]
        [Category("扫码枪")]
        public byte StopClampScan
        {
            get
            {
                if (stopClampScan < 1)
                {
                    stopClampScan = Byte.Parse(TengDa.WF.Option.GetOption("StopClampScan"));
                }
                return stopClampScan;
            }
            set
            {
                if (value != stopClampScan)
                {
                    TengDa.WF.Option.SetOption("StopClampScan", value.ToString());
                    stopClampScan = value;
                }
            }
        }

        private float robotPositionAmplify = -1;
        /// <summary>
        /// 机器人X轴位置放大倍数
        /// </summary>
        [DisplayName("机器人X轴位置放大倍数")]
        [Category("机器人")]
        public float RobotPositionAmplify
        {
            get
            {
                if (robotPositionAmplify < 0)
                {
                    robotPositionAmplify = _Convert.StrToFloat(TengDa.WF.Option.GetOption("RobotPositionAmplify"), 6);
                }
                return robotPositionAmplify;
            }
            set
            {
                if (value != robotPositionAmplify)
                {
                    TengDa.WF.Option.SetOption("RobotPositionAmplify", value.ToString());
                    robotPositionAmplify = value;
                }
            }
        }

        //private string xxxXXXXXXXXXXXX = string.Empty;
        ///// <summary>
        ///// YYYYYYYYYYYYYYYYY
        ///// </summary>
        //[Description("YYYYYYYYYYYYYYYYY")]
        //[DisplayName("YYYYYYYYYYYYYYYYY")]
        //[Category("ZZZZZ")]
        //public string XXXXXXXXXXXXXXX
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(xxxXXXXXXXXXXXX))
        //        {
        //            xxxXXXXXXXXXXXX = TengDa.WF.Option.GetOption("XXXXXXXXXXXXXXX");
        //        }
        //        return xxxXXXXXXXXXXXX;
        //    }
        //    set
        //    {
        //        if (value != xxxXXXXXXXXXXXX)
        //        {
        //            TengDa.WF.Option.SetOption("XXXXXXXXXXXXXXX", value);
        //            xxxXXXXXXXXXXXX = value;
        //        }
        //    }
        //}

        //private string xxxXXXXXXXXXXXX = string.Empty;
        ///// <summary>
        ///// YYYYYYYYYYYYYYYYY
        ///// </summary>
        //[Description("YYYYYYYYYYYYYYYYY")]
        //[DisplayName("YYYYYYYYYYYYYYYYY")]
        //[Category("ZZZZZ")]
        //public string XXXXXXXXXXXXXXX
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(xxxXXXXXXXXXXXX))
        //        {
        //            xxxXXXXXXXXXXXX = TengDa.WF.Option.GetOption("XXXXXXXXXXXXXXX");
        //        }
        //        return xxxXXXXXXXXXXXX;
        //    }
        //    set
        //    {
        //        if (value != xxxXXXXXXXXXXXX)
        //        {
        //            TengDa.WF.Option.SetOption("XXXXXXXXXXXXXXX", value);
        //            xxxXXXXXXXXXXXX = value;
        //        }
        //    }
        //}


        //private string xxxXXXXXXXXXXXX = string.Empty;
        ///// <summary>
        ///// YYYYYYYYYYYYYYYYY
        ///// </summary>
        //[Description("YYYYYYYYYYYYYYYYY")]
        //[DisplayName("YYYYYYYYYYYYYYYYY")]
        //[Category("ZZZZZ")]
        //public string XXXXXXXXXXXXXXX
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(xxxXXXXXXXXXXXX))
        //        {
        //            xxxXXXXXXXXXXXX = TengDa.WF.Option.GetOption("XXXXXXXXXXXXXXX");
        //        }
        //        return xxxXXXXXXXXXXXX;
        //    }
        //    set
        //    {
        //        if (value != xxxXXXXXXXXXXXX)
        //        {
        //            TengDa.WF.Option.SetOption("XXXXXXXXXXXXXXX", value);
        //            xxxXXXXXXXXXXXX = value;
        //        }
        //    }
        //}


        //private string xxxXXXXXXXXXXXX = string.Empty;
        ///// <summary>
        ///// YYYYYYYYYYYYYYYYY
        ///// </summary>
        //[Description("YYYYYYYYYYYYYYYYY")]
        //[DisplayName("YYYYYYYYYYYYYYYYY")]
        //[Category("ZZZZZ")]
        //public string XXXXXXXXXXXXXXX
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(xxxXXXXXXXXXXXX))
        //        {
        //            xxxXXXXXXXXXXXX = TengDa.WF.Option.GetOption("XXXXXXXXXXXXXXX");
        //        }
        //        return xxxXXXXXXXXXXXX;
        //    }
        //    set
        //    {
        //        if (value != xxxXXXXXXXXXXXX)
        //        {
        //            TengDa.WF.Option.SetOption("XXXXXXXXXXXXXXX", value);
        //            xxxXXXXXXXXXXXX = value;
        //        }
        //    }
        //}


        //private string xxxXXXXXXXXXXXX = string.Empty;
        ///// <summary>
        ///// YYYYYYYYYYYYYYYYY
        ///// </summary>
        //[Description("YYYYYYYYYYYYYYYYY")]
        //[DisplayName("YYYYYYYYYYYYYYYYY")]
        //[Category("ZZZZZ")]
        //public string XXXXXXXXXXXXXXX
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(xxxXXXXXXXXXXXX))
        //        {
        //            xxxXXXXXXXXXXXX = TengDa.WF.Option.GetOption("XXXXXXXXXXXXXXX");
        //        }
        //        return xxxXXXXXXXXXXXX;
        //    }
        //    set
        //    {
        //        if (value != xxxXXXXXXXXXXXX)
        //        {
        //            TengDa.WF.Option.SetOption("XXXXXXXXXXXXXXX", value);
        //            xxxXXXXXXXXXXXX = value;
        //        }
        //    }
        //}

        //private string xxxXXXXXXXXXXXX = string.Empty;
        ///// <summary>
        ///// YYYYYYYYYYYYYYYYY
        ///// </summary>
        //[Description("YYYYYYYYYYYYYYYYY")]
        //[DisplayName("YYYYYYYYYYYYYYYYY")]
        //[Category("ZZZZZ")]
        //public string XXXXXXXXXXXXXXX
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(xxxXXXXXXXXXXXX))
        //        {
        //            xxxXXXXXXXXXXXX = TengDa.WF.Option.GetOption("XXXXXXXXXXXXXXX");
        //        }
        //        return xxxXXXXXXXXXXXX;
        //    }
        //    set
        //    {
        //        if (value != xxxXXXXXXXXXXXX)
        //        {
        //            TengDa.WF.Option.SetOption("XXXXXXXXXXXXXXX", value);
        //            xxxXXXXXXXXXXXX = value;
        //        }
        //    }
        //}


        //private string xxxXXXXXXXXXXXX = string.Empty;
        ///// <summary>
        ///// YYYYYYYYYYYYYYYYY
        ///// </summary>
        //[Description("YYYYYYYYYYYYYYYYY")]
        //[DisplayName("YYYYYYYYYYYYYYYYY")]
        //[Category("ZZZZZ")]
        //public string XXXXXXXXXXXXXXX
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(xxxXXXXXXXXXXXX))
        //        {
        //            xxxXXXXXXXXXXXX = TengDa.WF.Option.GetOption("XXXXXXXXXXXXXXX");
        //        }
        //        return xxxXXXXXXXXXXXX;
        //    }
        //    set
        //    {
        //        if (value != xxxXXXXXXXXXXXX)
        //        {
        //            TengDa.WF.Option.SetOption("XXXXXXXXXXXXXXX", value);
        //            xxxXXXXXXXXXXXX = value;
        //        }
        //    }
        //}

        //private string xxxXXXXXXXXXXXX = string.Empty;
        ///// <summary>
        ///// YYYYYYYYYYYYYYYYY
        ///// </summary>
        //[Description("YYYYYYYYYYYYYYYYY")]
        //[DisplayName("YYYYYYYYYYYYYYYYY")]
        //[Category("ZZZZZ")]
        //public string XXXXXXXXXXXXXXX
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(xxxXXXXXXXXXXXX))
        //        {
        //            xxxXXXXXXXXXXXX = TengDa.WF.Option.GetOption("XXXXXXXXXXXXXXX");
        //        }
        //        return xxxXXXXXXXXXXXX;
        //    }
        //    set
        //    {
        //        if (value != xxxXXXXXXXXXXXX)
        //        {
        //            TengDa.WF.Option.SetOption("XXXXXXXXXXXXXXX", value);
        //            xxxXXXXXXXXXXXX = value;
        //        }
        //    }
        //}

        //private string xxxXXXXXXXXXXXX = string.Empty;
        ///// <summary>
        ///// YYYYYYYYYYYYYYYYY
        ///// </summary>
        //[Description("YYYYYYYYYYYYYYYYY")]
        //[DisplayName("YYYYYYYYYYYYYYYYY")]
        //[Category("ZZZZZ")]
        //public string XXXXXXXXXXXXXXX
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(xxxXXXXXXXXXXXX))
        //        {
        //            xxxXXXXXXXXXXXX = TengDa.WF.Option.GetOption("XXXXXXXXXXXXXXX");
        //        }
        //        return xxxXXXXXXXXXXXX;
        //    }
        //    set
        //    {
        //        if (value != xxxXXXXXXXXXXXX)
        //        {
        //            TengDa.WF.Option.SetOption("XXXXXXXXXXXXXXX", value);
        //            xxxXXXXXXXXXXXX = value;
        //        }
        //    }
        //}


        //private string xxxXXXXXXXXXXXX = string.Empty;
        ///// <summary>
        ///// YYYYYYYYYYYYYYYYY
        ///// </summary>
        //[Description("YYYYYYYYYYYYYYYYY")]
        //[DisplayName("YYYYYYYYYYYYYYYYY")]
        //[Category("ZZZZZ")]
        //public string XXXXXXXXXXXXXXX
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(xxxXXXXXXXXXXXX))
        //        {
        //            xxxXXXXXXXXXXXX = TengDa.WF.Option.GetOption("XXXXXXXXXXXXXXX");
        //        }
        //        return xxxXXXXXXXXXXXX;
        //    }
        //    set
        //    {
        //        if (value != xxxXXXXXXXXXXXX)
        //        {
        //            TengDa.WF.Option.SetOption("XXXXXXXXXXXXXXX", value);
        //            xxxXXXXXXXXXXXX = value;
        //        }
        //    }
        //}


        //private string xxxXXXXXXXXXXXX = string.Empty;
        ///// <summary>
        ///// YYYYYYYYYYYYYYYYY
        ///// </summary>
        //[Description("YYYYYYYYYYYYYYYYY")]
        //[DisplayName("YYYYYYYYYYYYYYYYY")]
        //[Category("ZZZZZ")]
        //public string XXXXXXXXXXXXXXX
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(xxxXXXXXXXXXXXX))
        //        {
        //            xxxXXXXXXXXXXXX = TengDa.WF.Option.GetOption("XXXXXXXXXXXXXXX");
        //        }
        //        return xxxXXXXXXXXXXXX;
        //    }
        //    set
        //    {
        //        if (value != xxxXXXXXXXXXXXX)
        //        {
        //            TengDa.WF.Option.SetOption("XXXXXXXXXXXXXXX", value);
        //            xxxXXXXXXXXXXXX = value;
        //        }
        //    }
        //}


        //private string xxxXXXXXXXXXXXX = string.Empty;
        ///// <summary>
        ///// YYYYYYYYYYYYYYYYY
        ///// </summary>
        //[Description("YYYYYYYYYYYYYYYYY")]
        //[DisplayName("YYYYYYYYYYYYYYYYY")]
        //[Category("ZZZZZ")]
        //public string XXXXXXXXXXXXXXX
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(xxxXXXXXXXXXXXX))
        //        {
        //            xxxXXXXXXXXXXXX = TengDa.WF.Option.GetOption("XXXXXXXXXXXXXXX");
        //        }
        //        return xxxXXXXXXXXXXXX;
        //    }
        //    set
        //    {
        //        if (value != xxxXXXXXXXXXXXX)
        //        {
        //            TengDa.WF.Option.SetOption("XXXXXXXXXXXXXXX", value);
        //            xxxXXXXXXXXXXXX = value;
        //        }
        //    }
        //}


        //private string xxxXXXXXXXXXXXX = string.Empty;
        ///// <summary>
        ///// YYYYYYYYYYYYYYYYY
        ///// </summary>
        //[Description("YYYYYYYYYYYYYYYYY")]
        //[DisplayName("YYYYYYYYYYYYYYYYY")]
        //[Category("ZZZZZ")]
        //public string XXXXXXXXXXXXXXX
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(xxxXXXXXXXXXXXX))
        //        {
        //            xxxXXXXXXXXXXXX = TengDa.WF.Option.GetOption("XXXXXXXXXXXXXXX");
        //        }
        //        return xxxXXXXXXXXXXXX;
        //    }
        //    set
        //    {
        //        if (value != xxxXXXXXXXXXXXX)
        //        {
        //            TengDa.WF.Option.SetOption("XXXXXXXXXXXXXXX", value);
        //            xxxXXXXXXXXXXXX = value;
        //        }
        //    }
        //}


        //private string xxxXXXXXXXXXXXX = string.Empty;
        ///// <summary>
        ///// YYYYYYYYYYYYYYYYY
        ///// </summary>
        //[Description("YYYYYYYYYYYYYYYYY")]
        //[DisplayName("YYYYYYYYYYYYYYYYY")]
        //[Category("ZZZZZ")]
        //public string XXXXXXXXXXXXXXX
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(xxxXXXXXXXXXXXX))
        //        {
        //            xxxXXXXXXXXXXXX = TengDa.WF.Option.GetOption("XXXXXXXXXXXXXXX");
        //        }
        //        return xxxXXXXXXXXXXXX;
        //    }
        //    set
        //    {
        //        if (value != xxxXXXXXXXXXXXX)
        //        {
        //            TengDa.WF.Option.SetOption("XXXXXXXXXXXXXXX", value);
        //            xxxXXXXXXXXXXXX = value;
        //        }
        //    }
        //}


        //private string xxxXXXXXXXXXXXX = string.Empty;
        ///// <summary>
        ///// YYYYYYYYYYYYYYYYY
        ///// </summary>
        //[Description("YYYYYYYYYYYYYYYYY")]
        //[DisplayName("YYYYYYYYYYYYYYYYY")]
        //[Category("ZZZZZ")]
        //public string XXXXXXXXXXXXXXX
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(xxxXXXXXXXXXXXX))
        //        {
        //            xxxXXXXXXXXXXXX = TengDa.WF.Option.GetOption("XXXXXXXXXXXXXXX");
        //        }
        //        return xxxXXXXXXXXXXXX;
        //    }
        //    set
        //    {
        //        if (value != xxxXXXXXXXXXXXX)
        //        {
        //            TengDa.WF.Option.SetOption("XXXXXXXXXXXXXXX", value);
        //            xxxXXXXXXXXXXXX = value;
        //        }
        //    }
        //}


        //private string xxxXXXXXXXXXXXX = string.Empty;
        ///// <summary>
        ///// YYYYYYYYYYYYYYYYY
        ///// </summary>
        //[Description("YYYYYYYYYYYYYYYYY")]
        //[DisplayName("YYYYYYYYYYYYYYYYY")]
        //[Category("ZZZZZ")]
        //public string XXXXXXXXXXXXXXX
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(xxxXXXXXXXXXXXX))
        //        {
        //            xxxXXXXXXXXXXXX = TengDa.WF.Option.GetOption("XXXXXXXXXXXXXXX");
        //        }
        //        return xxxXXXXXXXXXXXX;
        //    }
        //    set
        //    {
        //        if (value != xxxXXXXXXXXXXXX)
        //        {
        //            TengDa.WF.Option.SetOption("XXXXXXXXXXXXXXX", value);
        //            xxxXXXXXXXXXXXX = value;
        //        }
        //    }
        //}


        //private string xxxXXXXXXXXXXXX = string.Empty;
        ///// <summary>
        ///// YYYYYYYYYYYYYYYYY
        ///// </summary>
        //[Description("YYYYYYYYYYYYYYYYY")]
        //[DisplayName("YYYYYYYYYYYYYYYYY")]
        //[Category("ZZZZZ")]
        //public string XXXXXXXXXXXXXXX
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(xxxXXXXXXXXXXXX))
        //        {
        //            xxxXXXXXXXXXXXX = TengDa.WF.Option.GetOption("XXXXXXXXXXXXXXX");
        //        }
        //        return xxxXXXXXXXXXXXX;
        //    }
        //    set
        //    {
        //        if (value != xxxXXXXXXXXXXXX)
        //        {
        //            TengDa.WF.Option.SetOption("XXXXXXXXXXXXXXX", value);
        //            xxxXXXXXXXXXXXX = value;
        //        }
        //    }
        //}


        //private string xxxXXXXXXXXXXXX = string.Empty;
        ///// <summary>
        ///// YYYYYYYYYYYYYYYYY
        ///// </summary>
        //[Description("YYYYYYYYYYYYYYYYY")]
        //[DisplayName("YYYYYYYYYYYYYYYYY")]
        //[Category("ZZZZZ")]
        //public string XXXXXXXXXXXXXXX
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(xxxXXXXXXXXXXXX))
        //        {
        //            xxxXXXXXXXXXXXX = TengDa.WF.Option.GetOption("XXXXXXXXXXXXXXX");
        //        }
        //        return xxxXXXXXXXXXXXX;
        //    }
        //    set
        //    {
        //        if (value != xxxXXXXXXXXXXXX)
        //        {
        //            TengDa.WF.Option.SetOption("XXXXXXXXXXXXXXX", value);
        //            xxxXXXXXXXXXXXX = value;
        //        }
        //    }
        //}
    }
}
