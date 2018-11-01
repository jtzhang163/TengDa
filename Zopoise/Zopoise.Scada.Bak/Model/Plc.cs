using System.ComponentModel;
using System.Configuration;
using System.Data.Entity;
using TengDa.Wpf;

namespace Zopoise.Scada.Bak
{
    /// <summary>
    /// PLC
    /// </summary>
    [DisplayName("可编程逻辑控制器（PLC）")]
    public class Plc : EthernetTerminal
    {
        public Plc() : this(-1)
        {

        }
        public Plc(long id)
        {
            this.Id = Id;
        }
    }

    public class PlcContext : DbContext
    {

        private static string connectionString = ConfigurationManager.ConnectionStrings["DefaultDatabase"].ToString();
        public PlcContext() : base(connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Plc>().ToTable("t_plc");
        }

        public DbSet<Plc> Plcs { get; set; }
    }

    public class PlcInitializer : DropCreateDatabaseIfModelChanges<PlcContext>
    {
        protected override void Seed(PlcContext context)
        {
            var plc = new Plc
            {
                Name = "PLC",
                Company = "Mitsubishi",
                IsEnabled = true,
                Model = "FX5u",
                IP = "127.0.0.1",
                Port = 1000
            };
            context.Plcs.Add(plc);
        }
    }
}
