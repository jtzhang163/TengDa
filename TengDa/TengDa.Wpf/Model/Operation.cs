using System;
using System.Configuration;
using System.Data.Entity;

namespace TengDa.Wpf
{
    /// <summary>
    /// 操作日志
    /// </summary>
    public class Operation
    {
        public int Id { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 操作内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime Time { get; set; }

        public Operation() : this(string.Empty)
        {
        }

        public Operation(string content)
        {
            UserId = Current.User.Id;
            Time = DateTime.Now;
            Content = content;
        }
    }

    public class OperationContext : DbContext
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["DefaultDatabase"].ToString();
        public OperationContext() : base(connectionString)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Operation>().Property(o => o.Content).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Operation>().Property(o => o.Time).HasColumnType("datetime").IsRequired();
        }
        public DbSet<Operation> Operations { get; set; }
    }
}
