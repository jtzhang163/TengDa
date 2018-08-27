using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.NetworkInformation;

namespace TengDa.Wpf
{
    public abstract class Mes : CommunicateObject
    {

        [DisplayName("MES服务器")]
        [Required]
        [MaxLength(30)]
        public string Host { get; set; }

        /// <summary>
        /// 是否能Ping通，可判断远程主机是否存在
        /// </summary>
        [Description("是否能Ping通，可判断远程主机是否存在")]
        [DisplayName("是否能Ping通")]
        [ReadOnly(true)]
        [NotMapped]
        public bool IsPingSuccess
        {
            get
            {
                try
                {
                    Ping ping = new Ping();
                    PingReply pingReply = ping.Send(Host, 200);
                    if (pingReply.Status == IPStatus.Success)
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex);
                }
                IsAlive = false;
                return false;
            }
        }


        /// <summary>
        /// 是否离线
        /// </summary>
        [DisplayName("是否离线")]
        public bool IsOffline { get; set; }

        public bool Connect(out string msg)
        {
            msg = "";
            if (IsPingSuccess)
            {
                IsAlive = true;              
                return true;
            }
            msg = string.Format("无法连接到{0}服务器, IP:{1}", Name, Host);
            IsAlive = false;
            return false;
        }

        public bool DisConnect(out string msg)
        {
            msg = "";
            IsAlive = false;
            return true;
        }
    }
}