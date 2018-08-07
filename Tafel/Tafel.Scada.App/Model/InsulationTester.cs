using System.ComponentModel;
using System.Configuration;
using System.Data.Entity;
using TengDa.Wpf;

namespace Zopoise.Scada.App
{
    /// <summary>
    /// 绝缘电阻测试仪
    /// </summary>
    [DisplayName("绝缘电阻测试仪")]
    public class InsulationTester : SerialTerminal
    {
        public InsulationTester() : this(-1)
        {

        }
        public InsulationTester(long id)
        {
            this.Id = Id;
        }
    }

    public class CommunicatorContext : DbContext
    {

        private static string connectionString = ConfigurationManager.ConnectionStrings["DefaultDatabase"].ToString();
        public CommunicatorContext() : base(connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InsulationTester>().Property(c => c.Name).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<InsulationTester>().Property(c => c.PortName).HasMaxLength(10);
            modelBuilder.Entity<InsulationTester>().Property(c => c.Company).HasMaxLength(50);
            modelBuilder.Entity<InsulationTester>().Property(c => c.Location).HasMaxLength(50);
            modelBuilder.Entity<InsulationTester>().Property(c => c.Number).HasMaxLength(50);
            modelBuilder.Entity<InsulationTester>().Property(c => c.Model).HasMaxLength(50);
        }

        public DbSet<InsulationTester> Communicators { get; set; }
    }

    public class InsulationTesterInitializer : DropCreateDatabaseIfModelChanges<CommunicatorContext>
    {
        protected override void Seed(CommunicatorContext context)
        {
            var communicator = new InsulationTester
            {
                Name = "数据采集器",
                Company = "Tengda",
            };
            context.Communicators.Add(communicator);
        }
    }
}
