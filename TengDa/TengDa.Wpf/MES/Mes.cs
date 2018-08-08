using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
        public bool IsPingSuccess = false;


        /// <summary>
        /// 是否离线
        /// </summary>
        public bool IsOffline = false;
    }
}