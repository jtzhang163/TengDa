using System;
using System.ComponentModel;

namespace TengDa.Wpf
{
    public class OperationViewModel
    {
        [DisplayName("用户名称")]
        public string UserName { get; set; }
        /// <summary>
        /// 操作内容
        /// </summary>
        [DisplayName("操作内容")]
        public string Content { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        [DisplayName("时间")]
        public DateTime Time { get; set; }
    }
}
