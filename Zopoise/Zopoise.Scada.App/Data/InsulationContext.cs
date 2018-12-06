using System.Data.Entity;
using TengDa.Wpf;

namespace Zopoise.Scada.App
{
    public class InsulationContext : DbContext
    {
        public InsulationContext() : base(AppCurrent.ConnectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InsulationTester>().ToTable("t_tester");
            modelBuilder.Entity<InsulationDataLog>().ToTable("t_data_log");
        }

        public DbSet<InsulationTester> Testers { get; set; }
        public DbSet<InsulationDataLog> DataLogs { get; set; }
    }

    public class TesterInitializer : DropCreateDatabaseIfModelChanges<InsulationContext>
    {
        protected override void Seed(InsulationContext context)
        {
            var tester = new InsulationTester
            {
                Name = "绝缘电阻测试仪",
                Company = "日置(HIOKI)",
                Model = "ST5520",
                Number = "111111",
                IsEnabled = true,
                PortName = "COM1",
                BaudRate = 9600,
                DataBits = 8,
                Parity = System.IO.Ports.Parity.None,
                StopBits = System.IO.Ports.StopBits.One,
                IsPassiveReceiveSerialPort = true
            };
            context.Testers.Add(tester);
        }
    }
}
