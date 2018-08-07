using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TengDa.Wpf
{
    /// <summary>
    /// 操作日志
    /// </summary>
    public class Yield
    {
        public int Id { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }
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
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 保存时间
        /// </summary>
        public DateTime RecordTime { get; set; }

        public Yield() : this(0, 0, 0, 0, Common.DefaultTime)
        {
            this.Id = -1;
        }

        public Yield(int feedingOK, int feedingNG, int blankingOK, int blankingNG, DateTime startTime)
        {
            UserId = Current.User.Id;
            FeedingOK = feedingOK;
            FeedingNG = feedingNG;
            BlankingOK = blankingOK;
            BlankingNG = blankingNG;
            StartTime = startTime;
            RecordTime = DateTime.Now;
        }
    }

    public class RealtimeYield
    {

        public int Id { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public YieldKey Key { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 产量计数起始时间
        /// </summary>
        public DateTime StartTime { get; set; } = TengDa.Common.DefaultTime;

        public RealtimeYield()
        {
            this.Id = -1;
        }

        public RealtimeYield(YieldKey key, int value) : this(key, value, "")
        {
        }

        public RealtimeYield(YieldKey key, int value, string remark)
        {
            Key = key;
            Value = value;
            Remark = remark;
        }

        public static int GetRealtimeYield(YieldKey yieldKey)
        {
            return Context.YieldContext.RealtimeYields.Where(ry => ry.Key == yieldKey).First().Value;
        }

        public static void SetRealtimeYield(YieldKey yieldKey, int value)
        {
            Context.YieldContext.RealtimeYields.Where(ry => ry.Key == yieldKey).First().Value = value;
            Context.YieldContext.SaveChangesAsync();
        }

        public static async void SetRealtimeYield(DateTime startTime)
        {
            await Context.YieldContext.RealtimeYields.ForEachAsync(ry => ry.StartTime = startTime);
            await Context.YieldContext.SaveChangesAsync();
        }

        public static DateTime GetStartTime()
        {
            return Context.YieldContext.RealtimeYields.First().StartTime;
        }
    }

    public class YieldContext : DbContext
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["DefaultDatabase"].ToString();
        public YieldContext() : base(connectionString)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Yield>().Property(y => y.StartTime).HasColumnType("datetime").IsRequired();
            modelBuilder.Entity<Yield>().Property(y => y.RecordTime).HasColumnType("datetime").IsRequired();
            modelBuilder.Entity<RealtimeYield>().Property(ry => ry.Key).IsRequired();
            modelBuilder.Entity<RealtimeYield>().Property(ry => ry.Remark).HasMaxLength(30);
            modelBuilder.Entity<RealtimeYield>().Property(ry => ry.StartTime).HasColumnType("datetime");
        }
        public DbSet<Yield> Yields { get; set; }
        public DbSet<RealtimeYield> RealtimeYields { get; set; }
    }

    public class YieldInitializer : DropCreateDatabaseIfModelChanges<YieldContext>
    {
        protected override void Seed(YieldContext context)
        {
            var RealtimeYields = new List<RealtimeYield>()
      {
        new RealtimeYield(YieldKey.FeedingOK,0,"上料OK数"),
        new RealtimeYield(YieldKey.FeedingNG,0,"上料NG数"),
        new RealtimeYield(YieldKey.BlankingOK,0,"下料OK数"),
        new RealtimeYield(YieldKey.BlankingNG,0,"下料NG数")
      };
            RealtimeYields.ForEach(ry => context.RealtimeYields.Add(ry));
        }
    }
}