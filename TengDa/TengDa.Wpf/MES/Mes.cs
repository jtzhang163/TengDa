using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TengDa.Wpf
{
    public class Mes : Service
    {
        [ReadOnly(true)]
        [DisplayName("名称")]
        public string Name { get; set; }

        [Description("MES服务器")]
        [DisplayName("MES服务器")]
        public string Host { get; set; }

        /// <summary>
        /// 是否在线
        /// </summary>
        [DisplayName("是否在线")]
        [ReadOnly(true)]
        public bool IsAlive { get; set; }
        /// <summary>
        /// 是否能Ping通，可判断远程主机是否存在
        /// </summary>
        [DisplayName("是否能Ping通")]
        [ReadOnly(true)]
        public bool IsPingSuccess { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [ReadOnly(true)]
        [DisplayName("是否启用")]
        public bool IsEnable { get; set; }

        /// <summary>
        /// 是否离线
        /// </summary>
        [DisplayName("是否离线")]
        public bool IsOffline { get; set; }
    }
}
