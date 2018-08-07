using System;
using System.ComponentModel;
using TengDa;

namespace Tafel.Hipot.App
{
    public class UserIDLogViewModel
    {
        [DisplayName("用户名称")]
        public string UserName { get; set; }

        /// <summary>
        /// 测试器名称
        /// </summary>
        [DisplayName("测试器名称")]
        public string TesterName { get; set; }

        /// <summary>
        /// 阻值
        /// </summary>
        [DisplayName("阻值")]
        public float Resistance { get; set; }

        /// <summary>
        /// 记录时间
        /// </summary>
        [DisplayName("记录时间")]
        public DateTime RecordTime { get; set; }
    }
}
