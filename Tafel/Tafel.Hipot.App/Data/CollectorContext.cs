using System.Data.Entity;
using TengDa.Wpf;

namespace Tafel.Hipot.App
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
                Company = "",
                Model = "",
                Number = "",
                IsEnable = true,
                PortName = "COM2",
                BaudRate = 9600,
                DataBits = 8,
                Parity = System.IO.Ports.Parity.None,
                StopBits = System.IO.Ports.StopBits.One,
                IsPassiveReceiveSerialPort = true
            };
            context.Collectors.Add(collector);
        }
    }
}
