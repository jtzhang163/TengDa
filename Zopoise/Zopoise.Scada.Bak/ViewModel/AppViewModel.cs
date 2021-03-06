﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using TengDa;
using TengDa.Wpf;

namespace Zopoise.Scada.Bak
{
    /// <summary>
    /// 应用程序视图模型
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

        private string userName = string.Empty;
        [Browsable(false)]
        public string UserName
        {
            get => userName;
            set => SetProperty(ref userName, value);
        }

        private string userGroupName = string.Empty;
        [Browsable(false)]
        public string UserGroupName
        {
            get => userGroupName;
            set => SetProperty(ref userGroupName, value);
        }

        private string userProfilePicture = string.Empty;
        [Browsable(false)]
        public string UserProfilePicture
        {
            get => userProfilePicture;
            set => SetProperty(ref userProfilePicture, value);
        }

        private string userNumber = string.Empty;
        [Browsable(false)]
        public string UserNumber
        {
            get => userNumber;
            set => SetProperty(ref userNumber, value);
        }

        private string userPhoneNumber = string.Empty;
        [Browsable(false)]
        public string UserPhoneNumber
        {
            get => userPhoneNumber;
            set => SetProperty(ref userPhoneNumber, value);
        }

        private string userEmail = string.Empty;
        [Browsable(false)]
        public string UserEmail
        {
            get => userEmail;
            set => SetProperty(ref userEmail, value);
        }

        private bool mainWindowsBackstageIsOpen = true;
        [Browsable(false)]
        public bool MainWindowsBackstageIsOpen
        {
            get => mainWindowsBackstageIsOpen;
            set => SetProperty(ref mainWindowsBackstageIsOpen, value);
        }

        private bool isLogin = false;
        [Browsable(false)]
        public bool IsLogin
        {
            get => isLogin;
            set => SetProperty(ref isLogin, value);
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

        public List<Tester> GetTesters()
        {
            return Current.Testers;
        }

        private int selectTesterIndex = -1;
        [Browsable(false)]
        public int SelectTesterIndex
        {
            get
            {
                if (selectTesterIndex < 0)
                {
                    selectTesterIndex = Current.Option.SelectTesterIndex;
                }
                return selectTesterIndex;
            }
            set
            {
                if (selectTesterIndex != value)
                {
                    Current.Option.SelectTesterIndex = value;
                }
                SetProperty(ref selectTesterIndex, value);
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
