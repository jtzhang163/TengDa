using System;
using System.ComponentModel.DataAnnotations;

namespace TengDa.Wpf
{
    /// <summary>
    /// 产量历史
    /// </summary>
    public class YieldLog : Record
    {
        /// <summary>
        /// 上料OK数
        /// </summary>
        public int FeedingOK { get; set; }
        /// <summary>
        /// 上料NG数
        /// </summary>
        public int FeedingNG { get; set; }
        /// <summary>
        /// 下料OK数
        /// </summary>
        public int BlankingOK { get; set; }
        /// <summary>
        /// 下料NG数
        /// </summary>
        public int BlankingNG { get; set; }
        /// <summary>
        /// 起始时间
        /// </summary>
        [Required]
        public DateTime StartTime { get; set; }

        public YieldLog() : this(0, 0, 0, 0, Common.DefaultTime)
        {
            this.Id = -1;
        }

        public YieldLog(int feedingOK, int feedingNG, int blankingOK, int blankingNG, DateTime startTime)
        {
            User = AppCurrent.User;
            FeedingOK = feedingOK;
            FeedingNG = feedingNG;
            BlankingOK = blankingOK;
            BlankingNG = blankingNG;
            StartTime = startTime;
            DateTime = DateTime.Now;
        }
    }
    /// <summary>
    /// 实时产量
    /// </summary>
    public class YieldNow : Service
    {

        /// <summary>
        /// 上料OK数
        /// </summary>
        public int FeedingOK { get; set; }
        /// <summary>
        /// 上料NG数
        /// </summary>
        public int FeedingNG { get; set; }
        /// <summary>
        /// 下料OK数
        /// </summary>
        public int BlankingOK { get; set; }
        /// <summary>
        /// 下料NG数
        /// </summary>
        public int BlankingNG { get; set; }
        /// <summary>
        /// 产量计数起始时间
        /// </summary>
        [Required]
        public DateTime StartTime { get; set; } = TengDa.Common.DefaultTime;

        public YieldNow() : this(0, 0, 0, 0, DateTime.Now)
        {
            this.Id = -1;
        }

        public YieldNow(int feedingOK, int feedingNG, int blankingOK, int blankingNG, DateTime startTime)
        {
            this.FeedingOK = feedingOK;
            this.FeedingOK = feedingNG;
            this.FeedingOK = blankingOK;
            this.FeedingOK = blankingNG;
            this.StartTime = startTime;
        }

    }

}
