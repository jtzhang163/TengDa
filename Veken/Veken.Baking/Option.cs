using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using TengDa;

namespace Veken.Baking
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
        /// 温度曲线腔体Id
        /// </summary>
        [ReadOnly(true), Description("温度曲线腔体Id")]
        [DisplayName("温度曲线腔体Id")]
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

        private string curveColorsStr = string.Empty;//曲线颜色

        [Description("温度曲线颜色")]
        [DisplayName("温度曲线颜色")]
        [Category("温度曲线")]
        [Browsable(false)]
        public List<Color> CurveColors
        {
            get
            {
                if (string.IsNullOrEmpty(curveColorsStr))
                {
                    curveColorsStr = TengDa.WF.Option.GetOption("CurveColorsStr");
                }
                List<Color> colors = new List<Color>();
                string[] curveColors = curveColorsStr.Split(',');
                for (int i = 0; i < curveColors.Length; i++)
                {
                    colors.Add(Color.FromName(curveColors[i]));
                }
                return colors;
            }
            set
            {
                List<Color> colors = value;
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < colors.Count; i++)
                {
                    sb.Append(colors[i].Name + ",");
                }
                curveColorsStr = sb.ToString().TrimEnd(',');
                TengDa.WF.Option.SetOption("CurveColorsStr", curveColorsStr);
            }
        }

        private string floorNumberRegexStr = string.Empty;
        /// <summary>
        /// 腔体资源号正则表达式
        /// </summary>
        [Description("腔体资源号正则表达式")]
        [DisplayName("腔体资源号正则表达式")]
        [Category("扫码枪")]
        public string FloorNumberRegexStr
        {
            get
            {
                if (string.IsNullOrEmpty(floorNumberRegexStr))
                {
                    floorNumberRegexStr = TengDa.WF.Option.GetOption("FloorNumberRegexStr");
                }
                return floorNumberRegexStr;
            }
            set
            {
                if (value != floorNumberRegexStr)
                {
                    TengDa.WF.Option.SetOption("FloorNumberRegexStr", value);
                    floorNumberRegexStr = value;
                }
            }
        }


        private string clampCodeRegexStr = string.Empty;
        /// <summary>
        /// 料盒条码正则表达式
        /// </summary>
        [Description("料盒条码正则表达式")]
        [DisplayName("料盒条码正则表达式")]
        [Category("扫码枪")]
        public string ClampCodeRegexStr
        {
            get
            {
                if (string.IsNullOrEmpty(clampCodeRegexStr))
                {
                    clampCodeRegexStr = TengDa.WF.Option.GetOption("ClampCodeRegexStr");
                }
                return clampCodeRegexStr;
            }
            set
            {
                if (value != clampCodeRegexStr)
                {
                    TengDa.WF.Option.SetOption("ClampCodeRegexStr", value);
                    clampCodeRegexStr = value;
                }
            }
        }

        private string isPassiveReceiveSerialPort = string.Empty;
        /// <summary>
        /// 是否被动接受串口返回的数据
        /// </summary>
        [Description("是否被动接受串口返回的数据")]
        [DisplayName("是否被动接受串口返回的数据")]
        [Category("扫码枪")]
        public bool IsPassiveReceiveSerialPort
        {
            get
            {
                if (string.IsNullOrEmpty(isPassiveReceiveSerialPort))
                {
                    isPassiveReceiveSerialPort = TengDa.WF.Option.GetOption("IsPassiveReceiveSerialPort");
                }
                return _Convert.StrToBool(isPassiveReceiveSerialPort, false);
            }
            set
            {
                if (value.ToString() != isPassiveReceiveSerialPort)
                {
                    TengDa.WF.Option.SetOption("IsPassiveReceiveSerialPort", value.ToString());
                    isPassiveReceiveSerialPort = value.ToString();
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
        /// 查询真空温度时间范围，单位：Day
        /// </summary>
        [Description("查询真空温度时间范围，单位：Day")]
        [DisplayName("查询真空温度时间范围")]
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

        private string isBakingValue = string.Empty;
        /// <summary>
        /// baking进行中信号值
        /// </summary>
        [Description("烘烤进行中信号值")]
        [DisplayName("烘烤进行中信号值")]
        [Category("PLC通讯")]
        public string IsBakingValue
        {
            get
            {
                if (string.IsNullOrEmpty(isBakingValue))
                {
                    isBakingValue = TengDa.WF.Option.GetOption("IsBakingValue");
                }
                return isBakingValue;
            }
            set
            {
                if (value != isBakingValue)
                {
                    TengDa.WF.Option.SetOption("IsBakingValue", value);
                    isBakingValue = value;
                }
            }
        }

        private string isNotBakingValue = string.Empty;
        /// <summary>
        /// baking未进行信号值
        /// </summary>
        [Description("烘烤未进行信号值")]
        [DisplayName("烘烤未进行信号值")]
        [Category("PLC通讯")]
        public string IsNotBakingValue
        {
            get
            {
                if (string.IsNullOrEmpty(isNotBakingValue))
                {
                    isNotBakingValue = TengDa.WF.Option.GetOption("IsNotBakingValue");
                }
                return isNotBakingValue;
            }
            set
            {
                if (value != isNotBakingValue)
                {
                    TengDa.WF.Option.SetOption("IsNotBakingValue", value);
                    isNotBakingValue = value;
                }
            }
        }

        private string getRunStatusStr = string.Empty;
        /// <summary>
        /// 获取PLC运行状态的字符串
        /// </summary>
        [Description("获取PLC运行状态的字符串")]
        [DisplayName("获取PLC运行状态的字符串")]
        [Category("PLC通讯")]
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

        private string getVacuumStr = string.Empty;
        /// <summary>
        /// 获取烤箱真空度的字符串
        /// </summary>
        [Description("获取烤箱真空度的字符串")]
        [DisplayName("获取烤箱真空度的字符串")]
        [Category("PLC通讯")]
        public string GetVacuumStr
        {
            get
            {
                if (string.IsNullOrEmpty(getVacuumStr))
                {
                    getVacuumStr = TengDa.WF.Option.GetOption("GetVacuumStr");
                }
                return getVacuumStr;
            }
            set
            {
                if (value != getVacuumStr)
                {
                    TengDa.WF.Option.SetOption("GetVacuumStr", value);
                    getVacuumStr = value;
                }
            }
        }

        private string getTemStrs = string.Empty;
        /// <summary>
        /// 获取烤箱温度的字符串
        /// </summary>
        [Description("获取烤箱温度的字符串")]
        [DisplayName("获取烤箱温度的字符串")]
        [Category("PLC通讯")]
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

        private string getRunMinutesSetStr = string.Empty;
        /// <summary>
        /// 获取烤箱温度的字符串
        /// </summary>
        [Description("获取烤箱运行总设置时间的字符串")]
        [DisplayName("获取烤箱运行总设置时间的字符串")]
        [Category("PLC通讯")]
        public string GetRunMinutesSetStr
        {
            get
            {
                if (string.IsNullOrEmpty(getRunMinutesSetStr))
                {
                    getRunMinutesSetStr = TengDa.WF.Option.GetOption("GetRunMinutesSetStr");
                }
                return getRunMinutesSetStr;
            }
            set
            {
                if (value != getRunMinutesSetStr)
                {
                    TengDa.WF.Option.SetOption("GetRunMinutesSetStr", value);
                    getRunMinutesSetStr = value;
                }
            }
        }

        private string getRuntimeStr = string.Empty;
        /// <summary>
        /// 获取烤箱已运行时间的字符串
        /// </summary>
        [Description("获取烤箱已运行时间的字符串")]
        [DisplayName("获取烤箱已运行时间的字符串")]
        [Category("PLC通讯")]
        public string GetRuntimeStr
        {
            get
            {
                if (string.IsNullOrEmpty(getRuntimeStr))
                {
                    getRuntimeStr = TengDa.WF.Option.GetOption("GetRuntimeStr");
                }
                return getRuntimeStr;
            }
            set
            {
                if (value != getRuntimeStr)
                {
                    TengDa.WF.Option.SetOption("GetRuntimeStr", value);
                    getRuntimeStr = value;
                }
            }
        }

        private string getTrichromaticLampStr = string.Empty;
        /// <summary>
        /// 获取烤箱三色灯的字符串
        /// </summary>
        [Description("获取烤箱三色灯的字符串")]
        [DisplayName("获取烤箱三色灯的字符串")]
        [Category("PLC通讯")]
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
        [Category("PLC通讯")]
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

        /// <summary>
        /// 入腔是否校验
        /// </summary>
        [ReadOnly(true), Description("入腔是否校验")]
        [DisplayName("入腔是否校验")]
        public bool InOvenCheck
        {
            get
            {
                return _Convert.StrToBool(TengDa.WF.Option.GetOption("InOvenCheck"), false);
            }
            set
            {
                TengDa.WF.Option.SetOption("InOvenCheck", value.ToString());
            }
        }
    }
}
