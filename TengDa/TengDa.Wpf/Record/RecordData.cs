using System;

namespace TengDa.Wpf
{
    /// <summary>
    /// 日志型数据基类
    /// </summary>
    public abstract class RecordData : Service
    {
        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime RecordTime { get; set; } = Common.DefaultTime;

        protected RecordData()
        {
            this.RecordTime = DateTime.Now;
        }
    }
}
