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
