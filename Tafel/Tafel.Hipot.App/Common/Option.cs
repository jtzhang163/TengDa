using System.ComponentModel;
using TengDa;
using TengDa.Wpf;

namespace Tafel.Hipot.App
{
    /// <summary>
    /// 基本配置
    /// </summary>
    [DisplayName("基本配置")]
    public class Option : BindableObject
    {

        private int checkTesterInfoInterval = -1;
        /// <summary>
        /// 检测测试仪信息的时间间隔
        /// </summary>
        [DisplayName("检测测试仪信息的时间间隔")]
        [Description("检测测试仪信息的时间间隔, 单位（ms）")]
        [Category("通信")]
        public int CheckTesterInfoInterval
        {
            get
            {
                if (checkTesterInfoInterval < 0)
                {
                    checkTesterInfoInterval = _Convert.StrToInt(TengDa.Wpf.Option.GetOption("CheckTesterInfoInterval"), -1);
                    if (checkTesterInfoInterval < 0)
                    {
                        checkTesterInfoInterval = 100;
                        TengDa.Wpf.Option.SetOption("CheckTesterInfoInterval", checkTesterInfoInterval.ToString(), "检测测试仪信息的时间间隔");
                    }
                }
                return checkTesterInfoInterval;
            }
            set
            {
                if (checkTesterInfoInterval != value)
                {
                    TengDa.Wpf.Option.SetOption("CheckTesterInfoInterval", value.ToString());
                    SetProperty(ref checkTesterInfoInterval, value);
                }
            }
        }

        private int checkCollectorInfoInterval = -1;
        /// <summary>
        /// 检测温度采集器信息的时间间隔
        /// </summary>
        [DisplayName("检测温度采集器信息的时间间隔")]
        [Description("检测温度采集器信息的时间间隔, 单位（ms）")]
        [Category("通信")]
        public int CheckCollectorInfoInterval
        {
            get
            {
                if (checkCollectorInfoInterval < 0)
                {
                    checkCollectorInfoInterval = _Convert.StrToInt(TengDa.Wpf.Option.GetOption("CheckCollectorInfoInterval"), -1);
                    if (checkCollectorInfoInterval < 0)
                    {
                        checkCollectorInfoInterval = 100;
                        TengDa.Wpf.Option.SetOption("CheckCollectorInfoInterval", checkCollectorInfoInterval.ToString(), "检测温度采集器信息的时间间隔");
                    }
                }
                return checkCollectorInfoInterval;
            }
            set
            {
                if (checkCollectorInfoInterval != value)
                {
                    TengDa.Wpf.Option.SetOption("CheckCollectorInfoInterval", value.ToString());
                    SetProperty(ref checkCollectorInfoInterval, value);
                }
            }
        }

        private int checkCoolerInfoInterval = -1;
        /// <summary>
        /// 检测冷却机PLC信息的时间间隔
        /// </summary>
        [DisplayName("检测冷却机PLC信息的时间间隔")]
        [Description("检测冷却机PLC信息的时间间隔, 单位（ms）")]
        [Category("通信")]
        public int CheckCoolerInfoInterval
        {
            get
            {
                if (checkCoolerInfoInterval < 0)
                {
                    checkCoolerInfoInterval = _Convert.StrToInt(TengDa.Wpf.Option.GetOption("CheckCoolerInfoInterval"), -1);
                    if (checkCoolerInfoInterval < 0)
                    {
                        checkCoolerInfoInterval = 1000;
                        TengDa.Wpf.Option.SetOption("CheckCoolerInfoInterval", checkCoolerInfoInterval.ToString(), "检测冷却机PLC信息的时间间隔");
                    }
                }
                return checkCoolerInfoInterval;
            }
            set
            {
                if (checkCoolerInfoInterval != value)
                {
                    TengDa.Wpf.Option.SetOption("CheckCoolerInfoInterval", value.ToString());
                    SetProperty(ref checkCoolerInfoInterval, value);
                }
            }
        }

        private int checkScanerInfoInterval = -1;
        /// <summary>
        /// 检测扫码枪信息的时间间隔
        /// </summary>
        [DisplayName("检测扫码枪信息的时间间隔")]
        [Description("检测扫码枪信息的时间间隔, 单位（ms）")]
        [Category("通信")]
        public int CheckScanerInfoInterval
        {
            get
            {
                if (checkScanerInfoInterval < 0)
                {
                    checkScanerInfoInterval = _Convert.StrToInt(TengDa.Wpf.Option.GetOption("CheckScanerInfoInterval"), -1);
                    if (checkScanerInfoInterval < 0)
                    {
                        checkScanerInfoInterval = 1000;
                        TengDa.Wpf.Option.SetOption("CheckScanerInfoInterval", checkScanerInfoInterval.ToString(), "检测扫码枪信息的时间间隔");
                    }
                }
                return checkScanerInfoInterval;
            }
            set
            {
                if (checkScanerInfoInterval != value)
                {
                    TengDa.Wpf.Option.SetOption("CheckScanerInfoInterval", value.ToString());
                    SetProperty(ref checkScanerInfoInterval, value);
                }
            }
        }

        private int checkMesInfoInterval = -1;
        /// <summary>
        /// 检测数据并上传MES的时间间隔
        /// </summary>
        [DisplayName("检测数据并上传MES的时间间隔")]
        [Description("检测数据并上传MES的时间间隔, 单位（s）")]
        [Category("通信")]
        public int CheckMesInfoInterval
        {
            get
            {
                if (checkMesInfoInterval < 0)
                {
                    checkMesInfoInterval = _Convert.StrToInt(TengDa.Wpf.Option.GetOption("CheckMesInfoInterval"), -1);
                    if (checkMesInfoInterval < 0)
                    {
                        checkMesInfoInterval = 60;
                        TengDa.Wpf.Option.SetOption("CheckMesInfoInterval", checkMesInfoInterval.ToString(), "检测数据并上传MES的时间间隔");
                    }
                }
                return checkMesInfoInterval;
            }
            set
            {
                if (checkMesInfoInterval != value)
                {
                    TengDa.Wpf.Option.SetOption("CheckMesInfoInterval", value.ToString());
                    SetProperty(ref checkMesInfoInterval, value);
                }
            }
        }


        private int lastLoginUserId = -1;

        /// <summary>
        /// 最后登录系统的用户ID
        /// </summary>
        [Browsable(false)]
        public int LastLoginUserId
        {
            get
            {
                if (lastLoginUserId < 0)
                {
                    lastLoginUserId = _Convert.StrToInt(TengDa.Wpf.Option.GetOption("LastLoginUserId"), -1);
                    if (lastLoginUserId < 0)
                    {
                        lastLoginUserId = 1;
                        TengDa.Wpf.Option.SetOption("LastLoginUserId", lastLoginUserId.ToString(), "最后登录系统的用户ID");
                    }
                }
                return lastLoginUserId;
            }
            set
            {
                if (lastLoginUserId != value)
                {
                    TengDa.Wpf.Option.SetOption("LastLoginUserId", value.ToString());
                    lastLoginUserId = value;
                }
            }
        }


        private bool? isRememberMe = null;

        /// <summary>
        /// 是否记住我
        /// </summary>
        [Browsable(false)]
        public bool IsRememberMe
        {
            get
            {
                if (isRememberMe == null)
                {
                    isRememberMe = _Convert.StrToBoolOrNull(TengDa.Wpf.Option.GetOption("IsRememberMe"));
                    if (isRememberMe == null)
                    {
                        isRememberMe = false;
                        TengDa.Wpf.Option.SetOption("IsRememberMe", isRememberMe.ToString(), "是否记住我");
                    }
                }
                return isRememberMe.Value;
            }
            set
            {
                if (isRememberMe.Value != value)
                {
                    TengDa.Wpf.Option.SetOption("IsRememberMe", value.ToString());
                    SetProperty(ref isRememberMe, value);
                }
            }
        }

        private bool? isMesLogin = null;
        /// <summary>
        /// 是否为MES登录
        /// </summary>
        [Browsable(false)]
        public bool IsMesLogin
        {
            get
            {
                if (isMesLogin == null)
                {
                    isMesLogin = _Convert.StrToBoolOrNull(TengDa.Wpf.Option.GetOption("IsMesLogin"));
                    if (isMesLogin == null)
                    {
                        isMesLogin = false;
                        TengDa.Wpf.Option.SetOption("IsMesLogin", isMesLogin.ToString(), "是否为MES登录");
                    }
                }
                return isMesLogin.Value;
            }
            set
            {
                if (isMesLogin.Value != value)
                {
                    TengDa.Wpf.Option.SetOption("IsMesLogin", value.ToString());
                    SetProperty(ref isMesLogin, value);
                }
            }
        }



        private int constVoltage = -1;

        /// <summary>
        /// 电压固定值
        /// </summary>
        [DisplayName("电压固定值")]
        [Description("电压固定值, 单位（V）")]
        [Category("电阻测试器")]
        public int ConstVoltage
        {
            get
            {
                if (constVoltage < 0)
                {
                    constVoltage = _Convert.StrToInt(TengDa.Wpf.Option.GetOption("ConstVoltage"), -1);
                    if (constVoltage < 0)
                    {
                        constVoltage = 100;
                        TengDa.Wpf.Option.SetOption("ConstVoltage", constVoltage.ToString(), "电压固定值, 单位（V）");
                    }
                }
                return constVoltage;
            }
            set
            {
                if (constVoltage != value)
                {
                    TengDa.Wpf.Option.SetOption("ConstVoltage", value.ToString());
                    constVoltage = value;
                }
            }
        }



        private int constTimeSpan = -1;

        /// <summary>
        /// 测试时间固定值
        /// </summary>
        [DisplayName("测试时间固定值")]
        [Description("测试时间固定值, 单位（s）")]
        [Category("电阻测试器")]
        public int ConstTimeSpan
        {
            get
            {
                if (constTimeSpan < 0)
                {
                    constTimeSpan = _Convert.StrToInt(TengDa.Wpf.Option.GetOption("ConstTimeSpan"), -1);
                    if (constTimeSpan < 0)
                    {
                        constTimeSpan = 5;
                        TengDa.Wpf.Option.SetOption("ConstTimeSpan", constTimeSpan.ToString(), "测试时间固定值, 单位（s）");
                    }
                }
                return constTimeSpan;
            }
            set
            {
                if (constTimeSpan != value)
                {
                    TengDa.Wpf.Option.SetOption("ConstTimeSpan", value.ToString());
                    constTimeSpan = value;
                }
            }
        }

        private string getIsReadyScanCommand = string.Empty;
        /// <summary>
        /// 获取是否可扫码信息的指令
        /// </summary>
        [DisplayName("获取是否可扫码信息的指令")]
        [Category("冷却机PLC通信")]
        public string GetIsReadyScanCommand
        {
            get
            {
                if (string.IsNullOrEmpty(getIsReadyScanCommand))
                {
                    getIsReadyScanCommand = TengDa.Wpf.Option.GetOption("GetIsReadyScanCommand");
                    if (string.IsNullOrEmpty(getIsReadyScanCommand))
                    {
                        getIsReadyScanCommand = "%01#RCP1R2000**";
                        TengDa.Wpf.Option.SetOption("GetIsReadyScanCommand", getIsReadyScanCommand, "获取是否可扫码信息的指令");
                    }
                }
                return getIsReadyScanCommand;
            }
            set
            {
                if (getIsReadyScanCommand != value)
                {
                    TengDa.Wpf.Option.SetOption("GetIsReadyScanCommand", value.ToString());
                    SetProperty(ref getIsReadyScanCommand, value);
                }
            }
        }


        private string setScanOKCommand = string.Empty;
        /// <summary>
        /// 扫码OK结果写入冷却炉PLC的指令
        /// </summary>
        [DisplayName("扫码OK结果写入冷却炉PLC的指令")]
        [Category("冷却机PLC通信")]
        public string SetScanOKCommand
        {
            get
            {
                if (string.IsNullOrEmpty(setScanOKCommand))
                {
                    setScanOKCommand = TengDa.Wpf.Option.GetOption("SetScanOKCommand");
                    if (string.IsNullOrEmpty(setScanOKCommand))
                    {
                        setScanOKCommand = "%01#WCP1R05841**";
                        TengDa.Wpf.Option.SetOption("SetScanOKCommand", setScanOKCommand, "扫码OK结果写入冷却炉PLC的指令");
                    }
                }
                return setScanOKCommand;
            }
            set
            {
                if (setScanOKCommand != value)
                {
                    TengDa.Wpf.Option.SetOption("SetScanOKCommand", value.ToString());
                    SetProperty(ref setScanOKCommand, value);
                }
            }
        }


        private string setScanNGCommand = string.Empty;
        /// <summary>
        /// 扫码NG结果写入冷却炉PLC的指令
        /// </summary>
        [DisplayName("扫码NG结果写入冷却炉PLC的指令")]
        [Category("冷却机PLC通信")]
        public string SetScanNGCommand
        {
            get
            {
                if (string.IsNullOrEmpty(setScanNGCommand))
                {
                    setScanNGCommand = TengDa.Wpf.Option.GetOption("SetScanNGCommand");
                    if (string.IsNullOrEmpty(setScanNGCommand))
                    {
                        setScanNGCommand = "%01#WCP1R05851**";
                        TengDa.Wpf.Option.SetOption("SetScanNGCommand", setScanNGCommand, "扫码NG结果写入冷却炉PLC的指令");
                    }
                }
                return setScanNGCommand;
            }
            set
            {
                if (setScanNGCommand != value)
                {
                    TengDa.Wpf.Option.SetOption("SetScanNGCommand", value.ToString());
                    SetProperty(ref setScanNGCommand, value);
                }
            }
        }


        private string triggerScanCommand = string.Empty;
        /// <summary>
        /// 触发扫码指令字符
        /// </summary>
        [DisplayName("触发扫码指令字符")]
        [Category("扫码枪")]
        public string TriggerScanCommand
        {
            get
            {
                if (string.IsNullOrEmpty(triggerScanCommand))
                {
                    triggerScanCommand = TengDa.Wpf.Option.GetOption("TriggerScanCommand");
                    if (string.IsNullOrEmpty(triggerScanCommand))
                    {
                        triggerScanCommand = "T";
                        TengDa.Wpf.Option.SetOption("TriggerScanCommand", triggerScanCommand, "触发扫码指令字符");
                    }
                }
                return triggerScanCommand;
            }
            set
            {
                if (triggerScanCommand != value)
                {
                    TengDa.Wpf.Option.SetOption("TriggerScanCommand", value.ToString());
                    SetProperty(ref triggerScanCommand, value);
                }
            }
        }


        private string batteryCodeRegularExpression = string.Empty;
        /// <summary>
        /// 电池条码正则表达式
        /// </summary>
        [DisplayName("电池条码正则表达式")]
        [Category("扫码枪")]
        public string BatteryCodeRegularExpression
        {
            get
            {
                if (string.IsNullOrEmpty(batteryCodeRegularExpression))
                {
                    batteryCodeRegularExpression = TengDa.Wpf.Option.GetOption("BatteryCodeRegularExpression");
                    if (string.IsNullOrEmpty(batteryCodeRegularExpression))
                    {
                        batteryCodeRegularExpression = "[A-Za-z0-9]{24}";
                        TengDa.Wpf.Option.SetOption("BatteryCodeRegularExpression", batteryCodeRegularExpression, "电池条码正则表达式");
                    }
                }
                return batteryCodeRegularExpression;
            }
            set
            {
                if (batteryCodeRegularExpression != value)
                {
                    TengDa.Wpf.Option.SetOption("BatteryCodeRegularExpression", value.ToString());
                    SetProperty(ref batteryCodeRegularExpression, value);
                }
            }
        }


        private string scanFailedReturnStr = string.Empty;
        /// <summary>
        /// 扫码失败返回字符串
        /// </summary>
        [DisplayName("扫码失败返回字符串")]
        [Category("扫码枪")]
        public string ScanFailedReturnStr
        {
            get
            {
                if (string.IsNullOrEmpty(scanFailedReturnStr))
                {
                    scanFailedReturnStr = TengDa.Wpf.Option.GetOption("ScanFailedReturnStr");
                    if (string.IsNullOrEmpty(scanFailedReturnStr))
                    {
                        scanFailedReturnStr = "NG";
                        TengDa.Wpf.Option.SetOption("ScanFailedReturnStr", scanFailedReturnStr, "扫码失败返回字符串");
                    }
                }
                return scanFailedReturnStr;
            }
            set
            {
                if (scanFailedReturnStr != value)
                {
                    TengDa.Wpf.Option.SetOption("ScanFailedReturnStr", value.ToString());
                    SetProperty(ref scanFailedReturnStr, value);
                }
            }
        }

        /// <summary>
        /// 当前工序编号
        /// </summary>
        [DisplayName("当前工序编号")]
        [Category("MES")]
        public string CurrentProcessCode
        {
            get => CurrentProcess.Split(',')[1];
        }

        /// <summary>
        /// 当前工位编号
        /// </summary>
        [DisplayName("当前工位编号")]
        [Category("MES")]
        public string CurrentStationCode
        {
            get => CurrentStation.Split(',')[1];
        }



        private string currentProcess = string.Empty;
        /// <summary>
        /// 当前工序名称和编号
        /// </summary>
        [DisplayName("当前工序名称和编号")]
        [Category("MES")]
        public string CurrentProcess
        {
            get
            {
                if (string.IsNullOrEmpty(currentProcess))
                {
                    currentProcess = TengDa.Wpf.Option.GetOption("CurrentProcess");
                    if (string.IsNullOrEmpty(currentProcess))
                    {
                        currentProcess = "Unknown,Unknown";
                        TengDa.Wpf.Option.SetOption("CurrentProcess", currentProcess, "当前工序名称和编号");
                    }
                }
                return currentProcess;
            }
            set
            {
                if (currentProcess != value)
                {
                    TengDa.Wpf.Option.SetOption("CurrentProcess", value.ToString());
                    SetProperty(ref currentProcess, value);
                }
            }
        }

        private string currentStation = string.Empty;
        /// <summary>
        /// 当前工位名称和编号
        /// </summary>
        [DisplayName("当前工位名称和编号")]
        [Category("MES")]
        public string CurrentStation
        {
            get
            {
                if (string.IsNullOrEmpty(currentStation))
                {
                    currentStation = TengDa.Wpf.Option.GetOption("CurrentStation");
                    if (string.IsNullOrEmpty(currentStation))
                    {
                        currentStation = "Unknown,Unknown";
                        TengDa.Wpf.Option.SetOption("CurrentStation", currentStation, "当前工位名称和编号");
                    }
                }
                return currentStation;
            }
            set
            {
                if (currentStation != value)
                {
                    TengDa.Wpf.Option.SetOption("CurrentStation", value);
                    SetProperty(ref currentStation, value);
                }
            }
        }

        private string currentOrderNo = string.Empty;
        /// <summary>
        /// 当前工单
        /// </summary>
        [DisplayName("当前工单")]
        [Category("MES")]
        public string CurrentOrderNo
        {
            get
            {
                if (string.IsNullOrEmpty(currentOrderNo))
                {
                    currentOrderNo = TengDa.Wpf.Option.GetOption("CurrentOrderNo");
                    if (string.IsNullOrEmpty(currentOrderNo))
                    {
                        currentOrderNo = "Unknown";
                        TengDa.Wpf.Option.SetOption("CurrentOrderNo", currentOrderNo, "当前工单");
                    }
                }
                return currentOrderNo;
            }
            set
            {
                if (currentOrderNo != value)
                {
                    TengDa.Wpf.Option.SetOption("CurrentOrderNo", value);
                    SetProperty(ref currentOrderNo, value);
                }
            }
        }

        private string currentMaterialOrderNo = string.Empty;
        /// <summary>
        /// 当前来料工单
        /// </summary>
        [DisplayName("当前来料工单")]
        [Category("MES")]
        public string CurrentMaterialOrderNo
        {
            get
            {
                if (string.IsNullOrEmpty(currentMaterialOrderNo))
                {
                    currentMaterialOrderNo = TengDa.Wpf.Option.GetOption("CurrentMaterialOrderNo");
                    if (string.IsNullOrEmpty(currentMaterialOrderNo))
                    {
                        currentMaterialOrderNo = "Unknown";
                        TengDa.Wpf.Option.SetOption("CurrentMaterialOrderNo", currentMaterialOrderNo, "当前来料工单");
                    }
                }
                return currentMaterialOrderNo;
            }
            set
            {
                if (currentMaterialOrderNo != value)
                {
                    TengDa.Wpf.Option.SetOption("CurrentMaterialOrderNo", value.ToString());
                    SetProperty(ref currentMaterialOrderNo, value);
                }
            }
        }


        private string iPAddressRegex = string.Empty;
        /// <summary>
        /// 局域网IP地址正则表达式
        /// </summary>
        [DisplayName("局域网IP地址正则表达式")]
        [Category("MES")]
        public string IPAddressRegex
        {
            get
            {
                if (string.IsNullOrEmpty(iPAddressRegex))
                {
                    iPAddressRegex = TengDa.Wpf.Option.GetOption("IPAddressRegex");
                    if (string.IsNullOrEmpty(iPAddressRegex))
                    {
                        iPAddressRegex = "192.168.*";
                        TengDa.Wpf.Option.SetOption("IPAddressRegex", iPAddressRegex, "局域网IP地址正则表达式");
                    }
                }
                return iPAddressRegex;
            }
            set
            {
                if (iPAddressRegex != value)
                {
                    TengDa.Wpf.Option.SetOption("IPAddressRegex", value.ToString());
                    SetProperty(ref iPAddressRegex, value);
                }
            }
        }


        private float thresholdResistance = -1;
        /// <summary>
        /// 电阻设定值
        /// </summary>
        [DisplayName("电阻设定值")]
        [Description("电阻设定值，若测得电阻大于该值，则判断为合格")]
        public float ThresholdResistance
        {
            get
            {
                if (thresholdResistance < 0)
                {
                    thresholdResistance = _Convert.StrToInt(TengDa.Wpf.Option.GetOption("ThresholdResistance"), -1);
                    if (thresholdResistance < 0)
                    {
                        thresholdResistance = 1;
                        TengDa.Wpf.Option.SetOption("ThresholdResistance", thresholdResistance.ToString(), "电阻设定值");
                    }
                }
                return thresholdResistance;
            }
            set
            {
                if (thresholdResistance != value)
                {
                    TengDa.Wpf.Option.SetOption("ThresholdResistance", value.ToString());
                    SetProperty(ref thresholdResistance, value);
                }
            }
        }

        private float thresholdTemperature = -1;
        /// <summary>
        /// 温度设定值
        /// </summary>
        [DisplayName("温度设定值")]
        [Description("温度设定值，若测得温度小于该值，则判断为合格")]
        public float ThresholdTemperature
        {
            get
            {
                if (thresholdTemperature < 0)
                {
                    thresholdTemperature = _Convert.StrToInt(TengDa.Wpf.Option.GetOption("ThresholdTemperature"), -1);
                    if (thresholdTemperature < 0)
                    {
                        thresholdTemperature = 200;
                        TengDa.Wpf.Option.SetOption("ThresholdTemperature", thresholdTemperature.ToString(), "温度设定值");
                    }
                }
                return thresholdTemperature;
            }
            set
            {
                if (thresholdTemperature != value)
                {
                    TengDa.Wpf.Option.SetOption("ThresholdTemperature", value.ToString());
                    SetProperty(ref thresholdTemperature, value);
                }
            }
        }


    }
}
