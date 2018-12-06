using System.Data.Entity;
using TengDa.Wpf;

namespace Zopoise.Scada.App
{
    public class ControllerContext : DbContext
    {
        public ControllerContext() : base(AppCurrent.ConnectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Controller>().ToTable("t_controller");
            modelBuilder.Entity<PLC>().ToTable("t_plc");
        }

        public DbSet<Controller> Controllers { get; set; }
        public DbSet<PLC> PLCs { get; set; }
    }

    public class ControllerInitializer : DropCreateDatabaseIfModelChanges<ControllerContext>
    {
        protected override void Seed(ControllerContext context)
        {
            var controller = new Controller
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
            context.Controllers.Add(controller);
        }
    }
}
