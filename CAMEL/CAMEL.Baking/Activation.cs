using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TengDa;

namespace CAMEL.Baking
{
    /// <summary>
    /// 激活相关
    /// </summary>
    public class Activation
    {
        private static DateTime showActiveMsgTime = TengDa.Common.DefaultTime;
        public static DateTime ShowActiveMsgTime
        {
            get
            {
                if (showActiveMsgTime == TengDa.Common.DefaultTime)
                {
                    var samt = TengDa.WF.Option.GetOption("ShowActiveMsgTime");
                    showActiveMsgTime = _Convert.StrToDateTime(TengDa.Encrypt.Base64.DecodeBase64(samt), TengDa.Common.DefaultTime);
                }
                return showActiveMsgTime;
            }
            set
            {
                if (value != showActiveMsgTime)
                {
                    TengDa.WF.Option.SetOption("ShowActiveMsgTime", TengDa.Encrypt.Base64.EncodeBase64(value.ToString("yyyy-MM-dd HH:mm:ss")));
                    showActiveMsgTime = value;
                }
            }
        }

        private static DateTime expirationTime = TengDa.Common.DefaultTime;
        public static DateTime ExpirationTime
        {
            get
            {
                if (expirationTime == TengDa.Common.DefaultTime)
                {
                    var et = TengDa.WF.Option.GetOption("ExpirationTime");
                    expirationTime = _Convert.StrToDateTime(TengDa.Encrypt.Base64.DecodeBase64(et), TengDa.Common.DefaultTime);
                }
                return expirationTime;
            }
            set
            {
                if (value != expirationTime)
                {
                    TengDa.WF.Option.SetOption("ExpirationTime", TengDa.Encrypt.Base64.EncodeBase64(value.ToString("yyyy-MM-dd HH:mm:ss")));
                    expirationTime = value;
                }
            }
        }

        private static bool? isShowMsg;
        public static bool IsShowMsg
        {
            get
            {
                if (isShowMsg == null)
                {
                    isShowMsg = Convert.ToBoolean(TengDa.Encrypt.Base64.DecodeBase64(TengDa.WF.Option.GetOption("IsShowMsg")));
                }
                return isShowMsg.Value;
            }
            set
            {
                if (value != isShowMsg)
                {
                    TengDa.WF.Option.SetOption("IsShowMsg", TengDa.Encrypt.Base64.EncodeBase64(value.ToString()));
                    isShowMsg = value;
                }
            }
        }

        private static bool? isExpired;
        public static bool IsExpired
        {
            get
            {
                if (isExpired == null)
                {
                    isExpired = Convert.ToBoolean(TengDa.Encrypt.Base64.DecodeBase64(TengDa.WF.Option.GetOption("IsExpired")));
                }
                return isExpired.Value;
            }
            set
            {
                if (value != isExpired)
                {
                    TengDa.WF.Option.SetOption("IsExpired", TengDa.Encrypt.Base64.EncodeBase64(value.ToString()));
                    isExpired = value;
                }
            }
        }

        private static bool? isActivated;
        public static bool IsActivated
        {
            get
            {
                if (isActivated == null)
                {
                    isActivated = Convert.ToBoolean(TengDa.Encrypt.Base64.DecodeBase64(TengDa.WF.Option.GetOption("IsActivated")));
                }
                return isActivated.Value;
            }
            set
            {
                if (value != isActivated)
                {
                    TengDa.WF.Option.SetOption("IsActivated", TengDa.Encrypt.Base64.EncodeBase64(value.ToString()));
                    isActivated = value;
                }
            }
        }


        public static bool IsShowActiveMsg(out string activeMsg)
        {
            activeMsg = "程序即将过期，点击此处激活";
            if (IsActivated)
            {
                return false;
            }
            if (IsExpired)
            {
                activeMsg = "程序已过期，点击此处激活";
                return true;
            }
            return IsShowMsg;
        }

        public static bool IsStopRunApp()
        {
            if (IsActivated)
            {
                return false;
            }
            return IsExpired;
        }

        public static void Run()
        {
            if (IsActivated)
            {
                return;
            }

            if (!IsShowMsg && DateTime.Now >= ShowActiveMsgTime)
            {
                IsShowMsg = true;
            }

            if (!IsExpired && DateTime.Now >= ExpirationTime)
            {
                IsExpired = true;
            }
        }

        public static bool SetValue(string name, object val)
        {
            try
            {
                Type type = typeof(Activation);
                var propertyInfo = type.GetProperty(name);
                propertyInfo.SetValue(null, val);
                return true;
            }
            catch (Exception ex)
            {
                Error.Alert(ex);
            }
            return false;
        }
    }
}

