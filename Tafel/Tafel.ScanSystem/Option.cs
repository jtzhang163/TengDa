using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using TengDa;

namespace Tafel.ScanSystem
{
    public class Option
    {

        private string appName = string.Empty;
        /// <summary>
        /// 程序名称
        /// </summary>
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

        private string checkPlcPeriod = string.Empty;
        /// <summary>
        /// 检测PLC状态周期，单位：毫秒
        /// </summary>
        [Description("检测PLC状态周期，单位：毫秒")]
        [DisplayName("检测PLC状态周期")]
        [Category("PLC通讯")]
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

        private string uploadMesInterval = string.Empty;
        /// <summary>
        /// MES上传数据间隔时间，单位：毫秒
        /// </summary>
        [Description("MES上传数据间隔时间，单位：毫秒")]
        [DisplayName("MES上传数据间隔时间")]
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

        public int rememberUserId = -2;


        private string toScanStr = string.Empty;
        /// <summary>
        /// 触发扫码枪扫描字符
        /// </summary>
        [Description("触发扫码枪扫描字符")]
        [DisplayName("触发扫码枪扫描字符")]
        [Category("扫码枪")]
        public string ToScanStr
        {
            get
            {
                if (string.IsNullOrEmpty(toScanStr))
                {
                    toScanStr = TengDa.WF.Option.GetOption("ToScanStr");
                }
                return toScanStr;
            }
            set
            {
                if (value != toScanStr)
                {
                    TengDa.WF.Option.SetOption("ToScanStr", value);
                    toScanStr = value;
                }
            }
        }

        private string clampCodeStr = string.Empty;
        /// <summary>
        /// 夹具条码正则表达式
        /// </summary>
        [Description("夹具条码正则表达式")]
        [DisplayName("夹具条码正则表达式")]
        public string ClampCodeStr
        {
            get
            {
                if (string.IsNullOrEmpty(clampCodeStr))
                {
                    clampCodeStr = TengDa.WF.Option.GetOption("ClampCodeStr");
                }
                return clampCodeStr;
            }
            set
            {
                if (value != clampCodeStr)
                {
                    TengDa.WF.Option.SetOption("ClampCodeStr", value);
                    clampCodeStr = value;
                }
            }
        }

        private string codeStr = string.Empty;
        /// <summary>
        /// 条码正则表达式
        /// </summary>
        [Description("条码正则表达式")]
        [DisplayName("条码正则表达式")]
        [Category("扫码枪")]
        public string CodeStr
        {
            get
            {
                if (string.IsNullOrEmpty(codeStr))
                {
                    codeStr = TengDa.WF.Option.GetOption("CodeStr");
                }
                return codeStr;
            }
            set
            {
                if (value != codeStr)
                {
                    TengDa.WF.Option.SetOption("CodeStr", value);
                    codeStr = value;
                }
            }
        }

        private string ngStr = string.Empty;
        /// <summary>
        /// NG正则表达式
        /// </summary>
        [Description("扫描失败扫码枪返回的字符串表达式")]
        [DisplayName("NG正则表达式")]
        [Category("扫码枪")]
        public string NgStr
        {
            get
            {
                if (string.IsNullOrEmpty(ngStr))
                {
                    ngStr = TengDa.WF.Option.GetOption("NgStr");
                }
                return ngStr;
            }
            set
            {
                if (value != ngStr)
                {
                    TengDa.WF.Option.SetOption("NgStr", value);
                    ngStr = value;
                }
            }
        }

        private string scanIntervalTime = string.Empty;
        /// <summary>
        /// 发送指令到扫码枪到接收数据的时间间隔，单位：毫秒
        /// </summary>
        [Description("发送指令到扫码枪到接收数据的时间间隔，单位：毫秒")]
        [DisplayName("发送指令到扫码枪到接收数据的时间间隔")]
        [Category("扫码枪")]
        public string ScanIntervalTime
        {
            get
            {
                if (string.IsNullOrEmpty(scanIntervalTime))
                {
                    scanIntervalTime = TengDa.WF.Option.GetOption("ScanIntervalTime");
                }
                return scanIntervalTime;
            }
            set
            {
                if (value != scanIntervalTime)
                {
                    TengDa.WF.Option.SetOption("ScanIntervalTime", value);
                    scanIntervalTime = value;
                }
            }
        }
        private string jawPosition = string.Empty;
        /// <summary>
        /// 夹爪位置（需实时保存）
        /// </summary>
        [Description("夹爪位置（需实时保存）")]
        [DisplayName("夹爪位置")]
        public string JawLocation
        {
            get
            {
                if (string.IsNullOrEmpty(jawPosition))
                {
                    jawPosition = TengDa.WF.Option.GetOption("JawLocation");
                }
                return jawPosition;
            }
            set
            {
                if (value != jawPosition)
                {
                    TengDa.WF.Option.SetOption("JawLocation", value);
                    jawPosition = value;
                }
            }
        }

        private string getInfoStr = string.Empty;
        /// <summary>
        /// 获取信息（电池是否到位，工位是否禁用）时发送给PLC的指令
        /// </summary>
        [Description("获取信息（电池是否到位，工位是否禁用）时发送给PLC的指令")]
        [DisplayName("获取信息（电池是否到位，工位是否禁用）指令")]
        [Category("PLC通讯")]
        public string GetInfoStr
        {
            get
            {
                if (string.IsNullOrEmpty(getInfoStr))
                {
                    getInfoStr = TengDa.WF.Option.GetOption("GetInfoStr");
                }
                return getInfoStr;
            }
            set
            {
                if (value != getInfoStr)
                {
                    TengDa.WF.Option.SetOption("GetInfoStr", value);
                    getInfoStr = value;
                }
            }
        }


        private string sendScanOkStr = string.Empty;
        /// <summary>
        /// 扫码OK反馈给PLC的指令
        /// </summary>
        [Description("扫码OK反馈给PLC的指令")]
        [DisplayName("扫码OK反馈给PLC的指令")]
        [Category("PLC通讯")]
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
        [Category("PLC通讯")]
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



        private string currentProcess = string.Empty;
        /// <summary>
        /// 当前工序名称和编号
        /// </summary>
        [Description("当前工序名称和编号")]
        [DisplayName("当前工序名称和编号")]
        public string CurrentProcess
        {
            get
            {
                if (string.IsNullOrEmpty(currentProcess))
                {
                    currentProcess = TengDa.WF.Option.GetOption("CurrentProcess");
                }
                return currentProcess;
            }
            set
            {
                if (value != currentProcess)
                {
                    TengDa.WF.Option.SetOption("CurrentProcess", value);
                    currentProcess = value;
                }
            }
        }

        private string currentStation = string.Empty;
        /// <summary>
        /// 当前工位名称和编号
        /// </summary>
        [Description("当前工位名称和编号")]
        [DisplayName("当前工位名称和编号")]
        public string CurrentStation
        {
            get
            {
                if (string.IsNullOrEmpty(currentStation))
                {
                    currentStation = TengDa.WF.Option.GetOption("CurrentStation");
                }
                return currentStation;
            }
            set
            {
                if (value != currentStation)
                {
                    TengDa.WF.Option.SetOption("CurrentStation", value);
                    currentStation = value;
                }
            }
        }


        /// <summary>
        /// 当前工序编号
        /// </summary>
        [Description("当前工序编号")]
        [DisplayName("当前工序编号")]
        public string CurrentProcessCode
        {
            get
            {
                return CurrentProcess.Split(',')[1];
            }
        }

        /// <summary>
        /// 当前工位编号
        /// </summary>
        [Description("当前工位编号")]
        [DisplayName("当前工位编号")]
        public string CurrentStationCode
        {
            get
            {
                return CurrentStation.Split(',')[1];
            }
        }


        private string currentOrderNo = string.Empty;
        /// <summary>
        /// 当前工单
        /// </summary>
        [Description("当前工单")]
        [DisplayName("当前工单")]
        public string CurrentOrderNo
        {
            get
            {
                if (string.IsNullOrEmpty(currentOrderNo))
                {
                    currentOrderNo = TengDa.WF.Option.GetOption("CurrentOrderNo");
                }
                return currentOrderNo;
            }
            set
            {
                if (value != currentOrderNo)
                {
                    TengDa.WF.Option.SetOption("CurrentOrderNo", value);
                    currentOrderNo = value;
                }
            }
        }



        private string currentMaterialOrderNo = string.Empty;
        /// <summary>
        /// 当前来料工单
        /// </summary>
        [Description("当前来料工单")]
        [DisplayName("当前来料工单")]
        public string CurrentMaterialOrderNo
        {
            get
            {
                if (string.IsNullOrEmpty(currentMaterialOrderNo))
                {
                    currentMaterialOrderNo = TengDa.WF.Option.GetOption("CurrentMaterialOrderNo");
                }
                return currentMaterialOrderNo;
            }
            set
            {
                if (value != currentMaterialOrderNo)
                {
                    TengDa.WF.Option.SetOption("CurrentMaterialOrderNo", value);
                    currentMaterialOrderNo = value;
                }
            }
        }


        private string tbClampCodes = string.Empty;
        /// <summary>
        /// 当前工位两个夹具的条码
        /// </summary>
        [Description("当前工位两个夹具的条码")]
        [DisplayName("当前工位两个夹具的条码")]
        public string TbClampCodes
        {
            get
            {
                if (string.IsNullOrEmpty(tbClampCodes))
                {
                    tbClampCodes = TengDa.WF.Option.GetOption("TbClampCodes");
                }
                return tbClampCodes;
            }
            set
            {
                if (value != tbClampCodes)
                {
                    TengDa.WF.Option.SetOption("TbClampCodes", value);
                    tbClampCodes = value;
                }
            }
        }
        private string scanNgCount = string.Empty;
        /// <summary>
        /// 扫描NG总次数设置
        /// </summary>
        [Description("扫描NG总次数设置，当扫描NG总次数达到该值时，将结果反馈该PLC报警提示")]
        [DisplayName("扫描NG总次数设置")]
        public int ScanNgCount
        {
            get
            {
                if (string.IsNullOrEmpty(scanNgCount))
                {
                    scanNgCount = TengDa.WF.Option.GetOption("ScanNgCount");
                }
                return TengDa._Convert.StrToInt(scanNgCount, 1);
            }
            set
            {
                if (value.ToString() != scanNgCount)
                {
                    TengDa.WF.Option.SetOption("ScanNgCount", value.ToString());
                    scanNgCount = value.ToString();
                }
            }
        }
    }
}
