using System.Collections.Generic;
using System.Data.Entity;

namespace TengDa.Wpf
{
    /// <summary>
    /// 用户上下文
    /// </summary>
    public class OptionContext : DbContext
    {
        public OptionContext() : base(AppCurrent.ConnectionString)
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
            };
            options.ForEach(o => context.Options.Add(o));
        }
    }

}
