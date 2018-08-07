using System.ComponentModel;
using TengDa;
using TengDa.Wpf;

namespace Zopoise.Scada.App
{
    /// <summary>
    /// 基本配置
    /// </summary>
    [DisplayName("基本配置")]
    public class AppOption : BindableObject
    {


        private int communicatorCommunicateInterval = -1;
        /// <summary>
        /// 通信器通信间隔
        /// </summary>
        [DisplayName("通信器通信间隔")]
        [Description("通信器通信间隔, 单位（ms）")]
        [Category("通信")]
        public int CommunicatorCommunicateInterval
        {
            get
            {
                if (communicatorCommunicateInterval < 0)
                {
                    communicatorCommunicateInterval = _Convert.StrToInt(TengDa.Wpf.Option.GetOption("CommunicatorCommunicateInterval"), -1);
                    if (communicatorCommunicateInterval < 0)
                    {
                        communicatorCommunicateInterval = 1000;
                        TengDa.Wpf.Option.SetOption("CommunicatorCommunicateInterval", communicatorCommunicateInterval.ToString(), "通信器通信间隔");
                    }
                }
                return communicatorCommunicateInterval;
            }
            set
            {
                if (communicatorCommunicateInterval != value)
                {
                    TengDa.Wpf.Option.SetOption("CommunicatorCommunicateInterval", value.ToString());
                    SetProperty(ref communicatorCommunicateInterval, value);
                }
            }
        }


        private int plcCommunicateInterval = -1;
        /// <summary>
        /// PLC通信间隔
        /// </summary>
        [DisplayName("PLC通信间隔")]
        [Description("PLC通信间隔, 单位（ms）")]
        [Category("通信")]
        public int PlcCommunicateInterval
        {
            get
            {
                if (plcCommunicateInterval < 0)
                {
                    plcCommunicateInterval = _Convert.StrToInt(TengDa.Wpf.Option.GetOption("PlcCommunicateInterval"), -1);
                    if (plcCommunicateInterval < 0)
                    {
                        plcCommunicateInterval = 1000;
                        TengDa.Wpf.Option.SetOption("PlcCommunicateInterval", plcCommunicateInterval.ToString(), "PLC通信间隔");
                    }
                }
                return plcCommunicateInterval;
            }
            set
            {
                if (plcCommunicateInterval != value)
                {
                    TengDa.Wpf.Option.SetOption("PlcCommunicateInterval", value.ToString());
                    SetProperty(ref plcCommunicateInterval, value);
                }
            }
        }




        private int selectTesterIndex = -1;
        /// <summary>
        /// 选择显示曲线的工装板索引
        /// </summary>
        [Browsable(false)]
        public int SelectTesterIndex
        {
            get
            {
                if (selectTesterIndex < 0)
                {
                    selectTesterIndex = _Convert.StrToInt(TengDa.Wpf.Option.GetOption("SelectTesterIndex"), -1);
                    if (selectTesterIndex < 0)
                    {
                        selectTesterIndex = 0;
                        TengDa.Wpf.Option.SetOption("SelectTesterIndex", selectTesterIndex.ToString(), "选择显示曲线的工装板索引");
                    }
                }
                return selectTesterIndex;
            }
            set
            {
                if (selectTesterIndex != value)
                {
                    TengDa.Wpf.Option.SetOption("SelectTesterIndex", value.ToString());
                    SetProperty(ref selectTesterIndex, value);
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

        //private string xxxxxx = string.Empty;
        ///// <summary>
        ///// ZZZZZZZZZ
        ///// </summary>
        //[DisplayName("ZZZZZZZZZ")]
        //[Category("配置")]
        //public string YYYYYYYYY
        //{
        //  get
        //  {
        //    if (string.IsNullOrEmpty(xxxxxx))
        //    {
        //      xxxxxx = TengDa.Wpf.Option.GetOption("YYYYYYYYY");
        //      if (xxxxxx == string.Empty)
        //      {
        //        xxxxxx = "众普森灯具老化线数据监控采集系统";
        //        TengDa.Wpf.Option.SetOption("YYYYYYYYY", xxxxxx, "ZZZZZZZZZ");
        //      }
        //    }
        //    return xxxxxx;
        //  }
        //  set
        //  {
        //    if (xxxxxx != value)
        //    {
        //      TengDa.Wpf.Option.SetOption("YYYYYYYYY", value);
        //      SetProperty(ref xxxxxx, value);
        //    }
        //  }
        //}


        //private string xxxxxx = string.Empty;
        ///// <summary>
        ///// ZZZZZZZZZ
        ///// </summary>
        //[DisplayName("ZZZZZZZZZ")]
        //[Category("配置")]
        //public string YYYYYYYYY
        //{
        //  get
        //  {
        //    if (string.IsNullOrEmpty(xxxxxx))
        //    {
        //      xxxxxx = TengDa.Wpf.Option.GetOption("YYYYYYYYY");
        //      if (xxxxxx == string.Empty)
        //      {
        //        xxxxxx = "众普森灯具老化线数据监控采集系统";
        //        TengDa.Wpf.Option.SetOption("YYYYYYYYY", xxxxxx, "ZZZZZZZZZ");
        //      }
        //    }
        //    return xxxxxx;
        //  }
        //  set
        //  {
        //    if (xxxxxx != value)
        //    {
        //      TengDa.Wpf.Option.SetOption("YYYYYYYYY", value);
        //      SetProperty(ref xxxxxx, value);
        //    }
        //  }
        //}


        //private string xxxxxx = string.Empty;
        ///// <summary>
        ///// ZZZZZZZZZ
        ///// </summary>
        //[DisplayName("ZZZZZZZZZ")]
        //[Category("配置")]
        //public string YYYYYYYYY
        //{
        //  get
        //  {
        //    if (string.IsNullOrEmpty(xxxxxx))
        //    {
        //      xxxxxx = TengDa.Wpf.Option.GetOption("YYYYYYYYY");
        //      if (xxxxxx == string.Empty)
        //      {
        //        xxxxxx = "众普森灯具老化线数据监控采集系统";
        //        TengDa.Wpf.Option.SetOption("YYYYYYYYY", xxxxxx, "ZZZZZZZZZ");
        //      }
        //    }
        //    return xxxxxx;
        //  }
        //  set
        //  {
        //    if (xxxxxx != value)
        //    {
        //      TengDa.Wpf.Option.SetOption("YYYYYYYYY", value);
        //      SetProperty(ref xxxxxx, value);
        //    }
        //  }
        //}


        //private string xxxxxx = string.Empty;
        ///// <summary>
        ///// ZZZZZZZZZ
        ///// </summary>
        //[DisplayName("ZZZZZZZZZ")]
        //[Category("配置")]
        //public string YYYYYYYYY
        //{
        //  get
        //  {
        //    if (string.IsNullOrEmpty(xxxxxx))
        //    {
        //      xxxxxx = TengDa.Wpf.Option.GetOption("YYYYYYYYY");
        //      if (xxxxxx == string.Empty)
        //      {
        //        xxxxxx = "众普森灯具老化线数据监控采集系统";
        //        TengDa.Wpf.Option.SetOption("YYYYYYYYY", xxxxxx, "ZZZZZZZZZ");
        //      }
        //    }
        //    return xxxxxx;
        //  }
        //  set
        //  {
        //    if (xxxxxx != value)
        //    {
        //      TengDa.Wpf.Option.SetOption("YYYYYYYYY", value);
        //      SetProperty(ref xxxxxx, value);
        //    }
        //  }
        //}


        //private string xxxxxx = string.Empty;
        ///// <summary>
        ///// ZZZZZZZZZ
        ///// </summary>
        //[DisplayName("ZZZZZZZZZ")]
        //[Category("配置")]
        //public string YYYYYYYYY
        //{
        //  get
        //  {
        //    if (string.IsNullOrEmpty(xxxxxx))
        //    {
        //      xxxxxx = TengDa.Wpf.Option.GetOption("YYYYYYYYY");
        //      if (xxxxxx == string.Empty)
        //      {
        //        xxxxxx = "众普森灯具老化线数据监控采集系统";
        //        TengDa.Wpf.Option.SetOption("YYYYYYYYY", xxxxxx, "ZZZZZZZZZ");
        //      }
        //    }
        //    return xxxxxx;
        //  }
        //  set
        //  {
        //    if (xxxxxx != value)
        //    {
        //      TengDa.Wpf.Option.SetOption("YYYYYYYYY", value);
        //      SetProperty(ref xxxxxx, value);
        //    }
        //  }
        //}

        //private string xxxxxx = string.Empty;
        ///// <summary>
        ///// ZZZZZZZZZ
        ///// </summary>
        //[DisplayName("ZZZZZZZZZ")]
        //[Category("配置")]
        //public string YYYYYYYYY
        //{
        //  get
        //  {
        //    if (string.IsNullOrEmpty(xxxxxx))
        //    {
        //      xxxxxx = TengDa.Wpf.Option.GetOption("YYYYYYYYY");
        //      if (xxxxxx == string.Empty)
        //      {
        //        xxxxxx = "众普森灯具老化线数据监控采集系统";
        //        TengDa.Wpf.Option.SetOption("YYYYYYYYY", xxxxxx, "ZZZZZZZZZ");
        //      }
        //    }
        //    return xxxxxx;
        //  }
        //  set
        //  {
        //    if (xxxxxx != value)
        //    {
        //      TengDa.Wpf.Option.SetOption("YYYYYYYYY", value);
        //      SetProperty(ref xxxxxx, value);
        //    }
        //  }
        //}


        //private string xxxxxx = string.Empty;
        ///// <summary>
        ///// ZZZZZZZZZ
        ///// </summary>
        //[DisplayName("ZZZZZZZZZ")]
        //[Category("配置")]
        //public string YYYYYYYYY
        //{
        //  get
        //  {
        //    if (string.IsNullOrEmpty(xxxxxx))
        //    {
        //      xxxxxx = TengDa.Wpf.Option.GetOption("YYYYYYYYY");
        //      if (xxxxxx == string.Empty)
        //      {
        //        xxxxxx = "众普森灯具老化线数据监控采集系统";
        //        TengDa.Wpf.Option.SetOption("YYYYYYYYY", xxxxxx, "ZZZZZZZZZ");
        //      }
        //    }
        //    return xxxxxx;
        //  }
        //  set
        //  {
        //    if (xxxxxx != value)
        //    {
        //      TengDa.Wpf.Option.SetOption("YYYYYYYYY", value);
        //      SetProperty(ref xxxxxx, value);
        //    }
        //  }
        //}


        //private string xxxxxx = string.Empty;
        ///// <summary>
        ///// ZZZZZZZZZ
        ///// </summary>
        //[DisplayName("ZZZZZZZZZ")]
        //[Category("配置")]
        //public string YYYYYYYYY
        //{
        //  get
        //  {
        //    if (string.IsNullOrEmpty(xxxxxx))
        //    {
        //      xxxxxx = TengDa.Wpf.Option.GetOption("YYYYYYYYY");
        //      if (xxxxxx == string.Empty)
        //      {
        //        xxxxxx = "众普森灯具老化线数据监控采集系统";
        //        TengDa.Wpf.Option.SetOption("YYYYYYYYY", xxxxxx, "ZZZZZZZZZ");
        //      }
        //    }
        //    return xxxxxx;
        //  }
        //  set
        //  {
        //    if (xxxxxx != value)
        //    {
        //      TengDa.Wpf.Option.SetOption("YYYYYYYYY", value);
        //      SetProperty(ref xxxxxx, value);
        //    }
        //  }
        //}

        //private string xxxxxx = string.Empty;
        ///// <summary>
        ///// ZZZZZZZZZ
        ///// </summary>
        //[DisplayName("ZZZZZZZZZ")]
        //[Category("配置")]
        //public string YYYYYYYYY
        //{
        //  get
        //  {
        //    if (string.IsNullOrEmpty(xxxxxx))
        //    {
        //      xxxxxx = TengDa.Wpf.Option.GetOption("YYYYYYYYY");
        //      if (xxxxxx == string.Empty)
        //      {
        //        xxxxxx = "众普森灯具老化线数据监控采集系统";
        //        TengDa.Wpf.Option.SetOption("YYYYYYYYY", xxxxxx, "ZZZZZZZZZ");
        //      }
        //    }
        //    return xxxxxx;
        //  }
        //  set
        //  {
        //    if (xxxxxx != value)
        //    {
        //      TengDa.Wpf.Option.SetOption("YYYYYYYYY", value);
        //      SetProperty(ref xxxxxx, value);
        //    }
        //  }
        //}

        //private string xxxxxx = string.Empty;
        ///// <summary>
        ///// ZZZZZZZZZ
        ///// </summary>
        //[DisplayName("ZZZZZZZZZ")]
        //[Category("配置")]
        //public string YYYYYYYYY
        //{
        //  get
        //  {
        //    if (string.IsNullOrEmpty(xxxxxx))
        //    {
        //      xxxxxx = TengDa.Wpf.Option.GetOption("YYYYYYYYY");
        //      if (xxxxxx == string.Empty)
        //      {
        //        xxxxxx = "众普森灯具老化线数据监控采集系统";
        //        TengDa.Wpf.Option.SetOption("YYYYYYYYY", xxxxxx, "ZZZZZZZZZ");
        //      }
        //    }
        //    return xxxxxx;
        //  }
        //  set
        //  {
        //    if (xxxxxx != value)
        //    {
        //      TengDa.Wpf.Option.SetOption("YYYYYYYYY", value);
        //      SetProperty(ref xxxxxx, value);
        //    }
        //  }
        //}

        //private string xxxxxx = string.Empty;
        ///// <summary>
        ///// ZZZZZZZZZ
        ///// </summary>
        //[DisplayName("ZZZZZZZZZ")]
        //[Category("配置")]
        //public string YYYYYYYYY
        //{
        //  get
        //  {
        //    if (string.IsNullOrEmpty(xxxxxx))
        //    {
        //      xxxxxx = TengDa.Wpf.Option.GetOption("YYYYYYYYY");
        //      if (xxxxxx == string.Empty)
        //      {
        //        xxxxxx = "众普森灯具老化线数据监控采集系统";
        //        TengDa.Wpf.Option.SetOption("YYYYYYYYY", xxxxxx, "ZZZZZZZZZ");
        //      }
        //    }
        //    return xxxxxx;
        //  }
        //  set
        //  {
        //    if (xxxxxx != value)
        //    {
        //      TengDa.Wpf.Option.SetOption("YYYYYYYYY", value);
        //      SetProperty(ref xxxxxx, value);
        //    }
        //  }
        //}

        //private string xxxxxx = string.Empty;
        ///// <summary>
        ///// ZZZZZZZZZ
        ///// </summary>
        //[DisplayName("ZZZZZZZZZ")]
        //[Category("配置")]
        //public string YYYYYYYYY
        //{
        //  get
        //  {
        //    if (string.IsNullOrEmpty(xxxxxx))
        //    {
        //      xxxxxx = TengDa.Wpf.Option.GetOption("YYYYYYYYY");
        //      if (xxxxxx == string.Empty)
        //      {
        //        xxxxxx = "众普森灯具老化线数据监控采集系统";
        //        TengDa.Wpf.Option.SetOption("YYYYYYYYY", xxxxxx, "ZZZZZZZZZ");
        //      }
        //    }
        //    return xxxxxx;
        //  }
        //  set
        //  {
        //    if (xxxxxx != value)
        //    {
        //      TengDa.Wpf.Option.SetOption("YYYYYYYYY", value);
        //      SetProperty(ref xxxxxx, value);
        //    }
        //  }
        //}

        //private string xxxxxx = string.Empty;
        ///// <summary>
        ///// ZZZZZZZZZ
        ///// </summary>
        //[DisplayName("ZZZZZZZZZ")]
        //[Category("配置")]
        //public string YYYYYYYYY
        //{
        //  get
        //  {
        //    if (string.IsNullOrEmpty(xxxxxx))
        //    {
        //      xxxxxx = TengDa.Wpf.Option.GetOption("YYYYYYYYY");
        //      if (xxxxxx == string.Empty)
        //      {
        //        xxxxxx = "众普森灯具老化线数据监控采集系统";
        //        TengDa.Wpf.Option.SetOption("YYYYYYYYY", xxxxxx, "ZZZZZZZZZ");
        //      }
        //    }
        //    return xxxxxx;
        //  }
        //  set
        //  {
        //    if (xxxxxx != value)
        //    {
        //      TengDa.Wpf.Option.SetOption("YYYYYYYYY", value);
        //      SetProperty(ref xxxxxx, value);
        //    }
        //  }
        //}

        //private string xxxxxx = string.Empty;
        ///// <summary>
        ///// ZZZZZZZZZ
        ///// </summary>
        //[DisplayName("ZZZZZZZZZ")]
        //[Category("配置")]
        //public string YYYYYYYYY
        //{
        //  get
        //  {
        //    if (string.IsNullOrEmpty(xxxxxx))
        //    {
        //      xxxxxx = TengDa.Wpf.Option.GetOption("YYYYYYYYY");
        //      if (xxxxxx == string.Empty)
        //      {
        //        xxxxxx = "众普森灯具老化线数据监控采集系统";
        //        TengDa.Wpf.Option.SetOption("YYYYYYYYY", xxxxxx, "ZZZZZZZZZ");
        //      }
        //    }
        //    return xxxxxx;
        //  }
        //  set
        //  {
        //    if (xxxxxx != value)
        //    {
        //      TengDa.Wpf.Option.SetOption("YYYYYYYYY", value);
        //      SetProperty(ref xxxxxx, value);
        //    }
        //  }
        //}

        //private string xxxxxx = string.Empty;
        ///// <summary>
        ///// ZZZZZZZZZ
        ///// </summary>
        //[DisplayName("ZZZZZZZZZ")]
        //[Category("配置")]
        //public string YYYYYYYYY
        //{
        //  get
        //  {
        //    if (string.IsNullOrEmpty(xxxxxx))
        //    {
        //      xxxxxx = TengDa.Wpf.Option.GetOption("YYYYYYYYY");
        //      if (xxxxxx == string.Empty)
        //      {
        //        xxxxxx = "众普森灯具老化线数据监控采集系统";
        //        TengDa.Wpf.Option.SetOption("YYYYYYYYY", xxxxxx, "ZZZZZZZZZ");
        //      }
        //    }
        //    return xxxxxx;
        //  }
        //  set
        //  {
        //    if (xxxxxx != value)
        //    {
        //      TengDa.Wpf.Option.SetOption("YYYYYYYYY", value);
        //      SetProperty(ref xxxxxx, value);
        //    }
        //  }
        //}


        //private string xxxxxx = string.Empty;
        ///// <summary>
        ///// ZZZZZZZZZ
        ///// </summary>
        //[DisplayName("ZZZZZZZZZ")]
        //[Category("配置")]
        //public string YYYYYYYYY
        //{
        //  get
        //  {
        //    if (string.IsNullOrEmpty(xxxxxx))
        //    {
        //      xxxxxx = TengDa.Wpf.Option.GetOption("YYYYYYYYY");
        //      if (xxxxxx == string.Empty)
        //      {
        //        xxxxxx = "众普森灯具老化线数据监控采集系统";
        //        TengDa.Wpf.Option.SetOption("YYYYYYYYY", xxxxxx, "ZZZZZZZZZ");
        //      }
        //    }
        //    return xxxxxx;
        //  }
        //  set
        //  {
        //    if (xxxxxx != value)
        //    {
        //      TengDa.Wpf.Option.SetOption("YYYYYYYYY", value);
        //      SetProperty(ref xxxxxx, value);
        //    }
        //  }
        //}

        //private string xxxxxx = string.Empty;
        ///// <summary>
        ///// ZZZZZZZZZ
        ///// </summary>
        //[DisplayName("ZZZZZZZZZ")]
        //[Category("配置")]
        //public string YYYYYYYYY
        //{
        //  get
        //  {
        //    if (string.IsNullOrEmpty(xxxxxx))
        //    {
        //      xxxxxx = TengDa.Wpf.Option.GetOption("YYYYYYYYY");
        //      if (xxxxxx == string.Empty)
        //      {
        //        xxxxxx = "众普森灯具老化线数据监控采集系统";
        //        TengDa.Wpf.Option.SetOption("YYYYYYYYY", xxxxxx, "ZZZZZZZZZ");
        //      }
        //    }
        //    return xxxxxx;
        //  }
        //  set
        //  {
        //    if (xxxxxx != value)
        //    {
        //      TengDa.Wpf.Option.SetOption("YYYYYYYYY", value);
        //      SetProperty(ref xxxxxx, value);
        //    }
        //  }
        //}

        //private string xxxxxx = string.Empty;
        ///// <summary>
        ///// ZZZZZZZZZ
        ///// </summary>
        //[DisplayName("ZZZZZZZZZ")]
        //[Category("配置")]
        //public string YYYYYYYYY
        //{
        //  get
        //  {
        //    if (string.IsNullOrEmpty(xxxxxx))
        //    {
        //      xxxxxx = TengDa.Wpf.Option.GetOption("YYYYYYYYY");
        //      if (xxxxxx == string.Empty)
        //      {
        //        xxxxxx = "众普森灯具老化线数据监控采集系统";
        //        TengDa.Wpf.Option.SetOption("YYYYYYYYY", xxxxxx, "ZZZZZZZZZ");
        //      }
        //    }
        //    return xxxxxx;
        //  }
        //  set
        //  {
        //    if (xxxxxx != value)
        //    {
        //      TengDa.Wpf.Option.SetOption("YYYYYYYYY", value);
        //      SetProperty(ref xxxxxx, value);
        //    }
        //  }
        //}

        //private string xxxxxx = string.Empty;
        ///// <summary>
        ///// ZZZZZZZZZ
        ///// </summary>
        //[DisplayName("ZZZZZZZZZ")]
        //[Category("配置")]
        //public string YYYYYYYYY
        //{
        //  get
        //  {
        //    if (string.IsNullOrEmpty(xxxxxx))
        //    {
        //      xxxxxx = TengDa.Wpf.Option.GetOption("YYYYYYYYY");
        //      if (xxxxxx == string.Empty)
        //      {
        //        xxxxxx = "众普森灯具老化线数据监控采集系统";
        //        TengDa.Wpf.Option.SetOption("YYYYYYYYY", xxxxxx, "ZZZZZZZZZ");
        //      }
        //    }
        //    return xxxxxx;
        //  }
        //  set
        //  {
        //    if (xxxxxx != value)
        //    {
        //      TengDa.Wpf.Option.SetOption("YYYYYYYYY", value);
        //      SetProperty(ref xxxxxx, value);
        //    }
        //  }
        //}

    }
}
