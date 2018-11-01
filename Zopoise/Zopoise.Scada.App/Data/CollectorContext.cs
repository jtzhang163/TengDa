using System.Data.Entity;
using TengDa.Wpf;

namespace Zopoise.Scada.App
{
    public class CollectorContext : DbContext
    {
        public CollectorContext() : base(AppCurrent.ConnectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TemperatureCollector>().ToTable("t_collector");
        }

        public DbSet<TemperatureCollector> Collectors { get; set; }
    }

    public class CollectorInitializer : DropCreateDatabaseIfModelChanges<CollectorContext>
    {
        protected override void Seed(CollectorContext context)
        {
            var collector = new TemperatureCollector
            {
                Name = "温度采集器",
                Company = "深圳有控技术",
                Model = "",
                Number = "",
                IsEnabled = true,
                PortName = "COM2",
                BaudRate = 9600,
                DataBits = 8,
                Parity = System.IO.Ports.Parity.Even,
                StopBits = System.IO.Ports.StopBits.One,
                IsPassiveReceiveSerialPort = false
            };
            context.Collectors.Add(collector);
        }
    }
}
