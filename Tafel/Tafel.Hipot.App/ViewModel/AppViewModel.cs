using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using TengDa;
using TengDa.Wpf;

namespace Tafel.Hipot.App
{
    /// <summary>
    /// 应用程序ViewModel
    /// </summary>
    [DisplayName("应用程序视图模型")]
    public class AppViewModel : BindableObject
    {

        private string appName = string.Empty;
        /// <summary>
        /// 应用程序名称
        /// </summary>
        [DisplayName("应用程序名称")]
        [Category("基本设置")]
        public string AppName
        {
            get
            {
                if (string.IsNullOrEmpty(appName))
                {
                    appName = TengDa.Wpf.Option.GetOption("AppName");
                    if (appName == string.Empty)
                    {
                        appName = "东莞塔菲尔Hipot自动采集上传系统";
                        TengDa.Wpf.Option.SetOption("AppName", appName, "应用程序名称");
                    }
                }
                return appName;
            }
            set
            {
                if (appName != value)
                {
                    TengDa.Wpf.Option.SetOption("AppName", value);
                    SetProperty(ref appName, value);
                }
            }
        }

        /// <summary>
        /// 当前版本
        /// </summary>
        [DisplayName("当前版本")]
        public string Version => Regex.Match(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(), @"^[\d]+.[\d]+.[\d]+").Value;

        /// <summary>
        /// 更新时间
        /// </summary>
        [DisplayName("更新时间")]
        public string VersionTime => System.IO.File.GetLastWriteTime(this.GetType().Assembly.Location).ToString("yyyy/M/d HH:mm");


        private DateTime timeNow = DateTime.Now;
        [Browsable(false)]
        public DateTime TimeNow
        {
            get => timeNow;
            set => SetProperty(ref timeNow, value);
        }

        private string currentUserNameTip = string.Empty;
        [Browsable(false)]
        public string CurrentUserNameTip
        {
            get
            {
                if (AppCurrent.User.Id < 1)
                {
                    currentUserNameTip = string.Empty;
                }
                else
                {
                    currentUserNameTip = string.Format("当前用户: {0}", UserName);
                }
                return currentUserNameTip;
            }
            set => SetProperty(ref currentUserNameTip, value);
        }

        [Browsable(false)]
        public string UserName => AppCurrent.User.Name;

        [Browsable(false)]
        public string UserGroupName => AppCurrent.User.Role.Name;

        [Browsable(false)]
        public string UserProfilePicture => AppCurrent.User.ProfilePicture;

        [Browsable(false)]
        public string UserNumber => AppCurrent.User.Number;

        [Browsable(false)]
        public string UserPhoneNumber => AppCurrent.User.PhoneNumber;

        [Browsable(false)]
        public string UserEmail => AppCurrent.User.Email;


        private bool mainWindowsBackstageIsOpen = false;
        [Browsable(false)]
        public bool MainWindowsBackstageIsOpen
        {
            get => mainWindowsBackstageIsOpen;
            set => SetProperty(ref mainWindowsBackstageIsOpen, value);
        }


        private RunStatus runStatus = RunStatus.闲置;
        [ReadOnly(true), DisplayName("当前系统运行状态")]
        public RunStatus RunStatus
        {
            get => runStatus;
            set => SetProperty(ref runStatus, value);
        }


        private GraphShowMode graphShowMode = GraphShowMode.实时数据;
        [ReadOnly(true), DisplayName("图表数据显示模式")]
        public GraphShowMode GraphShowMode
        {
            get => graphShowMode;
            set => SetProperty(ref graphShowMode, value);
        }

        [Browsable(false)]
        public bool IsMesLogin
        {
            get => Current.Option.IsMesLogin;
            set => Current.Option.IsMesLogin = value;
        }

        [Browsable(false)]
        public bool IsRememberMe
        {
            get => Current.Option.IsRememberMe;
            set => Current.Option.IsRememberMe = value;
        }

        public List<CommunicateObject> GetComms()
        {
            return new List<CommunicateObject>()
            {
                Current.Tester,
                Current.Collector,
                Current.Cooler,
                Current.Scaner,
                Current.Mes
            };
        }

    }
}
