using System.ComponentModel;
using TengDa;
using TengDa.Wpf;

namespace Tafel.Hipot.App
{
    /// <summary>
    /// 基本配置
    /// </summary>
    [DisplayName("基本配置")]
    public class AppOption : BindableObject
    {


        private int checkTesterInfoInterval = -1;
        /// <summary>
        /// 检查测试仪信息的时间间隔
        /// </summary>
        [DisplayName("检查测试仪信息的时间间隔")]
        [Description("检查测试仪信息的时间间隔, 单位（ms）")]
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
                        TengDa.Wpf.Option.SetOption("CheckTesterInfoInterval", checkTesterInfoInterval.ToString(), "检查测试仪信息的时间间隔");
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

        private int checkMesInfoInterval = -1;
        /// <summary>
        /// 检查数据并上传MES的时间间隔
        /// </summary>
        [DisplayName("检查数据并上传MES的时间间隔")]
        [Description("检查数据并上传MES的时间间隔, 单位（s）")]
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
                        TengDa.Wpf.Option.SetOption("CheckMesInfoInterval", checkMesInfoInterval.ToString(), "检查数据并上传MES的时间间隔");
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


        /// <summary>
        /// 当前工序编号
        /// </summary>
        public string CurrentProcessCode
        {
            get => CurrentProcess.Split(',')[1];
        }

        /// <summary>
        /// 当前工位编号
        /// </summary>
        public string CurrentStationCode
        {
            get => CurrentStation.Split(',')[1];
        }



        private string currentProcess = string.Empty;
        /// <summary>
        /// 当前工序名称和编号
        /// </summary>
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
        public string CurrentOrderNo
        {
            get
            {
                if (string.IsNullOrEmpty(currentOrderNo))
                {
                    currentOrderNo = TengDa.Wpf.Option.GetOption("CurrentOrderNo");
                    if (string.IsNullOrEmpty(currentOrderNo))
                    {
                        currentOrderNo = "Known";
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


        //private int xxxxxx = -1;
        ///// <summary>
        ///// ZZZZZZZZZ
        ///// </summary>
        //[DisplayName("ZZZZZZZZZ")]
        //[Category("通信")]
        //public int YYYYYYYYY
        //{
        //  get
        //  {
        //    if (xxxxxx < 0)
        //    {
        //      xxxxxx = _Convert.StrToInt(TengDa.Wpf.Option.GetOption("YYYYYYYYY"), -1);
        //      if (xxxxxx < 0)
        //      {
        //        xxxxxx = 1000;
        //        TengDa.Wpf.Option.SetOption("YYYYYYYYY", xxxxxx.ToString(), "ZZZZZZZZZ");
        //      }
        //    }
        //    return xxxxxx;
        //  }
        //  set
        //  {
        //    if (xxxxxx != value)
        //    {
        //      TengDa.Wpf.Option.SetOption("YYYYYYYYY", value.ToString());
        //      SetProperty(ref xxxxxx, value);
        //    }
        //  }
        //}

    }
}
