using System;
using System.ComponentModel;
using System.Net.NetworkInformation;

namespace TengDa.WF.MES
{
    public class MES
    {
        private static string tableName = string.Empty;
        public static string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    tableName = Config.DbTableNamePre + ".MES";
                }
                return tableName;
            }
        }

        protected string host = string.Empty;
        protected string name = string.Empty;
        protected bool isEnable = true;
        protected bool isOffline = false;

        [ReadOnly(true)]
        public int Id { get; set; }

        [ReadOnly(true)]
        [DisplayName("名称")]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    UpdateDbField("Name", value);
                }
                name = value;
            }
        }

        [DisplayName("MES服务器")]
        [Category("基本设置")]
        public string Host
        {
            get
            {
                return host;
            }
            set
            {
                if (host != value)
                {
                    UpdateDbField("Host", value);
                }
                host = value;
            }
        }


        private int notPingSuccessCount = 0;
        private bool isAlive = false;

        /// <summary>
        /// 是否在线
        /// </summary>
        [DisplayName("是否在线")]
        [ReadOnly(true)]
        public bool IsAlive
        {
            get
            {
                if (IsEnable)
                {
                    notPingSuccessCount = (++notPingSuccessCount) % 10;
                    if (notPingSuccessCount == 2)
                    {
                        isAlive = IsPingSuccess;
                    }
                }
                return IsEnable && isAlive;
            }
            set
            {
                isAlive = value;
            }
        }
        /// <summary>
        /// 是否能Ping通，可判断远程主机是否存在
        /// </summary>
        [DisplayName("是否能Ping通")]
        [ReadOnly(true)]
        public bool IsPingSuccess
        {
            get
            {
                try
                {
                    Ping ping = new Ping();
                    PingReply pingReply = ping.Send(Host, 1000);
                    if (pingReply.Status == IPStatus.Success)
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex);
                }
                return false;
            }
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        [ReadOnly(true)]
        [DisplayName("是否启用")]
        public bool IsEnable
        {
            get
            {
                return isEnable;
            }
            set
            {
                if (isEnable != value)
                {
                    UpdateDbField("IsEnable", value);
                }
                isEnable = value;
            }
        }

        /// <summary>
        /// 是否离线
        /// </summary>
        [DisplayName("是否离线")]
        public bool IsOffline
        {
            get
            {
                return isOffline;
            }
            set
            {
                if (isOffline != value)
                {
                    UpdateDbField("IsOffline", value);
                }
                isOffline = value;
            }
        }

        protected void UpdateDbField(string field, object value)
        {
            string msg = string.Empty;
            if (!Database.UpdateField(Id, TableName, field, value.ToString(), out msg))
            {
                Error.Alert(msg);
            }
        }
    }
}
