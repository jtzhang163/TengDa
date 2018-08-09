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
