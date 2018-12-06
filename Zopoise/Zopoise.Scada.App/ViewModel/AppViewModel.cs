using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using TengDa;
using TengDa.Wpf;

namespace Zopoise.Scada.App
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
                    if (string.IsNullOrEmpty(appName))
                    {
                        appName = "众普森灯具老化线数据监控采集系统";
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
    
        [Browsable(false)]
        public string LocalIPAddress => "127.0.0.1";

        [Browsable(false)]
        public string CurrentProcess
        {
            get
            {
                var tmp = Current.Option.CurrentProcess.Split(',');

                if(tmp.Length < 2)
                {
                    return "";
                }

                return string.Format("{0}【{1}】",tmp[0],tmp[1]);
            }
        }

        [Browsable(false)]
        public string CurrentStation
        {
            get
            {
                var tmp = Current.Option.CurrentStation.Split(',');

                if (tmp.Length < 2)
                {
                    return "";
                }

                return string.Format("{0}【{1}】", tmp[0], tmp[1]);
            }
        }
        [Browsable(false)]
        public string UserName => AppCurrent.User.Name;

        [Browsable(false)]
        public string UserGroupName => AppCurrent.User.Role.Name;

        private string userProfilePicture = string.Empty;
        [Browsable(false)]
        public string UserProfilePicture
        {
            get
            {
                userProfilePicture = AppCurrent.User.ProfilePicture;
                return userProfilePicture;
            }
            set
            {
                AppCurrent.User.ProfilePicture = value;
                SetProperty(ref userProfilePicture, value);
            }
        }

        private string userNickname = string.Empty;
        [Browsable(false)]
        public string UserNickname
        {
            get
            {
                userNickname = AppCurrent.User.Nickname;
                return userNickname;
            }
            set
            {
                AppCurrent.User.Nickname = value;
                SetProperty(ref userNickname, value);
            }
        }

        private string userNumber = string.Empty;
        [Browsable(false)]
        public string UserNumber
        {
            get
            {
                userNumber = AppCurrent.User.Number;
                return userNumber;
            }
            set
            {
                AppCurrent.User.Number = value;
                SetProperty(ref userNumber, value);
            }
        }

        private string userPhoneNumber = string.Empty;
        [Browsable(false)]
        public string UserPhoneNumber
        {
            get
            {
                userPhoneNumber = AppCurrent.User.PhoneNumber;
                return userPhoneNumber;
            }
            set
            {
                AppCurrent.User.PhoneNumber = value;
                SetProperty(ref userPhoneNumber, value);
            }
        }

        private string userEmail = string.Empty;
        [Browsable(false)]
        public string UserEmail
        {
            get
            {
                userEmail = AppCurrent.User.Email;
                return userEmail;
            }
            set
            {
                AppCurrent.User.Email = value;
                SetProperty(ref userEmail, value);
            }
        }


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
                Current.Controller,
                Current.Mes
            };
        }

        private bool isLoginWindow = true;
        /// <summary>
        /// LoginWindow是登录界面还是注册界面
        /// </summary>
        [Browsable(false)]
        public bool IsLoginWindow
        {
            get => isLoginWindow;
            set => SetProperty(ref isLoginWindow, value);
        }

        private bool isCurrentUserIsEdit = false;
        /// <summary>
        /// 当前用户界面处于编辑状态
        /// </summary>
        [Browsable(false)]
        public bool IsCurrentUserIsEdit
        {
            get => isCurrentUserIsEdit;
            set => SetProperty(ref isCurrentUserIsEdit, value);
        }
    }
}
