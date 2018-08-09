using System;

namespace TengDa.Wpf
{
    public class RealtimeYieldViewModel : BindableObject
    {
        private int feedingOK = -1;
        /// <summary>
        /// 上料OK数
        /// </summary>
        public int FeedingOK
        {
            get
            {
                if (feedingOK < 0)
                {
                    feedingOK = RealtimeYield.GetRealtimeYield(YieldKey.FeedingOK);
                }
                return feedingOK;
            }
            set
            {
                if (feedingOK != value)
                {
                    RealtimeYield.SetRealtimeYield(YieldKey.FeedingOK, value);
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
                if (feedingNG < 0)
                {
                    feedingNG = RealtimeYield.GetRealtimeYield(YieldKey.FeedingNG);
                }
                return feedingNG;
            }
            set
            {
                if (feedingNG != value)
                {
                    RealtimeYield.SetRealtimeYield(YieldKey.FeedingNG, value);
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
                if (blankingOK < 0)
                {
                    blankingOK = RealtimeYield.GetRealtimeYield(YieldKey.BlankingOK);
                }
                return blankingOK;
            }
            set
            {
                if (blankingOK != value)
                {
                    RealtimeYield.SetRealtimeYield(YieldKey.BlankingOK, value);
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
                if (blankingNG < 0)
                {
                    blankingNG = RealtimeYield.GetRealtimeYield(YieldKey.BlankingNG);
                }
                return blankingNG;
            }
            set
            {
                if (blankingNG != value)
                {
                    RealtimeYield.SetRealtimeYield(YieldKey.BlankingNG, value);
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
                if (startTime == TengDa.Common.DefaultTime)
                {
                    startTime = RealtimeYield.GetStartTime();
                }
                return startTime;
            }
            set
            {
                if (startTime != value)
                {
                    RealtimeYield.SetRealtimeYield(value);
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
            Current.AddOperation("清空产量");
            StartTime = DateTime.Now;
        }
    }
}
