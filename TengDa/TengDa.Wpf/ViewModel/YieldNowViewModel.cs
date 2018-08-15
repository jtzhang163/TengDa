using System;
using System.Linq;

namespace TengDa.Wpf
{
    /// <summary>
    /// 实时产量
    /// </summary>
    public class YieldNowViewModel : BindableObject
    {
        private int feedingOK = -1;
        /// <summary>
        /// 上料OK数
        /// </summary>
        public int FeedingOK
        {
            get
            {
                feedingOK = Context.YieldContext.YieldNows.First().FeedingOK;
                return feedingOK;
            }
            set
            {
                if (feedingOK != value)
                {
                    Context.YieldContext.YieldNows.First().FeedingOK = value;
                    Context.YieldContext.SaveChanges();
                }
                SetProperty(ref feedingOK, value);
            }
        }

        private int feedingNG = -1;
        /// <summary>
        /// 上料NG数
        /// </summary>
        public int FeedingNG
        {
            get
            {
                feedingNG = Context.YieldContext.YieldNows.First().FeedingOK;
                return feedingNG;
            }
            set
            {
                if (feedingNG != value)
                {
                    Context.YieldContext.YieldNows.First().FeedingNG = value;
                    Context.YieldContext.SaveChanges();
                }
                SetProperty(ref feedingNG, value);
            }
        }


        private int blankingOK = -1;
        /// <summary>
        /// 下料OK数
        /// </summary>
        public int BlankingOK
        {
            get
            {
                blankingOK = Context.YieldContext.YieldNows.First().BlankingOK;
                return blankingOK;
            }
            set
            {
                if (blankingOK != value)
                {
                    Context.YieldContext.YieldNows.First().BlankingOK = value;
                    Context.YieldContext.SaveChanges();
                }
                SetProperty(ref blankingOK, value);
            }
        }



        private int blankingNG = -1;
        /// <summary>
        /// 下料NG数
        /// </summary>
        public int BlankingNG
        {
            get
            {
                blankingNG = Context.YieldContext.YieldNows.First().BlankingNG;
                return blankingNG;
            }
            set
            {
                if (blankingOK != value)
                {
                    Context.YieldContext.YieldNows.First().BlankingNG = value;
                    Context.YieldContext.SaveChanges();
                }
                SetProperty(ref blankingNG, value);
            }
        }

        private string feedingOKContent = "上料数";

        public string FeedingOKContent
        {
            get => feedingOKContent;
            set => SetProperty(ref feedingOKContent, value);
        }

        private string blankingOKContent = "下料数";

        public string BlankingOKContent
        {
            get => blankingOKContent;
            set => SetProperty(ref blankingOKContent, value);
        }

        private DateTime startTime = TengDa.Common.DefaultTime;
        public DateTime StartTime
        {
            get
            {
                startTime = Context.YieldContext.YieldNows.First().StartTime;
                return startTime;
            }
            set
            {
                if (startTime != value)
                {
                    Context.YieldContext.YieldNows.First().StartTime = value;
                    Context.YieldContext.SaveChanges();
                }
                SetProperty(ref startTime, value);
            }
        }

        public void ClearYield()
        {
            FeedingOK = 0;
            FeedingNG = 0;
            BlankingOK = 0;
            BlankingNG = 0;
            OperationHelper.ShowTips("清空产量");
            StartTime = DateTime.Now;
        }
    }
}
