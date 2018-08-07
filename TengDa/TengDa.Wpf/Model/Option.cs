using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TengDa.Wpf
{
    public class Option
    {

        public int Id { get; set; }

        /// <summary>
        /// 键名
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public Option()
        {
            this.Id = -1;
        }

        public Option(string key, string value) : this(key, value, "")
        {
        }

        public Option(string key, string value, string remark)
        {
            Key = key;
            Value = value;
            Remark = remark;
        }

        public static string GetOption(string key)
        {
            using (var data = new OptionContext())
            {
                Option option = data.Options.Where(o => o.Key == key).FirstOrDefault();
                return option != null ? option.Value : string.Empty;
            }
        }

        public static void SetOption(string key, string value)
        {
            SetOption(key, value, string.Empty);
        }

        public static void SetOption(string key, string value, string remark)
        {
            using (var data = new OptionContext())
            {
                Option option = data.Options.Where(o => o.Key == key).FirstOrDefault();
                // return option != null ? option.Value : string.Empty;
                if (option != null)
                {
                    data.Options.Where(o => o.Key == key).First().Value = value;
                }
                else
                {
                    data.Options.Add(new Option { Key = key, Value = value, Remark = remark });
                }
                data.SaveChanges();
            }
        }
    }

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
            modelBuilder.Entity<Option>().Property(o => o.Key).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Option>().Property(o => o.Value).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Option>().Property(o => o.Remark).HasMaxLength(50);
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