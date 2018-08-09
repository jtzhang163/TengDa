using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TengDa.Wpf
{
    /// <summary>
    /// 通信对象
    /// 包括MES、设备等
    /// </summary>
    public class CommunicateObject : Service
    {
        private string name = "";
        /// <summary>
        /// 名称
        /// </summary>
        [DisplayName("名称"), Category("基本信息")]
        [MaxLength(50)]
        [Required]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                SetProperty(ref name, value);
            }
        }
        /// <summary>
        /// 是否启用
        /// </summary>
        [DisplayName("是否启用"), Category("常见设置"), ReadOnly(true)]
        public bool IsEnable { get; set; } = true;

        private bool isAlive;
        /// <summary>
        /// 是否在线
        /// </summary>
        [NotMapped,Browsable(false)]
        public bool IsAlive
        {
            get
            {
                return isAlive;
            }
            set
            {
                SetProperty(ref isAlive, value);
            }
        }

        private string realtimeStatus = "尚未连接";
        [NotMapped, Browsable(false)]
        public string RealtimeStatus
        {
            get
            {
                return realtimeStatus;
            }
            set
            {
                SetProperty(ref realtimeStatus, value);
            }
        }
    }
}
