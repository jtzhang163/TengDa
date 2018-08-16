using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;

namespace TengDa.Wpf.Data
{
    /// <summary>
    /// 用户上下文
    /// </summary>
    public class OptionContext : DbContext
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["DefaultDatabase"].ToString();
        public OptionContext() : base(connectionString)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Option>().ToTable("t_option");
        }
        public DbSet<Option> Options { get; set; }
    }

    public class OptionInitializer : DropCreateDatabaseIfModelChanges<OptionContext>
    {
        protected override void Seed(OptionContext context)
        {
            var options = new List<Option>()
            {
                new Option("AppName","XXXXXX系统","应用程序名称"),
                new Option("RememberUserId","1")
            };
            options.ForEach(o => context.Options.Add(o));
        }
    }

}
