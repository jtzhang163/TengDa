using System;
using System.Data.Entity;

namespace TengDa.Wpf
{
    public class YieldContext : DbContext
    {
        public YieldContext() : base(AppCurrent.ConnectionString)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<YieldLog>().ToTable("t_yield_log");
            modelBuilder.Entity<YieldNow>().ToTable("t_yield_now");
        }
        public DbSet<YieldLog> YieldLogs { get; set; }
        public DbSet<YieldNow> YieldNows { get; set; }
    }

    public class YieldInitializer : DropCreateDatabaseIfModelChanges<YieldContext>
    {
        protected override void Seed(YieldContext context)
        {
            context.YieldNows.Add(new YieldNow(0, 0, 0, 0, DateTime.Now));
        }
    }
}
