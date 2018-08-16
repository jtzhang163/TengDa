using System.Data.Entity;

namespace TengDa.Wpf
{
    public class OperationContext : DbContext
    {
        public OperationContext() : base(AppCurrent.ConnectionString)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OperationLog>().ToTable("t_operation_log");
        }
        public DbSet<OperationLog> Operations { get; set; }
    }
}
