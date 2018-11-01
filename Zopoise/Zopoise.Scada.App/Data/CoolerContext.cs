using System.Data.Entity;
using TengDa.Wpf;

namespace Zopoise.Scada.App
{
    public class CoolerContext : DbContext
    {
        public CoolerContext() : base(AppCurrent.ConnectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cooler>().ToTable("t_cooler");
            modelBuilder.Entity<PLC>().ToTable("t_plc");
        }

        public DbSet<Cooler> Coolers { get; set; }
        public DbSet<PLC> PLCs { get; set; }
    }

    public class CoolerInitializer : DropCreateDatabaseIfModelChanges<CoolerContext>
    {
        protected override void Seed(CoolerContext context)
        {
            var cooler = new Cooler
            {
                Name = "冷却机",
                Company = "TengDa",
                Model = "",
                Number = "",
                IsEnabled = true,
                PLC = new PLC
                {
                    Name = "PLC",             
                    Company = "Panasonic",
                    Model = "FP-XH C60T",
                    Number = "",
                    IP = "192.168.1.5",
                    Port = 9094,
                    IsEnabled = true,
                }
            };
            context.Coolers.Add(cooler);
        }
    }
}
