using System;

namespace TengDa.Wpf
{
    /// <summary>
    /// 日志型数据基类
    /// </summary>
    public abstract class Record : Service
    {
        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime DateTime { get; set; } = Common.DefaultTime;

        protected Record()
        {
            this.DateTime = DateTime.Now;
        }
    }
}
