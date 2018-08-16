using System;
using System.Configuration;
using System.Data.Entity;


namespace TengDa.Wpf.Data
{
    public class YieldContext : DbContext
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["DefaultDatabase"].ToString();
        public YieldContext() : base(connectionString)
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
