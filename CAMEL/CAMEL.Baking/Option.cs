using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using TengDa;

namespace CAMEL.Baking
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

        public const int TemperaturePointCount = 3;

        public const int VacuumPointCount = 1;

        public const int TemperatureSetPointCount = 10;

        private static int lineNum = -1;
        /// <summary>
        /// 设备布局类型    
        /// 
        /// </summary>
        public static int LineNum
        {
            get
            {
                if (lineNum < 0)
                {
                    lineNum = TengDa._Convert.StrToInt(ConfigurationManager.AppSettings["LINE_NUM"], -1);
                }
                return lineNum;
            }
        }

        public string[] TemperNames = new string[Option.TemperaturePointCount]
        {
            "温度1", "温度2", "温度3"
        };

        public string[] TemperSetNames = new string[Option.TemperatureSetPointCount]
        {
            "上左温度设定值", "上右温度设定值", "下左温度设定值", "下右温度设定值","左侧温度设定值",
            "右侧温度设定值", "后左温度设定值", "后右温度设定值", "门左温度设定值","门右温度设定值"
        };

        private List<Color> curveColors = new List<Color>();
        [DisplayName("温度曲线颜色")]
        [Category("温度曲线")]
        [Browsable(false)]
        public List<Color> CurveColors
        {
            get
            {
                if (curveColors.Count == 0)
                {
                    //for (int i = 0; i < TemperaturePointCount; i++)
                    //{
                    //    curveColors.Add(Color.FromArgb(i * 6 + 30, 255 - i * 6, i * 6 + 30));
                    //}
                    curveColors = new Color[TemperaturePointCount] { Color.Red, Color.Green, Color.Blue }.ToList();
                }
                return curveColors;
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


        private int clampBatteryCount = -1;
        /// <summary>
        /// 夹具中可以装载的电池个数
        /// </summary>
        [ReadOnly(true)]
        [DisplayName("夹具中可以装载的电池个数")]
        public int ClampBatteryCount
        {
            get
            {
                if (clampBatteryCount < 0)
                {
                    clampBatteryCount = _Convert.StrToInt(TengDa.WF.Option.GetOption("ClampBatteryCount"), 1);
                }
                return clampBatteryCount;
            }
            set
            {
                TengDa.WF.Option.SetOption("ClampBatteryCount", value.ToString());
                clampBatteryCount = value;
            }
        }

        #region 扫码枪

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


        private string clampScanerTriggerStr = string.Empty;
        /// <summary>
        /// 夹具扫码枪触发字符串
        /// </summary>
        [Description("夹具扫码枪触发字符串")]
        [DisplayName("夹具扫码枪触发字符串")]
        [Category("扫码枪")]
        public string ClampScanerTriggerStr
        {
            get
            {
                if (string.IsNullOrEmpty(clampScanerTriggerStr))
                {
                    clampScanerTriggerStr = TengDa.WF.Option.GetOption("ClampScanerTriggerStr");
                }
                return clampScanerTriggerStr;
            }
            set
            {
                if (value != clampScanerTriggerStr)
                {
                    TengDa.WF.Option.SetOption("ClampScanerTriggerStr", value);
                    clampScanerTriggerStr = value;
                }
            }
        }

        private string clampCodeRegularExpression = string.Empty;
        /// <summary>
        /// 夹具条码正则表达式
        /// </summary>
        [Description("夹具条码正则表达式")]
        [DisplayName("夹具条码正则表达式")]
        [Category("扫码枪")]
        public string ClampCodeRegularExpression
        {
            get
            {
                if (string.IsNullOrEmpty(clampCodeRegularExpression))
                {
                    clampCodeRegularExpression = TengDa.WF.Option.GetOption("ClampCodeRegularExpression");
                }
                return clampCodeRegularExpression;
            }
            set
            {
                if (value != clampCodeRegularExpression)
                {
                    TengDa.WF.Option.SetOption("ClampCodeRegularExpression", value);
                    clampCodeRegularExpression = value;
                }
            }
        }


        private string clampScanerFailedStr = string.Empty;
        /// <summary>
        /// 夹具扫码枪扫码失败返回字符串
        /// </summary>
        [DisplayName("夹具扫码枪扫码失败返回字符串")]
        [Category("扫码枪")]
        public string ClampScanerFailedStr
        {
            get
            {
                if (string.IsNullOrEmpty(clampScanerFailedStr))
                {
                    clampScanerFailedStr = TengDa.WF.Option.GetOption("ClampScanerFailedStr");
                }
                return clampScanerFailedStr;
            }
            set
            {
                if (value != clampScanerFailedStr)
                {
                    TengDa.WF.Option.SetOption("ClampScanerFailedStr", value);
                    clampScanerFailedStr = value;
                }
            }
        }


        private string batteryScanerStopStr = string.Empty;
        /// <summary>
        /// 电池扫码枪停止扫码指令
        /// </summary>
        [DisplayName("电池扫码枪停止扫码指令")]
        [Category("扫码枪")]
        public string BatteryScanerStopStr
        {
            get
            {
                if (string.IsNullOrEmpty(batteryScanerStopStr))
                {
                    batteryScanerStopStr = TengDa.WF.Option.GetOption("BatteryScanerStopStr");
                }
                return batteryScanerStopStr;
            }
            set
            {
                if (value != batteryScanerStopStr)
                {
                    TengDa.WF.Option.SetOption("BatteryScanerStopStr", value);
                    batteryScanerStopStr = value;
                }
            }
        }


        private string clampScanerStopStr = string.Empty;
        /// <summary>
        /// 夹具扫码枪停止扫码指令
        /// </summary>
        [DisplayName("夹具扫码枪停止扫码指令")]
        [Category("扫码枪")]
        public string ClampScanerStopStr
        {
            get
            {
                if (string.IsNullOrEmpty(clampScanerStopStr))
                {
                    clampScanerStopStr = TengDa.WF.Option.GetOption("ClampScanerStopStr");
                }
                return clampScanerStopStr;
            }
            set
            {
                if (value != clampScanerStopStr)
                {
                    TengDa.WF.Option.SetOption("ClampScanerStopStr", value);
                    clampScanerStopStr = value;
                }
            }
        }


        #endregion

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

        //private string uploadMesInterval = string.Empty;
        ///// <summary>
        ///// MES上传数据间隔时间，单位：秒
        ///// </summary>
        //[Description("MES上传数据间隔时间，单位：秒")]
        //[DisplayName("MES上传数据间隔时间")]
        //[Category("MES")]
        //public string UploadMesInterval
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(uploadMesInterval))
        //        {
        //            uploadMesInterval = TengDa.WF.Option.GetOption("UploadMesInterval");
        //        }
        //        return uploadMesInterval;
        //    }
        //    set
        //    {
        //        if (value != uploadMesInterval)
        //        {
        //            TengDa.WF.Option.SetOption("UploadMesInterval", value);
        //            uploadMesInterval = value;
        //        }
        //    }
        //}

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


        private int taskInterval = -1;
        /// <summary>
        /// 检测是否生成RGV搬运任务时间间隔，单位：毫秒
        /// </summary>
        [Description("检测是否生成RGV搬运任务时间间隔，单位：毫秒")]
        [DisplayName("检测是否生成RGV搬运任务时间间隔")]
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

        private float robotPositionAmplify = -1;
        /// <summary>
        /// RGVX轴位置放大倍数
        /// </summary>
        [DisplayName("RGVX轴位置放大倍数")]
        [Category("RGV")]
        public float RGVPositionAmplify
        {
            get
            {
                if (robotPositionAmplify < 0)
                {
                    robotPositionAmplify = _Convert.StrToFloat(TengDa.WF.Option.GetOption("RGVPositionAmplify"), 6);
                }
                return robotPositionAmplify;
            }
            set
            {
                if (value != robotPositionAmplify)
                {
                    TengDa.WF.Option.SetOption("RGVPositionAmplify", value.ToString());
                    robotPositionAmplify = value;
                }
            }
        }

        private int robotMinCoordinate = 0;
        /// <summary>
        /// RGVX轴坐标最小值
        /// </summary>
        [DisplayName("RGVX轴坐标最小值")]
        [Category("RGV")]
        public int RGVMinCoordinate
        {
            get
            {
                if (robotMinCoordinate == 0)
                {
                    robotMinCoordinate = _Convert.StrToInt(TengDa.WF.Option.GetOption("RGVMinCoordinate"), 308);
                }
                return robotMinCoordinate;
            }
            set
            {
                if (value != robotMinCoordinate)
                {
                    TengDa.WF.Option.SetOption("RGVMinCoordinate", value.ToString());
                    robotMinCoordinate = value;
                }
            }
        }


        private int robotStopPosition4RasterInductive = 0;
        /// <summary>
        /// 下料人员安全光栅感应时RGV急停最大位置
        /// 若人员进入安全光栅感应区，且搬运RGV位置小于该值，则远程发送急停指令到搬运RGV
        /// </summary>
        [DisplayName("下料人员安全光栅感应时RGV急停最大位置")]
        [Description("若人员进入安全光栅感应区，且搬运RGV位置小于该值，则远程发送急停指令到搬运RGV")]
        [Category("RGV")]
        public int RGVStopPosition4RasterInductive
        {
            get
            {
                if (robotStopPosition4RasterInductive == 0)
                {
                    robotStopPosition4RasterInductive = _Convert.StrToInt(TengDa.WF.Option.GetOption("RGVStopPosition4RasterInductive"), 4000);
                }
                return robotStopPosition4RasterInductive;
            }
            set
            {
                if (value != robotStopPosition4RasterInductive)
                {
                    TengDa.WF.Option.SetOption("RGVStopPosition4RasterInductive", value.ToString());
                    robotStopPosition4RasterInductive = value;
                }
            }
        }

        

        [Browsable(false)]
        public string FloorShowInfoType { get; set; } = "默认信息";


        private int currentWorkNum = 0;

        /// <summary>
        /// 库卡RGV当前任务编号
        /// </summary>
        [Browsable(false)]
        public int CurrentWorkNum
        {
            get
            {
                if (currentWorkNum == 0)
                {
                    currentWorkNum = _Convert.StrToInt(TengDa.WF.Option.GetOption("CurrentWorkNum"), 1);
                }
                return currentWorkNum;
            }
            set
            {
                if (value != currentWorkNum)
                {
                    TengDa.WF.Option.SetOption("CurrentWorkNum", value.ToString());
                    currentWorkNum = value;
                }
            }
        }




        private string mesManuIdentityVerificationInput = string.Empty;
        /// <summary>
        /// MES手动调试身份验证请求数据
        /// </summary>
        [DisplayName("MES手动调试身份验证请求数据")]
        [Browsable(false)]
        public string MesManuIdentityVerificationInput
        {
            get
            {
                if (string.IsNullOrEmpty(mesManuIdentityVerificationInput))
                {
                    mesManuIdentityVerificationInput = TengDa.WF.Option.GetOption("MesManuIdentityVerificationInput");
                }
                return mesManuIdentityVerificationInput;
            }
            set
            {
                if (value != mesManuIdentityVerificationInput)
                {
                    TengDa.WF.Option.SetOption("MesManuIdentityVerificationInput", value);
                    mesManuIdentityVerificationInput = value;
                }
            }
        }


        private string mesManuGetTrayBindingInfoInput = string.Empty;
        /// <summary>
        /// MES手动调试电芯与托盘信息查询请求数据
        /// </summary>
        [DisplayName("MES手动调试电芯与托盘信息查询请求数据")]
        [Browsable(false)]
        public string MesManuGetTrayBindingInfoInput
        {
            get
            {
                if (string.IsNullOrEmpty(mesManuGetTrayBindingInfoInput))
                {
                    mesManuGetTrayBindingInfoInput = TengDa.WF.Option.GetOption("MesManuGetTrayBindingInfoInput");
                }
                return mesManuGetTrayBindingInfoInput;
            }
            set
            {
                if (value != mesManuGetTrayBindingInfoInput)
                {
                    TengDa.WF.Option.SetOption("MesManuGetTrayBindingInfoInput", value);
                    mesManuGetTrayBindingInfoInput = value;
                }
            }
        }

        private string mesManuRecordDeviceStatusInput = string.Empty;
        /// <summary>
        /// MES手动调试记录设备状态请求数据
        /// </summary>
        [DisplayName("MES手动调试记录设备状态请求数据")]
        [Browsable(false)]
        public string MesManuRecordDeviceStatusInput
        {
            get
            {
                if (string.IsNullOrEmpty(mesManuRecordDeviceStatusInput))
                {
                    mesManuRecordDeviceStatusInput = TengDa.WF.Option.GetOption("MesManuRecordDeviceStatusInput");
                }
                return mesManuRecordDeviceStatusInput;
            }
            set
            {
                if (value != mesManuRecordDeviceStatusInput)
                {
                    TengDa.WF.Option.SetOption("MesManuRecordDeviceStatusInput", value);
                    mesManuRecordDeviceStatusInput = value;
                }
            }
        }


        private string mesManuUploadSecondaryHighTempDataInput = string.Empty;
        /// <summary>
        /// MES手动调试二次高温数据上传请求数据
        /// </summary>
        [DisplayName("MES手动调试二次高温数据上传请求数据")]
        [Browsable(false)]
        public string MesManuUploadSecondaryHighTempDataInput
        {
            get
            {
                if (string.IsNullOrEmpty(mesManuUploadSecondaryHighTempDataInput))
                {
                    mesManuUploadSecondaryHighTempDataInput = TengDa.WF.Option.GetOption("MesManuUploadSecondaryHighTempDataInput");
                }
                return mesManuUploadSecondaryHighTempDataInput;
            }
            set
            {
                if (value != mesManuUploadSecondaryHighTempDataInput)
                {
                    TengDa.WF.Option.SetOption("MesManuUploadSecondaryHighTempDataInput", value);
                    mesManuUploadSecondaryHighTempDataInput = value;
                }
            }
        }


        private string currentMoCode = string.Empty;
        /// <summary>
        /// MES当前工单
        /// </summary>
        [DisplayName("MES当前工单")]
        [Category("MES相关")]
        public string CurrentMoCode
        {
            get
            {
                if (string.IsNullOrEmpty(currentMoCode))
                {
                    currentMoCode = TengDa.WF.Option.GetOption("CurrentMoCode");
                }
                return currentMoCode;
            }
            set
            {
                if (value != currentMoCode)
                {
                    TengDa.WF.Option.SetOption("CurrentMoCode", value);
                    currentMoCode = value;
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
    }
}
